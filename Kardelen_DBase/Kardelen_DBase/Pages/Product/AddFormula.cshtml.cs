using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Transactions;

namespace Kardelen_DBase.Pages.Product
{
    public class AddFormulaModel : PageModel
    {
        Connection conn = new Connection();
        public FormulaInfo formula = new FormulaInfo();
        public List<SelectListItem> productOptions = new List<SelectListItem>();
        public List<SelectListItem> materialOptions = new List<SelectListItem>();
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
                    sql = "SELECT materialID FROM Material";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materialOptions.Add(new SelectListItem
                                {
                                    Value = reader["materialID"].ToString(),
                                    Text = reader["materialID"].ToString()
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
            formula.p_id = Request.Form["p_id"];
            formula.m_id = Request.Form["m_id"];
            formula.quantity = Request.Form["quantity"];

            if (formula.p_id.Length == 0 || formula.m_id.Length == 0
                || formula.quantity.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            else if (Int32.Parse(formula.quantity) < 0)
            {
                errorMessage = "Quantity can not be negative";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_EnterProductFormula " + "@p_id, @m_id, @quantity";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@p_id", formula.p_id);
                        command.Parameters.AddWithValue("@m_id", formula.m_id);
                        command.Parameters.AddWithValue("@quantity", formula.quantity);

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

            formula.quantity = "";
            successMessage = "New Product Formula Added Correctly";
        }
    }
}
