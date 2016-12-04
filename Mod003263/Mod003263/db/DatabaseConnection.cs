using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Markup;
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
        private bool longConnectionOpen;
        private bool connectionOpen = false;

        private static DatabaseConnection instance;

        public static DatabaseConnection GetInstance() {
//            return instance ?? (instance = new DatabaseConnection());
            return new DatabaseConnection();
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
            if (connectionOpen) return true;
            try {
                connection.Open();
                connectionOpen = true;
                return true;
            }catch (DbException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Close connection
        private bool CloseConnection() {
            if (!connectionOpen) return true;
            try {
                connection.Close();
                connectionOpen = false;
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

        public int Insert(String q) {
            if (!OpenConnection()) return -1;
            DbCommand cmd = factory.CreateCommand(q, connection);
            Int32 rows = cmd.ExecuteNonQuery();
            CloseConnection();
            return rows;
        }

        public bool OpenLongConnection() {
            longConnectionOpen = true;
            return OpenConnection();
        }

        public DbDataReader SelectLong(String q) {
            DbCommand cmd = factory.CreateCommand(q, connection);
            DbDataReader data = cmd.ExecuteReader();
            return data;
        }

        public int InsertCollectionLong<T>(List<T> collection, Func<T, String> toSql) {
            int rows = collection.Sum(t => InsertLong(toSql(t)));
            return rows;
        }
        public int InsertCollection<T>(List<T> collection, Func<T, String> toSql) {
            OpenLongConnection();
            int rows = collection.Sum(t => InsertLong(toSql(t)));
            CloseLongConnection();
            return rows;
        }
        public int InsertCollection<T, U>(Dictionary<T, U> collection, Func<KeyValuePair<T, U>, String> toSql) {
            OpenLongConnection();
            int rows = collection.Sum(pair => InsertLong(toSql(pair)));
            CloseLongConnection();
            return rows;
        }

        public int InsertLong(String q) {
            if (!longConnectionOpen) return -1;
            DbCommand cmd = factory.CreateCommand(q, connection);
            return cmd.ExecuteNonQuery();
        }

        public bool CloseLongConnection() {
            longConnectionOpen = false;
            return CloseConnection();
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

        public bool isLongOpen() {
            return longConnectionOpen;
        }

        public int GetLatestId(string table, string idColumn = "ID") {
            if (!OpenConnection()) return -1;
            DbCommand cmd = factory.CreateCommand($"SELECT MAX({idColumn}) FROM {table}", connection);
            object id = cmd.ExecuteScalar();
            CloseConnection();
            return id as int? ?? 0;
        }

        public string Escape(string q) {

        }

        public int ExecuteParameterQuery(string query, params object[] args) {
            //open connection
            if (!OpenConnection()) return -1;
            //create command and assign the query and connection from the constructor
            DbCommand cmd = factory.CreateCommand(query, connection);
            cmd.Parameters.Add(args);
            //Execute command
            int rows = cmd.ExecuteNonQuery();
            //close connection
            CloseConnection();
            return rows;
        }
    }
}
