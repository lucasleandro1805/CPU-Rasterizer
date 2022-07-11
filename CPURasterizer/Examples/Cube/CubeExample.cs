using CPURasterizer.Examples.Common;
using CPURasterizer.Renderer;
using CPURasterizer.Shader;
using CPURasterizer.Surface;
using CPURasterizer.Utils;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = CPURasterizer.Utils.Random;

namespace CPURasterizer.Examples.Cube
{
    public class CubeExample : CPUSurfaceView
    {
        private readonly CubeMesh cubeMesh = new CubeMesh();
        private readonly Camera camera = new Camera();
        private readonly Transform cubeTransfom = new Transform();

        private readonly ShaderProgram program;
        public CubeExample()
        {
            program = new ShaderProgram(new CubeVertexShader(), new CubeFragmentShader());
        }

        public override void OnFrameRender(CPURenderer gl)
        {
            {;
                camera.position.Set(0, 0, 5f);
                camera.lookDirection.Set(0,0,-1);

                cubeTransfom.scale.Set(0.5f);
                cubeTransfom.Rotate(0, 25f * GetDeltaTime(), 5f * GetDeltaTime());
            }
            {               
                gl.ClearColor(0,0,0,1.0f);
                gl.ClearColorBuffer();
            }

            gl.SetCullFace(CPURenderer.FACE_CULL_BACK);
            gl.BindProgram(program);
            {
                int loc = gl.AttributeLocation("aPosition");
                gl.BindAttributeArray(loc, cubeMesh.vertices);
            }
            {
                int loc = gl.AttributeLocation("aColor");
                gl.BindAttributeArray(loc, cubeMesh.colors);
            }
            {
                int loc = gl.AttributeLocation("aNormal");
                gl.BindAttributeArray(loc, cubeMesh.normals);
            }
            {
                int loc = gl.UniformLocation("color");
                gl.Uniform4f(loc, 1, 0, 1, 1);
            }            
            {
                float[] matrix = camera.GetView();
                int loc = gl.UniformLocation("vMatrix");
                gl.UniformMat4f(loc, matrix);
            }
            {
                float[] matrix = camera.GetFrustum(gl.GetWidth(), gl.GetHeight());
                int loc = gl.UniformLocation("pMatrix");
                gl.UniformMat4f(loc, matrix);
            }
            {
                Mat4 matrix = cubeTransfom.GetMatrix();
                int loc = gl.UniformLocation("mMatrix");
                gl.UniformMat4f(loc, matrix);
            }
            gl.DrawElements(CPURenderer.DRAW_TRIANGLES, cubeMesh.indices);
        }
    }
}
