using CPURasterizer.Data.VaryingSetters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data
{
    public class Varying
    {
        public int loc;
        public string name;
        public VaryingSetter setter;
        public FieldInfo vertexField;
        public FieldInfo fragmentField;
    }
}
