using CPURasterizer.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = CPURasterizer.Utils.Random;

namespace CPURasterizer.Surface
{
    public class CPUSurfaceView
    {
        public CPURenderer gl;
        private PictureBox pictureBox;
        private Form form;
        private TimeCounter timeCounter = new TimeCounter();
        private float framesPerSecond;
        private float deltaTime;
        public CPUSurfaceView()
        {
            this.form = new Form();

            gl = new CPURenderer(form,
                new EventHandler(delegate (Object o, EventArgs a)
                {
                    PreRender();
                    OnFrameRender(gl);
                    PosRender();
                })
                );
            gl.Start();
            gl.SetBruteModeEnabled(false);

            this.form.Text = "CPU Rasterizer";
            this.pictureBox = new PictureBox();
            this.pictureBox.Image = gl.GetInternalScreen();
            this.pictureBox.Dock = DockStyle.Fill;
            this.form.Controls.Add(this.pictureBox);
            this.form.FormClosed += OnFormClosed;
            try
            {
                Application.Run(this.form);
            } catch (System.InvalidOperationException e)
            {
                this.form.Show();
            }
        }

        private void OnFormClosed(object? sender, FormClosedEventArgs e)
        {
            gl.Stop();
        }

     
        private void PreRender()
        {
            timeCounter.Finish();
            deltaTime = (float)timeCounter.ElapsedTime();
            framesPerSecond = (float)(1f / timeCounter.ElapsedTime());
            timeCounter.Start();
            
            gl.SetFormSize(form.Size.Width, form.Size.Height);

            this.pictureBox.Image = gl.GetInternalScreen();
            this.pictureBox.Update();

            int pixels = form.Size.Width * form.Size.Height;
            int pixelsPerSecond = (int)((1d / timeCounter.ElapsedTime()) * pixels);
            Debug.WriteLine(
                  " FrameTime:" + gl.GetFrameTime() +"\n"
                + "       FPS:" + Math.Round(framesPerSecond) + "\n"
                + "     Limit:" + gl.GetMaxFrameRate() + "\n"
                + "   MayUpTo:" + Math.Round((1f/gl.GetFrameTime())) + "\n"
                + "       PPS:" + pixelsPerSecond + "\n"
                + "Resolution:" + form.Size.Width +"x"+ form.Size.Height + "\n"
                + " BruteMode:" + gl.IsBruteModeEnabled()
                );
        }

        public virtual void OnFrameRender(CPURenderer gl)
        {
            gl.ClearColorBuffer();
        }

        public void PosRender()
        {

        }

        public float GetDeltaTime()
        {
            return deltaTime;
        }
    }
}
