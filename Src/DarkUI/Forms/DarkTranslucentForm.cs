using DarkUI.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Forms
{
    internal class DarkTranslucentForm : Form
    {
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= (int)WM.WS_EX_TRANSPARENT;
                return cp;
            }
        }

        public DarkTranslucentForm(Color backColor, double opacity = 0.6)
        {
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(1, 1);
            ShowInTaskbar = false;
            AllowTransparency = true;
            Opacity = opacity;
            BackColor = backColor;
        }
    }
}
