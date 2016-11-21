using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;
using MySql.Data.MySqlClient;

namespace Mod003263.db {
    public class DbFactory {

        public string CreateConnectionString(string provider) {
            switch (provider.ToLower()) {
                case "mysql": return CreateMySQLConnectionString();
                case "microsoft.ace.oledb.15.0": return CreateOleConnectionString();
            }
            return "";
        }

        public DbCommand CreateCommand(string query, DbConnection conn) {
            if (conn is MySqlConnection)
                return new MySqlCommand(query, (MySqlConnection) conn);
            if(conn is OleDbConnection)
                return new OleDbCommand(query, (OleDbConnection) conn);
            return null;
        }

        public DbConnection CreateConnection(string provider, string connectionString) {
            switch (provider.ToLower()) {
                case "mysql": return new MySqlConnection(connectionString);
                case "microsoft.ace.oledb.15.0": return new OleDbConnection(connectionString);
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

    }
}