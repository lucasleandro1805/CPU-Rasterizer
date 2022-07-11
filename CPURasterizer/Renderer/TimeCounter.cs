using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Renderer
{
    internal class TimeCounter
    {
        private double startTime;
        private Stopwatch stopWatch = new Stopwatch();
        private double deltaTime;


        public TimeCounter()
        {
            stopWatch.Start();
        }
        public void Start()
        {
            TimeSpan ts = stopWatch.Elapsed;
            startTime = ts.TotalMilliseconds / 1000d;
        }
        public void Finish()
        {
            TimeSpan ts = stopWatch.Elapsed;
            double current = ts.TotalMilliseconds / 1000d;
            deltaTime = current - startTime;
        }

        public double ElapsedTime()
        {
            return deltaTime;
        }
    }
}
