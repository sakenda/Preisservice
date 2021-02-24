using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using static Preisservice.DatabaseClientCast;

namespace Preisservice
{
    public class Database : IDatabase
    {
        private string DBCONNECTION;
        private readonly IConfiguration _configuration;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
            DBCONNECTION = _configuration.GetConnectionString("localhost");
        }

        public UserPriceModel GetProcessedProductPrices(int productID, int userID)
        {
            string sql;
            SqlCommand cmd;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();

                sql = "SELECT * FROM ECommerceDB.dbo.GetPricesForProducts(@userID, @productID)";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@productID", SqlDbType.Int).Value = productID;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new UserPriceModel(
                            Convert.ToInt32(DBToValue<int>(reader["User_ID"])),
                            Convert.ToInt32(DBToValue<int>(reader["Product_ID"])),
                            Convert.ToDecimal(DBToValue<decimal>(reader["Price_Base"])),
                            Convert.ToDecimal(DBToValue<decimal>(reader["Price_Shipping"])),
                            Convert.ToDecimal(DBToValue<decimal>(reader["Price_Discount"])),
                            Convert.ToDecimal(DBToValue<decimal>(reader["Price_Total"]))
                        );
                    }

                    conn.Close();
                    return null;
                }
            }
        }

        public UserPriceProxy GetRawProductPrices(int productID, int userID)
        {
            string sql = null;
            SqlCommand cmd;

            decimal priceBase;
            decimal? priceShipping;
            decimal? userDiscount;
            decimal? userProductPrice;

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);

                (priceBase, priceShipping) = GetProductPriceAndShipping(cmd, productID, userID);
                userDiscount = GetPriceDiscount(cmd);
                userProductPrice = GetUserProductPrice(cmd);
            }

            return new UserPriceProxy(userID, productID, priceBase, priceShipping, userDiscount, userProductPrice);
        }

        private (decimal, decimal?) GetProductPriceAndShipping(SqlCommand cmd, int productID, int userID)
        {
            cmd.CommandText = "SELECT                                                               "
                            + "     pr.price_base       AS Price_Base,                              "
                            + "     pr.price_shipping   AS Price_Shipping                           "
                            + "FROM products p                                                      "
                            + "         LEFT JOIN prices pr ON p.product_id = pr.fk_product_id      "
                            + "WHERE p.product_id = @productID                                      ";

            cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@productID", SqlDbType.Int).Value = productID;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    decimal priceBase = Convert.ToDecimal(reader["Price_Base"]);
                    decimal? priceShipping = Convert.ToDecimal(DBToValue<decimal>(reader["Price_Shipping"]));

                    return (priceBase, priceShipping);
                }
            }
            return (0, null);
        }

        private decimal? GetPriceDiscount(SqlCommand cmd)
        {
            cmd.CommandText = "SELECT ud.userdiscount_value AS User_Discount                            "
                            + "FROM users u                                                             "
                            + "     LEFT JOIN userdiscount ud ON u.user_id = ud.fk_user_id              "
                            + "WHERE u.user_id = @userID                                                ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return Convert.ToDecimal(DBToValue<decimal>(reader["User_Discount"]));
                }
            }
            return null;
        }

        private decimal? GetUserProductPrice(SqlCommand cmd)
        {
            cmd.CommandText = "SELECT userspecificprice_price AS User_ProductPrice          "
                            + "FROM userspecificprice                                       "
                            + "WHERE fk_product_id = @productID AND fk_user_id = @userID    ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return Convert.ToDecimal(DBToValue<decimal>(reader["User_ProductPrice"]));
                }
            }
            return null;
        }
    }
}