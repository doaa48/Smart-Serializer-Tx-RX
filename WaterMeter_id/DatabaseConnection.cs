using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WaterMeter_id
{
    public class Database
    {
        private string connectionString;
        private string serverIp;
        private string dbName;
        private string username;
        private string password;
        private SqlTransaction transaction; // Added SqlTransaction field

        private void ReadConfig(string filePath)
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Define a regular expression pattern to match key-value pairs
            string pattern = @"(\w+)\s*=\s*""(.*)""";

            foreach (string line in lines)
            {
                Match match = Regex.Match(line, pattern);

                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    string value = match.Groups[2].Value;

                    // Assign values to the appropriate variables
                    switch (key)
                    {
                        case "serverIp":
                            serverIp = value;
                            break;
                        case "dbName":
                            dbName = value;
                            break;
                        case "username":
                            username = value;
                            break;
                        case "password":
                            password = value;
                            break;
                    }
                }
            }
        }

        public Database()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data_input\\Database.txt");
            ReadConfig(path);
            // Construct the connection string using the provided parameters
            connectionString = $"Data Source={serverIp};Initial Catalog={dbName};User ID={username};Password={password}";
        }

        public SqlConnection Connect()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connected to the database.");

                // Perform database operations here

                return connection; // Return the SqlConnection object
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                connection?.Close(); // Close the connection if it was opened
                return null; // Connection failed, return null
            }
        }

        // Method to begin a transaction
        public void BeginTransaction()
        {
            try
            {
                // Make sure connection is open
                if (transaction == null)
                {
                    // Begin transaction
                    transaction = Connect().BeginTransaction();
                }
                else
                {
                    throw new InvalidOperationException("Transaction already in progress.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting transaction: " + ex.Message);
            }
        }

        // Method to commit a transaction
        public void CommitTransaction()
        {
            try
            {
                // Make sure transaction is not null
                if (transaction != null)
                {
                    // Commit transaction
                    transaction.Commit();
               
                    // Reset transaction to null
                    transaction = null;
                }
                else
                {
                    throw new InvalidOperationException("No transaction in progress.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error committing transaction: " + ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                // Make sure transaction is not null
                if (transaction != null)
                {
                    // Rollback transaction
                    transaction.Rollback();
                 
                    // Reset transaction to null
                    transaction = null;
                }
                else
                {
                    throw new InvalidOperationException("No transaction in progress.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error rolling back transaction: " + ex.Message);
            }
        }

    }
}
