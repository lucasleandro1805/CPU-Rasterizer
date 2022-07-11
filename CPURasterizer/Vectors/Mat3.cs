using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Vectors
{
    public class Mat3
    {
        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

        public void LoadIdentity()
        {
            m01 = m02 = m10 = m12 = m20 = m21 = 0;
            m00 = m11 = m22 = 1;
        }

        public void SetQuaternion(Vec4 q)
        {
            float norm = q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z;
            // we explicitly test norm against one here, saving a division
            // at the cost of a test and branch.  Is it worth it?
            float s = (norm == 1f) ? 2f : (norm > 0f) ? 2f / norm : 0;

            // compute xs/ys/zs first to save 6 multiplications, since xs/ys/zs
            // will be used 2-4 times each.
            float xs = q.x * s;
            float ys = q.y * s;
            float zs = q.z * s;
            float xx = q.x * xs;
            float xy = q.x * ys;
            float xz = q.x * zs;
            float xw = q.w * xs;
            float yy = q.y * ys;
            float yz = q.y * zs;
            float yw = q.w * ys;
            float zz = q.z * zs;
            float zw = q.w * zs;

            // using s=2/norm (instead of 1/norm) saves 9 multiplications by 2 here
            m00 = 1 - (yy + zz);
            m01 = (xy - zw);
            m02 = (xz + yw);
            m10 = (xy + zw);
            m11 = 1 - (xx + zz);
            m12 = (yz - xw);
            m20 = (xz - yw);
            m21 = (yz + xw);
            m22 = 1 - (xx + yy);
        }

        public static Mat3 FromQuaternion(Vec4 q)
        {
            Mat3 m = new Mat3();
            m.SetQuaternion(q);
            return m;
        }
    }
}
