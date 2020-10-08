using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasterServer.Database;
using MasterServer.Properties;

namespace MasterServer
{
    internal static class DatabaseManager
    {
        private static SqlConnection _sqlConnection;
        public static void Initialize()
        {
            Console.WriteLine(Resources.DatabaseManager_Initialize_Load);
            if (DatabaseHelper.TestDbConnection(out _sqlConnection))
            {
                var form1 = (Form1) Application.OpenForms[0];
                form1.DbCheckBox.Checked = true;
            }
            Console.WriteLine(Resources.DatabaseManager_Initialize_Success);
        }


    }
}
