using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Drawer
{
    internal struct TriangleDrawData
    {
        public Vec2 v0, v1, v2;
        public Bounding bounding;
        public Object[] v0Varyings;
        public Object[] v1Varyings;
        public Object[] v2Varyings;
    }
}
