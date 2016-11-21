﻿using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Media;
using MySql.Data.MySqlClient;

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

        public int Insert(String q) {
            //open connection
            if (!OpenConnection()) return -1;
            //create command and assign the query and connection from the constructor
            DbCommand cmd = factory.CreateCommand(q, connection);
            //Execute command
            int rows = cmd.ExecuteNonQuery();
            //close connection
            CloseConnection();
            return rows;
        }
        //Update statement
        public int Update(string query) {
            //Open connection
            if (!OpenConnection()) return -1;
            //create mysql command
            DbCommand cmd = factory.CreateCommand(query, connection);
            //Execute query
            int rows = cmd.ExecuteNonQuery();
            //close connection
            CloseConnection();
            return rows;
        }

        //Delete statement
        public int Delete(string query) {
            if (!OpenConnection()) return -1;
            DbCommand cmd = factory.CreateCommand(query, connection);
            int rows = cmd.ExecuteNonQuery();
            CloseConnection();
            return rows;
        }
    }
}
