using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace shiftgame
{
    delegate void LevelMapFinished(int level, int ellapsedSeconds, int steps);
    delegate void PlayerKilled(int level);
    class GameComponents
    {
        private LevelMap map;
        private int level;
        private PlayerFigure playerFigure;
        public event LevelMapFinished levelFinishedEvent;
        public event PlayerKilled playerKilledEvent;
        public int mapCount = 4; // pályák száma
        public int m; // mód-előjel
        internal bool gameOver = false;
        private Stopwatch stopper;
        internal DispatcherTimer timer;
        private static Random rnd = new Random();
        
        internal LevelMap Map
        {
            get { return map; }
        }

        internal PlayerFigure PlayerFigure
        {
            get { return playerFigure; }
        }

        public GameComponents(int mapNumber)
        {
            map = new LevelMap(mapNumber);
            level = mapNumber;
            m = 1;
            playerFigure = new PlayerFigure(new RectangleGeometry(new Rect(map.PlayerStartPoint.X, map.PlayerStartPoint.Y, 20, 20)));
            playerFigure.playerFigureNotInAnimation += playerFigure_playerNotInAnimation;
            stopper = new Stopwatch();
            timer = new DispatcherTimer();
            if (map.DisappearingSectors.Children.Count > 0)
            {
                timer.Interval = TimeSpan.FromSeconds(rnd.Next(2, 5));
                timer.Tick += timer_Tick;
            }
        }

        // billentyű-kezelés: játékos mozgatás, shift mód aktiválása
        // első lépésnél: stopper indítás, eltűnő szektorok indítása
        // esemény-kiváltás: szint teljesítve
        internal void PlayerFigureMoveOnKeyDown(Key key)
        {
            if (key == Key.Left)
            {
                PlayerFigureMove(-10 * m, 0);
            }
            else if (key == Key.Right)
            {
                PlayerFigureMove(10 * m, 0);
            }
            else if (key == Key.LeftShift || key == Key.RightShift)
            {
                map.ShiftMode = !map.ShiftMode;
                map.ShiftMapSectors();
                PlayerFigureShift(0, 20 * m);
               
                m = -m;
            }

            if (playerFigure.Steps == 1)
            {
                timer.Start();
                stopper.Start();
            }

            if (playerFigure.Sector.X == map.MapFinishPoint.X && PlayerFigure.Sector.Y == map.MapFinishPoint.Y && levelFinishedEvent != null)
            {
                stopper.Stop();
                timer.Stop();
                m = 1;
                levelFinishedEvent(level, (int)TimeSpan.FromMilliseconds(stopper.ElapsedMilliseconds).TotalSeconds, playerFigure.Steps);
            }
        }
        // játékos mozgatás, ütközésvizsgálat
        private void PlayerFigureMove(int dx, int dy)
        {
            Geometry clone = playerFigure.Figure.Clone();
            TranslateTransform transform = new TranslateTransform(dx, dy);
            clone.Transform = transform;

            int g = 0;
            while (!map.CollideMapSectors(clone) && !map.CollideFatalSectors(clone) && PlayerInMapBounds(dx, dy))
            {
                g++;
                transform = new TranslateTransform(dx, dy + (20 * g) * m);
                clone.Transform = transform;
                clone.GetFlattenedPathGeometry();
            }

            if (map.CollideFatalSectors(clone))
            {
                gameOver = true;
            }

            transform = new TranslateTransform(dx, dy + 20 * (g - 1) * m);
            clone.Transform = transform;

            if (!map.CollideMapSectors(clone) && PlayerInMapBounds(dx, dy))
            {
                playerFigure.Transform(transform, gameOver);
            }
        }
        // shift mód: játékos átmozgatása
        private void PlayerFigureShift(int dx, int dy)
        {
            TranslateTransform transform = new TranslateTransform(dx, dy);
            playerFigure.Transform(transform, true);
        }
        // játékos a pálya keretén belül van-e
        private bool PlayerInMapBounds(int dx, int dy)
        {
            double newX = playerFigure.Sector.X + dx;
            double newY = playerFigure.Sector.Y + dy;

            return (newY >= 0 && newY < (map.Sectors.GetLength(0) * 20))
                && (newX >= 0 && newX < (map.Sectors.GetLength(1) * 20));
        }
        // játékos meghalt: játék leállítása, miután a játékos animálás befejeződött
        private void playerFigure_playerNotInAnimation()
        {
            if (gameOver && playerKilledEvent != null)
            {
                stopper.Stop();
                timer.Stop();
                playerKilledEvent(level);
            }
        }
        // eltűnő szektorok: ha eltűnt, játékos leesik
        private void timer_Tick(object sender, EventArgs e)
        {
            map.ShowDisappearingSectors = !map.ShowDisappearingSectors;
            if (!map.ShowDisappearingSectors)
                PlayerFigureMove(0, 0);
        }
    }
}
