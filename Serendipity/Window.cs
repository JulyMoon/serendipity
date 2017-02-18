using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Serendipity
{
    class Window : GameWindow
    {
        private const int WINDOW_WIDTH = 300;
        private const int WINDOW_HEIGHT = 450;
        private const int HOR_BORDER = 30;
        private const int VER_BORDER = 30;

        private const double WIDTH_K = 2d / WINDOW_WIDTH;
        private const double HEIGHT_K = -2d / WINDOW_HEIGHT;

        private int tileWidth;
        private int tileHeight;

        private Game game = new Game();

        private bool prevMDown;
        private int mx, my;

        private bool dragging;
        private int dragTileX, dragTileY;
        private int dragOffsetX, dragOffsetY;

        public Window() : base(WINDOW_WIDTH, WINDOW_HEIGHT, GraphicsMode.Default, "Serendipity", GameWindowFlags.FixedWindow) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tileWidth = (WINDOW_WIDTH - 2 * HOR_BORDER) / game.Width;
            tileHeight = (WINDOW_HEIGHT - 2 * VER_BORDER) / game.Height;

            GL.ClearColor(Color.Black);
            GL.PointSize(4);
            GL.Ortho(0, Width, Height, 0, -1, 1);
            GL.Viewport(ClientSize);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!Focused)
                return;

            //KeyboardState ks = OpenTK.Input.Keyboard.GetState();

            //if (ks.IsKeyDown(Key.Escape))
            //    Exit();

            var ms = OpenTK.Input.Mouse.GetCursorState();
            var p = PointToClient(new Point(ms.X, ms.Y));
            mx = p.X;
            my = p.Y;

            var mdown = ms.IsButtonDown(MouseButton.Left);
            if (prevMDown != mdown)
            {
                int xx = mx - HOR_BORDER;
                int yy = my - VER_BORDER;
                if (xx >= 0 && yy >= 0)
                {
                    int ox = xx % tileWidth;
                    int oy = yy % tileHeight;

                    xx /= tileWidth;
                    yy /= tileHeight;
                    if (xx < game.Width && yy < game.Height && !game.IsLocked(xx, yy))
                    {
                        if (mdown)
                        {
                            dragging = true;
                            dragTileX = xx;
                            dragTileY = yy;
                            dragOffsetX = ox;
                            dragOffsetY = oy;
                        }
                        else if (dragging)
                        {
                            dragging = false;

                            if (dragTileX != xx || dragTileY != yy)
                                game.Swap(dragTileX, dragTileY, xx, yy);
                        }
                    }
                    else if (!mdown)
                    {
                        dragging = false;
                    }
                }
                else if (!mdown)
                {
                    dragging = false;
                }
            }

            prevMDown = mdown;
        }

        private void DrawDrag()
        {
            if (!dragging)
                return;

            DrawTile(mx - dragOffsetX, my - dragOffsetY, game.Get(dragTileX, dragTileY));
        }
    
        private void DrawMap()
        {
            GL.PushMatrix();
            GL.Translate(HOR_BORDER * WIDTH_K, VER_BORDER * HEIGHT_K, 0);

            for (int x = 0; x < game.Width; ++x)
                for (int y = 0; y < game.Height; ++y)
                    if (!dragging || x != dragTileX || y != dragTileY)
                        DrawGridTile(x, y);

            GL.PopMatrix();
        }

        private void DrawGridTile(int x, int y)
        {
            DrawTile(x * tileWidth, y * tileHeight, game.Get(x, y));

            if (game.IsLocked(x, y))
            {
                GL.Begin(PrimitiveType.Points);
                GL.Color4(Color.Black);
                
                GL.Vertex2((x + 0.5) * tileWidth, (y + 0.5) * tileHeight);

                GL.End();
            }
        }

        private void DrawTile(int x, int y, Color tile)
        {
            GL.PushMatrix();
            GL.Translate(x * WIDTH_K, y * HEIGHT_K, 0);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(tile);

            GL.Vertex2(0, 0);
            GL.Vertex2(tileWidth, 0);
            GL.Vertex2(tileWidth, tileHeight);
            GL.Vertex2(0, tileHeight);

            GL.End();

            GL.PopMatrix();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            DrawMap();
            DrawDrag();

            SwapBuffers();
        }

        private static int LoadTexture(Bitmap bitmap)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            System.Drawing.Imaging.BitmapData bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);
            bitmap.UnlockBits(bitmap_data);
            bitmap.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return texture;
        }
    }
}
