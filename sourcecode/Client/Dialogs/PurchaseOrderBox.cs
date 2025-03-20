using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DP = DataProxy;
using PaymentOption = DataProxy.enumPaymentMethod;
using DV = ClientHelper.UI;
using Ex = ClientHelper.Errors.Tips;
using ClientHelper;


namespace Client.Dialogs
{
    public partial class PurchaseOrderBox : Form
    {
        private string[] _titles = { "Mr", "Mrs", "Miss", "Madam", "Sir", "Dr", "Ms" };
        private string[] _doorbars = { "Single", "Double", "Cover", "Z Section" };

        private int _orderID = 0;
        private int _custid = 0;
        private bool _bEditing = false;

        private ClientHelper.MessageBalloon _tip;

        #region Stable
        public PurchaseOrderBox()
        {
            InitializeComponent();
        }
        
        //private void btnAddNewCarpet_Click(object sender, EventArgs e)
        //{
        //    using (CarpetDetailBox dlg = new CarpetDetailBox())
        //    {
        //        dlg.ShowDialog(this);
        //    }
        //}

        public int OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        public void Initialize()
        {
            //Initialize Titles
            cbTitle.Items.Clear();
            foreach (string item in _titles)
                cbTitle.Items.Add(item);
            cbTitle.SelectedIndex = 0;
            
            //Initialize Order Detail
            dgvOrderDetail.Rows.Clear();

            dtPurchase.Value = DateTime.Now;
            dtFitting.Value = DateTime.Now;
            dtMeasurement.Value = DateTime.Now;

            //Initialize Payment method
            ddMethod.Items.Add(new ImageComboItem("American Express", 0, PaymentOption.pmAMEX));
            ddMethod.Items.Add(new ImageComboItem("Visa", 1, PaymentOption.pmVisa));
            ddMethod.Items.Add(new ImageComboItem("Master Card", 2, PaymentOption.pmMaster));
            ddMethod.Items.Add(new ImageComboItem("Cash", 3, PaymentOption.pmCash));
            ddMethod.Items.Add(new ImageComboItem("Cheque", 4, PaymentOption.pmCheque));

            //Doorbars Grid
            InitializeDoorBars();
        }

        private void InitializeDoorBars()
        {
            foreach (string item in _doorbars)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                cell.Value = item;
                row.Cells.Add(cell);
                for (int i = 0; i < 2; i++)
                {
                    cell = new DataGridViewTextBoxCell();
                    cell.Value = "";
                    row.Cells.Add(cell);
                }
                dgvDoorbar.Rows.Add(row);
            }
        }

        //public void Cleanup()
        //{
        //    Dispose();
        //}

        private void PurchaseOrder_Load(object sender, EventArgs e)
        {
            Initialize();
            if (_orderID > 0)
            {
                LoadRecord();
                btnPrint.Enabled = true;
            }
        }

        private void LoadRecord()
        {
            try
            {
                _custid =LoadOrder(_orderID);
                LoadCustomer(_custid);
                LoadOrderDetail(_orderID);
                _bEditing = true;
            }
            catch (Exception ex)
            {
               Ex.Catch(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChoices())
                return;

            try
            {
                if (_bEditing)
                {
                    DP.Customer.Update(_custid, CustomerTitle, txtFirst.Text.Trim(),
                        txtLast.Text.Trim(), txtFamily.Text.Trim(), txtAddress.Text.Trim(),
                        txtPostalCode.Text, txtPhone.Text.Trim(), txtCell.Text.Trim());

                    DP.Order.Update(_orderID, txtReferenceNo.Text.Trim(), dtPurchase.Value,
                        _custid, dtFitting.Value, dtMeasurement.Value,
                        txtUnderlayType.Text.Trim(), txtUnderlayQty.Text.Trim(), txtRequest.Text.Trim(),
                        PaymentMethod, DV.SafeDbl(txtDeposit.Text.Trim()), DV.SafeDbl(txtTotal.Text.Trim()), DV.SafeDbl(txtBalance.Text.Trim()));

                    SaveOrderDetail(_orderID);
                    SaveDoorBar(_orderID);
                    ShowTip(btnSave, "Order Saved Successfully!", "Modified information has been stored in the system");
                }
                else
                {
                    _custid = SaveCustomer();
                    _orderID = SaveOrder(_custid);
                    SaveDoorBar(_orderID);
                    SaveOrderDetail(_orderID);
                    txtOrderNo.Text = _orderID.ToString();
                    ShowTip(txtOrderNo, "Order Saved Successfully!", "Your new order number generated by the system");
                }
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Ex.Catch(ex); 
            }

            //Deal with Customer
        //    int icustid = 0;
        //    if (rbCustomerNew.Checked)
        //    {
        //        icustid = DP.Customer.Add(cbTitle.SelectedItem.ToString(), txtFirst.Text, txtFamily.Text, txtLast.Text, txtAddress.Text,txtPostalCode.Text,
        //            txtPhone.Text, txtCell.Text);
        //    }
        //    else
        //    {
        //        icustid = Convert.ToInt32(cbCustomer.SelectedValue);
        //    }

        //    //Deal With order 
        //    double ddeposit = 0.0, dtotal = 0.0;
        //    ddeposit = Convert.ToDouble(txtDeposit.Text);
        //    dtotal = (double)(Convert.ToDouble(txtDeposit.Text) + Convert.ToDouble(txtBalance.Text));
        //    int iorder = DP.Order.Add(icustid, dtPurchase.Value, PaymentValue(), ddeposit, dtotal );
            

        //    // Deal with order detail
        //    foreach (DataGridViewRow row in dgvOrderDetail.Rows)
        //    {
        //        int icarpet = 0, idesign = 0, icolor = 0, iqty = 0, icarpetdetail=0;
        //        double flength = 0.0f, fbreadth = 0.0f, fcost = 0.0f;
                
        //        icarpet = Convert.ToInt32(GetSelectedValue(row, 0));
        //        idesign = Convert.ToInt32(GetSelectedValue(row, 1));
        //        icolor = Convert.ToInt32(GetSelectedValue(row, 2));
                
        //        iqty = Convert.ToInt32(row.Cells[3].Value);
        //        flength = Convert.ToDouble(row.Cells[4].Value);
        //        fbreadth = Convert.ToDouble(row.Cells[5].Value);
        //        fcost = Convert.ToDouble(row.Cells[6].Value);

        //        if (icarpet == 0 && idesign == 0 && icolor == 0)
        //        {
        //            //this must be the culprit row
        //            break;
        //        }
        //        else
        //        {
        //            icarpetdetail = DP.Carpet.FindDetailORAdd(icarpet, idesign, icolor);
        //            DP.Order.AddDetail(iorder, icarpetdetail, iqty, flength, fbreadth, fcost);
        //        }
        //        _orderID = iorder;
        //        MessageBox.Show("The order has been saved. \nOrder id: " + _orderID.ToString());
        //    }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Evt_PrintPreview(object o, EventArgs e)
        {
            using (PrintBox dlg = new PrintBox())
            {
                dlg.OrderID = _orderID;
                dlg.ShowDialog(this);
            }
        }
        #endregion


        //private void AddOrderDetail(bool bAutoAdd)
        //{
        //    //int irow = 0;
        //    //if(bAutoAdd)
        //    //    dgvOrderDetail.Rows.Add(1);

        //    //irow = dgvOrderDetail.Rows.Count -1;

        //    //using (IDataReader rd = DP.Carpet.GetAll(DP.enumEntityType.etCarpet))
        //    //{
        //    //    BindingSource bs = new BindingSource();
        //    //    bs.DataSource = rd;
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[0, irow]).DataSource = bs;
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[0, irow]).DisplayMember = "Carpet Code";
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[0, irow]).ValueMember = "carpetid";
        //    //}

        //    //using (IDataReader rd = DP.Carpet.GetAll(DataProxy.enumEntityType.etCarpetDesign))
        //    //{
        //    //    BindingSource bs = new BindingSource();
        //    //    bs.DataSource = rd;
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[1, irow]).DataSource = bs;
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[1, irow]).DisplayMember = "DesignCode";
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[1, irow]).ValueMember = "id";
        //    //}

        //    //using (IDataReader rd = DP.Carpet.GetAll(DataProxy.enumEntityType.etCarpetColor))
        //    //{
        //    //    BindingSource bs = new BindingSource();
        //    //    bs.DataSource = rd;
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[2, irow]).DataSource = bs;
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[2, irow]).DisplayMember = "ColorCode";
        //    //    ((DataGridViewComboBoxCell)dgvOrderDetail[2, irow]).ValueMember = "id";
        //    //}
        //}


        

        //private DP.enumPaymentMethod PaymentValue()
        //{
        //    DP.enumPaymentMethod eret = DataProxy.enumPaymentMethod.pmUnknown;

        //    if (rbCheque.Checked) eret = DataProxy.enumPaymentMethod.pmCheque;
        //    if (rbCash.Checked) eret = DataProxy.enumPaymentMethod.pmCash;
        //    if (rbMaster.Checked) eret = DataProxy.enumPaymentMethod.pmMaster;
        //    if (rbVisa.Checked) eret = DataProxy.enumPaymentMethod.pmVisa;
        //    return eret;
        //}

        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    if (_orderID < 1)
        //        MessageBox.Show("Order has not been saved, Please save the order");
        //    else
        //    {
        //        Dialogs.PrintForm ps = new Client.Dialogs.PrintForm();
        //        ps.OrderID = _orderID;
        //        ps.ShowDialog(this);
        //        ps.Dispose();
        //    }

        //}


        private PaymentOption PaymentMethod
        {
            get
            {
                ImageComboItem it = (ImageComboItem)ddMethod.Items[ddMethod.SelectedIndex];
                return (PaymentOption)it.Tag;
            }
            set
            {
                foreach (ImageComboItem it in ddMethod.Items)
                {
                    if ((PaymentOption)it.Tag == value)
                    {
                        ddMethod.SelectedItem = it;
                        break;
                    }
                }
            }
        }
        private string CustomerTitle
        {
            get
            {
                string sTitle = cbTitle.SelectedItem.ToString().Trim();
                if (sTitle == "")
                    sTitle = _titles[0];

                return sTitle;
            }
            set
            {
                if (value == "")
                    value = _titles[0];
                foreach (object o in cbTitle.Items)
                {
                    if (o.ToString() == value)
                    {
                        cbTitle.SelectedItem = o;
                        break;
                    }
                }
            }
        }
        private double GetDouble(MaskedTextBox tt)
        {
            string stext = tt.Text.Trim();
            if (stext == "") stext = "0.0";
            return Convert.ToDouble(stext);
        }
        private object GetSelectedValue(DataGridViewRow row, int iColumn)
        {
            object obj = null;

            obj = ((DataGridViewComboBoxCell)row.Cells[iColumn]).Value;
            return obj;
        }

        private int SaveCustomer()
        {
            bool bSave = true;

            int id = DP.Customer.Exists(txtFirst.Text.Trim(), txtLast.Text.Trim());
            if (id > 0)
            {
                if (DialogResult.Yes == MessageBox.Show("Customer " + txtFirst.Text + " " + txtLast.Text + " already exists. Use the existing one?", "Customer Already Present", MessageBoxButtons.YesNo))
                    bSave = false;
            }
            if (bSave)
            {
                id = DP.Customer.Add(CustomerTitle, txtFirst.Text.Trim(), txtFamily.Text.Trim(), txtLast.Text.Trim(), txtAddress.Text.Trim(),
                        txtPostalCode.Text, txtPhone.Text.Trim(), txtCell.Text.Trim());
            }
            return id;
        }
        private int SaveOrder(int customerid)
        {
            int id = -1;
            double dDeposit = DV.SafeDbl(txtDeposit.Text), dTotal = DV.SafeDbl(txtTotal.Text),
                dbalance = DV.SafeDbl(txtBalance.Text);
                
            id = DP.Order.Add(customerid, dtPurchase.Value, txtReferenceNo.Text.Trim(), dtFitting.Value, dtMeasurement.Value, 
                txtRequest.Text.Trim(), txtUnderlayType.Text.Trim(),  txtUnderlayQty.Text.Trim(), 
                PaymentMethod, dDeposit, dTotal, dbalance);
            return id;
        }

        private void SaveOrderDetail(int iorder)
        {
            DP.Order.ClearDetail(iorder);
            foreach (DataGridViewRow row in dgvOrderDetail.Rows)
            {
                try
                {
                    int icarpet = DV.SafeInt(row.Cells[0].Value);
                    int icolor = DV.SafeInt(row.Cells[1].Value);
                    int ibacking = DV.SafeInt(row.Cells[2].Value);
                    string No = DV.SafeStr(row.Cells[3].Value);
                    int idetail = DP.Carpet.ExistsNo(icarpet, icolor, ibacking, No);
                    
                    if(idetail<1)
                        throw new Exception("Carpet detail does not exist. Internal System error");

                    double length = DV.SafeDbl(row.Cells[4].Value);
                    double breadth = DV.SafeDbl(row.Cells[5].Value);
                    int iqty = 0;           //DV.SafeInt(row.Cells[6].Value);
                    double fcost = 0.0f;    // DV.SafeDbl(row.Cells[7].Value);
                    string notes = DV.SafeStr(row.Cells[8].Value);

                    DP.Order.AddDetail(iorder, idetail, length, breadth, iqty, fcost, notes);

                }
                catch (Exception ex)
                {
                    Ex.Debug(ex);
                }
            }
        }
        private void SaveDoorBar(int id)
        {
            string sstd, sstp, dstd, dstp, cstd, cstp, zstd, zstp;

            sstd = DV.SafeStr(dgvDoorbar.Rows[0].Cells["STD"].Value);
            sstp = DV.SafeStr(dgvDoorbar.Rows[0].Cells["STP"].Value);

            dstd = DV.SafeStr(dgvDoorbar.Rows[1].Cells["STD"].Value);
            dstp = DV.SafeStr(dgvDoorbar.Rows[1].Cells["STP"].Value);

            cstd = DV.SafeStr(dgvDoorbar.Rows[2].Cells["STD"].Value);
            cstp = DV.SafeStr(dgvDoorbar.Rows[2].Cells["STP"].Value);

            zstd = DV.SafeStr(dgvDoorbar.Rows[3].Cells["STD"].Value);
            zstp = DV.SafeStr(dgvDoorbar.Rows[3].Cells["STP"].Value);

            DP.Order.SaveDoorBars(id, sstd, sstp, dstd, dstp, cstd, cstp, zstd, zstp);
        }
        
        private bool ValidateChoices()
        {
            double dDeposit=GetDouble(txtDeposit), dTotal=GetDouble(txtTotal);
            bool bRet = true;

            //Stage 1
            if (txtReferenceNo.Text.Trim() == "")
            {
                ShowError(txtReferenceNo, "Reference No Missing", "Eneter a reference no for this order");
                bRet = false;
            }

            //Stage 2
            if (txtFirst.Text.Trim() == "")
            {
                ShowError(txtFirst, "Name Missing", "Customer first name is missing");
                bRet = false;
            }

            //Stage 3
            if (ddMethod.SelectedIndex == -1)
            {
                ShowError(ddMethod, "No Method Selected", "Select a payment method");
                bRet = false;
            }

            if(dTotal == 0.0)
            {
                tbPayment.SelectedIndex = 1;
                ShowError(txtTotal, "Error in Calculation", "No amount specified");
                bRet = false;
            }

            if (dDeposit > dTotal)
            {
                tbPayment.SelectedIndex = 1;
                ShowError(txtTotal, "Error in Calculation", "Deposit exceeds the total amount");
                bRet = false;
            }
            else
            {
                txtBalance.Text = Convert.ToString((dTotal - dDeposit));
            }

            return bRet;
        }

        private void ShowError(Control ct, string title, string text)
        {
            _tip = new ClientHelper.MessageBalloon(ct);
            _tip.Title = title;
            _tip.Text = text;
            _tip.TitleIcon = ClientHelper.TooltipIcon.Warning;
            _tip.Show();
        }
        private void ShowTip(Control ct, string title, string text)
        {
            _tip = new ClientHelper.MessageBalloon(ct);
            _tip.Title = title;
            _tip.Text = text;
            _tip.TitleIcon = ClientHelper.TooltipIcon.Info;
            _tip.Show();
        }

        private void LoadCustomer(int id)
        {
            using (IDataReader rd = DP.Customer.Get(id))
            {
                while (rd.Read())
                {
                    CustomerTitle = DV.SafeStr(rd["title"]);
                    txtFirst.Text = DV.SafeStr(rd["firstname"]);
                    txtFamily.Text = DV.SafeStr(rd["midname"]);
                    txtLast.Text = DV.SafeStr(rd["lastname"]);
                    txtAddress.Text = DV.SafeStr(rd["address"]);
                    txtPostalCode.Text = DV.SafeStr(rd["postalcode"]);
                    txtPhone.Text = DV.SafeStr(rd["phone"]);
                    txtCell.Text = DV.SafeStr(rd["cell"]);

                    break;
                }
                rd.Close();
            }
        }
        private int LoadOrder(int id)
        {
            int icustid = 0;
            
            //InitializeDoorBars();
            using (IDataReader rd = DP.Order.Get(id))
            {
                while (rd.Read())
                {
                    icustid= DV.SafeInt(rd["customerid"]);
                    txtOrderNo.Text = DV.SafeInt(rd["id"]).ToString();
                    txtReferenceNo.Text = DV.SafeStr(rd["referenceno"]);
                    dtPurchase.Value = DV.SafeDate(rd["purchasedate"]);

                    PaymentMethod = (PaymentOption)DV.SafeInt(rd["paymentmethod"]);
                    txtDeposit.Text = DV.SafeDbl(rd["deposit"]).ToString();
                    txtBalance.Text = DV.SafeDbl(rd["balance"]).ToString();
                    txtTotal.Text = DV.SafeDbl(rd["total"]).ToString();

                    dtFitting.Value = DV.SafeDate(rd["fittingdate"]);
                    dtMeasurement.Value = DV.SafeDate(rd["measurementdate"]);

                    txtUnderlayType.Text = DV.SafeStr(rd["underlaytype"]);
                    txtUnderlayQty.Text = DV.SafeStr(rd["underlayqty"]);
                    txtRequest.Text = DV.SafeStr(rd["request"]);

                    //Load DoorBar Grid
                    LoadDoorBarGridRow(0, rd["DBSingleSTD"], rd["DBSingleSTP"]);
                    LoadDoorBarGridRow(1, rd["DBDoubleSTD"], rd["DBDoubleSTP"]);
                    LoadDoorBarGridRow(2, rd["DBCoverSTD"], rd["DBCoverSTP"]);
                    LoadDoorBarGridRow(3, rd["DBZSectionSTD"], rd["DBZSectionSTP"]);
                    /////////////////

                    break;
                }
                rd.Close();
            }
            return icustid;
        }
        private void LoadOrderDetail(int id)
        {
            bool bhasdata = false;

            dgvOrderDetail.Rows.Clear();

            using (IDataReader rd = DP.Order.GetDetail(id))
            {
                int i = 0;
                while (rd.Read())
                {
                    bhasdata = true;
                    
                    //dgvOrderDetail.Rows.Insert(dgvOrderDetail.Rows.Count, 1);
                    dgvOrderDetail.Rows.Add(1);
                    DataGridViewRow row = dgvOrderDetail.Rows[i];//dgvOrderDetail.Rows[dgvOrderDetail.Rows.Count - 1];
                    int icarpetdetail = DV.SafeInt(rd["carpetdetailid"]);
                    using (IDataReader rd2 = DP.Carpet.GetDetailByDetailID(icarpetdetail))
                    {
                        while (rd2.Read())
                        {
                            int icarpet = DV.SafeInt(rd2["carpetid"]),  
                                icolor = DV.SafeInt(rd2["colorid"]), ibacking = DV.SafeInt(rd2["backingid"]);
                            string sNo = DV.SafeStr(rd2["No"]);

                            BindComboBoxCell(row.Cells[0], DP.Carpet.GetAll(), "carpet", "id", icarpet);
                            BindComboBoxCell(row.Cells[1], DP.Carpet.GetColorsFor(icarpet), "color", "id", icolor);
                            BindComboBoxCell(row.Cells[2], DP.Carpet.GetBackingFor(icarpet, icolor), "backing", "id", ibacking);
                            BindComboBoxCell(row.Cells[3], DP.Carpet.GetNosFor(icarpet, icolor, ibacking), "No", "No", sNo);

                            row.Cells[4].Value = DV.SafeDbl(rd["length"]);
                            row.Cells[5].Value = DV.SafeDbl(rd["breadth"]);

                            //row.Cells[6].Value = (DV.SafeInt(rd["length"]) * DV.SafeDbl(rd["breadth"]))/9;
                            //row.Cells[7].Value = DV.SafeDbl(rd["cost"]);
                            row.Cells[8].Value = DV.SafeStr(rd["notes"]);
                            FinalizeRow(row);
                        }
                        rd2.Close();
                        i++;
                    }
                    
                }
                rd.Close();
            }
            if (!bhasdata)
                dgvOrderDetail.Rows.Add(1);

            try
            {
               // dgvOrderDetail.Rows.RemoveAt(0);
            }
            catch { }

        }
        private void LoadDoorBarGridRow(int i, object std, object stp)
        {
            // IMPORTANT: DO NOT advance or close the Data reader
            // fetch the values and let it go
            DataGridViewRow row = dgvDoorbar.Rows[i];
            row.Cells["STD"].Value = DV.SafeStr(std);
            row.Cells["STP"].Value = DV.SafeStr(stp);
        }


        private DataGridViewComboBoxCell CreateCarpetCell(DataGridViewComboBoxCell cell)
        {
            if(cell==null)
                cell = new DataGridViewComboBoxCell();
            using (IDataReader rd = DP.Carpet.GetAll())
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = rd;
                cell.DisplayMember = "Carpet";
                cell.ValueMember = "id";
                cell.DataSource = bs;
            }
            return cell;
        }
        private DataGridViewComboBoxCell CreateColorCell(DataGridViewComboBoxCell cell, int carpet)
        {
            if (cell == null)
                cell = new DataGridViewComboBoxCell();
            try
            {
                using (IDataReader rd = DP.Carpet.GetColorsFor(carpet))
                {
                    BindingSource bs = new BindingSource();
                    bs.DataSource = rd;
                    cell.DataSource = bs;
                    cell.DisplayMember = "color";
                    cell.ValueMember = "id";

                }
            }
            catch (Exception ex)
            {
                Ex.Debug(ex);
            }
            return cell;
        }
        private DataGridViewComboBoxCell CreateBackingCell(DataGridViewComboBoxCell cell ,int carpet, int color)
        {
            if(cell==null)
                cell = new DataGridViewComboBoxCell();
            try
            {
                using (IDataReader rd = DP.Carpet.GetBackingFor(carpet, color))
                {
                    BindingSource bs = new BindingSource();
                    bs.DataSource = rd;
                    cell.DataSource = bs; 
                    cell.DisplayMember = "backing";
                    cell.ValueMember = "id";

                }
            }
            catch (Exception ex)
            {
                Ex.Debug(ex);
            }

            return cell;
        }
        private DataGridViewComboBoxCell CreateNoCell(DataGridViewComboBoxCell cell, int carpet, int color, int backing)
        {
            if(cell==null)
                cell = new DataGridViewComboBoxCell();

            try
            {
                using (IDataReader rd = DP.Carpet.GetNosFor(carpet, color, backing))
                {
                    BindingSource bs = new BindingSource();
                    bs.DataSource = rd;
                    cell.DataSource = bs;
                    cell.DisplayMember = "no";
                    cell.ValueMember = "no";

                }
            }
            catch (Exception ex)
            {
                Ex.Debug(ex);
            }

            return cell;
        }

        [System.Diagnostics.DebuggerStepThrough()]
        private int CurrentCellIndex()
        {
            return dgvOrderDetail.CurrentCell.ColumnIndex;
        }
        [System.Diagnostics.DebuggerStepThrough()]
        private int CurrentRowIndex()
        {
            return dgvOrderDetail.CurrentRow.Index;
        }




        private ComboBox _ddcarpet, _ddcolor, _ddbacking;
        private int _icarpet, _icolor,_ibacking;

        private void dgvOrderDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = null;
//            DataGridViewRow row = dgvOrderDetail.Rows[dgvOrderDetail.Rows.Count - 1];

            
            try
            {
                row = dgvOrderDetail.Rows[e.RowIndex];
                //if (row.Index > 1)
                //    FinalizeRow(dgvOrderDetail.Rows[row.Index - 1]);
                CreateCarpetCell(row.Cells[0] as DataGridViewComboBoxCell);
                CreateColorCell(row.Cells[1] as DataGridViewComboBoxCell, 0);
                CreateBackingCell(row.Cells[2] as DataGridViewComboBoxCell, 0, 0);
                CreateNoCell(row.Cells[3] as DataGridViewComboBoxCell, 0, 0, 0);
                row.Cells[4].Value = "0.0";
                row.Cells[5].Value = "0.0";
                row.Cells[6].Value = "0.0";
                row.Cells[7].Value = "0.0";
            }
            catch (Exception ex)
            {
               Ex.Catch(ex);
            }

        }
        private void dgvOrderDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            switch (CurrentCellIndex())
            {
                case 0:
                    _ddcarpet = (ComboBox)e.Control;
                    _ddcarpet.Leave -= new EventHandler(Evt_CarpetLeave);
                    _ddcarpet.Leave += new EventHandler(Evt_CarpetLeave);
                    break;

                case 1:
                    _ddcolor = (ComboBox)e.Control;
                    BindComboBox(_ddcolor, DP.Carpet.GetColorsFor(_icarpet), "color", "id");
                    _ddcolor.Leave -= new EventHandler(Evt_ColorLeave);
                    _ddcolor.Leave += new EventHandler(Evt_ColorLeave);
                    break;

                case 2:
                    _ddbacking = (ComboBox)e.Control;
                    BindComboBox(_ddbacking, DP.Carpet.GetBackingFor(_icarpet, _icolor), "backing", "id");
                    _ddbacking.Leave -= new EventHandler(Evt_BackingLeave);
                    _ddbacking.Leave += new EventHandler(Evt_BackingLeave);
                    break;

                //case 3:
                //    _ddno = (ComboBox)e.Control;
                //    BindComboBox(_ddno, DP.Carpet.GetNosFor(_icarpet, _icolor, _ibacking), "no", "no");
                //    _ddbacking.Leave -= new EventHandler(Evt_NoLeave);
                //    _ddbacking.Leave += new EventHandler(Evt_NoLeave);
                //    break;
            }

        }
        private void dgvOrderDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }
       
        //private void Evt_CarpetIndexChanged(object o, EventArgs e)
        //{
        //    int i = Convert.ToInt32(_ddcarpet.SelectedValue);

        //    if (i == 0)
        //        return;
        //    try
        //    {
        //        BindingSource bs = new BindingSource();
        //        bs.DataSource = DP.Carpet.GetColorsFor(i);
        //        (dgvOrderDetail.Rows[CurrentRowIndex()].Cells[1] as DataGridViewComboBoxCell).DataSource = bs;
        //        (dgvOrderDetail.Rows[CurrentRowIndex()].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "color";
        //        (dgvOrderDetail.Rows[CurrentRowIndex()].Cells[1] as DataGridViewComboBoxCell).ValueMember = "id";
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientHelper.Errors.Tips.Catch(ex);
        //    }
        //}


        private void BindComboBox(ComboBox cb, IDataReader rd, string display, string value)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = rd;
            cb.DataSource = bs;
            cb.DisplayMember = display;
            cb.ValueMember = value;
        }
        private void BindComboBoxCell(DataGridViewCell cell, IDataReader rd, string display, string value, object selectedvalue)
        {
            DataGridViewComboBoxCell cb = null;

            if(cell is DataGridViewComboBoxCell)
                cb= (cell as DataGridViewComboBoxCell);

            try
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = rd;
                cb.DataSource = bs;
                cb.DisplayMember = display;
                cb.ValueMember = value;
                cb.Value = selectedvalue;
            }
            catch { }
        }
        private void FinalizeRow(DataGridViewRow row)
        {
            // 6 = SQ Yards , 7 = Sq Meters

            try
            {
                double sqYards = (DV.SafeDbl(row.Cells[4].Value) * DV.SafeDbl(row.Cells[5].Value))/9;
                double sqMeters = sqYards / 1.196;
                row.Cells[6].Value = sqYards.ToString();
                row.Cells[7].Value = sqMeters.ToString();
            }
            catch(Exception ex)
            {
                Ex.Debug(ex);    
            }

        }

        private void Evt_CarpetLeave(object o, EventArgs e)
        {
            _ddcarpet.Leave -= new EventHandler(Evt_CarpetLeave);
            _icarpet = Convert.ToInt32(_ddcarpet.SelectedValue);
            CreateColorCell((dgvOrderDetail.Rows[CurrentRowIndex()].Cells[1] as DataGridViewComboBoxCell), _icarpet);
            _ddcarpet = null;
        }
        private void Evt_ColorLeave(object o, EventArgs e)
        {
            _ddcolor.Leave -= new EventHandler(Evt_ColorLeave);
            _icolor = (int)_ddcolor.SelectedValue;
            CreateBackingCell((dgvOrderDetail.Rows[CurrentRowIndex()].Cells[2] as DataGridViewComboBoxCell), _icarpet, _icolor);
            _ddcolor = null;
        }
        private void Evt_BackingLeave(object o, EventArgs e)
        {
            _ddbacking.Leave -= new EventHandler(Evt_BackingLeave);
            _ibacking = (int)_ddbacking.SelectedValue;
            CreateNoCell((dgvOrderDetail.Rows[CurrentRowIndex()].Cells[3] as DataGridViewComboBoxCell), _icarpet, _icolor, _ibacking);
            _ddbacking = null;
        }

        private void dgvOrderDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 4) || (e.ColumnIndex == 5))
            {
                if((e.RowIndex>=0 )&&(e.RowIndex<dgvOrderDetail.Rows.Count))
                    FinalizeRow(dgvOrderDetail.Rows[e.RowIndex]);
            }
        }

        //private void dgvOrderDetail_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        //{
        //    if(!e.Row.IsNewRow)
        //        FinalizeRow(e.Row);
        //}
    };
}