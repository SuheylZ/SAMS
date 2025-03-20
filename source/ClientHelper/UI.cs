using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;



namespace ClientHelper
{
    public delegate void CheckBoxClickedHandler(bool state);
    public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
    {
        bool _bChecked;
        public DataGridViewCheckBoxHeaderCellEventArgs(bool bChecked)
        {
            _bChecked = bChecked;
        }
        public bool Checked
        {
            get { return _bChecked; }
        }
    }

    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DatagridViewCheckBoxHeaderCell()
        {
            OnCheckBoxClicked = CheckBoxClicked;
        }

        [DebuggerStepThrough()]
        protected override void Paint(System.Drawing.Graphics graphics,
            System.Drawing.Rectangle clipBounds,
            System.Drawing.Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);
            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics,
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            p.X = cellBounds.Location.X +
                (cellBounds.Width / 2) - (s.Width / 2);
            p.Y = cellBounds.Location.Y +
                (cellBounds.Height / 2) - (s.Height / 2);
            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.UncheckedNormal;
            CheckBoxRenderer.DrawCheckBox
            (graphics, checkBoxLocation, _cbState);
        }

        [DebuggerStepThrough()]
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(_checked);
                    this.DataGridView.InvalidateCell(this);
                }

            }
            base.OnMouseClick(e);
        }

        [DebuggerStepThrough()]
       private void CheckBoxClicked(bool bState)
       {
           foreach(DataGridViewRow row in this.DataGridView.Rows){
               row.Cells[this.ColumnIndex].Value = bState;
           }
           this.DataGridView.Refresh();
       }
    }

    public class UI
    {
        [DebuggerStepThrough()]
        public static string SafeStr(object obj)
        {
            string s = "";
            if (obj != DBNull.Value)
                s = Convert.ToString(obj);
            return s.Trim();
        }

        [DebuggerStepThrough()]
        public static int SafeInt(object obj)
        {
            int i = 0;
            if (obj != DBNull.Value)
                i = Convert.ToInt32(obj);
            return i;
        }
        
        [DebuggerStepThrough()]
        public static double SafeDbl(object obj)
        {
            double d = 0;
            if (obj != DBNull.Value)
                d = Convert.ToDouble(obj);
            return d;
        }
        
        [DebuggerStepThrough()]
        public static DateTime SafeDate(object obj)
        {
            DateTime dt = DateTime.Parse("1/1/1900");
            if (obj != DBNull.Value)
                dt = Convert.ToDateTime(obj);
            
                
            return dt;
        }
        
        [DebuggerStepThrough()]
        public static object DBVal(object s)
        {
            object o = DBNull.Value;
            if (s is string)
            {
                if (((string)s).Trim() != "")
                    o = s;
            }
            else if (s is int)
            {
                o = (int)s;
            }
            else if (s is double)
            {
                o = (double)s;
            }
            else if (s is DateTime)
            {
                o = (DateTime)s;
            }
            return o;
        }
    }
}
