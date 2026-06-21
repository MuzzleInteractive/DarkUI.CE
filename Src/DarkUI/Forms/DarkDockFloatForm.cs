using DarkUI.Docking;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Forms
{
    public class DarkDockFloatForm : DarkForm
    {
        public DarkDockPanel DockPanel { get; private set; } = new DarkDockPanel { Dock = DockStyle.Fill };

        private DarkDockArea _contentArea;
        private bool _filling;

        public DarkDockFloatForm(DarkDockContent content, Size clientSize, Point location)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = true;
            ShowIcon = false;
            ClientSize = clientSize;
            Location = location;
            Text = content.DockText;

            Controls.Add(DockPanel);

            Application.AddMessageFilter(DockPanel.DockContentDragFilter);
            Application.AddMessageFilter(DockPanel.DockResizeFilter);

            DockPanel.ContentAdded += DockPanel_ContentAdded;
            DockPanel.ContentRemoved += DockPanel_ContentRemoved;
            DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            _contentArea = GetContentArea(content);

            content.DockArea = _contentArea;

            DockPanel.AddContent(content);
        }

        private static DarkDockArea GetContentArea(DarkDockContent content)
        {
            if (content is DarkDocument)
                return DarkDockArea.Document;

            var area = content.DefaultDockArea;

            if (area == DarkDockArea.None || area == DarkDockArea.Document)
                area = DarkDockArea.Left;

            return area;
        }

        private void UpdateFloatLayout()
        {
            if (IsDisposed)
                return;

            if (_contentArea != DarkDockArea.Left &&
                _contentArea != DarkDockArea.Right &&
                _contentArea != DarkDockArea.Bottom)
                return;

            var documentRegion = DockPanel.Regions[DarkDockArea.Document];
            var hasDocument = documentRegion.GetContents().Count > 0;

            if (!hasDocument)
            {
                _filling = true;
                documentRegion.Visible = false;
                FitContentRegion();
            }
            else if (_filling)
            {
                _filling = false;
                documentRegion.Visible = true;
                ResetContentRegion();
            }
        }

        private void FitContentRegion()
        {
            var region = DockPanel.Regions[_contentArea];

            switch (_contentArea)
            {
                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    region.Width = DockPanel.ClientSize.Width;
                    break;
                case DarkDockArea.Bottom:
                    region.Height = DockPanel.ClientSize.Height;
                    break;
            }
        }

        private void ResetContentRegion()
        {
            var region = DockPanel.Regions[_contentArea];

            switch (_contentArea)
            {
                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    region.Width = Math.Max(region.MinimumSize.Width, DockPanel.ClientSize.Width / 3);
                    break;
                case DarkDockArea.Bottom:
                    region.Height = Math.Max(region.MinimumSize.Height, DockPanel.ClientSize.Height / 3);
                    break;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateFloatLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateFloatLayout();
        }

        private void DockPanel_ActiveContentChanged(object sender, DockContentEventArgs e)
        {
            if (!(e.Content is null))
                Text = e.Content.DockText;
        }

        private void DockPanel_ContentAdded(object sender, DockContentEventArgs e)
        {
            UpdateFloatLayout();
        }

        private void DockPanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (!IsHandleCreated || IsDisposed)
                return;

            BeginInvoke((MethodInvoker)delegate
            {
                if (IsDisposed)
                    return;

                if (DockPanel.GetAllContents().Count == 0)
                    Close();
                else
                    UpdateFloatLayout();
            });
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Application.RemoveMessageFilter(DockPanel.DockContentDragFilter);
            Application.RemoveMessageFilter(DockPanel.DockResizeFilter);

            DockPanel.ContentAdded -= DockPanel_ContentAdded;
            DockPanel.ContentRemoved -= DockPanel_ContentRemoved;
            DockPanel.ActiveContentChanged -= DockPanel_ActiveContentChanged;
        }
    }
}
