using CPURasterizer.Data;
using CPURasterizer.Data.Annotations;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = CPURasterizer.Utils.Random;

namespace CPURasterizer.Shader.ClearShaders
{
    internal class CubeFragmentShader : FragmentShader
    {
        [Uniform]
        public Vec4 color;
        public override Vec4 Main()
        {
            return color;
        }
    }
}
