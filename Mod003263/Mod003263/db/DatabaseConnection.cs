using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mod003263.interview;
using MySql.Data.MySqlClient;

namespace Mod003263.DBstuff {
    class DatabaseConnection {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DatabaseConnection() {
            Initialize();
        }

        private void Initialize() {
            server = "HappyTec Server";
            database = "Applicant";
            uid = "admin";
            password = "123";
            string connectionString = "SERVER=" + server + "; DATABASE=" +
                database + "; UserID=" + uid + "; PASSWORD=" + password;

            connection = new MySqlConnection(connectionString);
        }
        //open connection to database
        private bool OpenConnection() {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Close connection
        private bool CloseConnection() {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static void Insert(String q) {
            //open connection
            if (this.OpenConnection() != true) return;
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(q, connection);
            //Execute command
            cmd.ExecuteNonQuery();
            //close connection
            this.CloseConnection();
        }
        //Update statement
        public void Update() {
            string query = "UPDATE Current_JA SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete() {
            string query = "DELETE FROM Current_JA WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
    }
}
