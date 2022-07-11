using CPURasterizer.Data;
using CPURasterizer.Data.Annotations;
using CPURasterizer.Drawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Shader
{
    public class ShaderProgram
    {
        public readonly ScreenDrawer drawer = new ScreenDrawer();
        public readonly VertexShader vertex;
        public readonly FragmentShader fragment;

        public readonly List<ShaderUniform> uniforms = new List<ShaderUniform>();
        public readonly List<VertexAttribute> vertexAttributes = new List<VertexAttribute>();
        public readonly List<Varying> varyings = new List<Varying>();
       
        public ShaderProgram(VertexShader vertex, FragmentShader fragment)
        {
            this.vertex = vertex;
            this.fragment = fragment;
            ProgramCompiler.Compile(this);
        }

    }
}
