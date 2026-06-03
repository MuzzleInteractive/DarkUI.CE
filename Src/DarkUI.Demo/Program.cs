using DarkUI.Demo.Forms;
using System;
using System.Windows.Forms;

namespace DarkUI.Demo
{
    internal static class Program
    {
        [STAThread]
        static void Main(params string[] args)
        {
#if NETFRAMEWORK
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#elif NET
            ApplicationConfiguration.Initialize();
#endif
            if (args.Length == 0)
                Application.Run(new MainForm());
            else if (args[0].ToUpper() == "-TEST")
                Application.Run(new TestForm());
        }
    }
}
