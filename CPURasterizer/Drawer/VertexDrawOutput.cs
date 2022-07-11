using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Drawer
{
    public class VertexDrawOutput
    {
        public Vec4 position;
        public int index;
        public bool drawed = false;
        public Object[] varyings;
        public readonly Object lockRender = new object();
    }
}
