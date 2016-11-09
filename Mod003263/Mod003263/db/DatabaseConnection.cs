﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mod003263.interview;
using MySql.Data.MySqlClient;

namespace Mod003263.DBstuff {
    class Database_Connection {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public Database_Connection() {
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

        private void Insert(String q) {
            //open connection
            if (this.OpenConnection() != true) return;
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(q, connection);
            //Execute command
            cmd.ExecuteNonQuery();
            //close connection
            this.CloseConnection();
        }

        //Insert statement
        public void Insert(Applicant applicant) {
            string q = "INSERT INTO Current_JA (First_Name, Last_Name, Dob, EmailAddress, Applying_Position, Picture, Phone_Number)"
                + $"VALUES('{applicant.First_Name}','{applicant.Last_Name}','{applicant.Dob}','{applicant.Email}',"
                + $"'{applicant.Applying_Position}','{applicant.Picture}','{applicant.Phone_Number}')";
            Insert(q);
        }
        public void Insert(Question question) {
            string q = $"INSERT INTO Questions (Question, Category) VALUES('{question.Text()}', '{question.Cat()}')";
            Insert(q);
        }
        public void Insert(int questionId, Answer answer) {
            string q = $"INSERT INTO Question_Answers (Question_ID, Value, Weight) " +
                       $"VALUES( '{questionId}','{answer.Text}', '{answer.Weight}')";
            Insert(q);
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
