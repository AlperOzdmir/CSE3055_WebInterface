using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Transactions;

namespace Kardelen_DBase.Pages.Product
{
    public class SellProductModel : PageModel
    {
        Connection conn = new Connection();
        public ProductInfo product = new ProductInfo();
        public List<SelectListItem> productOptions = new List<SelectListItem>();
        public List<SelectListItem> customerOptions = new List<SelectListItem>();
        public List<SelectListItem> transactionOptions = new List<SelectListItem>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT TOP 3 transactionID, transactionDate FROM Work_Flow WHERE transactionType = 'sell' ORDER BY transactionID DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactionOptions.Add(new SelectListItem
                                {
                                    Value = reader["transactionID"].ToString(),
                                    Text = reader["transactionID"].ToString() + " - " + reader["transactionDate"].ToString()
                                });
                            }
                        }
                    }
                    sql = "SELECT productID FROM Product WHERE quantity > 1";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productOptions.Add(new SelectListItem
                                {
                                    Value = reader["productID"].ToString(),
                                    Text = reader["productID"].ToString()
                                });
                            }
                        }
                    }
                    sql = "SELECT customerID, firstName, lastName FROM Customer";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerOptions.Add(new SelectListItem
                                {
                                    Value = reader["customerID"].ToString(),
                                    Text = reader["customerID"].ToString() + " - " + reader["firstName"] + " " + reader["lastName"]
                                });
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void OnPost()
        {
            product.t_id = Request.Form["t_id"];
            String customerID = Request.Form["c_id"];
            product.p_id = Request.Form["p_id"];
            if (product.t_id.Length == 0 || product.p_id.Length == 0
                || customerID.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_SellProduct " + "@p_id, @c_id, @t_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@t_id", product.t_id);
                        command.Parameters.AddWithValue("@p_id", product.p_id);
                        command.Parameters.AddWithValue("@c_id", customerID);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            product.t_id = ""; product.p_id = ""; customerID = "";
            successMessage = "Product Sold Correctly";
        }
    }
}
