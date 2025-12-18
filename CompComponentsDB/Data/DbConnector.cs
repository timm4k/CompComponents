using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CompComponentsDB.Data
{
    public static class DbConnector
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["Db"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var con = GetConnection())
                {
                    con.Open();
                    Console.WriteLine("Connected to database successfully\n");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                Console.WriteLine("Check if Docker container is running and database exists");
                return false;
            }
        }
    }
}