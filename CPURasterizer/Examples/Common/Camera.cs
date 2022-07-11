using CPURasterizer.Utils;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Examples.Common
{
    public class Camera
    {
        public Vec3 position = new Vec3(0,0,0);
        public Vec3 lookDirection = new Vec3(0,0,-1);
        public Vec3 up = new Vec3(0,1,0);
        public float near = 0.5f;
        public float far = 10f;
        public float fov = 45f;
        private float[] frustumM = new float[16];
        private float[] viewM = new float[16];

        public float[] GetFrustum(int w, int h)
        {
            float ratio = (float) w / (float)h;
            Matrix.Perspective(frustumM, 0, fov, ratio, near, far);
            return frustumM;
        }
        public float[] GetView()
        {
            Matrix.SetLookAt(
                    viewM, 0,
                    position,
                    position + lookDirection,
                    up);
            return viewM;
        }
    }
}
