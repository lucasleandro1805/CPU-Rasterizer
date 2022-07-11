using CPURasterizer.Data;
using CPURasterizer.Data.Annotations;
using CPURasterizer.Data.AttributeSetter;
using CPURasterizer.Data.VaryingSetters;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Shader
{
    public class ProgramCompiler
    {
        public static void Compile(ShaderProgram program)
        {
            LoadVertex(program);
            LoadFragment(program);
        }

        private static void LoadVertex(ShaderProgram program)
        {
            Object script = program.vertex;
            FieldInfo[] fields = script.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttribute(typeof(Uniform)) != null)
                {
                    ShaderUniform uniform = FindUniform(field.Name, program);
                    uniform.vertexField = field;
                }
                else if (field.GetCustomAttribute(typeof(In)) != null)
                {
                    VertexAttribute attribute = FindVertexAttribute(field.Name, program);
                    attribute.field = field;

                    if(field.FieldType == typeof(float))
                    {
                        attribute.size = 1;
                        attribute.setter = new FloatSetter();
                    }
                    else if (field.FieldType == typeof(Vec2))
                    {
                        attribute.size = 2;
                        attribute.setter = new Vec2Setter();
                    }
                    else if (field.FieldType == typeof(Vec3))
                    {
                        attribute.size = 3;
                        attribute.setter = new Vec3Setter();
                    }
                    else if (field.FieldType == typeof(Vec4))
                    {
                        attribute.size = 4;
                        attribute.setter = new Vec4Setter();
                    }
                    else
                    {
                        throw new Exception("Invalid attribute type " + field.FieldType.Name + " at field " + field.Name + " is class " + field.GetType().IsClass);
                    }
                }
                else if (field.GetCustomAttribute(typeof(Out)) != null)
                {
                    Varying varying = FindVarying(field.Name, program);
                    varying.vertexField = field;

                    if (field.FieldType == typeof(float))
                    {
                        varying.setter = new FloatVSetter();
                    }
                    else if (field.FieldType == typeof(Vec2))
                    {
                        varying.setter = new Vec2VSetter();
                    }
                    else if (field.FieldType == typeof(Vec3))
                    {
                        varying.setter = new Vec3VSetter();
                    }
                    else if (field.FieldType == typeof(Vec4))
                    {
                        varying.setter = new Vec4VSetter();
                    }
                    else
                    {
                        throw new Exception("Invalid attribute type " + field.FieldType.Name + " at field " + field.Name + " is class " + field.GetType().IsClass);
                    }
                }
            }
        }
        private static void LoadFragment(ShaderProgram program)
        {
            Object script = program.fragment;
            FieldInfo[] fields = script.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetCustomAttribute(typeof(Uniform)) != null)
                {
                    ShaderUniform uniform = FindUniform(field.Name, program);
                    uniform.fragmentField = field;
                }
                else if (field.GetCustomAttribute(typeof(In)) != null)
                {
                    Varying varying = FindVarying(field.Name, program);
                    varying.fragmentField = field;
                }
            }
        }

        private static Varying FindVarying(string name, ShaderProgram program)
        {
            for (int x = 0; x < program.varyings.Count; x++)
            {
                Varying v = program.varyings[x];
                if (v.name == name)
                {
                    return v;
                }
            }

            Varying v2 = new Varying();
            v2.loc = program.varyings.Count();
            v2.name = name;
            program.varyings.Add(v2);
            return v2;
        }

        private static VertexAttribute FindVertexAttribute(string name, ShaderProgram program)
        {
            for (int x = 0; x < program.vertexAttributes.Count; x++)
            {
                VertexAttribute att = program.vertexAttributes[x];
                if (att.name == name)
                {
                    return att;
                }
            }

            VertexAttribute attribute = new VertexAttribute();
            attribute.loc = program.vertexAttributes.Count();
            attribute.name = name;
            program.vertexAttributes.Add(attribute);
            return attribute;
        }

        private static ShaderUniform FindUniform(string name, ShaderProgram program)
        {
            for(int x = 0; x<program.uniforms.Count; x++)
            {
                ShaderUniform uni = program.uniforms[x];
                if(uni.name == name)
                {
                    return uni;
                }
            }

            ShaderUniform uniform = new ShaderUniform();
            uniform.loc = program.uniforms.Count();
            uniform.name = name;
            program.uniforms.Add(uniform);
            return uniform;
        }
    }
}
