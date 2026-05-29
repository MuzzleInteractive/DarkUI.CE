using DarkUI.Config;
using DarkUI.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkRichTextBox : RichTextBox
    {
        public DarkRichTextBox()
        {
            Padding = new Padding(2, 2, 2, 2);
            BorderStyle = BorderStyle.FixedSingle;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            base.BackColor = ThemeProvider.CurrentTheme.LightBackground;
            base.ForeColor = ThemeProvider.CurrentTheme.LightText;
        }

        public override void Refresh()
        {
            SetControlBackColor(ThemeProvider.CurrentTheme.LightBackground);
            SetDefaultTextColor(ThemeProvider.CurrentTheme.LightText);
            base.Refresh();
        }

        public void SetDefaultTextColor(Color color)
        {
            var cf = new CHARFORMAT2
            {
                cbSize = Marshal.SizeOf(typeof(CHARFORMAT2)),
                szFaceName = new char[32],
                dwMask = (int)WM.CFM_COLOR,
                crTextColor = ColorTranslator.ToWin32(color)
            };
            Native.SendMessage(Handle, (int)WM.EM_SETCHARFORMAT, (IntPtr)WM.SCF_DEFAULT, ref cf);
        }

        public void SetControlBackColor(Color color)
        {
            Native.SendMessage(Handle, (uint)WM.EM_SETBKGNDCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(color));
        }
    }
}
