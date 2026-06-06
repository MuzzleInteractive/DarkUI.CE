using DarkUI.Config;
using DarkUI.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkTabControl : TabControl
    {
        public DarkTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            BackColor = ThemeProvider.CurrentTheme.GreyBackground;
            ForeColor = ThemeProvider.CurrentTheme.LightText;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM.TCM_ADJUSTRECT)
            {
                // Let the native control compute the default rect first.
                base.WndProc(ref m);

                RECT rc = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));

                // Enlarge the display area (move edges outward).
                rc.Left -= 3;
                rc.Top -= 2;
                rc.Right += 3;
                rc.Bottom += 3;

                Marshal.StructureToPtr(rc, m.LParam, false);
                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle clientRect = ClientRectangle;
            Rectangle displayRect = DisplayRectangle;

            // Draw background
            using (SolidBrush b = new SolidBrush(ThemeProvider.CurrentTheme.GreyBackground))
                g.FillRectangle(b, clientRect);

            // Draw display page border
            using (Pen p = new Pen(ThemeProvider.CurrentTheme.DarkBorder))
                g.DrawRectangle(p, displayRect.X - 1, displayRect.Y - 1, displayRect.Width + 1, displayRect.Height + 1);

            // Draw all unselected tabs first
            for (int i = 0; i < TabCount; i++)
            {
                if (i == SelectedIndex)
                    continue;

                Rectangle tabRect = GetTabRect(i); // This is the true interactible rect
                Rectangle textRect = new Rectangle(tabRect.X + 3, tabRect.Y + 1, tabRect.Width, tabRect.Height - 1);

                // Draw tab background
                using (SolidBrush b = new SolidBrush(ThemeProvider.CurrentTheme.HeaderBackground))
                    g.FillRectangle(b, tabRect);

                // Draw top edge
                using (Pen p = new Pen(ThemeProvider.CurrentTheme.LightBorder))
                    g.DrawLine(p, tabRect.Location.X, tabRect.Location.Y + 1, tabRect.Location.X + tabRect.Width - 1, tabRect.Location.Y + 1);

                // Draw tab border
                using (Pen p = new Pen(ThemeProvider.CurrentTheme.DarkBorder))
                {
                    if (i > 0)
                    {
                        g.DrawLine(p, tabRect.X, tabRect.Y, tabRect.X + tabRect.Width - 1, tabRect.Y); // Top
                        g.DrawLine(p, tabRect.X + tabRect.Width - 1, tabRect.Y, tabRect.X + tabRect.Width - 1, tabRect.Y + tabRect.Height - 1); // Right
                        g.DrawLine(p, tabRect.X, tabRect.Y + tabRect.Height - 1, tabRect.X + tabRect.Width - 1, tabRect.Y + tabRect.Height - 1); // Bottom
                    }
                    else
                        g.DrawRectangle(p, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
                }

                // Draw text
                using (SolidBrush b = new SolidBrush(ThemeProvider.CurrentTheme.LightText))
                {
                    using (StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    })
                        g.DrawString(TabPages[i].Text, Font, b, textRect, format);
                }

                //

                if (TabPages[i].UseVisualStyleBackColor)
                    TabPages[i].UseVisualStyleBackColor = false; // Must be false, otherwise visual styles override BackColor

                if (TabPages[i].BackColor != ThemeProvider.CurrentTheme.GreyBackground)
                    TabPages[i].BackColor = ThemeProvider.CurrentTheme.GreyBackground;

                if (TabPages[i].ForeColor != ThemeProvider.CurrentTheme.LightText)
                    TabPages[i].ForeColor = ThemeProvider.CurrentTheme.LightText;
            }

            // Draw selected tab
            if (SelectedIndex >= 0 && TabCount > 0)
            {
                Rectangle tabRect = GetTabRect(SelectedIndex);

                tabRect = new Rectangle(tabRect.X - 2, tabRect.Y - 2, tabRect.Width + 3, tabRect.Height + 2);

                Rectangle textRect = new Rectangle(tabRect.X + 3, tabRect.Y + 1, tabRect.Width, tabRect.Height - 1);

                // Draw tab background
                using (SolidBrush b = new SolidBrush(ThemeProvider.CurrentTheme.AccentBackground))
                    g.FillRectangle(b, tabRect);

                // Draw top edge
                using (Pen p = new Pen(ThemeProvider.CurrentTheme.LightAccentBorder))
                    g.DrawLine(p, tabRect.Location.X, tabRect.Location.Y + 1, tabRect.Location.X + tabRect.Width - 1, tabRect.Location.Y + 1);

                // Draw tab border
                using (Pen p = new Pen(ThemeProvider.CurrentTheme.DarkBorder))
                    g.DrawRectangle(p, tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height - 1);

                // Draw bottom edge
                using (Pen p = new Pen(ThemeProvider.CurrentTheme.DarkAccentBorder))
                    g.DrawLine(p, tabRect.Location.X + 1, tabRect.Size.Height - 1, tabRect.Location.X + tabRect.Width - 1, tabRect.Size.Height - 1);

                // Draw text
                using (SolidBrush b = new SolidBrush(ThemeProvider.CurrentTheme.LightText))
                {
                    using (StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    })
                        g.DrawString(TabPages[SelectedIndex].Text, Font, b, textRect, format);
                }

                //

                if (TabPages[SelectedIndex].UseVisualStyleBackColor)
                    TabPages[SelectedIndex].UseVisualStyleBackColor = false;

                if (TabPages[SelectedIndex].BackColor != ThemeProvider.CurrentTheme.GreyBackground)
                    TabPages[SelectedIndex].BackColor = ThemeProvider.CurrentTheme.GreyBackground;

                if (TabPages[SelectedIndex].ForeColor != ThemeProvider.CurrentTheme.LightText)
                    TabPages[SelectedIndex].ForeColor = ThemeProvider.CurrentTheme.LightText;
            }
        }
    }
}
