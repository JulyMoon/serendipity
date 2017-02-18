using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serendipity
{
    class Game
    {
        public Map CurrentMap { get; private set; }
        public Game()
        {
            CurrentMap = new Map(7, 7, Color.Blue, Color.Red, Color.DodgerBlue, Color.Yellow);
        }
    }
}
