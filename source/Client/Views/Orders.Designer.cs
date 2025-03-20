namespace Client.Views
{
    partial class Orders
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.lnkNewPurchase = new System.Windows.Forms.LinkLabel();
            this.pnlSearch = new ClientHelper.UC_OrderSearch();
            this.lnkPrint = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToResizeRows = false;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrders.Location = new System.Drawing.Point(0, 63);
            this.dgvOrders.MultiSelect = false;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(761, 213);
            this.dgvOrders.TabIndex = 1;
            this.dgvOrders.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvOrders_UserDeletingRow);
            this.dgvOrders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Controls.Add(this.lnkPrint);
            this.panel1.Controls.Add(this.lnkRefresh);
            this.panel1.Controls.Add(this.lnkNewPurchase);
            this.panel1.Controls.Add(this.pnlSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 63);
            this.panel1.TabIndex = 0;
            // 
            // lnkRefresh
            // 
            this.lnkRefresh.AutoSize = true;
            this.lnkRefresh.Location = new System.Drawing.Point(182, 41);
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.Size = new System.Drawing.Size(122, 13);
            this.lnkRefresh.TabIndex = 2;
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.Text = "Refresh Information";
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // lnkNewPurchase
            // 
            this.lnkNewPurchase.AutoSize = true;
            this.lnkNewPurchase.Location = new System.Drawing.Point(25, 41);
            this.lnkNewPurchase.Name = "lnkNewPurchase";
            this.lnkNewPurchase.Size = new System.Drawing.Size(124, 13);
            this.lnkNewPurchase.TabIndex = 1;
            this.lnkNewPurchase.TabStop = true;
            this.lnkNewPurchase.Text = "New Purchase Order";
            this.lnkNewPurchase.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNewPurchase_LinkClicked);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(761, 36);
            this.pnlSearch.TabIndex = 0;
            this.pnlSearch.Event_SearchPerformed += new System.EventHandler(this.uC_OrderSearch1_Event_SearchPerformed);
            this.pnlSearch.Event_ClearedSearch += new System.EventHandler(this.uC_OrderSearch1_Event_ClearedSearch);
            // 
            // lnkPrint
            // 
            this.lnkPrint.AutoSize = true;
            this.lnkPrint.Location = new System.Drawing.Point(342, 41);
            this.lnkPrint.Name = "lnkPrint";
            this.lnkPrint.Size = new System.Drawing.Size(115, 13);
            this.lnkPrint.TabIndex = 3;
            this.lnkPrint.TabStop = true;
            this.lnkPrint.Text = "Print Delivery Note";
            this.lnkPrint.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Evt_PrintNote);
            // 
            // Orders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 276);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Orders";
            this.Text = "Orders View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Orders_FormClosed);
            this.Load += new System.EventHandler(this.Orders_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Panel panel1;
        private ClientHelper.UC_OrderSearch pnlSearch;
        private System.Windows.Forms.LinkLabel lnkRefresh;
        private System.Windows.Forms.LinkLabel lnkNewPurchase;
        private System.Windows.Forms.LinkLabel lnkPrint;
    }
}