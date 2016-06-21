using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace shiftgame
{
    delegate void PlayerFigureNotInAnimation();
    // játékos játékeleme
    class PlayerFigure
    {
        // játékelem alakja
        private Geometry figure;
        // játékelem aktuális pozíciója (szektor)
        Point sector;
        public Point Sector
        {
            get { return sector; }
        }
        private int steps;
        DoubleAnimation offsetXAnimation;
        DoubleAnimation offsetYAnimation;
        public event PlayerFigureNotInAnimation playerFigureNotInAnimation;
        private bool animationStarted = false;
        public int Steps
        {
            get { return steps; }
        }

        internal Geometry Figure
        {
            get { return figure; }
        }

        public PlayerFigure(Geometry figure)
        {
            this.figure = figure;
            this.sector = new Point(figure.Bounds.X, figure.Bounds.Y);
            this.steps = 0;
            Duration durationX = new Duration(TimeSpan.FromMilliseconds(20));
            Duration durationY = new Duration(TimeSpan.FromMilliseconds(250));
            offsetXAnimation = new DoubleAnimation();
            offsetYAnimation = new DoubleAnimation();
            CubicEase easing = new CubicEase(); 
            easing.EasingMode = EasingMode.EaseOut;
            offsetYAnimation.EasingFunction = easing;
            offsetXAnimation.Duration = durationX;
            offsetYAnimation.Duration = durationY;
            offsetXAnimation.From = 0;
            offsetYAnimation.From = 0;
            offsetXAnimation.Completed += offsetXYAnimation_Completed;
            offsetYAnimation.Completed += offsetXYAnimation_Completed;
        }

        public void Transform(TranslateTransform transform, bool stayThere)
        {
            if (!animationStarted)
            {
                offsetXAnimation.To = transform.X;
                offsetYAnimation.To = transform.Y;

                // ugrás animáció shifteléskor
                if (stayThere)
                    transform.BeginAnimation(TranslateTransform.YProperty, offsetYAnimation);
                // balra-jobbra animáció 
                else if (transform.Y == 0)
                    transform.BeginAnimation(TranslateTransform.XProperty, offsetXAnimation);
                // zuhanás animáció
                else
                    transform.BeginAnimation(TranslateTransform.YProperty, offsetYAnimation);

                offsetXAnimation.From = transform.X;
                offsetYAnimation.From = 0;
                sector.X += transform.X;
                sector.Y += transform.Y;

                animationStarted = true;
                figure.Transform = transform;
                steps++;
            }
        }
        // X vagy Y irányban animáció befejeződött:
        // jelzés a játék kezelőnek (játékos meghalt?)
        void offsetXYAnimation_Completed(object sender, EventArgs e)
        {
            animationStarted = false;
            figure = figure.GetFlattenedPathGeometry();
            if (playerFigureNotInAnimation != null)
            {
                playerFigureNotInAnimation();
            }
        }
    }
}
