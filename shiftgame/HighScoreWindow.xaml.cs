using System;
using System.Collections.Generic;
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
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        HighScoreDatabase db;
        public HighScoreWindow()
        {
            InitializeComponent();
            db = new HighScoreDatabase();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = db;
            comboboxLevelHighscores.ItemsSource = db.LevelResults;
        }
    }
}
