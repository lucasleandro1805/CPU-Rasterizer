using CPURasterizer.Data;
using CPURasterizer.Data.Annotations;
using CPURasterizer.Shader;
using CPURasterizer.Vectors;
using Random = CPURasterizer.Utils.Random;

namespace CPURasterizer.Examples.Cube
{
    internal class CubeFragmentShader : FragmentShader
    {
        [Uniform]
        public Vec4 color;

        [In]
        public Vec3 oColor;
        [In]
        public Vec4 oNormal;
        public override Vec4 Main()
        {
            Vec4 cameraDirection = new Vec4(0, 0, -1, 0.0f);
            float dot = -cameraDirection.Dot(oNormal);
            dot *= dot * 2f;
            if (dot < 0.1f)
            {
                dot = 0.1f;
            }
            return new Vec4(oColor * dot, 1f);
        }
    }
}
