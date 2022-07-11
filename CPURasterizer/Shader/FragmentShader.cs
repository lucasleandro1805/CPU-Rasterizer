using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Shader
{
    public class FragmentShader
    {
        public virtual Vec4 Main()
        {
            return new Vec4(0,0,0,1);
        }
    }
}
