using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System;


namespace Client
{
    public interface IAction
    {
        void Initialize();
        void Execute();
        void Cleanup();
    }


    public enum enumViews
    {
        viewNone = 0,
        viewCarpets = 1,
        viewOrders = 2,
        viewHome = 3,
        viewCustomers = 4,
    };

    public interface IView
    {
        enumViews Type{ get; }
        string Title { get; }
    };

    internal class UIHelper
    {
        public static void BindDataGridView(DataGridView dgv, IDataReader rd, bool bCheckBoxes, params string[] ary)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            if(bCheckBoxes)
                dgv.RowHeadersVisible = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = rd;
            dgv.DataSource = bs;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                string title = ToSentenceCase(column.HeaderText);
                column.HeaderText = title;
                foreach (string th in ary)
                    if (title.ToUpper() == th.ToUpper().Trim())
                    {
                        column.Visible = false;
                        break;
                    }
                column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.ColumnHeader, false);
            }
            if (bCheckBoxes)
            {
                DataGridViewColumn col = new DataGridViewCheckBoxColumn();
                col.HeaderCell = new ClientHelper.DatagridViewCheckBoxHeaderCell();
                col.Resizable = DataGridViewTriState.False;
                col.Width = 20;
                dgv.Columns.Insert(0, col);
            }
        }

        private static string ToSentenceCase(string str)
        {
            string title = str.Trim();
            if (title.Length > 0)
                title = char.ToUpper(title.ToCharArray()[0]) + title.Substring(1);
            return title;
        }

        public static List<string> GetSelectedIDs(DataGridView dgv, string idName)
        {
            List<string> lstValues = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[0] is DataGridViewCheckBoxCell)
                {
                    DataGridViewCheckBoxCell cbc = (row.Cells[0] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(cbc.EditedFormattedValue))
                    {
                        lstValues.Add(Convert.ToString(row.Cells[idName].Value));
                    }
                }
            }
            return lstValues;
        }
    };
}