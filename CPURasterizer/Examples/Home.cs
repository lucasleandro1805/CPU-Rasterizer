using CPURasterizer.Examples.Cube;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPURasterizer.Examples
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();

            FlowLayoutPanel buttons = (FlowLayoutPanel)this.Controls["buttons"];
            {
                Button button = new Button();
                button.Text = "Cube";
                button.AutoSize = true;
                button.Click += new EventHandler(delegate (Object o, EventArgs a)
                {
                    new CubeExample();
                });
                buttons.Controls.Add(button);
            }
        }
    }
}
