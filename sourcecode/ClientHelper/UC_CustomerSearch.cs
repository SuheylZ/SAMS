using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DP = DataProxy;

namespace ClientHelper
{
    public partial class UC_CustomerSearch : UserControl
    {

        public enum enuSearchCustomerCriteria
        {
            escByNone = 0,
            escByName = 1,
            escByAddress = 2,
            escByPhone = 3
        }

        private IDataReader _reader=null;
        MessageBalloon m_tip;

        [Category("Action")]
        public event EventHandler Event_SearchPerformed, Event_SearchCleared;

        public UC_CustomerSearch()
        {
            InitializeComponent();
        }

        public IDataReader Result
        {
            get { return _reader; }
        }

        private void UC_CustomerSearch_Load(object sender, EventArgs e)
        {
            ddSearchBy.Items.Add("Name");
            ddSearchBy.Items.Add("Address and Postal Code");
            ddSearchBy.Items.Add("By Phone or Cell");
            ddSearchBy.SelectedIndex = 0;
            txtValue.Text = "";
            m_tip = new MessageBalloon(txtValue);
            m_tip.Title = "Problem in Searching";
            m_tip.TitleIcon = TooltipIcon.Warning;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ddSearchBy.SelectedIndex = 0;
            txtValue.Text = "";
            if (Event_SearchCleared != null)
                Event_SearchCleared(this, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sValue = txtValue.Text.Trim();

            try
            {
                m_tip.Hide();
                if (sValue == "")
                    throw new Exception("No search value is given, Try again");

                //Perform Search
                switch (GetCriteria())
                {
                    case enuSearchCustomerCriteria.escByName:
                        _reader= DP.CustomerSearch.ByName(sValue);
                        break;
                    case enuSearchCustomerCriteria.escByAddress:
                        _reader = DP.CustomerSearch.ByAddress(sValue);
                        break;
                    case enuSearchCustomerCriteria.escByPhone:
                        _reader = DP.CustomerSearch.ByPhone(sValue);
                        break;
                }
                if (Event_SearchPerformed != null)
                    Event_SearchPerformed(this, null);
            }
            catch (Exception ex)
            {
                m_tip.Text = ex.Message;
                m_tip.Show();
            }
        }

        private enuSearchCustomerCriteria GetCriteria()
        {
            enuSearchCustomerCriteria ret = enuSearchCustomerCriteria.escByNone;

            switch (ddSearchBy.SelectedIndex)
            {
                case 0: ret = enuSearchCustomerCriteria.escByName; break;
                case 1: ret = enuSearchCustomerCriteria.escByAddress; break;
                case 2: ret = enuSearchCustomerCriteria.escByPhone; break;
            }
            return ret;
        }


    }
}
