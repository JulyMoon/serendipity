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
            map = new Map(9, 10, Color.White, Color.Blue, Color.Purple, Color.DarkSeaGreen);
        }

        public void Swap(int x1, int y1, int x2, int y2)
            => map.Swap(x1, y1, x2, y2);

        public Color Get(int x, int y)
            => map.Get(x, y);

        public bool IsLocked(int x, int y)
            => map.IsLocked(x, y);

        public bool IsSolved()
            => map.IsSolved();
    }
}
