using DarkUI.Forms;
using System.Windows.Forms;

namespace DarkUI.Demo.Forms.Dialogs
{
    public partial class DialogAbout : DarkDialog
    {
        public DialogAbout()
        {
            InitializeComponent();

            lblVersion.Text = $"Version: {Application.ProductVersion}";
            btnOk.Text = "Close";
        }
    }
}
