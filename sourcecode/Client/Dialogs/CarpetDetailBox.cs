using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Carpet = DataProxy.Carpet;
using DV = ClientHelper.UI;
using Tips=ClientHelper.Errors.Tips;
using ClientHelper.Errors;

using DataProxy;

namespace Client.Dialogs
{
    public partial class CarpetDetailBox : Form
    {
        private int _carpetid = 0;

        public CarpetDetailBox()
        {
            InitializeComponent();
        }


        public int CarpetID
        {
            get { return _carpetid; }
            set { _carpetid = value; }
        }


        private void FormClose_Evt(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CarpetAdd_Evt(object sener, EventArgs e)
        {
            // Everything should be blank
        }

        private void CarpetEdit_Evt(object sener, EventArgs e)
        {
            try
            {
                using (IDataReader rd = Carpet.Get(_carpetid))
                {
                    rd.Read();
                    txtCarpet.Text = DV.SafeStr(rd["carpetcode"]);
                    txtNotes.Text = DV.SafeStr(rd["notes"]);
                    rd.Close();
                }
                RefreshGrid();
            }
            catch (Exception ex)
            {
                Tips.Catch(ex);
            }
        }

        private void RefreshGrid()
        {
            dgvDetails.Rows.Clear();
            using (IDataReader rd = Carpet.GetDetail(_carpetid))
            {
                while (rd.Read())
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.Add(CreateCell(rd["id"]));
                    //row.Cells.Add(CreateCell(rd["Colorid"]));
                    //row.Cells.Add(CreateCell(rd["designid"]));
                    row.Cells.Add(CreateCell(rd["Color"]));
                    row.Cells.Add(CreateCell(rd["Backing"]));
                    row.Cells.Add(CreateCell(rd["No"]));
                    row.Cells.Add(CreateCell(rd["Notes"]));
                    dgvDetails.Rows.Add(row);
                }
                rd.Close();
            }
        }

        private DataGridViewTextBoxCell CreateCell(object o)
        {
            string s = "";
            if (o != null || o != DBNull.Value)
                s = o.ToString();

            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = s;
            return cell;
        }

        private void AddCarpet()
        {
            _carpetid = Carpet.Add(txtCarpet.Text.Trim(), txtNotes.Text.Trim());
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (!IsValidRow(row))
                    continue;

                int id = Carpet.AddDetail(_carpetid, DV.SafeStr(row.Cells["color"].Value),
                        DV.SafeStr(row.Cells["backing"].Value),
                        DV.SafeStr(row.Cells["No"].Value),
                        DV.SafeStr(row.Cells["notes"].Value));
                row.Cells["id"].Value = id;
            }
        }

        private void UpdateCarpet()
        {
            Carpet.Update(_carpetid, txtCarpet.Text.Trim(), txtNotes.Text.Trim());
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (!IsValidRow(row))
                    continue;

                int id = DV.SafeInt(row.Cells["id"].Value);
                if (id < 1)
                {
                    //Add the new row
                    id = Carpet.AddDetail(_carpetid, DV.SafeStr(row.Cells["color"].Value),
                            DV.SafeStr(row.Cells["backing"].Value),
                            DV.SafeStr(row.Cells["No"].Value),
                            DV.SafeStr(row.Cells["notes"].Value));
                    row.Cells["id"].Value = id;
                }
                else
                {
                    //Update the exising row
                    Carpet.UpdateDetail(id, _carpetid, DV.SafeStr(row.Cells["color"].Value),
                            DV.SafeStr(row.Cells["backing"].Value),
                            DV.SafeStr(row.Cells["No"].Value),
                            DV.SafeStr(row.Cells["notes"].Value));
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) 
                    return;

                if (_carpetid < 1)
                    AddCarpet();
                else
                    UpdateCarpet();

                RefreshGrid();
                Tips.Inform(btnSave, "Information Saved", "Carpet information has been saved");
            }
            catch (Exception ex)
            {
                Tips.Catch(ex);
            }
        }

        private void CarpetDetail_Load(object sender, EventArgs e)
        {
            if(_carpetid>0)
                CarpetEdit_Evt(this, null);
        }

        private bool ValidateInput()
        {
            bool bAns = true;
            try
            {
                if (txtCarpet.Text.Trim() == "")
                    throw new ControlException(txtCarpet, "Carpet has no name", "Carpet Name Warning");
                if (dgvDetails.Rows.Count < 1)
                    throw new ControlException(dgvDetails, "This carpet has no available types", "Types Missing");
            }
            catch (Exception ex)
            {
                Tips.Catch(ex);
                bAns = false;
            }
            return bAns;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

 

 
 
        private void dgvDetails_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                int id = DV.SafeInt(e.Row.Cells["id"].Value);
                Carpet.DeleteDetail(id);
            }
            catch (Exception ex)
            {
                Tips.Catch(ex);
                e.Cancel = true;
            }
        }

        private void AddRowInDB(DataGridViewRow row)
        {
            int icolor, ibacking;
            icolor = Carpet.AddColor(DV.SafeStr(row.Cells["color"].Value));
            ibacking = Carpet.AddBacking(DV.SafeStr(row.Cells["backing"].Value));
            row.Cells["colorid"].Value = icolor;
            row.Cells["designid"].Value = ibacking;
        }

        private bool IsValidRow(DataGridViewRow row)
        {
            bool bAns = true;

            string color, backing, no;
            color = DV.SafeStr(row.Cells["color"].Value);
            backing = DV.SafeStr(row.Cells["backing"].Value);
            no = DV.SafeStr(row.Cells["No"].Value);

            if ((color == "") || (backing == "") || (no == ""))
                bAns = false;

            return bAns;
        }
    };
}