using CPURasterizer.Data;
using CPURasterizer.Data.Annotations;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Shader.ClearShaders
{
    internal class ClearColorVertexShader: VertexShader
    {
        [In]
        public Vec4 aPosition;

        public override Vec4 Main()
        {
            return new Vec4();
        }
    }
}
