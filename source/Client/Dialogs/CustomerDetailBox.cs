using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Customer = DataProxy.Customer;
using DV = ClientHelper.UI;
using Tips = ClientHelper.Errors.Tips;


namespace Client.Dialogs
{
    public partial class CustomerDetailBox : Form
    {
        private string[] _titles = { "Mr", "Mrs", "Miss", "Madam", "Sir", "Dr", "Ms" };

        private int _custid = 0;
        private bool _bEdit = false;
        private ClientHelper.MessageBalloon _tip = null;



        public CustomerDetailBox()
        {
            InitializeComponent();
        }

        public int CustomerID
        {
            get { return _custid; }
            set { _custid = value; }
        }

        private void CustomerDetailBox_Load(object sender, EventArgs e)
        {
            cbTitle.Items.Clear();
            foreach (string item in _titles)
                cbTitle.Items.Add(item);

            if (_custid > 0)
            {
                LoadCustomer();
                _bEdit = true;
            }
        }

        private void LoadCustomer()
        {
                using (IDataReader rd = Customer.Get(_custid))
                {
                    try
                    {
                        if (!rd.Read())
                            throw new Exception("No customer exists");
                        CustomerTitle = DV.SafeStr(rd["title"]);
                        txtFirst.Text = DV.SafeStr(rd["firstname"]);
                        txtLast.Text = DV.SafeStr(rd["lastname"]);
                        txtFamily.Text = DV.SafeStr(rd["midname"]);
                        txtAddress.Text = DV.SafeStr(rd["address"]);
                        txtPostalCode.Text = DV.SafeStr(rd["postalcode"]);
                        txtPhone.Text = DV.SafeStr(rd["phone"]);
                        txtCell.Text = DV.SafeStr(rd["cell"]);
                        txtNotes.Text = DV.SafeStr(rd["notes"]);
                        rd.Close();
                    }
                    catch (Exception ex)
                    {
                         Tips.Catch(ex);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (_bEdit)
            {
                Customer.Update(_custid, CustomerTitle, txtFirst.Text.Trim(), txtLast.Text.Trim(),
                    txtFamily.Text.Trim(), txtAddress.Text.Trim(), txtPostalCode.Text.Trim(),
                    txtPhone.Text.Trim(), txtCell.Text.Trim());
                Customer.AddDetails(_custid, txtNotes.Text.Trim());
            }
            else
            {
                if (Customer.Exists(txtFirst.Text.Trim(), txtLast.Text.Trim()) > 0)
                {
                    if (MessageBox.Show("A customer with this name already exists. Do yuo want to duplicate the customer?", "Customer Already Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                }

                _custid = Customer.Add(CustomerTitle, txtFirst.Text.Trim(), txtFamily.Text.Trim(), txtLast.Text.Trim(),
                    txtAddress.Text.Trim(), txtPostalCode.Text.Trim(),
                    txtPhone.Text.Trim(), txtCell.Text.Trim());
                Customer.AddDetails(_custid, txtNotes.Text.Trim());

                MessageBox.Show("New customer has been saved successfully", "Customer Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ValidateInput()
        {
            bool bOk = true;

            if (txtFirst.Text.Trim() == "")
            {
                ShowError(txtFirst, "No name for the customer is mentioned");
                bOk = false;
            }

            else if (txtLast.Text.Trim() == "")
            {
                ShowError(txtLast, "No last name is mentioned");
                bOk = false;
            }
            return bOk;
        }

        private void ShowError(Control ct, string text)
        {
            _tip = new ClientHelper.MessageBalloon(ct);
            _tip.Title = "Can't Save";
            _tip.Text = text;
            _tip.TitleIcon = ClientHelper.TooltipIcon.Warning;
            _tip.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


    }
}