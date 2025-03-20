using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ex = ClientHelper.Errors.Tips;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new Main());
            }
            catch (Exception ex)
            {
                Ex.Catch(ex);
            }
        }
    }


}