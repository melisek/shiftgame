using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace shiftgame
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameWindow gw;
        HighScoreWindow hsw;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_NewGame(object sender, RoutedEventArgs e)
        {
            if (comboboxMapNumber.SelectedIndex != -1)
            {
                gw.mapFrameworkElement.CreateGame(comboboxMapNumber.SelectedIndex + 1);
                gw.Show();

                this.Close();
            }
        }

        private void Button_Click_HighScores(object sender, RoutedEventArgs e)
        {
            hsw = new HighScoreWindow();
            hsw.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gw = new GameWindow();

            string[] mapNames = { "Lépcsők", "Empire State Building", "Csatorna", "Spirál" };
            comboboxMapNumber.ItemsSource = mapNames;
        }
    }
}
