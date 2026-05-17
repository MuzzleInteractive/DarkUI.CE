using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Resources;

namespace DarkUI.Demo.Forms.Docking
{
    public partial class DockLayers : DarkToolWindow
    {
        public DockLayers()
        {
            InitializeComponent();

            // Build dummy list data
            for (var i = 0; i < 100; i++)
            {
                var item = new DarkListItem($"List item #{i}");
                item.Icon = VS2019Icons.Application_16x;
                lstLayers.Items.Add(item);
            }

            // Build dropdown list data
            for (var i = 0; i < 5; i++)
            {
                cmbList.Items.Add(new DarkDropdownItem($"Dropdown item #{i}"));
            }
        }
    }
}
