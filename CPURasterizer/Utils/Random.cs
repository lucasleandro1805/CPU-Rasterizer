using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Utils
{
    internal class Random
    {
        public static float Range(float min, float max)
        {
            System.Random random = new System.Random();
            double val = random.NextDouble() * (max - min) + min;
            return (float)val;
        }
    }
}
