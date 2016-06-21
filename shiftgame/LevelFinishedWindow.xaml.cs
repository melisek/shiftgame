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
    /// Interaction logic for LevelFinishedWindow.xaml
    /// </summary>
    public partial class LevelFinishedWindow : Window
    {
        private HighScoreDatabase db;
        private int level;
        private int ellapsedSeconds;
        private int steps;
        public LevelFinishedWindow(int level, int ellapsedSeconds, int steps)
        {
            InitializeComponent();
            db = new HighScoreDatabase();
            this.level = level;
            this.ellapsedSeconds = ellapsedSeconds;
            this.steps = steps;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            labelLevelCompletedTitle.Content = level + ". szint teljesítve!";
            labelLevelCompletedTime.Content = ellapsedSeconds + " mp";
            labelLevelCompletedSteps.Content = steps + " lépés";
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            string playerName;
            if (textboxLabelFinishedPlayer.Text.Length > 0)
                playerName = textboxLabelFinishedPlayer.Text;
            else 
                playerName = "Névtelen";
            db.AddLevelResults(level, new Result(new Player(playerName), ellapsedSeconds, steps));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
