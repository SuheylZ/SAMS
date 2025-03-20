using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using REP = Microsoft.Reporting.WinForms;

namespace Client.Views
{
    public partial class PrintPreview : Form
    {
        private int _iorderid = 0;
        public PrintPreview()
        {
            InitializeComponent();
        }

        private void PrintPreview_Load(object sender, EventArgs e)
        {
            //REP.LocalReport lr = new Microsoft.Reporting.WinForms.LocalReport();
            //lr.ReportPath = @"../reports/reportouter.rdlc";
            //lr.DataSources.Add(new REP.ReportDataSource("Order", "
            //Viewer.LocalReport = lr;
            
            
            

        }

        public int OrderID
        {
            set { _iorderid = value; }
        }
    }
}