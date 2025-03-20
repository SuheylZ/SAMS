using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DP = DataProxy;
using Client.Dialogs;
using Ex = ClientHelper.Errors.Tips;

namespace Client.Views
{
    public partial class Customers : Form, IView
    {
        public Customers()
        {
            InitializeComponent();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            lnkRefresh_LinkClicked(null, null);
        }

        public enumViews Type
        {
            get { return enumViews.viewCustomers; }
        }

        public string Title
        {
            get { return "Customers"; }
        }

        private void dgvCustomers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            List<string> lstID = UIHelper.GetSelectedIDs(dgvCustomers, "id");

            try
            {
                if (lstID.Count > 0)
                {
                    DialogResult dr = Ex.Ask("Do you want to delete " + lstID.Count.ToString() + " customer(s)?", "Delete Confirmation");
                    if (dr == DialogResult.Yes)
                    {
                        //Perform Delete
                        foreach (string id in lstID)
                        {
                            if (!DP.Customer.Delete(Convert.ToInt32(id)))
                                throw new Exception("Cant delete. Customer might have placed an order(s)");
                        }
                    }
                }
                else
                {
                    if (dgvCustomers.Rows.Count > 0)
                        Ex.Inform("No Customer Selected", "Use the checkboxes to select the customer(s) and press delete button");
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
                e.Cancel = true;
            }
        }

        private void uC_CustomerSearch1_Event_SearchPerformed(object sender, EventArgs e)
        {
            UIHelper.BindDataGridView(dgvCustomers, uC_CustomerSearch1.Result, true);
        }

        private void uC_CustomerSearch1_Event_SearchCleared(object sender, EventArgs e)
        {
            lnkRefresh_LinkClicked(null, null);
        }

        private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells["id"].Value);
            using (CustomerDetailBox dlg = new CustomerDetailBox())
            {
                dlg.CustomerID = id;
                dlg.ShowDialog(this);
            }
            lnkRefresh_LinkClicked(null, null);
        }

        private void Customers_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Main)this.MdiParent).NotifyClose(enumViews.viewCustomers);
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UIHelper.BindDataGridView(dgvCustomers, DP.Customer.GetAll(), true, "id", "title", "firstname", "midname", "lastname");
            dgvCustomers.Columns[dgvCustomers.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (CustomerDetailBox dlg = new CustomerDetailBox())
            {
                dlg.ShowDialog(this);
            }
            lnkRefresh_LinkClicked(null, null);
        }

    }
}