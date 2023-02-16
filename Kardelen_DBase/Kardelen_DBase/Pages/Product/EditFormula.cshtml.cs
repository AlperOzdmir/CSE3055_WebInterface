using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Product
{
    public class EditFormulaModel : PageModel
    {
        Connection conn = new Connection();
        public FormulaInfo formula = new FormulaInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            formula.p_id = Request.Query["p_id"];
            formula.m_id = Request.Query["m_id"];
            formula.quantity = Request.Form["quantity"];

            if (formula.quantity.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Product_Formula SET materialQuantity = @quantity WHERE productID = @p_id " +
                        "AND materialID = @m_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@quantity", formula.quantity);
                        command.Parameters.AddWithValue("@p_id", formula.p_id);
                        command.Parameters.AddWithValue("@m_id", formula.m_id);

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
            Response.Redirect("/Product/ProductFormula");
            }
        }
    }
