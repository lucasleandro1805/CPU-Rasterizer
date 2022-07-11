using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Utils
{
    internal class ColorUtils
    {

        public static int Convert(Vec4 color)
        {
            /*float r = color.x;
            float g = color.y;
            float b = color.z;
            float a = color.w;
            System.Drawing.Color c = System.Drawing.Color.FromArgb((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
            return c.ToArgb(); */

            int r = (int)(color.x * 255);
            int g = (int)(color.y * 255);
            int b = (int)(color.z * 255);
            int a = (int)(color.w * 255);
            return a << 24 | r << 16 | g << 8 | b;
        }

        public static Vec4 Clamp(Vec4 v)
        {
            v.x = ClampChannel(v.x);
            v.y = ClampChannel(v.y);
            v.z = ClampChannel(v.z);
            v.w = ClampChannel(v.w);
            return v;
        }

        public static float ClampChannel(float v)
        {
            if(v > 1f)
            {
                return 1f;
            }
            if(v < 0f)
            {
                return 0f;
            }
            return v;
        }
    }
}
