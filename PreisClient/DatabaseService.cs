using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PreisClient
{
    public class DatabaseService
    {
        private const string DBCONNECTION = "SERVER=DESKTOP-9QI02R2\\LOCALSQLSERVER;DATABASE=ECommerceDB;UID=client;PWD=client;";

        public static List<int> UserList = new DatabaseService().GetAllUsers();
        public static List<int> ProductList = new DatabaseService().GetAllProductsAsync();

        public List<int> GetAllProductsAsync()
        {
            string sql;
            SqlCommand cmd;
            List<int> list = new List<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();

                sql = "SELECT product_id FROM products";
                cmd = new SqlCommand(sql, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Convert.ToInt32(reader["product_id"]));
                    }

                    conn.Close();
                    return list;
                }
            }
        }

        public List<int> GetAllUsers()
        {
            string sql;
            SqlCommand cmd;
            List<int> list = new List<int>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();

                sql = "SELECT user_id FROM users";
                cmd = new SqlCommand(sql, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Convert.ToInt32(reader["user_id"]));
                    }

                    conn.Close();
                    return list;
                }
            }
        }
    }
}