using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiftgame
{
    public class LevelResults : Bindable
    {
        private int level;
        static string[] mapNames = { "Lépcsők", "Empire State Building", "Csatorna", "Spirál" };
        private ObservableCollection<Result> results;

        public int Level
        {
            get { return level; }
            set { level = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Result> Results
        {
            get { return results; }
        }

        public LevelResults(int level)
        {
            this.level = level;
            this.results = new ObservableCollection<Result>();
        }

        public LevelResults(int level, Result result) : this(level)
        {
            this.results.Add(result);
        }

        public override string ToString()
        {
            return mapNames[level - 1];
        }
    }
}
