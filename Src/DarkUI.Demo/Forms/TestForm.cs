using DarkUI.Config;
using DarkUI.Extensions;
using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DarkUI.Demo.Forms
{
    public partial class TestForm : DarkForm
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void BTNOpenMainForm_Click(object sender, EventArgs e)
        {
            MainForm frm = new MainForm();
            frm.Show();
        }

        private void BTNDarkTheme_Click(object sender, EventArgs e)
        {
            ThemeProvider.CurrentTheme = new DarkTheme();
            ThemeProvider.ApplyTheme();
        }

        private void BTNLightTheme_Click(object sender, EventArgs e)
        {
            ThemeProvider.CurrentTheme = new LightTheme();
            ThemeProvider.ApplyTheme();
        }        
    }
}
