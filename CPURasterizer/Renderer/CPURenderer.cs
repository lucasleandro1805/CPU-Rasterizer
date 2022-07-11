using CPURasterizer.Data;
using CPURasterizer.Drawer;
using CPURasterizer.Image;
using CPURasterizer.Shader;
using CPURasterizer.Shader.ClearShaders;
using CPURasterizer.Vectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPURasterizer.Renderer
{
    public class CPURenderer
    {
        public static readonly int DRAW_TRIANGLES = 5654;
        public static readonly int DRAW_LINES = 5656;
        public static readonly int FACE_CULL_OFF = 5465;
        public static readonly int FACE_CULL_BACK = 5464;
        public static readonly int FACE_CULL_FRONT = 5463;

        private EventHandler onRenderFrameHandler;
        private ActiveConfigs configs = new ActiveConfigs();
        private Texture screen;
        private Thread thread;
        private Form form;
        private TimeCounter timeCounter;
        private bool stopped = false;
        private readonly ShaderProgram clearColorProgram;
        private ShaderProgram activeProgram;
        private readonly ShaderProgram defaultProgram;
        private float frameTime;

        public CPURenderer(Form form, EventHandler onRenderFrameHandler)
        {
            this.form = form;
            this.onRenderFrameHandler = onRenderFrameHandler;
            this.timeCounter = new TimeCounter();
            this.screen = new Texture(100, 100);
            this.defaultProgram = new ShaderProgram(new VertexShader(), new FragmentShader());           
            this.clearColorProgram = new ShaderProgram(new ClearColorVertexShader(), new CubeFragmentShader());
            ClearColorBuffer();
        }

        public void Start()
        {
            this.thread = new Thread(new ThreadStart(StartRender));
            this.thread.Name = "CPURenderer";
            this.thread.Start();
        }
        private void StartRender()
        {
            bool requestInvoke = true;
            bool skip = false;
            double timeOut = 0d;
            while (!stopped)
            {
                if (timeOut > 0)
                {
                    timeCounter.Start();
                    int x = 0;
                    while(x < 10)
                    {
                        x += 1;
                    }
                    timeCounter.Finish();
                    timeOut -= timeCounter.ElapsedTime();
                }
                else
                {
                    if (requestInvoke && form.Visible)
                    {
                        requestInvoke = false;
                        timeOut = 0d;
                        try
                        {
                            if (!skip)
                            {
                                form.Invoke(new MethodInvoker(delegate ()
                                {
                                    timeCounter.Start();
                                    onRenderFrameHandler.Invoke(this, EventArgs.Empty);
                                    timeCounter.Finish();
                                    double elapsed = timeCounter.ElapsedTime();
                                    frameTime = (float)elapsed;
                                    double limitedFrameTime = 1d / (double)configs.maxFrameRate;
                                    double diff = limitedFrameTime - elapsed;
                                    timeOut += diff;
                                    requestInvoke = true;
                                }));
                            }
                            else
                            {
                                form.Invoke(new MethodInvoker(delegate ()
                                {
                                    timeOut += 0.001;
                                    requestInvoke = true;
                                }));
                            }
                        } catch (ThreadInterruptedException e)
                        {
                            //
                        }
                        if (!configs.bruteMode)
                        {
                            skip = !skip;
                        } else
                        {
                            skip = false;
                        }
                    }
                }
            }
        }

        public void ClearColor(Vec4 color)
        {
            configs.clearColor = color;
        }
        public void ClearColor(float r, float g, float b, float a)
        {
            configs.clearColor.x = r;
            configs.clearColor.y = g;
            configs.clearColor.z = b;
            configs.clearColor.w = a;
        }

        public void ClearColorBuffer()
        {
            int loc = Binder.UniformLocation(clearColorProgram, "color");
            Binder.Uniform4f(clearColorProgram, loc, configs.clearColor);
            clearColorProgram.drawer.DrawFSQ(screen, clearColorProgram);
        }

        public void DrawElements(int type, int[] triangles)
        {
            GetProgram().drawer.DrawElements(type, configs.cullFaceMode, triangles, screen, GetProgram());
        }

        public void BindAttributeArray(int loc, float[] value)
        {
            Binder.BindAttributeArray(GetProgram(), loc, value);
        }

        public void UniformMat4f(int loc, float[] value)
        {
            Binder.UniformMat4f(GetProgram(), loc, new Mat4(value));
        }
        public void UniformMat4f(int loc, Mat4 value)
        {
            Binder.UniformMat4f(GetProgram(), loc, value);
        }
        public void UniformMat3f(int loc, Mat3 value)
        {
            Binder.UniformMat3f(GetProgram(), loc, value);
        }

        public void Uniform4f(int loc, Vec4 value)
        {
            Binder.Uniform4f(GetProgram(), loc, value);
        }
        public void Uniform4f(int loc, float x, float y, float z, float w)
        {
            Binder.Uniform4f(GetProgram(), loc, new Vec4(x, y, z, w));
        }

        public void Uniform3f(int loc, Vec3 value)
        {
            Binder.Uniform3f(GetProgram(), loc, value);
        }
        public void Uniform3f(int loc, float x, float y, float z)
        {
            Binder.Uniform3f(GetProgram(), loc, new Vec3(x, y, z));
        }


        public void Uniform2f(int loc, Vec2 value)
        {
            Binder.Uniform2f(GetProgram(), loc, value);
        }
        public void Uniform2f(int loc, float x, float y)
        {
            Binder.Uniform2f(GetProgram(), loc, new Vec2(x, y));
        }

        public int AttributeLocation(string name)
        {
            return Binder.AttributeLocation(GetProgram(), name);
        }
        public int UniformLocation(string name)
        {
            return Binder.UniformLocation(GetProgram(), name);
        }

        public void BindProgram(ShaderProgram program)
        {
            activeProgram = program;
        }
        public ShaderProgram GetProgram()
        {
            if(activeProgram != null)
            {
                return activeProgram;
            }
            return defaultProgram;
        }
        
        public int GetWidth()
        {
            return screen.GetWidth();
        }
        public int GetHeight()
        {
            return screen.GetHeight();
        }

        public Bitmap GetInternalScreen()
        {
            return screen.GetBitmap();
        }
        public void SetFormSize(int w, int h)
        {
            if (screen.GetWidth() != w || screen.GetHeight() != h)
            {
                if(screen != null)
                {
                    screen.Dispose();
                }
                screen = new Texture(w, h);
            }
        }

        public void SetCullFace(int type)
        {
            configs.cullFaceMode = type;
        }

        public int GetMaxFrameRate()
        {
            return configs.maxFrameRate;
        }
        public void SetMaxFrameRate(int v)
        {
            configs.maxFrameRate = v;
        }

        public bool IsBruteModeEnabled()
        {
            return configs.bruteMode;
        }
        public void SetBruteModeEnabled(bool enabled)
        {
            configs.bruteMode = enabled;
        }

        public void Stop()
        {
            if(thread != null)
            {
                thread.Interrupt();
                thread = null;
            }
            stopped = true;
        }

        public float GetFrameTime()
        {
            return frameTime;
        }
    }
}
