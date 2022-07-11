using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data.AttributeSetter
{
    public class FloatSetter : TypeSetter
    {
        public override void Set(FieldInfo field, Object obj, float[] data, int initIndex)
        {
            field.SetValue(obj, data[initIndex]);
        }
    }
}
