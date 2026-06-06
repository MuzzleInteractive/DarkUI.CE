using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DarkUI.Win32
{
    internal sealed class Native
    {
        // Backdrop types (Windows 11 22621+)
        internal enum SystemBackdropType
        {
            Auto = 0,       // DWMSBT_AUTO - Let DWM decide
            None = 1,       // DWMSBT_NONE - No backdrop
            Mica = 2,       // DWMSBT_MAINWINDOW - Wallpaper-tinted, efficient; for main windows
            Acrylic = 3,    // DWMSBT_TRANSIENTWINDOW - Real-time blur of content behind the window; more GPU-intensive
            MicaAlt = 4,    // DWMSBT_TABBEDWINDOW - Like Mica but stronger wallpaper tint (more contrast)
        }

        internal enum WindowCornerPreference
        {
            Default = 0,    //DWMWCP_DEFAULT
            DoNotRound = 1, //DWMWCP_DONOTROUND
            Round = 2,      //DWMWCP_ROUND
            RoundSmall = 3  //DWMWC1P_ROUNDSMALL
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref CHARFORMAT2 lp);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        internal static void EnableImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            int useDarkMode = enabled ? 1 : 0;
            int result = DwmSetWindowAttribute(handle, (int)WM.DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));

            if (result != 0)
                DwmSetWindowAttribute(handle, (int)WM.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref useDarkMode, sizeof(int));
        }

        // Applies Win11 corner preferences
        internal static void SetCornerPreference(IntPtr handle, WindowCornerPreference preference)
        {
            int value = (int)preference;
            DwmSetWindowAttribute(handle, (int)WM.DWMWA_WINDOW_CORNER_PREFERENCE, ref value, sizeof(int));
        }

        // Applies Win11 materials to the form
        internal static void SetBackdrop(IntPtr hwnd, SystemBackdropType type)
        {
            int value = (int)type;
            DwmSetWindowAttribute(hwnd, (int)WM.DWMWA_SYSTEMBACKDROP_TYPE, ref value, sizeof(int));
        }

        // Applies the effects to the form client area, will glitchout with Color.Transparent and so TransparencyKey is needed.
        // Many controls in DarkUI tend to use Color.Transparent and so GDI+ tends to glitch alot, will fix later.
        internal static void ExtendFrameFully(IntPtr hwnd)
        {
            // -1 on all sides = "sheet of glass" (extend into entire window)
            var margins = new MARGINS
            {
                cxLeftWidth = -1,
                cxRightWidth = -1,
                cyTopHeight = -1,
                cyBottomHeight = -1
            };
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct CHARFORMAT2
    {
        public int cbSize;
        public int dwMask;
        public int dwEffects;
        public int yHeight;
        public int yOffset;
        public int crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szFaceName;
        public short wWeight;
        public short sSpacing;
        public int crBackColor;
        public int lcid;
        public int dwReserved;
        public short sStyle;
        public short wKerning;
        public byte bUnderlineType;
        public byte bAnimation;
        public byte bRevAuthor;
        public byte bReserved1;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
}
