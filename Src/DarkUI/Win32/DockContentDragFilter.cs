using DarkUI.Config;
using DarkUI.Docking;
using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Win32
{
    public class DockContentDragFilter : IMessageFilter
    {
        private DarkDockPanel _dockPanel;
        private DarkDockContent _dragContent;
        private DarkTranslucentForm _highlightForm;
        private bool _isDragging = false;
        private DarkDockRegion _targetRegion;
        private DarkDockGroup _targetGroup;
        private DockInsertType _insertType = DockInsertType.None;
        private bool _overDockPanel = false;
        private Dictionary<DarkDockRegion, DockDropArea> _regionDropAreas = new Dictionary<DarkDockRegion, DockDropArea>();
        private Dictionary<DarkDockGroup, DockDropCollection> _groupDropAreas = new Dictionary<DarkDockGroup, DockDropCollection>();

        public DockContentDragFilter(DarkDockPanel dockPanel)
        {
            _dockPanel = dockPanel;

            _highlightForm = new DarkTranslucentForm(ThemeProvider.CurrentTheme.AccentSelection);
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Exit out early if we're not dragging any content
            if (!_isDragging)
                return false;

            // We only care about mouse events
            if (!(m.Msg == (int)WM.MOUSEMOVE ||
                  m.Msg == (int)WM.LBUTTONDOWN || m.Msg == (int)WM.LBUTTONUP || m.Msg == (int)WM.LBUTTONDBLCLK ||
                  m.Msg == (int)WM.RBUTTONDOWN || m.Msg == (int)WM.RBUTTONUP || m.Msg == (int)WM.RBUTTONDBLCLK))
                return false;

            // Move content
            if (m.Msg == (int)WM.MOUSEMOVE)
            {
                HandleDrag();
                return false;
            }

            // Drop content
            if (m.Msg == (int)WM.LBUTTONUP)
            {
                if (_targetRegion != null)
                {
                    var targetPanel = _targetRegion.DockPanel;

                    _dragContent.DockPanel.RemoveContent(_dragContent);
                    _dragContent.DockArea = _targetRegion.DockArea;
                    targetPanel.AddContent(_dragContent);
                }
                else if (_targetGroup != null)
                {
                    var targetPanel = _targetGroup.DockPanel;

                    _dragContent.DockPanel.RemoveContent(_dragContent);

                    switch (_insertType)
                    {
                        case DockInsertType.None:
                            targetPanel.AddContent(_dragContent, _targetGroup);
                            break;

                        case DockInsertType.Before:
                        case DockInsertType.After:
                            targetPanel.InsertContent(_dragContent, _targetGroup, _insertType);
                            break;
                    }
                }
                else if (!_overDockPanel)
                    TearOutContent();

                StopDrag();
                return false;
            }

            return true;
        }

        public void StartDrag(DarkDockContent content)
        {
            _regionDropAreas = new Dictionary<DarkDockRegion, DockDropArea>();
            _groupDropAreas = new Dictionary<DarkDockGroup, DockDropCollection>();

            foreach (var panel in DarkDockPanel.ActivePanels)
            {
                foreach (var region in panel.Regions.Values)
                {
                    if (region.Visible)
                    {
                        if (region.Groups.Count == 0)
                        {
                            _regionDropAreas.Add(region, new DockDropArea(panel, region));
                        }
                        else
                        {
                            foreach (var group in region.Groups)
                                _groupDropAreas.Add(group, new DockDropCollection(panel, group));
                        }
                    }
                    else
                        _regionDropAreas.Add(region, new DockDropArea(panel, region));
                }
            }

            _dragContent = content;
            _isDragging = true;
        }

        private void StopDrag()
        {
            Cursor.Current = Cursors.Default;

            _highlightForm.Hide();
            _dragContent = null;
            _isDragging = false;
        }

        private void UpdateHighlightForm(Rectangle rect)
        {
            Cursor.Current = Cursors.SizeAll;

            _highlightForm.SuspendLayout();

            _highlightForm.Size = new Size(rect.Width, rect.Height);
            _highlightForm.Location = new Point(rect.X, rect.Y);

            _highlightForm.ResumeLayout();

            if (!_highlightForm.Visible)
            {
                _highlightForm.Show();
                _highlightForm.BringToFront();
            }
        }

        private void HandleDrag()
        {
            var location = Cursor.Position;

            _insertType = DockInsertType.None;

            _targetRegion = null;
            _targetGroup = null;
            _overDockPanel = false;

            DarkDockPanel activePanel = GetPanelAtScreenPoint(location);
            _overDockPanel = activePanel != null;

            // Check all region drop areas
            foreach (var area in _regionDropAreas.Values)
            {
                if (area.DockPanel != activePanel)
                    continue;

                if (!CanDropInArea(area.DockRegion.DockArea))
                    continue;

                if (area.DropArea.Contains(location))
                {
                    _insertType = DockInsertType.None;
                    _targetRegion = area.DockRegion;
                    UpdateHighlightForm(area.HighlightArea);
                    return;
                }
            }

            // Check all group drop areas
            foreach (var collection in _groupDropAreas.Values)
            {
                if (collection.DropArea.DockPanel != activePanel)
                    continue;

                if (!CanDropInArea(collection.DropArea.DockGroup.DockArea))
                    continue;

                var sameRegion = false;
                var sameGroup = false;
                var groupHasOtherContent = false;

                if (collection.DropArea.DockGroup == _dragContent.DockGroup)
                    sameGroup = true;

                if (collection.DropArea.DockGroup.DockRegion == _dragContent.DockRegion)
                    sameRegion = true;

                if (_dragContent.DockGroup.ContentCount > 1)
                    groupHasOtherContent = true;

                var isDocument = collection.DropArea.DockGroup.DockArea == DarkDockArea.Document;

                // If we're hovering over the group itself, only allow inserting before/after if multiple content is tabbed.
                if (!isDocument && (!sameGroup || groupHasOtherContent))
                {
                    var skipBefore = false;
                    var skipAfter = false;

                    // Inserting before/after other content might cause the content to be dropped on to its own location.
                    // Check if the group above/below the hovered group contains our drag content.
                    if (sameRegion && !groupHasOtherContent)
                    {
                        if (collection.InsertBeforeArea.DockGroup.Order == _dragContent.DockGroup.Order + 1)
                            skipBefore = true;

                        if (collection.InsertAfterArea.DockGroup.Order == _dragContent.DockGroup.Order - 1)
                            skipAfter = true;
                    }

                    if (!skipBefore)
                    {
                        if (collection.InsertBeforeArea.DropArea.Contains(location))
                        {
                            _insertType = DockInsertType.Before;
                            _targetGroup = collection.InsertBeforeArea.DockGroup;
                            UpdateHighlightForm(collection.InsertBeforeArea.HighlightArea);
                            return;
                        }
                    }

                    if (!skipAfter)
                    {
                        if (collection.InsertAfterArea.DropArea.Contains(location))
                        {
                            _insertType = DockInsertType.After;
                            _targetGroup = collection.InsertAfterArea.DockGroup;
                            UpdateHighlightForm(collection.InsertAfterArea.HighlightArea);
                            return;
                        }
                    }
                }

                // Don't allow content to be dragged on to itself
                if (!sameGroup)
                {
                    if (collection.DropArea.DropArea.Contains(location))
                    {
                        _insertType = DockInsertType.None;
                        _targetGroup = collection.DropArea.DockGroup;
                        UpdateHighlightForm(collection.DropArea.HighlightArea);
                        return;
                    }
                }
            }

            // Not hovering over a valid target - hide the highlight
            if (_highlightForm.Visible)
                _highlightForm.Hide();

            Cursor.Current = _overDockPanel ? Cursors.No : Cursors.SizeAll;
        }

        private DarkDockPanel GetPanelAtScreenPoint(Point location)
        {
            IntPtr hWnd = Native.WindowFromPoint(location);
            if (hWnd == IntPtr.Zero)
                return null;

            Control control = Control.FromChildHandle(hWnd);
            if (control == null)
                return null;

            Form form = control.FindForm();
            if (form == null)
                return null;

            foreach (var panel in DarkDockPanel.ActivePanels)
            {
                if (panel.FindForm() == form)
                    return panel;
            }

            return null;
        }

        private bool CanDropInArea(DarkDockArea targetArea)
        {
            bool contentIsDocument = _dragContent is DarkDocument;
            bool targetIsDocument = targetArea == DarkDockArea.Document;

            return contentIsDocument == targetIsDocument;
        }

        private void TearOutContent()
        {
            DarkDockContent content = _dragContent;
            DarkDockPanel source = content.DockPanel;

            if (source == null)
                return;

            Form ownerForm = source.FindForm();

            if (ownerForm is DarkDockFloatForm && source.GetAllContents().Count <= 1)
                return;

            Size size = content.DockGroup != null ? content.DockGroup.Size : content.Size;

            if (size.Width < 300)
                size = new Size(300, size.Height);

            if (size.Height < 300)
                size = new Size(size.Width, 300);

            Point location = new Point(Cursor.Position.X - 60, Cursor.Position.Y - 15);

            while (ownerForm is DarkDockFloatForm && ownerForm.Owner != null)
                ownerForm = ownerForm.Owner;

            source.RemoveContent(content);

            DarkDockFloatForm floatForm = new DarkDockFloatForm(content, size, location);

            if (ownerForm != null)
                floatForm.Owner = ownerForm;

            floatForm.Show();
        }
    }
}
