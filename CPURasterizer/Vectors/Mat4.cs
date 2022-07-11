using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Vectors
{
    // Source code adapted from java
    // https://github.com/jMonkeyEngine/jmonkeyengine/blob/master/jme3-core/src/main/java/com/jme3/math/Matrix4f.java   
    public class Mat4
    {       
        public float m00;        
        public float m01;        
        public float m02;        
        public float m03;        
        public float m10;        
        public float m11;        
        public float m12;        
        public float m13;        
        public float m20;        
        public float m21;        
        public float m22;        
        public float m23;        
        public float m30;        
        public float m31;        
        public float m32;       
        public float m33;

        public Mat4()
        {
            loadIdentity();
        }
        public Mat4(float[] m)
        {
            setCollumMajor(m);
        }

        public void setCollumMajor(float[] matrix)
        {
            if (matrix.Length != 16)
            {
                throw new Exception(
                        "Array must be of size 16.");
            }

            m00 = matrix[0];
            m01 = matrix[4];
            m02 = matrix[8];
            m03 = matrix[12];
            m10 = matrix[1];
            m11 = matrix[5];
            m12 = matrix[9];
            m13 = matrix[13];
            m20 = matrix[2];
            m21 = matrix[6];
            m22 = matrix[10];
            m23 = matrix[14];
            m30 = matrix[3];
            m31 = matrix[7];
            m32 = matrix[11];
            m33 = matrix[15];
        }

        public void SetTransform(Vec3 position, Vec3 scale, Mat3 rotMat)
        {
            // Ordering:
            //    1. Scale
            //    2. Rotate
            //    3. Translate

            // Set up final matrix with scale, rotation and translation
            m00 = scale.x * rotMat.m00;
            m01 = scale.y * rotMat.m01;
            m02 = scale.z * rotMat.m02;
            m03 = position.x;
            m10 = scale.x * rotMat.m10;
            m11 = scale.y * rotMat.m11;
            m12 = scale.z * rotMat.m12;
            m13 = position.y;
            m20 = scale.x * rotMat.m20;
            m21 = scale.y * rotMat.m21;
            m22 = scale.z * rotMat.m22;
            m23 = position.z;

            // No projection term
            m30 = 0;
            m31 = 0;
            m32 = 0;
            m33 = 1;
        }

        public static Mat4 operator *(Mat4 m, Mat4 in2)
        {
            float m00 = m.m00;
            float m01 = m.m01;
            float m02 = m.m02;
            float m03 = m.m03;
            float m10 = m.m10;
            float m11 = m.m11;
            float m12 = m.m12;
            float m13 = m.m13;
            float m20 = m.m20;
            float m21 = m.m21;
            float m22 = m.m22;
            float m23 = m.m23;
            float m30 = m.m30;
            float m31 = m.m31;
            float m32 = m.m32;
            float m33 = m.m33;

            Mat4 store = new Mat4();

            store.m00 = m00 * in2.m00
                    + m01 * in2.m10
                    + m02 * in2.m20
                    + m03 * in2.m30;
            store.m01 = m00 * in2.m01
                    + m01 * in2.m11
                    + m02 * in2.m21
                    + m03 * in2.m31;
            store.m02 = m00 * in2.m02
                    + m01 * in2.m12
                    + m02 * in2.m22
                    + m03 * in2.m32;
            store.m03 = m00 * in2.m03
                    + m01 * in2.m13
                    + m02 * in2.m23
                    + m03 * in2.m33;

            store.m10 = m10 * in2.m00
                    + m11 * in2.m10
                    + m12 * in2.m20
                    + m13 * in2.m30;
            store.m11 = m10 * in2.m01
                    + m11 * in2.m11
                    + m12 * in2.m21
                    + m13 * in2.m31;
            store.m12 = m10 * in2.m02
                    + m11 * in2.m12
                    + m12 * in2.m22
                    + m13 * in2.m32;
            store.m13 = m10 * in2.m03
                    + m11 * in2.m13
                    + m12 * in2.m23
                    + m13 * in2.m33;

            store.m20 = m20 * in2.m00
                    + m21 * in2.m10
                    + m22 * in2.m20
                    + m23 * in2.m30;
            store.m21 = m20 * in2.m01
                    + m21 * in2.m11
                    + m22 * in2.m21
                    + m23 * in2.m31;
            store.m22 = m20 * in2.m02
                    + m21 * in2.m12
                    + m22 * in2.m22
                    + m23 * in2.m32;
            store.m23 = m20 * in2.m03
                    + m21 * in2.m13
                    + m22 * in2.m23
                    + m23 * in2.m33;

            store.m30 = m30 * in2.m00
                    + m31 * in2.m10
                    + m32 * in2.m20
                    + m33 * in2.m30;
            store.m31 = m30 * in2.m01
                    + m31 * in2.m11
                    + m32 * in2.m21
                    + m33 * in2.m31;
            store.m32 = m30 * in2.m02
                    + m31 * in2.m12
                    + m32 * in2.m22
                    + m33 * in2.m32;
            store.m33 = m30 * in2.m03
                    + m31 * in2.m13
                    + m32 * in2.m23
                    + m33 * in2.m33;
            return store;
        }
        public static Vec3 operator *(Mat4 m, Vec3 vec)
        {
            float m00 = m.m00;
            float m01 = m.m01;
            float m02 = m.m02;
            float m03 = m.m03;
            float m10 = m.m10;
            float m11 = m.m11;
            float m12 = m.m12;
            float m13 = m.m13;
            float m20 = m.m20;
            float m21 = m.m21;
            float m22 = m.m22;
            float m23 = m.m23;
            float m30 = m.m30;
            float m31 = m.m31;
            float m32 = m.m32;
            float m33 = m.m33;

            Vec3 store = new Vec3();
            float vx = vec.x, vy = vec.y, vz = vec.z;
            store.x = m00 * vx + m01 * vy + m02 * vz + m03;
            store.y = m10 * vx + m11 * vy + m12 * vz + m13;
            store.z = m20 * vx + m21 * vy + m22 * vz + m23;
            return store;
        }
        public static Vec4 operator *(Mat4 m, Vec4 vec)
        {
            float m00 = m.m00;
            float m01 = m.m01;
            float m02 = m.m02;
            float m03 = m.m03;
            float m10 = m.m10;
            float m11 = m.m11;
            float m12 = m.m12;
            float m13 = m.m13;
            float m20 = m.m20;
            float m21 = m.m21;
            float m22 = m.m22;
            float m23 = m.m23;
            float m30 = m.m30;
            float m31 = m.m31;
            float m32 = m.m32;
            float m33 = m.m33;

            Vec4 store = new Vec4();
            float vx = vec.x, vy = vec.y, vz = vec.z, vw = vec.w;
            store.x = m00 * vx + m01 * vy + m02 * vz + m03 * vw;
            store.y = m10 * vx + m11 * vy + m12 * vz + m13 * vw;
            store.z = m20 * vx + m21 * vy + m22 * vz + m23 * vw;
            store.w = m30 * vx + m31 * vy + m32 * vz + m33 * vw;
            return store;
        }

        public void loadIdentity()
        {
            m01 = m02 = m03 = 0.0f;
            m10 = m12 = m13 = 0.0f;
            m20 = m21 = m23 = 0.0f;
            m30 = m31 = m32 = 0.0f;
            m00 = m11 = m22 = m33 = 1.0f;
        }
    }
}
