using CPURasterizer.Data;
using CPURasterizer.Image;
using CPURasterizer.Renderer;
using CPURasterizer.Shader;
using CPURasterizer.Utils;
using CPURasterizer.Vectors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = CPURasterizer.Utils.Random;

namespace CPURasterizer.Drawer
{
    public class ScreenDrawer
    {        
        private ConcurrentBag<VertexShader> vertexShaders = new ConcurrentBag<VertexShader>();
        private ConcurrentBag<FragmentShader> fragmentShaders = new ConcurrentBag<FragmentShader>();
        private List<VertexDrawOutput> vertexDrawOutputs = new List<VertexDrawOutput>();
        private readonly List<TriangleDrawData> triangleDrawDatas = new List<TriangleDrawData>();
        private readonly List<TriangleDrawData> unusedTriangleDrawDatas = new List<TriangleDrawData>();
        private ShaderProgram lastRenderProgram;
        private readonly CullFace cullFaceFront = new CullFaceFront();
        private readonly CullFace cullFaceBack = new CullFaceBack();
        private readonly CullFace cullFaceOff = new CullFaceOff();
        private readonly Object lockTriangleArray = new Object();

        public ScreenDrawer()
        {
            
        }

        public void DrawElements(int type, int faceCull, int[] triangles, Texture output, ShaderProgram program)
        {
            Int32[] pixels = output.GetBits();
            int w = output.GetWidth();
            int h = output.GetHeight();

            CullFace cullMethod = null;
            if(faceCull == CPURenderer.FACE_CULL_BACK)
            {
                cullMethod = cullFaceBack;
            }
            else if(faceCull == CPURenderer.FACE_CULL_FRONT){
                cullMethod = cullFaceFront;
            } 
            else {
                cullMethod = cullFaceOff;
            }


            PrepareProgram(program);            

            Parallel.For(0, (triangles.Length/3),
                triIndex =>
                {
                    int v0, v1, v2;
                    lock (lockTriangleArray)
                    {
                        v0 = triangles[triIndex * 3];
                        v1 = triangles[triIndex * 3 + 1];
                        v2 = triangles[triIndex * 3 + 2];
                    }

                    VertexDrawOutput dv0 = FindVertexOutput(v0);
                    VertexDrawOutput dv1 = FindVertexOutput(v1);
                    VertexDrawOutput dv2 = FindVertexOutput(v2);

                    DrawVertex(dv0, program);
                    DrawVertex(dv1, program);
                    DrawVertex(dv2, program);

                    Vec2 upSide = new Vec2(1, -1);
                    Vec2 pv0 = upSide * (dv0.position.xy() / (dv0.position.w * 0.5f + 0.5f));
                    Vec2 pv1 = upSide * (dv1.position.xy() / (dv1.position.w * 0.5f + 0.5f));
                    Vec2 pv2 = upSide * (dv2.position.xy() / (dv2.position.w * 0.5f + 0.5f));

                    //Debug.WriteLine("V0 " + dv0.position.ToString());
                    //Debug.WriteLine("V1 " + pv1.ToString());
                    //Debug.WriteLine("V2 " + pv2.ToString());

                    Bounding bound = CalculateBounding(
                        PositionToScreenCoords(pv0, w, h),
                        PositionToScreenCoords(pv1, w, h),
                        PositionToScreenCoords(pv2, w, h));

                    TriangleDrawData triangleData = RequestTriangleData();
                    triangleData.v0 = pv0;
                    triangleData.v1 = pv1;
                    triangleData.v2 = pv2;
                    triangleData.bounding = bound;

                    triangleData.v0Varyings = new Object[program.varyings.Count];
                    triangleData.v1Varyings = new Object[program.varyings.Count];
                    triangleData.v2Varyings = new Object[program.varyings.Count];
                    for (int x = 0; x < program.varyings.Count; x++)
                    {
                        Varying varing = program.varyings[x];
                        if (varing.vertexField != null && varing.fragmentField != null)
                        {
                            triangleData.v0Varyings[varing.loc] = dv0.varyings[varing.loc];
                            triangleData.v1Varyings[varing.loc] = dv1.varyings[varing.loc];
                            triangleData.v2Varyings[varing.loc] = dv2.varyings[varing.loc];
                        }
                    }

                    lock (triangleDrawDatas)
                    {
                        triangleDrawDatas.Add(triangleData);
                    }
                });

            for(int t = 0; t < triangleDrawDatas.Count; t++)
            {
                TriangleDrawData data = triangleDrawDatas[t];
                Bounding bound = data.bounding;

                //Debug.WriteLine("Bounds " + bound.ToString());

                int firstPixelIndex = output.getIndex(data.bounding.minX, data.bounding.minY);
                int lastPixelIndex = output.getIndex(data.bounding.maxX, data.bounding.maxY);

                if(firstPixelIndex < 0)
                {
                    firstPixelIndex = 0;
                }
                if (lastPixelIndex < 0)
                {
                    lastPixelIndex = 0;
                }
                if(lastPixelIndex > pixels.Length)
                {
                    lastPixelIndex = pixels.Length;
                }
                Parallel.For(firstPixelIndex, lastPixelIndex,
                   index => {
                       if (index < pixels.Length && index >= 0)
                       {
                           float yf = (float)index / (float)output.GetWidth();
                           int y = (int)MathF.Floor(yf);
                           float xf = yf - y;
                           int x = (int)(xf * (float)output.GetWidth());

                           if (bound.IsInside(x, y))
                           {
                               Vec2 p = new Vec2();
                               p.x = (((float)x / (float)w) * 2.0f) - 1.0f;
                               p.y = (((float)y / (float)h) * 2.0f) - 1.0f;
                               float z1 = RasterizeEdge(p, data.v1, data.v2);
                               float z2 = RasterizeEdge(p, data.v2, data.v0);
                               float z3 = RasterizeEdge(p, data.v0, data.v1);

                               if (cullMethod.Cull(z1, z2, z3))
                               {
                                   float totalWeight = 0;

                                   float dis0 = p.Distance(data.v0);
                                   float dis1 = p.Distance(data.v1);
                                   float dis2 = p.Distance(data.v2);
                                   float max = Math.Max(dis0, Math.Max(dis1, dis2));
                                   dis0 = max - dis0;
                                   dis1 = max - dis1;
                                   dis2 = max - dis2;

                                   totalWeight += dis0 + dis1 + dis2;
                                   float w0 = dis0 / totalWeight;
                                   float w1 = dis1 / totalWeight;
                                   float w2 = dis2 / totalWeight;

                                   FragmentShader fs = RequestFragment(program);
                                   Binder.PrepareVaryings(fs, program, data, w0, w1, w2);
                                   Vec4 color = ColorUtils.Clamp(fs.Main());
                                   pixels[index] = ColorUtils.Convert(color);
                                   ReleaseFragment(fs);
                               }
                           }
                       }
                   });
            }

            
            vertexDrawOutputs.Clear();
            unusedTriangleDrawDatas.Clear();
            unusedTriangleDrawDatas.AddRange(triangleDrawDatas);
            triangleDrawDatas.Clear();
        }

        private TriangleDrawData RequestTriangleData()
        {
            lock (unusedTriangleDrawDatas)
            {
                if(unusedTriangleDrawDatas.Count > 0)
                {
                    TriangleDrawData data = unusedTriangleDrawDatas[0];
                    unusedTriangleDrawDatas.RemoveAt(0);
                    return data;
                }
            }
            TriangleDrawData d = new TriangleDrawData();
            return d;
        }

        private float RasterizeEdge(Vec2 c, Vec2 a, Vec2 b)
        {
            return (c.x - a.x) * (b.y - a.y) - (c.y - a.y) * (b.x - a.x);
        }

        private Bounding CalculateBounding(Vec2 v0, Vec2 v1, Vec2 v2)
        {
            Bounding b = new Bounding();

            float minX = MathF.Min(v0.x, MathF.Min(v1.x, v2.x));
            float maxX = MathF.Max(v0.x, MathF.Max(v1.x, v2.x));
            
            float minY = MathF.Min(v0.y, MathF.Min(v1.y, v2.y));
            float maxY = MathF.Max(v0.y, MathF.Max(v1.y, v2.y));

            b.minX = (int)minX;
            b.minY = (int)minY;
            b.maxX = (int)maxX-1;
            b.maxY = (int)maxY-1;
            return b;
        }

        private Vec2 PositionToScreenCoords(Vec2 position, int w, int h)
        {
            float normalizedX = position.x * 0.5f + 0.5f;
            float normalizedY = position.y * 0.5f + 0.5f;
            Vec2 o = new Vec2();
            o.x = normalizedX * w;
            o.y = normalizedY * h;
            return o;
        }

        private void DrawVertex(VertexDrawOutput data, ShaderProgram program)
        {
            lock (data.lockRender)
            {
                if (!data.drawed)
                {
                    VertexShader vertexShader = RequestVertex(program, data);
                    data.position = vertexShader.Main();
                    data.drawed = true;

                    if (data.varyings == null || data.varyings.Length != program.varyings.Count) {
                        data.varyings = new Object[program.varyings.Count];
                    }
                    for (int x = 0; x < program.varyings.Count; x++)
                    {
                        Varying varing = program.varyings[x];
                        if (varing.vertexField != null && varing.fragmentField != null)
                        {
                            data.varyings[varing.loc] = varing.vertexField.GetValue(vertexShader);
                        }
                    }
                }
            }
        }

        private VertexDrawOutput FindVertexOutput(int index)
        {
            lock (vertexDrawOutputs)
            {
                for (int x = 0; x < vertexDrawOutputs.Count; x++)
                {
                    VertexDrawOutput data = vertexDrawOutputs[x];
                    if(data.index == index)
                    {
                        return data;
                    }
                }
                VertexDrawOutput d = new VertexDrawOutput();
                d.index = index;
                vertexDrawOutputs.Add(d);
                return d;
            }
        }

        public void DrawFSQ(Texture output, ShaderProgram program)
        {
            Int32[] pixels = output.GetBits();

            PrepareProgram(program);
               
            Parallel.For(0, pixels.Length,
                   index => {
                       FragmentShader fs = RequestFragment(program);
                       pixels[index] = ColorUtils.Convert(fs.Main());
                       ReleaseFragment(fs);
                   });
        }

        private void PrepareProgram(ShaderProgram program)
        {
            if (lastRenderProgram != program)
            {
                vertexShaders.Clear();
                fragmentShaders.Clear();
            }
            lastRenderProgram = program;
        }

        private VertexShader RequestVertex(ShaderProgram program, VertexDrawOutput data)
        {
            VertexShader item;
            if (vertexShaders.TryTake(out item))
            {
                Binder.PrepareRender(item, program, data);
                return item;
            }

            VertexShader vs = (VertexShader)Activator.CreateInstance(program.vertex.GetType());
            Binder.PrepareRender(vs, program, data);
            return vs;
        }
        private void ReleaseVertex(VertexShader fs)
        {
            vertexShaders.Append(fs);
        }

        private FragmentShader RequestFragment(ShaderProgram program)
        {
            FragmentShader item;
            if (fragmentShaders.TryTake(out item))
            {
                Binder.PrepareRender(item, program);
                return item;
            }
            
            FragmentShader fs = (FragmentShader)Activator.CreateInstance(program.fragment.GetType());
            Binder.PrepareRender(fs, program);
            return fs;
        }
        private void ReleaseFragment(FragmentShader fs)
        {
            fragmentShaders.Append(fs);
        }
    }
}
