using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Vectors
{
    public struct Vec2
    {
        public float x;
        public float y;

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Set(Vec2 vec)
        {
            Set(vec.x, vec.y);
        }
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float Distance(Vec2 other)
        {
            return Sub(other).Length();
        }
        public float Length()
        {
            return MathF.Sqrt(x * x + y * y);
        }
        public Vec2 Sub(Vec2 other)
        {
            return Sub(other, new Vec2());
        }
        public Vec2 Sub(Vec2 other, Vec2 outVec)
        {
            outVec.x = this.x - other.x;
            outVec.y = this.y - other.y;
            return outVec;
        }

        public static Vec2 operator *(Vec2 w1, Vec2 w2)
        {
            return new Vec2(w1.x * w2.x, w1.y * w2.y);
        }
        public static Vec2 operator *(Vec2 w1, float w2)
        {
            return new Vec2(w1.x * w2, w1.y * w2);
        }

        public static Vec2 operator /(Vec2 w1, Vec2 w2)
        {
            return new Vec2(w1.x / w2.x, w1.y / w2.y);
        }
        public static Vec2 operator /(Vec2 w1, float w2)
        {
            return new Vec2(w1.x / w2, w1.y / w2);
        }

        public static Vec2 operator +(Vec2 w1, Vec2 w2)
        {
            return new Vec2(w1.x + w2.x, w1.y + w2.y);
        }
        public static Vec2 operator +(Vec2 w1, float w2)
        {
            return new Vec2(w1.x + w2, w1.y + w2);
        }
        public static Vec2 operator -(Vec2 w1, Vec2 w2)
        {
            return new Vec2(w1.x - w2.x, w1.y - w2.y);
        }
        public static Vec2 operator -(Vec2 w1, float w2)
        {
            return new Vec2(w1.x - w2, w1.y - w2);
        }

        public override string ToString()
        {
            return x + ", " + y;
        }
    }
}
