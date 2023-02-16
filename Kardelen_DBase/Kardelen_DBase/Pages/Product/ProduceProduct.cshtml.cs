using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Transactions;

namespace Kardelen_DBase.Pages.Product
{
    public class ProduceProductModel : PageModel
    {
        Connection conn = new Connection();
        public String employeeID;
        public ProductInfo product = new ProductInfo();
        public List<SelectListItem> productOptions = new List<SelectListItem>();
        public List<SelectListItem> employeeOptions = new List<SelectListItem>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT productID FROM Product";
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
                    sql = "SELECT employeeID FROM Employee";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employeeOptions.Add(new SelectListItem
                                {
                                    Value = reader["employeeID"].ToString(),
                                    Text = reader["employeeID"].ToString()
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
            String employeeID = Request.Form["e_id"];
            product.p_id = Request.Form["p_id"];
            product.p_quantity = Request.Form["quantity"];
            if (product.p_quantity.Length == 0 || product.p_id.Length == 0
                || employeeID.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_ProduceProduct " + "@p_id, @e_id, @quantity";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@p_id", product.p_id);
                        command.Parameters.AddWithValue("@e_id", employeeID);
                        command.Parameters.AddWithValue("@quantity", product.p_quantity);

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

            product.p_quantity = "";
            successMessage = "Product Produced Correctly";
        }
    }
}

