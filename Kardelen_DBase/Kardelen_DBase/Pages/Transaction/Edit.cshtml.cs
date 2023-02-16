using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.IO;

namespace Kardelen_DBase.Pages.Transaction
{
    public class EditModel : PageModel
    {
        Connection conn = new Connection();
        public TransactionInfo transaction = new TransactionInfo();
        public List<SelectListItem> customerOptions = new List<SelectListItem>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            customerOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = "0"
            });
            String id = Request.Query["t_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Work_Flow WHERE transactionID = @t_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@t_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transaction.t_id = reader["transactionID"].ToString();
                                transaction.c_id = reader["customerID"].ToString();
                                transaction.b_id = reader["billID"].ToString();
                                transaction.type = reader["transactionType"].ToString();
                                transaction.date = reader["transactionDate"].ToString();
                            }
                        }
                    }
                    sql = "SELECT customerID FROM Customer";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerOptions.Add(new SelectListItem
                                {
                                    Value = reader["customerID"].ToString(),
                                    Text = reader["customerID"].ToString()
                                });
                            }
                        }
                    }
                    connection.Close();
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
            transaction.t_id = Request.Query["t_id"];
            transaction.c_id = Request.Form["c_id"];
            transaction.b_id = Request.Form["b_id"];
            transaction.type = Request.Form["type"];
            transaction.date = Request.Form["date"];

            if (transaction.b_id.Length == 0 || transaction.c_id.Length == 0
                || transaction.type.Length == 0 || transaction.date.Length == 0
                || transaction.t_id.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Work_Flow SET customerID = @c_id, billID = @b_id, " +
                        "transactionType = @type, transactionDate = @date WHERE transactionID = @t_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@c_id", transaction.c_id);
                        command.Parameters.AddWithValue("@b_id", transaction.b_id);
                        command.Parameters.AddWithValue("@type", transaction.type);
                        command.Parameters.AddWithValue("@date", transaction.date);
                        command.Parameters.AddWithValue("@t_id", transaction.t_id);

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
            Response.Redirect("/Transaction/Index");
        }
    }
}
