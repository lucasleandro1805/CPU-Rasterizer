using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Vectors
{
    public struct Vec3
    {
        public float x;
        public float y;
        public float z;
     
        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vec3(Vec2 v, float z)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = z;
        }

        public void Set(Vec3 vec)
        {
            Set(vec.x, vec.y, vec.z);
        }
        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public void Set(float a)
        {
            this.x = a;
            this.y = a;
            this.z = a;
        }

        public float Length()
        {
            return MathF.Sqrt(x*x + y*y + z*z);
        }
        public void Sum(Vec3 other, Vec3 outVec)
        {
            outVec.x = this.x + other.x;
            outVec.y = this.y + other.y;
            outVec.z = this.z + other.z;
        }
        public void Sub(Vec3 other, Vec3 outVec)
        {
            outVec.x = this.x - other.x;
            outVec.y = this.y - other.y;
            outVec.z = this.z - other.z;
        }

        public Vec2 xy()
        {
            return new Vec2(x, y);
        }

        public float Dot(Vec3 vec)
        {
            return x * vec.x + y * vec.y + z * vec.z;
        }

        public static Vec3 Cross(Vec3 a, Vec3 b) 
        {
            return new Vec3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x);
        }

        public static Vec3 operator *(Vec3 w1, Vec3 w2)
        {
            return new Vec3(w1.x * w2.x, w1.y * w2.y, w1.z * w2.z);
        }
        public static Vec3 operator *(Vec3 w1, float w2)
        {
            return new Vec3(w1.x * w2, w1.y * w2, w1.z * w2);
        }

        public static Vec3 operator /(Vec3 w1, Vec3 w2)
        {
            return new Vec3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);
        }
        public static Vec3 operator /(Vec3 w1, float w2)
        {
            return new Vec3(w1.x / w2, w1.y / w2, w1.z / w2);
        }
        public static Vec3 operator +(Vec3 w1, Vec3 w2)
        {
            return new Vec3(w1.x + w2.x, w1.y + w2.y, w1.z + w2.z);
        }
        public static Vec3 operator +(Vec3 w1, float w2)
        {
            return new Vec3(w1.x + w2, w1.y + w2, w1.z + w2);
        }

        public static Vec3 operator -(Vec3 w1, Vec3 w2)
        {
            return new Vec3(w1.x - w2.x, w1.y - w2.y, w1.z - w2.z);
        }
        public static Vec3 operator -(Vec3 w1, float w2)
        {
            return new Vec3(w1.x - w2, w1.y - w2, w1.z - w2);
        }

        public override string ToString()
        {
            return x + ", " + y + ", " + z;
        }
    }
}
