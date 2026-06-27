using DarkUI.Config;
using DarkUI.Renderers;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkStatusStrip : StatusStrip
    {
        [Category("Appearance")]
        [Description("Defines if the border is drawn.")]
        [DefaultValue(true)]
        public bool DrawBorder
        {
            get { return _drawBorder; }
            set
            {
                _drawBorder = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Specifies visibility of the grip on the ToolStrip.")]
        [DefaultValue(ToolStripGripStyle.Hidden)]
        public new ToolStripGripStyle GripStyle
        {
            get
            {
                return base.GripStyle;
            }
            set
            {
                base.GripStyle = value;
            }
        }

        [Category("Appearance")]
        [Description("Determines whether a StatusStrip has a sizing grip.")]
        [DefaultValue(false)]
        public new bool SizingGrip
        {
            get
            {
                return base.SizingGrip;
            }
            set
            {
                base.SizingGrip = value;
            }
        }

        private bool _drawBorder = true;

        public DarkStatusStrip()
        {
            AutoSize = false;
            Renderer = new DarkToolStripRenderer();
            Padding = new Padding(0, 5, 0, 3);
            Size = new Size(Size.Width, 24);
            SizingGrip = false;
            GripStyle = ToolStripGripStyle.Hidden;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;

            using (var b = new SolidBrush(ThemeProvider.CurrentTheme.GreyBackground))
                g.FillRectangle(b, ClientRectangle);

            if (!DrawBorder)
                return;

            using (var p = new Pen(ThemeProvider.CurrentTheme.DarkBorder))
                g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Right, 0);

            using (var p = new Pen(ThemeProvider.CurrentTheme.LightBorder))
                g.DrawLine(p, ClientRectangle.Left, 1, ClientRectangle.Right, 1);
        }
    }
}
