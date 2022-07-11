using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Examples.Common
{
    internal class QuaternionUtils
    {
        public static Vec4 Mul(Vec4 a, Vec4 q1)
        {
            float x, y, w;

            x = a.w * q1.x + q1.w * a.x + a.y * q1.z - a.z * q1.y;
            y = a.w * q1.y + q1.w * a.y - a.x * q1.z + a.z * q1.x;
            w = a.w * q1.w - a.x * q1.x - a.y * q1.y - a.z * q1.z;
            a.z = a.w * q1.z + q1.w * a.z + a.x * q1.y - a.y * q1.x;
            a.w = w;
            a.x = x;
            a.y = y;
            return a;
        }
        public static float AngleTo360Radian(float ze)
        {
            float i = ze;
            if (ze < 0)
            {
                float div = ze / 360;
                int inte = (int)Math.Ceiling(div);
                ze = (div - inte) * 360;
                if (ze < 0)
                {
                    ze += 360;
                }
                if (ze > 180)
                {
                    ze -= 360;
                }
            }
            else if (ze > 0)
            {
                float div = ze / 360;
                int inte = (int)Math.Floor(div);
                ze = (div - inte) * 360;
                if (ze < 0)
                {
                    ze += 360;
                }
                if (ze > 180)
                {
                    ze -= 360;
                }
            }
            return ToRadians(ze);
        }
        public static float ToRadians(float degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (float)radians;
        }

        public static Vec4 CreateFromAxisAngle(float xx, float yy, float zz, float degress)
        {
            float radians = AngleTo360Radian(degress);

            // Here we calculate the sin( theta / 2) once for optimization
            float factor = (float)Math.Sin(radians / 2.0);

            // Calculate the x, y and z of the quaternion
            float x = xx * factor;
            float y = yy * factor;
            float z = zz * factor;

            // Calculate the w value by cos( theta / 2 )
            float w = (float)Math.Cos(radians / 2.0);

            Vec4 output = new Vec4();
            output.x = x;
            output.y = y;
            output.z = z;
            output.w = w;
            return Normalize(output);
        }

        public static Vec4 Normalize(Vec4 v)
        {
            double norm = Math.Sqrt(
                v.w * v.w +
                v.x * v.y +
                v.y * v.y +
                v.z * v.z);

            v.w = (float)(v.w / norm);
            v.x = (float)(v.x / norm);
            v.y = (float)(v.y / norm);
            v.z = (float)(v.z / norm);
            return v;
        }

    }
}
