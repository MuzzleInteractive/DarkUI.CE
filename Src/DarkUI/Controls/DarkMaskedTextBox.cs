using DarkUI.Config;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkMaskedTextBox : MaskedTextBox
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new Color ForeColor { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new Color BackColor { get; set; }

        public DarkMaskedTextBox()
        {
            Padding = new Padding(2, 2, 2, 2);
            BorderStyle = BorderStyle.FixedSingle;

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw, true);

            base.ForeColor = ThemeProvider.CurrentTheme.LightText;
            base.BackColor = ThemeProvider.CurrentTheme.LightBackground;
        }

        public override void Refresh()
        {
            base.ForeColor = ThemeProvider.CurrentTheme.LightText;
            base.BackColor = ThemeProvider.CurrentTheme.LightBackground;
            base.Refresh();
        }
    }
}
