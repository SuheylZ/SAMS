using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DP = DataProxy;
using Client.Dialogs;
using Carpet = DataProxy.Carpet;
using DV = ClientHelper.UI;
using UI = Client.UIHelper;
using Ex = ClientHelper.Errors.Tips;


namespace Client.Views
{
    public partial class Carpets : Form, IView
    {
        private const string _sTitle = "Carpets";
        
        public Carpets()
        {
            InitializeComponent();
        }

        public enumViews Type
        {
            get { return enumViews.viewCarpets; }
        }

        public string Title
        {
            get { return _sTitle; }
        }

        private void Carpets_Load(object sender, EventArgs e)
        {
            lnkRefresh_LinkClicked(this, null);
        }

        private void Carpets_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Main)this.MdiParent).NotifyClose(enumViews.viewCarpets);
        }
        
        private void lnkAddCarpet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (CarpetDetailBox dlg = new CarpetDetailBox())
            {
                dlg.ShowDialog(this);
            }
            lnkRefresh_LinkClicked(sender, e);
        }
        
        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvCarpet.Rows.Clear();
            UIHelper.BindDataGridView(dgvCarpet, Carpet.GetAll(), true, "id");
            dgvCarpet.Columns[dgvCarpet.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if(dgvCarpet.Rows.Count >0)
                dgvCarpet.Rows[0].Selected = true;
        }

        private void dgvCarpet_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            List<string> lstID = UIHelper.GetSelectedIDs(dgvCarpet, "id");
            if (lstID.Count > 0)
            {
                if (Ex.Ask("Do you wish to delete " + lstID.Count.ToString() + " Carpet(s)?", "Delete Confirmation") == DialogResult.Yes)
                {
                    try
                    {
                        foreach (string sid in lstID)
                        {
                            Carpet.Delete(Convert.ToInt32(sid));
                        }
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
                if (dgvCarpet.Rows.Count > 0)
                    Ex.Inform("No Carpet Selected", "Use the checkboxes to select the carpet(s) and press delete button");
                e.Cancel = true;
            }
        }

        private void dgvCarpet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvCarpet.Rows[e.RowIndex].Cells["id"].Value);
                using (CarpetDetailBox dlg = new CarpetDetailBox())
                {
                    dlg.CarpetID = id;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }

            lnkRefresh_LinkClicked(null, null);
        }

        private void dgvCarpet_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvCarpet.CurrentRow == null)
                    return;

                int id = DV.SafeInt(dgvCarpet.CurrentRow.Cells["id"].Value);
                dgvDetails.Rows.Clear();
                UI.BindDataGridView(dgvDetails, Carpet.GetDetail(id), false, "id", "colorid", "designid");
                if(dgvDetails.Columns.Count>1)
                    dgvDetails.Columns[dgvDetails.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }
        }


    
    };
}