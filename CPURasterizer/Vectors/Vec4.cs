using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Vectors
{
    public struct Vec4
    {
        public float x;
        public float y;
        public float z;
        public float w;
   
        public Vec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public Vec4(Vec3 vec, float w)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
            this.w = w;
        }

        public void Set(Vec4 vec)
        {
            Set(vec.x, vec.y, vec.z, vec.w);
        }
        public void Set(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float Dot(Vec4 vec)
        {
            return x * vec.x + y * vec.y + z * vec.z + w * vec.w;
        }

        public float Length()
        {
            return MathF.Sqrt(x*x + y*y + z*z + w*w);
        }
        public void Sum(Vec4 other, Vec4 outVec)
        {
            outVec.x = this.x + other.x;
            outVec.y = this.y + other.y;
            outVec.z = this.z + other.z;
            outVec.w = this.w + other.w;
        }
        public void Sub(Vec4 other, Vec4 outVec)
        {
            outVec.x = this.x - other.x;
            outVec.y = this.y - other.y;
            outVec.z = this.z - other.z;
            outVec.w = this.w - other.w;
        }
        
        public Vec2 xy()
        {
            return new Vec2(x, y);
        }
        public Vec3 xyz()
        {
            return new Vec3(x, y, z);
        }

        public static Vec4 operator *(Vec4 w1, Vec4 w2)
        {
            return new Vec4(w1.x * w2.x, w1.y * w2.y, w1.z * w2.z, w1.w * w2.w);
        }
        public static Vec4 operator *(Vec4 w1, float w2)
        {
            return new Vec4(w1.x * w2, w1.y * w2, w1.z * w2, w1.w * w2);
        }

        public static Vec4 operator /(Vec4 w1, Vec4 w2)
        {
            return new Vec4(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z, w1.w / w2.w);
        }
        public static Vec4 operator /(Vec4 w1, float w2)
        {
            return new Vec4(w1.x / w2, w1.y / w2, w1.z / w2, w1.w / w2);
        }

        public static Vec4 operator +(Vec4 w1, Vec4 w2)
        {
            return new Vec4(w1.x + w2.x, w1.y + w2.y, w1.z + w2.z, w1.w + w2.w);
        }
        public static Vec4 operator +(Vec4 w1, float w2)
        {
            return new Vec4(w1.x + w2, w1.y + w2, w1.z + w2, w1.w + w2);
        }

        public static Vec4 operator -(Vec4 w1, Vec4 w2)
        {
            return new Vec4(w1.x - w2.x, w1.y - w2.y, w1.z - w2.z, w1.w - w2.w);
        }
        public static Vec4 operator -(Vec4 w1, float w2)
        {
            return new Vec4(w1.x - w2, w1.y - w2, w1.z - w2, w1.w - w2);
        }
        public override string ToString()
        {
            return x + ", " + y + ", " + z + ", " + w;
        }
    }
}
