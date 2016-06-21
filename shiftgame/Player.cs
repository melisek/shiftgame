using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shiftgame
{
    public class Player : Bindable
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        public Player(string name)
        {
            this.name = name;
        }
    }
}
