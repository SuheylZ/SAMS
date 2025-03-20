using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.Views
{
    public partial class Home : Form, IView
    {
        private const string _sTitle = "Home";
        public Home()
        {
            InitializeComponent();
        }

        public enumViews Type
        {
            get { return enumViews.viewHome; }
        }

        public string Title
        {
            get { return _sTitle; }
        }

        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Main)this.MdiParent).NotifyClose(enumViews.viewHome);
        }

    }
}