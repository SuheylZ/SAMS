using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client.Dialogs
{
    public partial class OptionsBox : Form
    {
        public OptionsBox()
        {
            InitializeComponent();
        }


        private void optRemote_CheckedChanged(object sender, EventArgs e)
        {
            pnlRemote.Visible = true;
            pnlLocal.Visible = false;
        }

        private void optLocal_CheckedChanged(object sender, EventArgs e)
        {
            pnlRemote.Visible = false;
            pnlLocal.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings cs = new Settings();
            
            cs.DataSource = txtDataSource.Text;
            cs.UserID = txtUserID.Text;
            cs.Passwd = txtpasswd.Text;
            cs.InitialCatalog = txtCatalog.Text;
            cs.SSPI = checkBox1.Checked;
            cs.Save();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtUserID.Enabled = false;
                txtpasswd.Enabled = false;
            }
            else
            {
                txtUserID.Enabled = true;
                txtpasswd.Enabled = true;
            }
        }

        private void ProxySetup_Load(object sender, EventArgs e)
        {
            Settings cs = new Settings();

            txtDataSource.Text = cs.DataSource;
            txtCatalog.Text = cs.InitialCatalog;
            checkBox1.Checked = cs.SSPI;
            if (cs.SSPI)
            {
                txtUserID.Enabled = false;
                txtpasswd.Enabled = false;
            }
            else {
                txtUserID.Text = cs.UserID;
                txtpasswd.Text = cs.Passwd;
            }
        }

    }
}