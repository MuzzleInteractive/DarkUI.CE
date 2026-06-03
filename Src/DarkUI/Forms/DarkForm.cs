using DarkUI.Config;
using DarkUI.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Forms
{
    [DefaultEvent("Shown")]
    public class DarkForm : Form
    {
        [Category("Appearance")]
        [Description("Determines whether a single pixel border should be rendered around the form.")]
        [DefaultValue(false)]
        public bool FlatBorder
        {
            get { return _flatBorder; }
            set
            {
                _flatBorder = value;
                Invalidate();
            }
        }

        private bool _flatBorder;

        public DarkForm()
        {
            BackColor = ThemeProvider.CurrentTheme.GreyBackground;
            Font = new Font("Segoe UI", 9);
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Dpi;
            this.EnableDoubleBufferFlagRecursive(true);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeProvider.ApplyTheme(this);
        }

        public override void Refresh()
        {
            base.Refresh();

            if (BackColor != ThemeProvider.CurrentTheme.GreyBackground)
                BackColor = ThemeProvider.CurrentTheme.GreyBackground;

            if (ForeColor != ThemeProvider.CurrentTheme.LightText)
                ForeColor = ThemeProvider.CurrentTheme.LightText;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (!_flatBorder)
                return;

            var g = e.Graphics;

            using (var p = new Pen(ThemeProvider.CurrentTheme.DarkBorder))
            {
                var modRect = new Rectangle(ClientRectangle.Location, new Size(ClientRectangle.Width - 1, ClientRectangle.Height - 1));
                g.DrawRectangle(p, modRect);
            }
        }
    }
}
