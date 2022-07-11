using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Examples.Common
{
    public class Transform
    {
        public Vec3 position = new Vec3();
        public Vec3 scale = new Vec3(1,1,1);
        public Vec4 rotation = new Vec4(0,0,0,1);


        public void Rotate(float x, float y, float z)
        {
            if (x != 0)
            {
                rotation = QuaternionUtils.Mul(rotation, QuaternionUtils.CreateFromAxisAngle(
                            1,
                            0,
                            0,
                            x
                    ));
            }

            if (y != 0)
            {
                rotation = QuaternionUtils.Mul(rotation, QuaternionUtils.CreateFromAxisAngle(
                            0,
                            1,
                            0,
                            y
                    ));
            }

            if (z != 0)
            {
                rotation = QuaternionUtils.Mul(rotation, QuaternionUtils.CreateFromAxisAngle(
                            0,
                            0,
                            1,
                            z
                    ));
            }
        }

        

        public Mat4 GetMatrix()
        {
            Mat4 m = new Mat4();
            m.SetTransform(position, scale, Mat3.FromQuaternion(rotation));
            return m;
        }
    }
}
