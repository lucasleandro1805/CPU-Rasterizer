using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Drawer
{
    public class CullFace
    {       
        public virtual bool Cull(float z1, float z2, float z3)
        {
            return false;
        }
    }    

    public class CullFaceOff : CullFace
    {
        public override bool Cull(float z1, float z2, float z3)
        {
            return ((z1 >= 0 && z2 >= 0 && z3 >= 0) || (z1 <= 0 && z2 <= 0 && z3 <= 0));
        }
    }

    public class CullFaceFront : CullFace
    {
        public override bool Cull(float z1, float z2, float z3)
        {
            return (z1 <= 0 && z2 <= 0 && z3 <= 0);
        }
    }

    public class CullFaceBack : CullFace
    {
        public override bool Cull(float z1, float z2, float z3)
        {
            return (z1 >= 0 && z2 >= 0 && z3 >= 0);
        }
    }
}
