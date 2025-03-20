
//////////////////////////////////////////////
//TCS: Consignment No: 3003876260
//
//
//


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;


namespace ClientHelper.Errors
{
    public class ControlException : Exception
    {
        private Control _cnt;
        private string _title;
        private string _text;
        ClientHelper.MessageBalloon _tip;

        public ControlException(Control cnt, string stext, string stitle)
        {
            _cnt = cnt;
            _title = stitle;
            _text = stext;
        }

        public ControlException(Exception ex)
        {
            _cnt = null;
            _title = "General Problem";
            _text = ex.Message;
        }

        internal void Show()
        {
            if (_cnt != null)
            {
                _tip = new ClientHelper.MessageBalloon(_cnt);
                _tip.Title = _title;
                _tip.Text = _text;
                _tip.TitleIcon = ClientHelper.TooltipIcon.Warning;
                _tip.Show();
                Dialogs.DebugBox.Console(_text);
            }
            else
            {
                MessageBox.Show(_text, _title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Dialogs.DebugBox.Console(_text);
            }
        }
    };

    public class Tips
    {
        public static void Inform(Control ct, string title, string text)
        {
            MessageBalloon tip = new MessageBalloon(ct);
            tip.Title = title;
            tip.Text = text;
            tip.TitleIcon = TooltipIcon.Info;
            tip.Show();
            Dialogs.DebugBox.Console(text);
        }
        public static void Inform(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Warning(Control ct, string title, string text)
        {
            MessageBalloon tip = new MessageBalloon(ct);
            tip.Title = title;
            tip.Text = text;
            tip.TitleIcon = TooltipIcon.Warning;
            tip.Show();
            Dialogs.DebugBox.Console(text);
        }

        public static void Catch(Exception ex)
        {
            if (ex is ControlException)
                (ex as ControlException).Show();
            else
                MessageBox.Show(ex.Message, "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Debug(ex);
        }
        public static void Catch(ControlException ex)
        {
            ex.Show();
            Debug(ex);
        }

        public static DialogResult Ask(string text, string title)
        {
            return MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void Debug(Exception ex)
        {
            Dialogs.DebugBox.Console(ex);
        }
    }


}
