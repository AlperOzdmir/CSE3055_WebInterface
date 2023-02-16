using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Product
{
    public class EditModel : PageModel
    {
        Connection conn = new Connection();
        public ProductInfo product = new ProductInfo();
        public List<SelectListItem> transactionOptions = new List<SelectListItem>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["p_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product WHERE productID = @p_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@p_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.p_id = reader.GetInt32(0).ToString();
                                if (!reader.IsDBNull(1))
                                {
                                    product.t_id = reader.GetInt32(1).ToString();
                                }
                                product.p_name = reader.GetString(2);
                                product.p_quantity = reader.GetInt32(3).ToString();
                                product.p_price = reader.GetDouble(4).ToString();
                                product.p_installingPrice = reader.GetDouble(5).ToString();
                            }
                        }
                    }
                    sql = "SELECT transactionID FROM Work_Flow WHERE transactionType = 'sell'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactionOptions.Add(new SelectListItem
                                {
                                    Value = reader["transactionID"].ToString(),
                                    Text = reader["transactionID"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage= ex.Message;
                return;
            }
        }
        public void OnPost()
        {
            product.p_id = Request.Form["p_id"];
            product.t_id = Request.Form["t_id"];
            product.p_name = Request.Form["p_name"];
            product.p_quantity = Request.Form["p_quantity"];
            product.p_price = Request.Form["p_price"];
            product.p_installingPrice = Request.Form["p_installingPrice"];

            if (product.t_id.Length == 0 || product.p_name.Length == 0
                || product.p_quantity.Length == 0 || product.p_price.Length == 0
                || product.p_installingPrice.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Product SET transactionID = @t_id, productName = @p_name, " +
                        "quantity = @p_quantity, price = @p_price, installingPrice = @p_installingPrice" +
                        " WHERE productID = @p_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@t_id", product.t_id);
                        command.Parameters.AddWithValue("@p_name", product.p_name);
                        command.Parameters.AddWithValue("@p_quantity", product.p_quantity);
                        command.Parameters.AddWithValue("@p_price", product.p_price);
                        command.Parameters.AddWithValue("@p_installingPrice", product.p_installingPrice);
                        command.Parameters.AddWithValue("@p_id", product.p_id);

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
            Response.Redirect("/Product/Index");
        }
    }
}
