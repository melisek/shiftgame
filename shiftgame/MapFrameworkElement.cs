using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace shiftgame
{
    class MapFrameworkElement : FrameworkElement
    {
        GameComponents game;
        public GameComponents Game
        {
            get { return game; }
        }

        public MapFrameworkElement()
        {
            this.Loaded += MapFrameworkElement_Loaded;
            game = new GameComponents(1);
        }

        public void CreateGame(int mapNumber)
        {
            game = new GameComponents(mapNumber);
            SubscribeGameEvents();
        }

        void MapFrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focusable = true;
            this.Focus();
            this.KeyDown += MapFrameworkElement_KeyDown;
            SubscribeGameEvents();

            InvalidateVisual();
        }
        void SubscribeGameEvents()
        {
            game.playerKilledEvent += game_playerKilledEvent;
            game.timer.Tick += game_showDisappearingSectors;
        }

        void game_showDisappearingSectors(object sender, EventArgs e)
        {
            InvalidateVisual();
        }
        void game_playerKilledEvent(int level)
        {
            InvalidateVisual();
        }

        void MapFrameworkElement_KeyDown(object sender, KeyEventArgs e)
        {
            game.PlayerFigureMoveOnKeyDown(e.Key);
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (game.Map != null)
            {
                // kijárat szektor
                drawingContext.DrawGeometry(Brushes.White, new Pen(Brushes.DeepSkyBlue, 2), game.Map.ExitSector);

                // shift módban: elfoglalt szektorok fehérek, halálos szektorok feketék fehér háttérrel
                if (game.Map.ShiftMode)
                {
                    drawingContext.DrawGeometry(Brushes.White, null, game.Map.MapStructure);

                    for (int i = 0; i < game.Map.FatalSectors.Children.Count; i++)
                    {
                        drawingContext.DrawRectangle(Brushes.White, null, game.Map.FatalSectors.Children.ElementAt(i).Bounds);
                    }

                    drawingContext.DrawGeometry(Brushes.Black, null, game.Map.FatalSectors);
                }
                // különben a szektorok feketék
                else
                    drawingContext.DrawGeometry(Brushes.Black, null, game.Map.MapStructure);

                // eltűnő szektorok: fehér/szürke
                if (game.Map.ShowDisappearingSectors)
                    drawingContext.DrawGeometry(Brushes.Gray, null, game.Map.DisappearingSectors);
                else
                    drawingContext.DrawGeometry(Brushes.White, null, game.Map.DisappearingSectors);
            }
            // játékos kirajzolás
            if (game.PlayerFigure != null)
            {
                drawingContext.DrawGeometry(Brushes.DeepSkyBlue, null, game.PlayerFigure.Figure);
            }
        }
    }
}

