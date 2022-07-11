using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data.AttributeSetter
{
    public class Vec3Setter : TypeSetter
    {
        public override void Set(FieldInfo field, Object obj, float[] data, int initIndex)
        {
            Vec3 v = new Vec3();
            v.x = data[initIndex];
            v.y = data[initIndex+1];
            v.z = data[initIndex+2];
            field.SetValue(obj, v);
        }
    }
}
