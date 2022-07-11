using CPURasterizer.Shader;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data.VaryingSetters
{
    public class FloatVSetter : VaryingSetter
    {
        public override void Set(FieldInfo field, FragmentShader fs, object ob0, object ob1, object ob2, float w0, float w1, float w2)
        {
            float v0 = (float)ob0;
            float v1 = (float)ob1;
            float v2 = (float)ob2;

            float vo = (v0 * w0) + (v1 * w1) + (v2 * w2);
            field.SetValue(fs, vo);
        }
    }
}
