namespace Client.Views
{
    partial class PrintPreview
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
            this.Viewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // Viewer
            // 
            this.Viewer.LocalReport.ReportPath = "../Reports/ReportOuter.rdlc";
            this.Viewer.Location = new System.Drawing.Point(-4, -2);
            this.Viewer.Name = "Viewer";
            this.Viewer.Size = new System.Drawing.Size(400, 250);
            this.Viewer.TabIndex = 0;
            // 
            // PrintPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 255);
            this.Controls.Add(this.Viewer);
            this.Name = "PrintPreview";
            this.Text = "Print Preview";
            this.Load += new System.EventHandler(this.PrintPreview_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer Viewer;


    }
}