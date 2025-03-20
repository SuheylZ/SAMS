using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Dialogs;
using Ex = ClientHelper.Errors.Tips;

namespace Client
{


    public partial class Main : Form
    {

        List<Form> _children = new List<Form>(8);
        private string _sTitle = "Carpet System";
        private const string C_begin = "   [";
        private const string C_end = "]";
        private const string STR_DEFAULT = "System ready, waiting for user input ... !";
        private ClientHelper.Dialogs.DebugBox _debug = null;

        public Main()
        {
            InitializeComponent();
            this.Text = _sTitle;
        }
        
#region Common Events
        private void ExitApplication_Evt(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void ShowToolBar_Evt(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void ShowStatusBar_Evt(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void Cascade_Evt(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVertical_Evt(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontal_Evt(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIcons_Evt(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAll_Evt(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
#endregion


        //private void NewCarpet_Evt(object sender, EventArgs e)
        //{
        //    ExecuteAction(new CarpetDetailBox());
        //}

        private void MainParent_Load(object sender, EventArgs e)
        {
            _debug = new ClientHelper.Dialogs.DebugBox();
            
            if (DataProxy.Proxy.IsConnected)
            {
                EnableUI(true);
                //miConnect.Text = "&Disconnect";
            }
            else
            {
                EnableUI(false);
                //miConnect.Text = "&Connect";   
            }

        }




        private void ShowProxySetup_Evt(object sender, EventArgs e)
        {
            using (Dialogs.OptionsBox dlg = new Dialogs.OptionsBox())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                }
            }
        }
        private void ShowOptions_Evt(object sender, EventArgs e)
        {
            using (Dialogs.OptionsBox dlg = new Dialogs.OptionsBox())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                { 

                   


                }
            }
        }
        private void ShowAbout_Evt(object sender, EventArgs e)
        {
            using (Dialogs.AboutBox dlg = new Client.Dialogs.AboutBox())
            {
                dlg.ShowDialog(this);
            }
        }
        private void ShowDebugWindow_Evt(object sender, EventArgs e)
        {
            if (!miConsole.Checked)
            {
                _debug.Show();
                miConsole.Checked = true;
            }
            else
            {
                _debug.Hide();
                miConsole.Checked = false;
            }
        }
        private void ShowCustomers_Evt(object sender, EventArgs e)
        {
            ShowView(enumViews.viewCustomers);
        }
        private void ShowCarpets_Evt(object sender, EventArgs e)
        {
            if (!IsPresent(enumViews.viewCarpets))
                ShowView(enumViews.viewCarpets);
            else
                ActivateView(enumViews.viewCarpets);

        }
        private void ShowOrders_Evt(object sender, EventArgs e)
        {
            if (!IsPresent(enumViews.viewOrders))
                ShowView(enumViews.viewOrders);
            else
                ActivateView(enumViews.viewOrders);
        }
        private void Connect_Evt(object sender, EventArgs args)
        {
            try
            {
                if (!((bool)miConnect.Tag))
                    Connect();
                else
                    Disconnect();
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }
        }        
        
        
        
        //private void NewPurchase_Evt(object sender, EventArgs e)
        //{
        //    SetStatus("Showing new purchase window");
        //    using (PurchaseOrderBox dlg = new PurchaseOrderBox())
        //    {
        //        dlg.ShowDialog(this);
        //        SetStatus();
        //    }
        //}




        //private void ExecuteAction(IAction action)
        //{
        //    action.Initialize();
        //    action.Execute();
        //    action.Cleanup();

        //}

        private void SetStatus(string stext)
        {
            if (stext == string.Empty)
                stext = STR_DEFAULT;
            statusStrip.Items[0].Text = stext;
        }
        private void SetStatus()
        {
            SetStatus(string.Empty);
        }




        private void Connect()
        {
            Settings cs = new Settings();
            DataProxy.Proxy.Start(cs.ConnectionString);
            EnableUI(true);
            ClientHelper.Dialogs.DebugBox.Console("Connected to the database");
        }
        private void Disconnect()
        {
            DataProxy.Proxy.Stop();
            while(_children.Count>0){
                _children[0].Close();
            }
            EnableUI(false);
            ClientHelper.Dialogs.DebugBox.Console("Connection closed from the database");
        }

        private void EnableUI(bool bEnable)
        {
            if (bEnable)
            {
                tbOrders.Enabled = tbCustomers.Enabled = tbCarpets.Enabled = true;
                mView.Enabled = true;
                //tbPurchase.Enabled = true;
                miConnect.Text = "Disconnect";
                tbConnect.Text = miConnect.Text;
                miConnect.Tag = true;
                ShowOrders_Evt(this, null);
            }
            else
            {
                tbOrders.Enabled = tbCustomers.Enabled = tbCarpets.Enabled = false;
                mView.Enabled = false;
                //tbPurchase.Enabled = false;
                miConnect.Text = "Connect";
                tbConnect.Text = miConnect.Text;
                miConnect.Tag = false;
                foreach (Form frm in _children)
                {
                    frm.Close();
                    frm.Dispose();
                }
            }
        }

        #region Views

        private void ManageViews(enumViews ev)
        {
            if (IsPresent(ev))
                ActivateView(ev);
            else
                ShowView(ev);

        }
        internal void NotifyClose(enumViews e)
        {
            foreach (IView vw in _children)
            {
                if (vw.Type == e)
                {
                    ((Form)vw).Dispose();
                    _children.Remove((Form)vw);
                    break;
                }
            }

        }
        private bool IsPresent(enumViews eView)
        {
            bool bRet = false;

            foreach (IView view in _children)
            {
                if (view.Type == eView)
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }
        private void ShowView(enumViews ev)
        {
            Form oview = null;
            switch (ev)
            {
                case enumViews.viewCarpets:
                    oview = new Views.Carpets();
                    break;

                case enumViews.viewOrders:
                    oview = new Views.Orders();
                    break;

                case enumViews.viewHome:
                    oview = new Views.Home();
                    break;

                case enumViews.viewCustomers:
                    oview = new Views.Customers();
                    break;

                default:

                    break;
            }
            oview.MdiParent = this;
            //this.Text = _sTitle + C_begin + (oview as IView).Title + C_end;
            _children.Add(oview);
            oview.Show();
            oview.WindowState = FormWindowState.Maximized;
        }
        private void ActivateView(enumViews eView)
        {
            foreach (IView view in _children)
            {
                if (eView == view.Type)
                {
                    Form frm = (view as Form);
                    if (frm.Visible == false)
                        frm.Show();
                    frm.BringToFront();
                    break;
                }
            }
        }

        #endregion

        private void troveTechWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

     };
}
