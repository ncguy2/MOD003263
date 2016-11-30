using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Media;
using MySql.Data.MySqlClient;

/**
 * Author: Callum Highley, Nick Guy
 * Date: 28/11/2016
 * Contains: DatabaseConnection
 */
namespace Mod003263.db {
    class DatabaseConnection {
        private DbConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private DbFactory factory;

        private static DatabaseConnection instance;

        public static DatabaseConnection GetInstance() {
            return instance ?? (instance = new DatabaseConnection());
        }

        private DatabaseConnection() {
            Initialize();
        }

        private void Initialize() {
            factory = new DbFactory();

            String provider = PropertiesManager.GetInstance().GetPropertyOrDefault("database.provider", "MySQL");
            string connectionString = factory.CreateConnectionString(provider);
            connection = factory.CreateConnection(provider, connectionString);
        }
        //open connection to database
        private bool OpenConnection() {
            try {
                connection.Open();
                return true;
            }catch (DbException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Close connection
        private bool CloseConnection() {
            try {
                connection.Close();
                return true;
            }catch (DbException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public DbDataReader Select(String q) {
            if (!OpenConnection()) return null;
            DbCommand cmd = factory.CreateCommand(q, connection);
            DbDataReader data = cmd.ExecuteReader();
            return data;
        }

        public int ExecuteQuery(string query) {
            //open connection
            if (!OpenConnection()) return -1;
            //create command and assign the query and connection from the constructor
            DbCommand cmd = factory.CreateCommand(query, connection);
            //Execute command
            int rows = cmd.ExecuteNonQuery();
            //close connection
            CloseConnection();
            return rows;
        }
    }
}
