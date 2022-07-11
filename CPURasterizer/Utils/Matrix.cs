using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Utils
{
    // Source from adapted from java
    // https://android.googlesource.com/platform/frameworks/base/+/6d2c0e5/opengl/java/android/opengl/Matrix.java
    public class Matrix
    {

        /**
         * Computes an orthographic projection matrix.
         *
         * @param m returns the result
         * @param mOffset
         * @param left
         * @param right
         * @param bottom
         * @param top
         * @param near
         * @param far
         */
        public static void Ortho(float[] m, int mOffset,
            float left, float right, float bottom, float top,
            float near, float far)
        {
            if (left == right)
            {
                throw new Exception("left == right");
            }
            if (bottom == top)
            {
                throw new Exception("bottom == top");
            }
            if (near == far)
            {
                throw new Exception("near == far");
            }
            float r_width = 1.0f / (right - left);
            float r_height = 1.0f / (top - bottom);
            float r_depth = 1.0f / (far - near);
            float x = 2.0f * (r_width);
            float y = 2.0f * (r_height);
            float z = -2.0f * (r_depth);
            float tx = -(right + left) * r_width;
            float ty = -(top + bottom) * r_height;
            float tz = -(far + near) * r_depth;
            m[mOffset + 0] = x;
            m[mOffset + 5] = y;
            m[mOffset + 10] = z;
            m[mOffset + 12] = tx;
            m[mOffset + 13] = ty;
            m[mOffset + 14] = tz;
            m[mOffset + 15] = 1.0f;
            m[mOffset + 1] = 0.0f;
            m[mOffset + 2] = 0.0f;
            m[mOffset + 3] = 0.0f;
            m[mOffset + 4] = 0.0f;
            m[mOffset + 6] = 0.0f;
            m[mOffset + 7] = 0.0f;
            m[mOffset + 8] = 0.0f;
            m[mOffset + 9] = 0.0f;
            m[mOffset + 11] = 0.0f;
        }
        /**
         * Defines a projection matrix in terms of six clip planes.
         *
         * @param m the float array that holds the output perspective matrix
         * @param offset the offset into float array m where the perspective
         *        matrix data is written
         * @param left
         * @param right
         * @param bottom
         * @param top
         * @param near
         * @param far
         */
        public static void Frustum(float[] m, int offset,
                float left, float right, float bottom, float top,
                float near, float far)
        {
            if (left == right)
            {
                throw new Exception("left == right");
            }
            if (top == bottom)
            {
                throw new Exception("top == bottom");
            }
            if (near == far)
            {
                throw new Exception("near == far");
            }
            if (near <= 0.0f)
            {
                throw new Exception("near <= 0.0f");
            }
            if (far <= 0.0f)
            {
                throw new Exception("far <= 0.0f");
            }
            float r_width = 1.0f / (right - left);
            float r_height = 1.0f / (top - bottom);
            float r_depth = 1.0f / (near - far);
            float x = 2.0f * (near * r_width);
            float y = 2.0f * (near * r_height);
            float A = (right + left) * r_width;
            float B = (top + bottom) * r_height;
            float C = (far + near) * r_depth;
            float D = 2.0f * (far * near * r_depth);
            m[offset + 0] = x;
            m[offset + 5] = y;
            m[offset + 8] = A;
            m[offset + 9] = B;
            m[offset + 10] = C;
            m[offset + 14] = D;
            m[offset + 11] = -1.0f;
            m[offset + 1] = 0.0f;
            m[offset + 2] = 0.0f;
            m[offset + 3] = 0.0f;
            m[offset + 4] = 0.0f;
            m[offset + 6] = 0.0f;
            m[offset + 7] = 0.0f;
            m[offset + 12] = 0.0f;
            m[offset + 13] = 0.0f;
            m[offset + 15] = 0.0f;
        }
        /**
         * Defines a projection matrix in terms of a field of view angle, an
         * aspect ratio, and z clip planes.
         *
         * @param m the float array that holds the perspective matrix
         * @param offset the offset into float array m where the perspective
         *        matrix data is written
         * @param fovy field of view in y direction, in degrees
         * @param aspect width to height aspect ratio of the viewport
         * @param zNear
         * @param zFar
         */
        public static void Perspective(float[] m, int offset,
              float fovy, float aspect, float zNear, float zFar)
        {
            float f = 1.0f / (float)Math.Tan(fovy * (Math.PI / 360.0));
            float rangeReciprocal = 1.0f / (zNear - zFar);
            m[offset + 0] = f / aspect;
            m[offset + 1] = 0.0f;
            m[offset + 2] = 0.0f;
            m[offset + 3] = 0.0f;
            m[offset + 4] = 0.0f;
            m[offset + 5] = f;
            m[offset + 6] = 0.0f;
            m[offset + 7] = 0.0f;
            m[offset + 8] = 0.0f;
            m[offset + 9] = 0.0f;
            m[offset + 10] = (zFar + zNear) * rangeReciprocal;
            m[offset + 11] = -1.0f;
            m[offset + 12] = 0.0f;
            m[offset + 13] = 0.0f;
            m[offset + 14] = 2.0f * zFar * zNear * rangeReciprocal;
            m[offset + 15] = 0.0f;
        }

        public static float Length(float x, float y, float z)
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public static void SetLookAt(float[] rm, int rmOffset, Vec3 eye, Vec3 center, Vec3 up)
        {
            SetLookAt(rm, rmOffset, eye.x, eye.y, eye.z, center.x, center.y, center.z, up.x, up.y, up.z);
        }
        /**
         * Defines a viewing transformation in terms of an eye point, a center of
         * view, and an up vector.
         *
         * @param rm returns the result
         * @param rmOffset index into rm where the result matrix starts
         * @param eyeX eye point X
         * @param eyeY eye point Y
         * @param eyeZ eye point Z
         * @param centerX center of view X
         * @param centerY center of view Y
         * @param centerZ center of view Z
         * @param upX up vector X
         * @param upY up vector Y
         * @param upZ up vector Z
         */
        public static void SetLookAt(float[] rm, int rmOffset,
                float eyeX, float eyeY, float eyeZ,
                float centerX, float centerY, float centerZ, float upX, float upY,
                float upZ)
        {
            // See the OpenGL GLUT documentation for gluLookAt for a description
            // of the algorithm. We implement it in a straightforward way:
            float fx = centerX - eyeX;
            float fy = centerY - eyeY;
            float fz = centerZ - eyeZ;
            // Normalize f
            float rlf = 1.0f / Length(fx, fy, fz);
            fx *= rlf;
            fy *= rlf;
            fz *= rlf;
            // compute s = f x up (x means "cross product")
            float sx = fy * upZ - fz * upY;
            float sy = fz * upX - fx * upZ;
            float sz = fx * upY - fy * upX;
            // and normalize s
            float rls = 1.0f / Length(sx, sy, sz);
            sx *= rls;
            sy *= rls;
            sz *= rls;
            // compute u = s x f
            float ux = sy * fz - sz * fy;
            float uy = sz * fx - sx * fz;
            float uz = sx * fy - sy * fx;
            rm[rmOffset + 0] = sx;
            rm[rmOffset + 1] = ux;
            rm[rmOffset + 2] = -fx;
            rm[rmOffset + 3] = 0.0f;
            rm[rmOffset + 4] = sy;
            rm[rmOffset + 5] = uy;
            rm[rmOffset + 6] = -fy;
            rm[rmOffset + 7] = 0.0f;
            rm[rmOffset + 8] = sz;
            rm[rmOffset + 9] = uz;
            rm[rmOffset + 10] = -fz;
            rm[rmOffset + 11] = 0.0f;
            rm[rmOffset + 12] = 0.0f;
            rm[rmOffset + 13] = 0.0f;
            rm[rmOffset + 14] = 0.0f;
            rm[rmOffset + 15] = 1.0f;
            Translate(rm, rmOffset, -eyeX, -eyeY, -eyeZ);
        }

        /**
         * Translates matrix m by x, y, and z in place.
         *
         * @param m matrix
         * @param mOffset index into m where the matrix starts
         * @param x translation factor x
         * @param y translation factor y
         * @param z translation factor z
         */
        public static void Translate(
                float[] m, int mOffset,
                float x, float y, float z)
        {
            for (int i = 0; i < 4; i++)
            {
                int mi = mOffset + i;
                m[12 + mi] += m[mi] * x + m[4 + mi] * y + m[8 + mi] * z;
            }
        }


        /**
         * Translates matrix m by x, y, and z, putting the result in tm.
         * <p>
         * m and tm must not overlap.
         *
         * @param tm returns the result
         * @param tmOffset index into sm where the result matrix starts
         * @param m source matrix
         * @param mOffset index into m where the source matrix starts
         * @param x translation factor x
         * @param y translation factor y
         * @param z translation factor z
         */
        public static void Translate(float[] tm, int tmOffset,
                float[] m, int mOffset,
                float x, float y, float z)
        {
            for (int i = 0; i < 12; i++)
            {
                tm[tmOffset + i] = m[mOffset + i];
            }
            for (int i = 0; i < 4; i++)
            {
                int tmi = tmOffset + i;
                int mi = mOffset + i;
                tm[12 + tmi] = m[mi] * x + m[4 + mi] * y + m[8 + mi] * z +
                    m[12 + mi];
            }
        }
    }
}
