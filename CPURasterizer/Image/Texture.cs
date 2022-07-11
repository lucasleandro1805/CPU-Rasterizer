using CPURasterizer.Utils;
using CPURasterizer.Vectors;
using System.Runtime.InteropServices;

namespace CPURasterizer.Image
{
    public class Texture : IDisposable
    {
        private int width;
        private int height;
        private bool disposed;
        private Bitmap bitmap;
        private Int32[] bits;
        private GCHandle bitsHandle;

        public Texture(int width, int height)
        {
            this.width = width;
            this.height = height;
            bits = new Int32[width * height];
            bitsHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);
            bitmap = new Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, 
                bitsHandle.AddrOfPinnedObject());
        }

        public Vec4 GetPixel(int x, int y)
        {
            return GetPixel(x, y, new Vec4());
        }
        public Vec4 GetPixel(int x, int y, Vec4 output)
        {
            int index = x + (y * width);
            int col = bits[index];
            System.Drawing.Color c = System.Drawing.Color.FromArgb(col);
            output.x = c.R / 255f;
            output.y = c.G / 255f;
            output.z = c.B / 255f;
            output.w = c.A / 255f;
            return output;
        }

        public void SetPixel(int x, int y, Vec4 color)
        {
            int index = x + (y * width);
            bits[index] = ColorUtils.Convert(color);
        }

        public int getIndex(int x, int y)
        {
            int index = x + (y * width);
            return index;
        }

        public int GetWidth()
        {
            return width;
        }
        public int GetHeight()
        {
            return height;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public Int32[] GetBits()
        {
            return bits;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            bitmap.Dispose();
            bitsHandle.Free();
        }
    }
}
