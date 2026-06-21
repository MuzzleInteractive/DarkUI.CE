using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Forms;
using System;
using System.Data;
using System.Windows.Forms;

namespace DarkUI.Demo.Forms
{
    public partial class TestForm : DarkForm
    {
        public TestForm()
        {
            InitializeComponent();

            //foreach (DataGridViewColumn c in darkDataGridView1.Columns)
            //{
            //    c.SortMode = DataGridViewColumnSortMode.NotSortable;
            //    c.Selected = false;
            //}
            //darkDataGridView1.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
            //darkDataGridView1.Columns[0].Selected = true;


            darkDataGridView1.Columns.Clear();
            darkDataGridView1.DataSource = null;
            darkDataGridView1.BindingContextChanged += DGV_Lista_BindingContextChanged;
            darkDataGridView1.DataBindingComplete += DGV_Lista_DataBindingComplete;
            var employees = new DataTable();
            employees.Columns.Add("Id", typeof(int));
            employees.Columns.Add("Name", typeof(string));
            employees.Columns.Add("Department", typeof(string));
            employees.Columns.Add("Salary", typeof(decimal));
            employees.Columns.Add("HireDate", typeof(DateTime));
            employees.Columns.Add("IsActive", typeof(bool));

            employees.Rows.Add(1, "Alice Johnson", "Engineering", 95_000m, new DateTime(2019, 3, 15), true);
            employees.Rows.Add(2, "Bob Martinez", "Marketing", 72_500m, new DateTime(2020, 7, 1), true);
            employees.Rows.Add(3, "Carol White", "Engineering", 88_000m, new DateTime(2018, 1, 22), false);
            employees.Rows.Add(4, "David Kim", "HR", 65_000m, new DateTime(2021, 9, 10), true);
            employees.Rows.Add(5, "Eva Rossi", "Finance", 79_000m, new DateTime(2017, 5, 30), true);

            var bindingSource = new BindingSource { DataSource = employees };
            darkDataGridView1.DataSource = bindingSource;
        }

        private void DGV_Lista_BindingContextChanged(object sender, EventArgs e)
        {
            if (darkDataGridView1.DataSource is null)
                return;

            foreach (DataGridViewColumn c in darkDataGridView1.Columns)
                c.HeaderCell = new DarkDataGridViewAutoFilterColumnHeaderCell(c.HeaderCell);

            darkDataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void DGV_Lista_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //string filtro = DarkDataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(darkDataGridView1);

            //if (string.IsNullOrEmpty(filtro))
            //{
            //    LBL_RemoverFiltro.Visible = false;

            //    BindingSource bs = (DGV.DataSource as BindingSource);
            //    string filter = bs.Filter;

            //    bs.RaiseListChangedEvents = false;
            //    bs.Filter = "";
            //    LBL_Filtro.Text = bs.Count + " linhas";
            //    bs.Filter = filter;
            //    bs.RaiseListChangedEvents = true;
            //}
            //else
            //{
            //    LBL_RemoverFiltro.Visible = true;
            //    LBL_Filtro.Text = filtro;
            //}
        }


        private void BTNOpenMainForm_Click(object sender, EventArgs e)
        {
            MainForm frm = new MainForm();
            frm.Show();
        }

        private void BTNSystemTheme_Click(object sender, EventArgs e)
        {
            ThemeProvider.CurrentTheme = ThemeProvider.Themes["System"];
        }

        private void BTNLightTheme_Click(object sender, EventArgs e)
        {
            ThemeProvider.CurrentTheme = ThemeProvider.Themes["Light"];
        }

        private void BTNDarkTheme_Click(object sender, EventArgs e)
        {
            ThemeProvider.CurrentTheme = ThemeProvider.Themes["Dark"];
        }
    }
}
