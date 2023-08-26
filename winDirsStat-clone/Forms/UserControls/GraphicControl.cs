using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winDirsStat_clone.Forms.UserControls
{
    public partial class GraphicControl : UserControl
    {
        public GraphicControl()
        {
            InitializeComponent();
        }

        void GraphicControl_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphicsObj;
            graphicsObj = this.CreateGraphics();            
        }
    }
}
