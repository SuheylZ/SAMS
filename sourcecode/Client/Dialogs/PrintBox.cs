using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Report = Microsoft.Reporting.WinForms;
using Ex = ClientHelper.Errors.Tips;
using DataProxy;


namespace Client.Dialogs
{
    public partial class PrintBox : Form
    {
        private Client.Reports.Datasets.DS_PurchaseOrder _dsOrder = null;
        private List<Report.ReportDataSource> _lstDataSources = null; 
        private int _orderid = 0;

        public PrintBox()
        {
            InitializeComponent();
        }


        public int OrderID
        {
            set { _orderid = value; }
        }

        private void Evt_Initialize(object sender, EventArgs e)
        {
            try
            {
                LoadPurchaseOrder(_orderid);
                LoadDataSources();

                foreach(Report.ReportDataSource rds in _lstDataSources)
                    rptViewer.LocalReport.DataSources.Add(rds);
                
                rptViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }
        }

        private void LoadPurchaseOrder(int id)
        {
            IDataReader rd = null;
            
            try
            {
                _dsOrder = new Client.Reports.Datasets.DS_PurchaseOrder();

                rd = Order.Get(id);
                _dsOrder.dtOrder.Load(rd);
                rd.Close();
                rd.Dispose();

                rd = Customer.Get(Order.GetCustomerIDOf(id));
                _dsOrder.dtCustomer.Load(rd);
                rd.Close();
                rd.Dispose();

                rd = Order.GetDetail2(id);
                _dsOrder.dtOrderDetail.Load(rd);
                rd.Close();
                rd.Dispose();
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }
        }
        private void LoadDataSources()
        {
            _lstDataSources = new List<Microsoft.Reporting.WinForms.ReportDataSource>();

            Report.ReportDataSource rds = new Microsoft.Reporting.WinForms.ReportDataSource();
            rds.Name = "DS_PurchaseOrder_dtOrder";
            rds.Value = _dsOrder.dtOrder;
            _lstDataSources.Add(rds);

            rds = new Microsoft.Reporting.WinForms.ReportDataSource();
            rds.Name = "DS_PurchaseOrder_dtCustomer";
            rds.Value = _dsOrder.dtCustomer;
            _lstDataSources.Add(rds);

            rds = new Microsoft.Reporting.WinForms.ReportDataSource();
            rds.Name = "DS_PurchaseOrder_dtOrderDetail";
            rds.Value = _dsOrder.dtOrderDetail;
            _lstDataSources.Add(rds);
        }
    };
}