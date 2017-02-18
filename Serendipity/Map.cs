using System.Drawing;

namespace Serendipity
{
    public class Map
    {
        public readonly Color TopLeft;
        public readonly Color TopRight;
        public readonly Color BottomRight;
        public readonly Color BottomLeft;

        public readonly int Width, Height;

        private Color[,] tiles;

        public Map(int width, int height, Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
            Width = width;
            Height = height;

            tiles = new Color[Width, Height];
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    tiles[x, y] = Blerp(topLeft, topRight, bottomRight, bottomLeft, (double)x / (Width - 1), (double)y / (Height - 1));
        }

        public Color Get(int x, int y)
            => tiles[x, y];

        //private static byte Lerp(byte a, byte b, double t)
        //    => (byte)((1 - t) * a + t * b);

        //private static Color Lerp(Color a, Color b, double t)
        //    => Color.FromArgb(Lerp(a.R, b.R, t), Lerp(a.G, b.G, t), Lerp(a.B, b.B, t));

        private static byte Blerp(byte topLeft, byte topRight, byte bottomRight, byte bottomLeft, double x, double y)
            => (byte)(topLeft     * (1 - x) * (1 - y) +
                      topRight    * x       * (1 - y) +
                      bottomLeft  * (1 - x) * y +
                      bottomRight * x       * y);

        private static Color Blerp(Color topLeft, Color topRight, Color bottomRight, Color bottomLeft, double x, double y)
            => Color.FromArgb(Blerp(topLeft.R, topRight.R, bottomRight.R, bottomLeft.R, x, y),
                              Blerp(topLeft.G, topRight.G, bottomRight.G, bottomLeft.G, x, y),
                              Blerp(topLeft.B, topRight.B, bottomRight.B, bottomLeft.B, x, y));
    }
}
