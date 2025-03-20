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
using DV = ClientHelper.UI;

namespace Client.Views
{
    public partial class Orders : Form, IView
    {
        private const string _sTitle = "Purchase Order";
        
        public Orders()
        {
            InitializeComponent();
        }

        public enumViews Type
        {
            get { return enumViews.viewOrders; }
        }

        public string Title
        {
            get { return _sTitle; }
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            lnkRefresh_LinkClicked(null, null);
        }

        private void uC_OrderSearch1_Event_SearchPerformed(object sender, EventArgs e)
        {
            UIHelper.BindDataGridView(dgvOrders,pnlSearch.Result, true, "id", "customerid");
        }

        private void uC_OrderSearch1_Event_ClearedSearch(object sender, EventArgs e)
        {
            lnkRefresh_LinkClicked(null, null);
        }

        private void dgvOrders_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            List<string> lstID = UIHelper.GetSelectedIDs(dgvOrders, "id");

            if (lstID.Count > 0)
            {
                if (DialogResult.Yes == Ex.Ask("Do you want to delete " + lstID.Count + " Order(s)?", "Delete Confirmation"))
                {
                    try
                    {
                        foreach (string sID in lstID)
                        {
                            DP.Order.Delete(Convert.ToInt32(sID));
                        }
                        lnkRefresh_LinkClicked(null, null);
                    }
                    catch (Exception ex)
                    {
                        Ex.Catch(ex);
                        e.Cancel = true;
                    }
                }
                else
                    e.Cancel = true;
            }
            else
            {
                if (dgvOrders.Rows.Count > 0)
                    Ex.Inform("No Purchase Order Selected", "Use the checkboxes to select the purchase order(s) and press delete button");
                e.Cancel = true;
            }
        }

        private void dgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    using (PurchaseOrderBox dlg = new PurchaseOrderBox())
                    {
                        dlg.OrderID = (int)dgvOrders.Rows[e.RowIndex].Cells["id"].Value;
                        dlg.ShowDialog(this);
                    }
                }
                catch (Exception ex){
                    ClientHelper.Errors.Tips.Catch(ex);
                }
            }
            lnkRefresh_LinkClicked(null, null);
        }

        private void Orders_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Main)this.MdiParent).NotifyClose(enumViews.viewOrders);
        }

        private void lnkNewPurchase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (PurchaseOrderBox dlg = new PurchaseOrderBox())
            {
                try
                {
                    dlg.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    ClientHelper.Errors.Tips.Catch(ex);
                }
            }
            lnkRefresh_LinkClicked(null, null);
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvOrders.Rows.Clear();
            UIHelper.BindDataGridView(dgvOrders, DP.Order.GetAll(), true, "id", "customerid");
        }

        private void Evt_PrintNote(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int id=0;
            try
            {
                if (dgvOrders.CurrentRow != null)
                    id = DV.SafeInt(dgvOrders.CurrentRow.Cells["id"].Value);
                using (Dialogs.PrintBox dlg = new PrintBox())
                {
                    dlg.OrderID = id;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }
        }
    }
}