using DarkUI.Demo.Forms;
using System;
using System.Windows.Forms;

namespace DarkUI.Demo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
#if NETFRAMEWORK
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#elif NET
            ApplicationConfiguration.Initialize();
#endif
            Application.Run(new MainForm());
        }
    }
}