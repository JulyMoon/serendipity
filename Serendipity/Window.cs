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
    private const int width = 640;
    private const int height = 480;
    private int testure;
    private int mx, my;

    public Window() : base(width, height, GraphicsMode.Default, "Hi", GameWindowFlags.FixedWindow) { }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        using (Bitmap bmp = new Bitmap(100, 100))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DeepSkyBlue);
                testure = LoadTexture(bmp);
            }
        }

        GL.ClearColor(Color.White);
        GL.Color4(Color.White);
        GL.Ortho(0, Width, Height, 0, -1, 1);
        GL.Viewport(ClientSize);
        GL.Enable(EnableCap.Texture2D);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        KeyboardState ks = OpenTK.Input.Keyboard.GetState();
        MouseState ms = OpenTK.Input.Mouse.GetCursorState();

        Point p = PointToClient(new Point(ms.X, ms.Y));
        mx = p.X;
        my = p.Y;

        if (ks.IsKeyDown(Key.Escape))
            Exit();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();

        GL.BindTexture(TextureTarget.Texture2D, testure);//
        GL.Begin(PrimitiveType.Quads);
        //GL.Color4(Color.White);

        GL.TexCoord2(0, 0);
        GL.Vertex2(mx, my);

        GL.TexCoord2(1, 0);
        GL.Vertex2(mx + 100, my);

        GL.TexCoord2(1, 1);
        GL.Vertex2(mx + 100, my + 100);

        GL.TexCoord2(0, 1);
        GL.Vertex2(mx, my + 100);

        GL.End();//
                 //GL.BindTexture(TextureTarget.Texture2D, 0);

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
