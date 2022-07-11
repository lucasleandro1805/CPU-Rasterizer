using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Vectors
{
    public struct Bounding
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;
   
        public Bounding(int minX, int minY, int maxX, int maxY)
        {
            this.minX = minX;
            this.minY = minY;

            this.maxX = maxX;
            this.maxY = maxY;
        }

        public bool IsInside(int x, int y)
        {
            if(x >= minX && x <= maxX)
            {
                if(y >= minY && y <= maxY)
                {
                    return true;
                }
            }            
            return false;
        }

        public override string ToString()
        {
            return "x [" + minX + ":" + maxX + "] y [" + minY + ", " + maxY + "]";
        }
    }
}
