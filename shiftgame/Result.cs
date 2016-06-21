using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiftgame
{
    public class Result : Bindable
    {
        private Player player;
        private int ellapsedSeconds;
        private int steps;

        public Player Player
        {
            get { return player; }
            set { player = value; OnPropertyChanged(); }
        }

        public int EllapsedSeconds
        {
            get { return ellapsedSeconds; }
            set { ellapsedSeconds = value; }
        }

        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        public Result(Player player, int ellapsedSeconds, int steps)
        {
            this.player = player;
            this.ellapsedSeconds = ellapsedSeconds;
            this.steps = steps;
        }
    }
}
