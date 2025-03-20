using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ClientHelper.Dialogs
{
    public partial class DebugBox : Form
    {
        private static DebugBox _this = null;

        public DebugBox()
        {
            InitializeComponent();
            _this = this;
        }

        public static void Console(string text)
        {
            if (_this != null)
            {
                _this.txtNotes.Text += Environment.NewLine + text;
            }
        }
        public static void Console(Exception ex)
        {
            if (_this != null)
            {
                _this.txtNotes.Text += Environment.NewLine;
                _this.txtNotes.Text += "[Exception Occured] " + ex.Message;
                _this.txtNotes.Text += "[Stack Trace]" + Environment.NewLine;
                _this.txtNotes.Text += ex.StackTrace;
            }




        }

        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _this = null;
        }
    }
}