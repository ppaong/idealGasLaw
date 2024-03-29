using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace idealGasLaw
{
    class GradientPanel : Panel
    {
        public Color TopColor { get; set; }
        public Color BottomColor { get; set; }
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush rgb = new LinearGradientBrush(this.ClientRectangle, this.TopColor, this.BottomColor, 90f);
            e.Graphics.FillRectangle(rgb, this.ClientRectangle);
            base.OnPaint(e);
        }
    }
}
