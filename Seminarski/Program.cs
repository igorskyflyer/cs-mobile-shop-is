using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Seminarski
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        public static Form mainForm;
        public static String user = "";
        public static String db = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\db.mdb;";
        public static bool loggedIn = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm=new FormLogin());
        }
    }
}
