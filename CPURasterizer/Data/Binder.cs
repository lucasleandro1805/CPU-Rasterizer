using CPURasterizer.Drawer;
using CPURasterizer.Shader;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Data
{
    internal class Binder
    {
        public static int AttributeLocation(ShaderProgram program, string name)
        {
            for (int x = 0; x < program.vertexAttributes.Count; x++)
            {
                VertexAttribute attribute = program.vertexAttributes[x];
                if (attribute.name == name)
                {
                    return attribute.loc;
                }
            }
            return -1;
        }

        public static void BindAttributeArray(ShaderProgram program, int loc, float[] value)
        {
            ValidateAttributeLoc(program, loc);
            VertexAttribute attribute = program.vertexAttributes[loc];
            attribute.value = value;
        }

        private static void ValidateAttributeLoc(ShaderProgram program, int loc)
        {
            if (program.vertexAttributes.Count() <= loc || loc < 0)
            {
                throw new Exception("Invalid location " + loc + " max:" + program.vertexAttributes.Count());
            }
        }

        public static int UniformLocation(ShaderProgram program, string name)
        {
            for (int x = 0; x < program.uniforms.Count; x++)
            {
                ShaderUniform uniform = program.uniforms[x];
                if(uniform.name == name)
                {
                    return uniform.loc;
                }
            }
            return -1;
        }

        public static void UniformMat4f(ShaderProgram program, int loc, Mat4 value)
        {
            ValidateUniformLoc(program, loc);
            ShaderUniform uniform = program.uniforms[loc];
            uniform.value = value;
        }
        public static void UniformMat3f(ShaderProgram program, int loc, Mat3 value)
        {
            ValidateUniformLoc(program, loc);
            ShaderUniform uniform = program.uniforms[loc];
            uniform.value = value;
        }
        public static void Uniform4f(ShaderProgram program, int loc, Vec4 value)
        {
            ValidateUniformLoc(program, loc);
            ShaderUniform uniform = program.uniforms[loc];
            uniform.value = value;
        }
        public static void Uniform3f(ShaderProgram program, int loc, Vec3 value)
        {
            ValidateUniformLoc(program, loc);
            ShaderUniform uniform = program.uniforms[loc];
            uniform.value = value;
        }
        public static void Uniform2f(ShaderProgram program, int loc, Vec2 value)
        {
            ValidateUniformLoc(program, loc);
            ShaderUniform uniform = program.uniforms[loc];
            uniform.value = value;
        }

        private static void ValidateUniformLoc(ShaderProgram program, int loc)
        {
            if (program.uniforms.Count() <= loc || loc < 0)
            {
                throw new Exception("Invalid location " + loc + " max:" + program.uniforms.Count());
            }
        }

        public static void PrepareRender(VertexShader script, ShaderProgram program, VertexDrawOutput data)
        {
            for (int x = 0; x < program.uniforms.Count; x++)
            {
                ShaderUniform uniform = program.uniforms[x];
                if(uniform.value != null && uniform.vertexField != null)
                {
                    uniform.vertexField.SetValue(script, uniform.value);
                }
            }
            for (int x = 0; x < program.vertexAttributes.Count; x++)
            {
                VertexAttribute attribute = program.vertexAttributes[x];
                try
                {
                    attribute.setter.Set(attribute.field, script, attribute.value, data.index * attribute.size);
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.WriteLine("Invalid setter index (data index:" + data.index + ") (size:" + attribute.size+")");
                }
            }
        }
        public static void PrepareRender(FragmentShader script, ShaderProgram program)
        {
            for (int x = 0; x < program.uniforms.Count; x++)
            {
                ShaderUniform uniform = program.uniforms[x];
                if (uniform.value != null && uniform.fragmentField != null)
                {
                    uniform.fragmentField.SetValue(script, uniform.value);
                }
            }            
        }

        public static void PrepareVaryings(FragmentShader script, ShaderProgram program, TriangleDrawData data, float w0, float w1, float w2)
        {
            for (int x = 0; x < program.varyings.Count; x++)
            {
                Varying varing = program.varyings[x];
                if (varing.vertexField != null && varing.fragmentField != null)
                {
                    Object ob0 = data.v0Varyings[varing.loc];
                    Object ob1 = data.v1Varyings[varing.loc];
                    Object ob2 = data.v2Varyings[varing.loc];
                    varing.setter.Set(varing.fragmentField, script, ob0, ob1, ob2, w0, w1, w2);
                }
            }
        }
    }
}
