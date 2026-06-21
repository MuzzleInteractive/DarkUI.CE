using DarkUI.Config;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkDataGridView : DataGridView
    {
        [Category("Appearance")]
        [Description("Alternate row background colors using the theme.")]
        [DefaultValue(true)]
        public bool AlternatingRowColors
        {
            get { return _alternatingRowColors; }
            set
            {
                if (_alternatingRowColors == value)
                    return;

                _alternatingRowColors = value;
                UpdateAlternatingRowStyle();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public new bool EnableHeadersVisualStyles
        {
            get => base.EnableHeadersVisualStyles;
            set => base.EnableHeadersVisualStyles = value;
        }

        [Category("Appearance")]
        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set => base.BorderStyle = value;
        }

        [Category("Appearance")]
        [DefaultValue(DataGridViewCellBorderStyle.Single)]
        public new DataGridViewCellBorderStyle CellBorderStyle
        {
            get => base.CellBorderStyle;
            set => base.CellBorderStyle = value;
        }

        [Category("Appearance")]
        [DefaultValue(DataGridViewHeaderBorderStyle.Single)]
        public new DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle
        {
            get => base.ColumnHeadersBorderStyle;
            set => base.ColumnHeadersBorderStyle = value;
        }

        [Category("Appearance")]
        [DefaultValue(DataGridViewHeaderBorderStyle.Single)]
        public new DataGridViewHeaderBorderStyle RowHeadersBorderStyle
        {
            get => base.RowHeadersBorderStyle;
            set => base.RowHeadersBorderStyle = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Category("Behavior")]
        [DefaultValue(DataGridViewColumnHeadersHeightSizeMode.DisableResizing)]
        public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get => base.ColumnHeadersHeightSizeMode;
            set => base.ColumnHeadersHeightSizeMode = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Category("Behavior")]
        [DefaultValue(DataGridViewRowHeadersWidthSizeMode.DisableResizing)]
        public new DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode
        {
            get => base.RowHeadersWidthSizeMode;
            set => base.RowHeadersWidthSizeMode = value;
        }

        [Category("Appearance")]
        [DefaultValue(25)]
        public new int ColumnHeadersHeight
        {
            get => base.ColumnHeadersHeight;
            set => base.ColumnHeadersHeight = value;
        }

        [Category("Appearance")]
        [DefaultValue(25)]
        public new int RowHeadersWidth
        {
            get => base.RowHeadersWidth;
            set => base.RowHeadersWidth = value;
        }

        [Category("Layout")]
        [DefaultValue(ScrollBars.Both)]
        public new ScrollBars ScrollBars
        {
            get => base.ScrollBars;
            set => base.ScrollBars = value;
        }

        private Control _vScrollCover = new Control { Visible = false, TabStop = false };
        private Control _hScrollCover = new Control { Visible = false, TabStop = false };
        private DarkScrollBar _vScrollBar = new DarkScrollBar { ScrollOrientation = DarkScrollOrientation.Vertical, Visible = false };
        private DarkScrollBar _hScrollBar = new DarkScrollBar { ScrollOrientation = DarkScrollOrientation.Horizontal, Visible = false };
        private bool _alternatingRowColors = true;
        private bool _isSyncingScroll;

        public DarkDataGridView()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            EnableHeadersVisualStyles = false;
            BorderStyle = BorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.Single;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; // DataGridViewDesigner is forcing this to AutoSize
            RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            ColumnHeadersHeight = 25;
            RowHeadersWidth = 25;
            RowTemplate.Height = 25;

            UpdateTheme();

            Controls.Add(_vScrollCover);
            Controls.Add(_hScrollCover);
            Controls.Add(_vScrollBar);
            Controls.Add(_hScrollBar);

            _vScrollCover.BringToFront();
            _hScrollCover.BringToFront();
            _vScrollBar.BringToFront();
            _hScrollBar.BringToFront();

            _vScrollBar.ValueChanged += VScrollBar_ValueChanged;
            _hScrollBar.ValueChanged += HScrollBar_ValueChanged;
            VerticalScrollBar.VisibleChanged += NativeVScrollBar_VisibleChanged;
            HorizontalScrollBar.VisibleChanged += NativeHScrollBar_VisibleChanged;
        }

        private void UpdateTheme()
        {
            BackgroundColor = ThemeProvider.CurrentTheme.GreyBackground;
            GridColor = ThemeProvider.CurrentTheme.DarkBorder;

            DefaultCellStyle.BackColor = ThemeProvider.CurrentTheme.GreyBackground;
            DefaultCellStyle.ForeColor = ThemeProvider.CurrentTheme.LightText;
            DefaultCellStyle.SelectionBackColor = Focused ? ThemeProvider.CurrentTheme.AccentSelection : ThemeProvider.CurrentTheme.GreySelection;
            DefaultCellStyle.SelectionForeColor = ThemeProvider.CurrentTheme.LightText;

            UpdateAlternatingRowStyle();

            ColumnHeadersDefaultCellStyle.BackColor = ThemeProvider.CurrentTheme.HeaderBackground;
            ColumnHeadersDefaultCellStyle.ForeColor = ThemeProvider.CurrentTheme.LightText;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeProvider.CurrentTheme.AccentBackground;
            ColumnHeadersDefaultCellStyle.SelectionForeColor = ThemeProvider.CurrentTheme.LightText;
            RowHeadersDefaultCellStyle.BackColor = ThemeProvider.CurrentTheme.HeaderBackground;
            RowHeadersDefaultCellStyle.ForeColor = ThemeProvider.CurrentTheme.LightText;
            RowHeadersDefaultCellStyle.SelectionBackColor = ThemeProvider.CurrentTheme.AccentBackground;
            RowHeadersDefaultCellStyle.SelectionForeColor = ThemeProvider.CurrentTheme.LightText;

            _vScrollCover.BackColor = ThemeProvider.CurrentTheme.DarkBorder;
            _hScrollCover.BackColor = ThemeProvider.CurrentTheme.DarkBorder;
        }

        private void UpdateAlternatingRowStyle()
        {
            if (_alternatingRowColors)
            {
                AlternatingRowsDefaultCellStyle.BackColor = ThemeProvider.CurrentTheme.HeaderBackground;
                AlternatingRowsDefaultCellStyle.ForeColor = ThemeProvider.CurrentTheme.LightText;
            }
            else
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle();
        }

        private void UpdateSelectionColors(bool focused)
        {
            DefaultCellStyle.SelectionBackColor = focused ? ThemeProvider.CurrentTheme.AccentSelection : ThemeProvider.CurrentTheme.GreySelection;
            AlternatingRowsDefaultCellStyle.SelectionBackColor = DefaultCellStyle.SelectionBackColor;
        }

        private void SyncScrollBars()
        {
            if (!IsHandleCreated || DesignMode)
                return;

            _isSyncingScroll = true;

            try
            {
                // Layout
                bool bothVisible = VerticalScrollBar.Visible && HorizontalScrollBar.Visible;
                int offset = bothVisible ? 1 : 2;

                _vScrollCover.Bounds = VerticalScrollBar.Bounds;
                _hScrollCover.Bounds = HorizontalScrollBar.Bounds;
                _vScrollCover.Visible = VerticalScrollBar.Visible;
                _hScrollCover.Visible = HorizontalScrollBar.Visible;

                _vScrollBar.Visible = VerticalScrollBar.Visible;
                _vScrollBar.Location = new Point(VerticalScrollBar.Location.X, VerticalScrollBar.Location.Y + 1);
                _vScrollBar.Size = new Size(Consts.ScrollBarSize + 1, VerticalScrollBar.Height - offset);

                _hScrollBar.Visible = HorizontalScrollBar.Visible;
                _hScrollBar.Location = new Point(HorizontalScrollBar.Location.X + 1, HorizontalScrollBar.Location.Y);
                _hScrollBar.Size = new Size(HorizontalScrollBar.Width - offset, Consts.ScrollBarSize + 1);

                _vScrollCover.BringToFront();
                _hScrollCover.BringToFront();
                _vScrollBar.BringToFront();
                _hScrollBar.BringToFront();

                // Values
                _vScrollBar.Minimum = VerticalScrollBar.Minimum;
                _vScrollBar.Maximum = VerticalScrollBar.Maximum;
                _vScrollBar.ViewSize = VerticalScrollBar.LargeChange;
                _vScrollBar.Value = VerticalScrollBar.Value;
                _hScrollBar.Minimum = HorizontalScrollBar.Minimum;
                _hScrollBar.Maximum = HorizontalScrollBar.Maximum;
                _hScrollBar.ViewSize = HorizontalScrollBar.LargeChange;
                _hScrollBar.Value = HorizontalScrollBar.Value;
            }
            finally
            {
                _isSyncingScroll = false;
            }
        }

        private int GetMaxFirstDisplayedRowIndex()
        {
            int available = ClientSize.Height;

            if (ColumnHeadersVisible)
                available -= ColumnHeadersHeight;

            if (HorizontalScrollBar.Visible)
                available -= HorizontalScrollBar.Height;

            int accumulated = 0;
            int lastVisible = -1;

            for (int i = Rows.Count - 1; i >= 0; i--)
            {
                if (!Rows[i].Visible)
                    continue;

                if (lastVisible == -1)
                    lastVisible = i;

                accumulated += Rows[i].Height;

                if (accumulated > available)
                {
                    for (int j = i + 1; j <= lastVisible; j++)
                        if (Rows[j].Visible)
                            return j;

                    return lastVisible;
                }
            }

            return 0;
        }

        // Events

        private void NativeVScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            SyncScrollBars();
        }

        private void NativeHScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            SyncScrollBars();
        }

        private void VScrollBar_ValueChanged(object sender, ScrollValueEventArgs e)
        {
            if (_isSyncingScroll || DesignMode)
                return;

            if (Rows.Count == 0)
                return;

            int target = 0;
            int acc = 0;

            for (int i = 0; i < Rows.Count; i++)
            {
                if (!Rows[i].Visible)
                    continue;

                if (acc + Rows[i].Height > e.Value)
                {
                    target = i;
                    break;
                }

                acc += Rows[i].Height;
                target = i;
            }

            int maxFirst = GetMaxFirstDisplayedRowIndex();
            int maxValue = _vScrollBar.Maximum - _vScrollBar.ViewSize;

            if (e.Value >= maxValue || target > maxFirst)
                target = maxFirst;

            if (target >= 0 && target != FirstDisplayedScrollingRowIndex)
            {
                try
                {
                    FirstDisplayedScrollingRowIndex = target;
                }
                catch
                {
                }
            }
        }

        private void HScrollBar_ValueChanged(object sender, ScrollValueEventArgs e)
        {
            if (_isSyncingScroll || DesignMode)
                return;

            int max = HorizontalScrollBar.Maximum - HorizontalScrollBar.LargeChange + 1;
            HorizontalScrollingOffset = Math.Max(0, Math.Min(e.Value, max));
        }

        // Overrides

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            UpdateTheme();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            UpdateSelectionColors(true);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            UpdateSelectionColors(false);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            SyncScrollBars();
            Invalidate();
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            SyncScrollBars();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SyncScrollBars();
        }

        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            SyncScrollBars();
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            base.OnRowsRemoved(e);
            SyncScrollBars();
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            base.OnColumnAdded(e);
            SyncScrollBars();
        }

        protected override void OnColumnRemoved(DataGridViewColumnEventArgs e)
        {
            base.OnColumnRemoved(e);
            SyncScrollBars();
        }

        protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
        {
            base.OnColumnWidthChanged(e);
            SyncScrollBars();
        }

        protected override void OnRowHeightChanged(DataGridViewRowEventArgs e)
        {
            base.OnRowHeightChanged(e);
            SyncScrollBars();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Fill the little square where both scrollbars meet so it matches the theme.
            if (_vScrollBar.Visible && _hScrollBar.Visible)
            {
                Rectangle corner = new Rectangle(VerticalScrollBar.Left, HorizontalScrollBar.Top, VerticalScrollBar.Width, HorizontalScrollBar.Height);

                using (SolidBrush b = new SolidBrush(ThemeProvider.CurrentTheme.GreyBackground))
                    e.Graphics.FillRectangle(b, corner);
            }

            using (Pen p = new Pen(ThemeProvider.CurrentTheme.DarkBorder))
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);

            if (e.RowIndex == -1 && e.ColumnIndex >= -1)
            {
                bool selected = e.ColumnIndex > -1 ? Columns[e.ColumnIndex].Selected : false;
                Rectangle b = e.CellBounds;
                Color topLine = selected ? ThemeProvider.CurrentTheme.LightAccentBorder : ThemeProvider.CurrentTheme.LightBorder;

                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                using (Pen p = new Pen(topLine))
                    e.Graphics.DrawLine(p, b.Left, b.Top + 1, b.Right - 2, b.Top + 1);

                if (selected)
                {
                    using (Pen p = new Pen(ThemeProvider.CurrentTheme.DarkAccentBorder))
                        e.Graphics.DrawLine(p, b.Left, b.Bottom - 1, b.Right - 2, b.Bottom - 1);
                }

                e.Handled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                VerticalScrollBar.VisibleChanged -= NativeVScrollBar_VisibleChanged;
                HorizontalScrollBar.VisibleChanged -= NativeHScrollBar_VisibleChanged;
                _vScrollBar.ValueChanged -= VScrollBar_ValueChanged;
                _hScrollBar.ValueChanged -= HScrollBar_ValueChanged;
            }

            base.Dispose(disposing);
        }
    }
}
