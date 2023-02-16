using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Transactions;

namespace Kardelen_DBase.Pages.Product
{
    public class EditEmployeeModel : PageModel
    {
        Connection conn = new Connection();
        public PProductInfo pproduct = new PProductInfo();
        public List<SelectListItem> employeeOptions = new List<SelectListItem>();
        public List<SelectListItem> productOptions = new List<SelectListItem>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["pi_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Produced_Products WHERE producedItemID = @pi_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@pi_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pproduct.pi_id = reader["producedItemID"].ToString();
                                pproduct.e_id = reader["employeeID"].ToString();
                                pproduct.p_id = reader["productID"].ToString();
                                pproduct.p_date = reader["productionDate"].ToString();
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
                    sql = "SELECT productID FROM Product";
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
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
        public void OnPost()
        {
            pproduct.pi_id = Request.Query["pi_id"];
            pproduct.e_id = Request.Form["e_id"];
            pproduct.p_id = Request.Form["p_id"];
            pproduct.p_date = Request.Form["p_date"];

            if (pproduct.pi_id.Length == 0 || pproduct.e_id.Length == 0
                || pproduct.p_id.Length == 0 || pproduct.p_date.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Produced_Products SET employeeID = @e_id, productID = @p_id, " +
                        "productionDate = @p_date WHERE producedItemID = @pi_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@pi_id", pproduct.pi_id);
                        command.Parameters.AddWithValue("@e_id", pproduct.e_id);
                        command.Parameters.AddWithValue("@p_id", pproduct.p_id);
                        command.Parameters.AddWithValue("@p_date", pproduct.p_date);

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
            Response.Redirect("/Product/ProductEmployee");
        }
    }
}
