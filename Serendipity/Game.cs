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
        public int Width => map.Width;
        public int Height => map.Height;

        private Map map;
        public Game()
        {
            map = new Map(7, 7, Color.Blue, Color.Red, Color.DodgerBlue, Color.Yellow);
        }

        public void Swap(int x1, int y1, int x2, int y2)
            => map.Swap(x1, y1, x2, y2);

        public Color Get(int x, int y)
            => map.Get(x, y);

        public bool IsSolved()
            => map.IsSolved();
    }
}
