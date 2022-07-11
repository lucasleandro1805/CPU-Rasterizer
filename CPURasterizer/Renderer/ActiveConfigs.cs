using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Renderer
{
    internal class ActiveConfigs
    {
        public Vec4 clearColor = new Vec4(0f,0f,0f,1f);
        public int maxFrameRate = 60;
        public bool bruteMode = false;
        public int cullFaceMode = CPURenderer.FACE_CULL_OFF;
    }
}
