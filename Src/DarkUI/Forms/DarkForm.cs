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
            //TransparencyKey = Color.Lime;
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

        // Refresh Win11 acrylic composition cache
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Invalidate();
        }

        /*
        // Do we want to render even when not focused?
        
        protected override void WndProc(ref Message m)
        {
            const int WM_NCACTIVATE = 0x0086;

            if (m.Msg == WM_NCACTIVATE)
            {
                // The REAL focus state is here, before we clobber it.
                bool reallyActive = m.WParam != IntPtr.Zero;
                ApplyCaptionState(reallyActive);

                // Force "active" appearance regardless of real focus state.
                // wParam = TRUE  -> draw as active (keeps the live backdrop)
                // lParam  = -1   -> tell DWM not to change the non-client region,
                //                   which avoids a flicker/partial repaint
                m.WParam = (IntPtr)1;
                m.LParam = (IntPtr)(-1);
            }

            base.WndProc(ref m);
        }

        private const int DWMWA_BORDER_COLOR = 34;
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 36;
        private const uint DWMWA_COLOR_DEFAULT = 0xFFFFFFFF; // "reset to system default"

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

        private bool? _lastState;   // avoid redundant calls

        private void ApplyCaptionState(bool active)
        {
            if (_lastState == active) return;
            _lastState = active;

            // COLORREF is 0x00BBGGRR
            int text = active
                ? unchecked((int)DWMWA_COLOR_DEFAULT)               // normal bright text
                : ColorToCOLORREF(Color.FromArgb(0x99, 0x99, 0x99)); // dimmed inactive text

            DwmSetWindowAttribute(Handle, DWMWA_TEXT_COLOR, ref text, sizeof(int));

            // Optional: also nudge the border to the inactive tone
            int border = active
                ? unchecked((int)DWMWA_COLOR_DEFAULT)
                : ColorToCOLORREF(Color.FromArgb(0x40, 0x40, 0x40));
            DwmSetWindowAttribute(Handle, DWMWA_BORDER_COLOR, ref border, sizeof(int));
        }

        private static int ColorToCOLORREF(Color c) => c.R | (c.G << 8) | (c.B << 16);
        */
    }
}
