using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;

namespace Kardelen_DBase.Pages.Transaction
{
    public class AddTransactionModel : PageModel
    {
        Connection conn = new Connection();
        public TransactionInfo transaction = new TransactionInfo();
        public List<SelectListItem> customerOptions = new List<SelectListItem>();
        public List<SelectListItem> billOptions = new List<SelectListItem>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            customerOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = "0 - if type is 'buy'"
            });
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT customerID FROM Customer";
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
                    sql = "SELECT billID FROM Bill";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                billOptions.Add(new SelectListItem
                                {
                                    Value = reader["billID"].ToString(),
                                    Text = reader["billID"].ToString()
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
            transaction.c_id = Request.Form["c_id"];
            transaction.b_id = Request.Form["b_id"];
            transaction.type = Request.Form["type"];

            if ( transaction.c_id.Length == 0
                || transaction.b_id.Length == 0 || transaction.type.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            else if (transaction.type == "buy" && transaction.c_id != "0")
            {
                errorMessage = "Conflict on transaction type and customer info";
                return;
            }
            else if (transaction.type == "sell" && transaction.c_id == "0")
            {
                errorMessage = "Conflict on transaction type and customer info";
                return;
            }
 
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_EnterTransaction " + "@c_id, @b_id, @type";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@c_id", transaction.c_id);
                        command.Parameters.AddWithValue("@b_id", transaction.b_id);
                        command.Parameters.AddWithValue("@type", transaction.type);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "Transaction Added Correctly";
        }
    }
}
