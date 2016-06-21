using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiftgame
{
    public class HighScoreDatabase
    {
        private ObservableCollection<LevelResults> levelResults;

        internal ObservableCollection<LevelResults> LevelResults
        {
            get { return levelResults; }
        }

        public HighScoreDatabase()
        {
            this.levelResults = new ObservableCollection<LevelResults>();
            ReadLevelResults();
        }

        public void AddLevelResults(int level, Result result)
        {
            int i = 0;
            while (i < levelResults.Count && levelResults[i].Level != level)
                i++;

            if (i < levelResults.Count)
                levelResults[i].Results.Add(result);
            else
                this.levelResults.Add(new LevelResults(level, result));

            StreamWriter sw = new StreamWriter(@"highscore-map_" + level + ".txt", true);
            sw.WriteLine(result.Player.Name + ";" + result.EllapsedSeconds + ";" + result.Steps + ";");
            sw.Flush();
            sw.Close();
        }

        private void ReadLevelResults()
        {
            int i = 1;
            while (i <= 5)
            {
                try
                {
                    StreamReader sr = new StreamReader(@"highscore-map_" + i + ".txt");
                    LevelResults levelAllResults = new LevelResults(i);

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] res = line.Split(';');
                        levelAllResults.Results.Add(new Result(new Player(res[0]), int.Parse(res[1]), int.Parse(res[2])));
                    }
                    this.levelResults.Add(levelAllResults);
                    sr.Close();
                    i++;
                }
                catch (FileNotFoundException)
                {
                    i++;
                }
            }
        }
    }
}
