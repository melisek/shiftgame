using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace shiftgame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Subscribe_GameEvents();
        }

        void Subscribe_GameEvents()
        {
            mapFrameworkElement.Game.levelFinishedEvent += Game_levelFinishedEvent;
            mapFrameworkElement.Game.playerKilledEvent += Game_playerKilledEvent;
        }
        // MessageBox: játékos meghalt
        // pálya újrakezdése: pálya újratöltése / GameWindow bezár, MainWindow megnyit 
        void Game_playerKilledEvent(int level)
        {
            MessageBoxResult result = MessageBox.Show("Meghaltál. :(\n" + level + ". pálya újrakezdése?", "Pálya újrakezdése?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                mapFrameworkElement.CreateGame(level);
                mapFrameworkElement.Game.m = 1;
                Subscribe_GameEvents();
            }
            else if (result == MessageBoxResult.No)
            {
                Show_MainWindow_Close_GameWindow();
            }
        }
        // LevelFinishedWindow: pálya teljesítve
        // következő pályára lépés / GameWindow bezár, MainWindow megnyit 
        void Game_levelFinishedEvent(int level, int ellapsedSeconds, int steps)
        {
            LevelFinishedWindow levelFinishedWindow = new LevelFinishedWindow(level, ellapsedSeconds, steps);
            if (levelFinishedWindow.ShowDialog() == true && level < mapFrameworkElement.Game.mapCount)
            {
                mapFrameworkElement.CreateGame(++level);
                mapFrameworkElement.Game.m = 1;
                Subscribe_GameEvents();
            }
            else
            {
                Show_MainWindow_Close_GameWindow();
            }
        }
        // GameWindow bezár, MainWindow megnyit 
        private void Show_MainWindow_Close_GameWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void mapFrameworkElement_KeyDown(object sender, KeyEventArgs e)
        {
            // shift mód: pálya elforgatás 180 fokkal
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                RotateGameGrid();
            }
            // GameWindow bezár, MainWindow megnyit 
            else if (e.Key == Key.Escape)
            {
                Show_MainWindow_Close_GameWindow();
            }
        }

        private void RotateGameGrid()
        {
            RotateTransform rot = new RotateTransform();
            if (mapFrameworkElement.Game.m == -1)
            {
                this.Background = Brushes.White;
                rot.Angle = 0;
            }

            else
            {
                this.Background = Brushes.Black;
                rot.Angle = 180;
            }
            rot.CenterX = mapFrameworkElement.ActualWidth / 2;
            rot.CenterY = mapFrameworkElement.ActualHeight / 2;
            gridGame.RenderTransform = rot;
        }

    }
}
