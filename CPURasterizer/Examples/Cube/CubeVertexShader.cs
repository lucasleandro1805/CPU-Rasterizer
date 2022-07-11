using CPURasterizer.Shader;
using CPURasterizer.Vectors;
using CPURasterizer.Data.Annotations;

namespace CPURasterizer.Examples.Cube
{
    public class CubeVertexShader : VertexShader
    {
        [Uniform]
        public Mat4 pMatrix;
        [Uniform]
        public Mat4 vMatrix;
        [Uniform]
        public Mat4 mMatrix;

        [In]
        public Vec3 aPosition;
        [In]
        public Vec3 aColor;
        [In]
        public Vec3 aNormal;

        [Out]
        public Vec3 oColor;
        [Out]
        public Vec4 oNormal;

        public override Vec4 Main()
        {           
            Vec4 n4 = new Vec4(aNormal, 0.0f);
            n4 = mMatrix * n4;
            oNormal = n4;
            oColor = aColor;
            
            Vec4 position = new Vec4(aPosition, 1.0f);
            return pMatrix * vMatrix * mMatrix * position;
        }
    }
}
