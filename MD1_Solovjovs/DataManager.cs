using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MD1_Solovjovs
{
    public class DataManager
    {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";

        static DataManager() {
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRootDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(exeDirectory).FullName).FullName).FullName;
            AppDomain.CurrentDomain.SetData("DataDirectory", projectRootDirectory);
        }

        public static int GetRecord()
        {
            string query = "SELECT * FROM Record";
            int record = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read()) //The View will return only one result
                {
                    record = (int)reader["Score"];
                }
            }
            return record;
        }

        public static DataTable GetAllResults()
        {
            string query = "SELECT * FROM Results ORDER BY DateTime DESC";
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.Fill(dt);
            }
            return dt;
        }

        public static void SaveScore(int score)
        {
            string query = "INSERT INTO Results (Score) VALUES (@Score)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Score", score);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
