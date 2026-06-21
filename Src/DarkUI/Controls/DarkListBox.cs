using DarkUI.Config;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkListBox : ListBox
    {
        public DarkListBox()
        {
            Padding = new Padding(2, 2, 2, 2);
            BorderStyle = BorderStyle.FixedSingle;

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw, true);

            base.ForeColor = ThemeProvider.CurrentTheme.LightText;
            base.BackColor = ThemeProvider.CurrentTheme.GreyBackground;
        }

        public override void Refresh()
        {
            base.ForeColor = ThemeProvider.CurrentTheme.LightText;
            base.BackColor = ThemeProvider.CurrentTheme.GreyBackground;
            base.Refresh();
        }
    }
}
