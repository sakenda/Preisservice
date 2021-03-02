using Microsoft.Extensions.Configuration;
using PreisService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static PreisService.DatabaseClientCast;

namespace PreisService
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

        public async Task<UserPriceModel> GetProcessedProductPricesAsync(int productID, int userID)
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
                        return await Task.Run(() =>
                            new UserPriceModel(
                                Convert.ToInt32(DBToValue<int>(reader["User_ID"])),
                                Convert.ToInt32(DBToValue<int>(reader["Product_ID"])),
                                Convert.ToDecimal(DBToValue<decimal>(reader["Price_Base"])),
                                Convert.ToDecimal(DBToValue<decimal>(reader["Price_Shipping"])),
                                Convert.ToDecimal(DBToValue<decimal>(reader["Price_Discount"])),
                                Convert.ToDecimal(DBToValue<decimal>(reader["Price_Total"]))
                            )
                        );
                    }

                    conn.Close();
                    return null;
                }
            }
        }

        public async Task<UserPriceProxy> GetRawProductPricesAsync(int productID, int userID)
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

                (priceBase, priceShipping) = await GetProductPriceAndShippingAsync(cmd, productID, userID);
                userDiscount = await GetPriceDiscountAsync(cmd);
                userProductPrice = await GetUserProductPriceAsync(cmd);
            }

            return new UserPriceProxy(userID, productID, priceBase, priceShipping, userDiscount, userProductPrice);
        }

        private async Task<(decimal, decimal?)> GetProductPriceAndShippingAsync(SqlCommand cmd, int productID, int userID)
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
                    return await Task.Run(() =>
                    (
                        Convert.ToDecimal(reader["Price_Base"]),
                        Convert.ToDecimal(DBToValue<decimal>(reader["Price_Shipping"]))
                    ));
                }
            }
            return (0, null);
        }

        private async Task<decimal?> GetPriceDiscountAsync(SqlCommand cmd)
        {
            cmd.CommandText = "SELECT ud.userdiscount_value AS User_Discount                            "
                            + "FROM users u                                                             "
                            + "     LEFT JOIN userdiscount ud ON u.user_id = ud.fk_user_id              "
                            + "WHERE u.user_id = @userID                                                ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return await Task.Run(() => Convert.ToDecimal(DBToValue<decimal>(reader["User_Discount"])));
                }
            }
            return null;
        }

        private async Task<decimal?> GetUserProductPriceAsync(SqlCommand cmd)
        {
            cmd.CommandText = "SELECT userspecificprice_price AS User_ProductPrice          "
                            + "FROM userspecificprice                                       "
                            + "WHERE fk_product_id = @productID AND fk_user_id = @userID    ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return await Task.Run(() => Convert.ToDecimal(DBToValue<decimal>(reader["User_ProductPrice"])));
                }
            }
            return null;
        }

        public List<ProductModel> GetAllProducts()
        {
            string sql;
            SqlCommand cmd;
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection conn = new SqlConnection(DBCONNECTION))
            {
                conn.Open();

                sql = "SELECT "
                    + "p.product_id, p.product_name, p.product_quantity, p.product_description, "
                    + "pr.price_base, pr.price_shipping, pr.price_profit, "
                    + "cat.category_id, cat.category_name, cat.category_description, "
                    + "sup.supplier_id, sup.supplier_name, "
                    + "adr.adress_street, adr.adress_number, adr.adress_city, adr.adress_zip, "
                    + "img.image_id, img.image_name "
                    + "FROM "
                    + "    products p "
                    + "        LEFT JOIN prices pr             ON p.product_id = pr.fk_product_id "
                    + "        LEFT JOIN productcategory pcat  ON p.product_id = pcat.fk_product_id "
                    + "        LEFT JOIN categories cat        ON pcat.fk_category_id = cat.category_id "
                    + "        LEFT JOIN productsupplier psup  ON p.product_id = psup.fk_product_id "
                    + "        LEFT JOIN suppliers sup         ON psup.fk_supplier_id = sup.supplier_id "
                    + "        LEFT JOIN productimage pimg     ON pimg.fk_product_id = p.product_id "
                    + "        LEFT JOIN images img            ON pimg.fk_image_id = img.image_id "
                    + "        LEFT JOIN productsarchived pa   ON p.product_id = pa.fk_product_id "
                    + "        LEFT JOIN supplieradress sadr   ON sup.supplier_id = sadr.fk_supplier_id "
                    + "        LEFT JOIN adresses adr          ON sadr.fk_adress_id = adr.adress_id "
                    + "WHERE "
                    + "    pa.fk_product_id IS NULL;";

                cmd = new SqlCommand(sql, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductModel()
                        {
                            ID = Convert.ToInt32(reader["product_id"]),
                            Name = reader["product_name"].ToString(),
                            Description = reader["product_description"].ToString(),
                            Quantity = Convert.ToInt32(DBToValue<int>(reader["product_quantity"])),
                            Price = Convert.ToDecimal(DBToValue<decimal>(reader["price_base"])),
                            Supplier = reader["supplier_name"].ToString(),
                            Category = reader["category_name"].ToString()
                        });
                    }

                    conn.Close();
                    return products;
                }
            }
        }
    }
}