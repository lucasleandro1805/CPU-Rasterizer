using CPURasterizer.Data.AttributeSetter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data
{
    public class VertexAttribute
    {
        public int loc;
        public string name;
        public float[] value;
        public int size;
        public FieldInfo field;
        public TypeSetter setter;
    }
}
