using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ClientHelper;
using DPOS = DataProxy.OrderSearch;


namespace ClientHelper
{
    public partial class UC_OrderSearch : UserControl
    {
        private enum enuSearchCriteria
        {
            PurchaseOrder = 0,
            ReferenceNo = 1,
            PurchaseDate = 2,
            CustomerName = 3,
            Unknown =100

        };

        private IDataReader _result = null;
        private MessageBalloon m_tip;

        [Category("Action")]
        public event EventHandler Event_SearchPerformed, Event_ClearedSearch;

        

        public UC_OrderSearch()
        {
            InitializeComponent();
        }

        private void UC_OrderSearch_Load(object sender, EventArgs e)
        {
            //Initialize the Search Criteria: 
            ddSearchBy.Items.Clear();
            ddSearchBy.Items.Add("Purchase Order No");
            ddSearchBy.Items.Add("Reference No");
            ddSearchBy.Items.Add("Purchase Order Date");
            ddSearchBy.Items.Add("Customer Name");
            ddSearchBy.SelectedIndex = 0;
            txtValue.Text = "";
            m_tip = new MessageBalloon(txtValue);
            m_tip.Title = "Problem in Searching";
            m_tip.TitleIcon = TooltipIcon.Warning;
        }

        private enuSearchCriteria GetCriteria()
        {
            enuSearchCriteria ret = enuSearchCriteria.Unknown;

            switch(ddSearchBy.SelectedIndex){
                case 0: 
                    ret = enuSearchCriteria.PurchaseOrder;
                    break;
                case 1: 
                    ret= enuSearchCriteria.ReferenceNo;
                    break;
                case 2:
                    ret= enuSearchCriteria.PurchaseDate;
                    break;
                case 3:
                    ret= enuSearchCriteria.CustomerName;
                    break;
            }
            return ret;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                m_tip.Hide();
                string sValue = txtValue.Text.Trim();
                if (sValue == "")
                    throw new Exception("No value entered to search. Try again!");
                switch (GetCriteria())
                {
                    case enuSearchCriteria.PurchaseOrder:
                        _result = DPOS.PurchaseOrder(Convert.ToInt32(sValue));
                        break;

                    case enuSearchCriteria.ReferenceNo:
                        _result = DPOS.ReferenceNo(sValue);
                        break;

                    case enuSearchCriteria.PurchaseDate:
                        _result = DPOS.PurchaseDate(Convert.ToDateTime(sValue));
                        break;

                    case enuSearchCriteria.CustomerName:
                        _result = DPOS.CustomerName(sValue);
                        break;
                }
                if (Event_SearchPerformed != null)
                    Event_SearchPerformed(this, new EventArgs());
            }
            catch (Exception ex)
            {
                m_tip.Text = ex.Message;
                m_tip.Show();
            }
        }

        
        public IDataReader Result
        {
            get { return _result; }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtValue.Text = "";
            ddSearchBy.SelectedIndex = 0;
            if (Event_ClearedSearch != null)
                Event_ClearedSearch(this, null);
        }

    }
}
