using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopManagementSystem.DAL
{
    public class DataProvider
    {
        private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=CoffeeShopManagement;Integrated Security=True";
        
        private static DataProvider instance;

        public static DataProvider Instance 
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; } 
            private set => instance = value; 
        }

        private DataProvider() { }

        public DataTable executeQuery(string query, object[] param = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (param != null) {
                    string[] listParam = query.Split(' ');

                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, param[i]);
                            i++; ;
                        }
                    }
                }
                    
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        public bool DrinkExists(string name)
        {
            string query = "SELECT COUNT(*) FROM Drink WHERE name = @name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
            }
        }

        public bool AccUsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Account WHERE username = @username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
            }
        }

        public bool CateExists(string name)
        {
            string query = "SELECT COUNT(*) FROM DrinkCategory WHERE name = @name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
            }
        }

        public bool TableExists(string name)
        {
            string query = "SELECT COUNT(*) FROM TablesList WHERE name = @name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
            }
        }

        public int executeNonQuery(string query, object[] param = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (param != null)
                {
                    string[] listParam = query.Split(' ');

                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, param[i]);
                            i++; ;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }

        public object executeScalar(string query, object[] param = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (param != null)
                {
                    string[] listParam = query.Split(' ');

                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, param[i]);
                            i++; ;
                        }
                    }
                }

                data = command.ExecuteScalar();

                connection.Close();
            }
            return data;
        }
    }
}
