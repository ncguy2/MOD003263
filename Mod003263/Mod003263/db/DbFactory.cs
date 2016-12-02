using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: DbFactory
 */
namespace Mod003263.db {

    /// <summary>
    /// Factory class to use the correct database objects for each DB provider
    /// </summary>
    public class DbFactory {

        /// <summary>
        /// Creates the connection string based on the supplied provider
        /// </summary>
        /// <param name="provider">The database provider string</param>
        /// <returns>A correctly formatted database connection string</returns>
        public string CreateConnectionString(string provider) {
            switch (provider.ToLower()) {
                case "mysql": return CreateMySQLConnectionString();
                case "microsoft.ace.oledb.15.0": return CreateOleConnectionString();
                case "sqlite": return CreateSQLiteConnectionString();
            }
            return "";
        }

        /// <summary>
        /// Creates the database command for each supported DB provider
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string query, DbConnection conn) {
            if (conn is MySqlConnection)
                return new MySqlCommand(query, (MySqlConnection) conn);
            if(conn is OleDbConnection)
                return new OleDbCommand(query, (OleDbConnection) conn);
            if (conn is SQLiteConnection)
                return new SQLiteCommand(query, (SQLiteConnection) conn);
            return null;
        }

        /// <summary>
        /// Creates the correct connection based upon the provider
        /// </summary>
        /// <param name="provider">The Database provider</param>
        /// <param name="connectionString">The connection string, should be generated using <see cref="CreateConnectionString"/></param>
        /// <returns>The active Database connection</returns>
        public DbConnection CreateConnection(string provider, string connectionString) {
            switch (provider.ToLower()) {
                case "mysql": return new MySqlConnection(connectionString);
                case "microsoft.ace.oledb.15.0": return new OleDbConnection(connectionString);
                case "sqlite": return new SQLiteConnection(connectionString);
            }
            return null;
        }

        private string CreateMySQLConnectionString() {
            string server = PropertiesManager.GetInstance().GetPropertyOrDefault("database.hostname", "127.0.0.1");
            string database = PropertiesManager.GetInstance().GetPropertyOrDefault("database.schema", "Applicant");
            string uid = PropertiesManager.GetInstance().GetPropertyOrDefault("database.username", "admin");
            string password = PropertiesManager.GetInstance().GetPropertyOrDefault("database.password", "123");

            return "SERVER=" + server + ";" +
                   "DATABASE=" + database + ";" +
                   "UserID=" + uid + ";" +
                   "PASSWORD=" + password+";";
        }
        private string CreateOleConnectionString() {
            string host = PropertiesManager.GetInstance().GetPropertyOrDefault("database.hostname", "127.0.0.1");
            string database = PropertiesManager.GetInstance().GetPropertyOrDefault("database.schema", "Applicant");
            string uid = PropertiesManager.GetInstance().GetPropertyOrDefault("database.username", "admin");
            string password = PropertiesManager.GetInstance().GetPropertyOrDefault("database.password", "123");
            String provider = PropertiesManager.GetInstance().GetPropertyOrDefault("database.provider", "Microsoft.ACE.OLEDB.15.0");

            return "Provider=" + provider + ";" +
                   "Data Source=\""+host+"\";" +
                   "User ID="+uid+";" +
                   "Password=\""+password+"\";";
        }
        private string CreateSQLiteConnectionString() {
            string host = PropertiesManager.GetInstance().GetPropertyOrDefault("database.hostname", "db.sqlite");

            return "Data Source=" + host + ";Version=3";
        }

    }
}