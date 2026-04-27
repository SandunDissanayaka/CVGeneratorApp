using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace CVGeneratorApp
{
    public static class DatabaseHelper
    {
        // Holds the address and security details to connect to the SQL Server
        private static readonly string connectionString = @"Data Source=MY-LAPTOP\SQLEXPRESS;Initial Catalog=CVGeneratorDB;Integrated Security=True;TrustServerCertificate=True";

        // Creates and returns a new connection to the database
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Checks if the application can successfully connect to the database
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open(); // Try to open the connection
                    return true; // Connection is successful
                }
            }
            catch (Exception ex)
            {
                // If connection fails, show the exact error message
                MessageBox.Show("Database Error: " + ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}