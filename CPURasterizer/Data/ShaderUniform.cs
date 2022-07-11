using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data
{
    public class ShaderUniform
    {
        public int loc;
        public string name;
        public Object value;
        public FieldInfo vertexField;
        public FieldInfo fragmentField;
    }
}
