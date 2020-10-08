using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using MasterServer.Properties;

namespace MasterServer.Database
{
    internal static class DatabaseHelper
    {
        private static string _connectionString;

        public static string GetConnectionString()
        {
            return _connectionString;
        }
        public static bool TestDbConnection(out SqlConnection connection)
        {
            string sqlConnectionString = LoadDatabaseConfig();
            Console.WriteLine(Resources.DatabaseHelper_TestDbConnection_Load);
            connection = null;
            try
            {
                connection = new SqlConnection(sqlConnectionString);
                connection.Open();
                connection.Close();
                Console.WriteLine(Resources.DatabaseHelper_TestDbConnection_Success);
                return true;
            }
            catch (SqlException e)
            {
                //TODO to errorLog
                return false;
            }
        }

        private static string LoadDatabaseConfig()
        {
            Console.WriteLine(Resources.DatabaseHelper_LoadDatabaseConfig_Load);
            try
            {
                string dataSource = ConfigurationManager.AppSettings["SQLServer"];
                string userId = ConfigurationManager.AppSettings["DbUserId"];
                string password = ConfigurationManager.AppSettings["DbPassword"];
                string initCatalog = ConfigurationManager.AppSettings["database"];

                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = dataSource,
                    UserID = userId,
                    Password = password,
                    InitialCatalog = initCatalog,
                    IntegratedSecurity = false
                };
                Console.WriteLine(Resources.DatabaseHelper_LoadDatabaseConfig_Success);
                _connectionString = sqlBuilder.ConnectionString;
                return sqlBuilder.ConnectionString;
            }
            catch (Exception e)
            {
                //TODO out to errorLog
                return null;
            }

        }
        public static string SelectString(string SelectColumn, string inTable, string conditionColumn,
            string conditionValue)
        {
            return "SELECT " + SelectColumn + " FROM dbo.[" + inTable + "] WHERE [" + conditionColumn + "]=" +
                   conditionValue;
        }
        public static string SelectString(string table, Dictionary<string,string> conditionDictionary, params string[] selectColumns)
        {
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            if (selectColumns.Length == 1)
            {
                if (selectColumns[0] == "*") sb.Append(selectColumns[0]);
                else sb.Append("[" + selectColumns[0] + "] ");
            }
            else
                foreach (var column in selectColumns)
                {
                    if(column == selectColumns.Last()) sb.Append("[" + column + "]");
                    else sb.Append("[" + column + "],");
                }

            sb.Append("FROM dbo.[" + table + "] WHERE ");
            foreach (var condition in conditionDictionary)
            {
                if(condition.Equals(conditionDictionary.Last()))
                    sb.Append("[" + condition.Key + "] = " + condition.Value);
                else
                    sb.Append("[" + condition.Key + "] = " + condition.Value+" AND ");
            }

            return sb.ToString();
        }

        public static string SelectString(string table)
        {
            return "SELECT * FROM dbo.[" + table + "]";
        }

        public static string SelectString(string table, string conditionColumn, string conditionValue)
        {
            return "SELECT * FROM dbo.[ " + table + "] WHERE [" + conditionColumn + "] =" + conditionValue;
        }

        //public static string SelectString(string table, string column, string conditionColumn, string conditionValue)
        //{
        //    return "SELECT ["+column+"] FROM dbo.[ " + table + "] WHERE [" + conditionColumn + "] =" + conditionValue;
        //}
    }
}
