using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Resources;

namespace DarkUI.Demo.Forms.Docking
{
    public partial class DockProject : DarkToolWindow
    {
        public DockProject()
        {
            InitializeComponent();

            // Build dummy nodes
            var childCount = 0;
            for (var i = 0; i < 20; i++)
            {
                var node = new DarkTreeNode($"Root node #{i}");
                node.ExpandedIcon = VS2019Icons.FolderOpened_16x;
                node.Icon = VS2019Icons.FolderClosed_16x;

                for (var x = 0; x < 10; x++)
                {
                    var childNode = new DarkTreeNode($"Child node #{childCount}");
                    childNode.Icon = VS2019Icons.DocumentGroup_16x;
                    childCount++;
                    node.Nodes.Add(childNode);
                }

                treeProject.Nodes.Add(node);
            }
        }
    }
}
