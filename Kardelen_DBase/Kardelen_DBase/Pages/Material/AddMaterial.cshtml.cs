using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;

namespace Kardelen_DBase.Pages.Material
{
    public class AddMaterialModel : PageModel
    {
        Connection conn = new Connection();
        public MaterialInfo material = new MaterialInfo();
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
                    String sql = "SELECT TOP 3 transactionID, transactionDate FROM Work_Flow WHERE transactionType = 'buy' ORDER BY transactionID DESC";
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
            material.t_id = Request.Form["t_id"];
            material.m_name = Request.Form["m_name"];
            material.m_state = Request.Form["m_state"];
            material.m_quantity = Request.Form["m_quantity"];
            material.m_criticalQuantity = Request.Form["m_criticalQuantity"];

            if (material.t_id.Length == 0 || material.m_name.Length == 0
                || material.m_state.Length == 0 || material.m_quantity.Length == 0
                || material.m_criticalQuantity.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_BuyMaterial " + "@t_id, @m_name, @m_state, @m_quantity, @m_criticalQuantity";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@t_id", material.t_id);
                        command.Parameters.AddWithValue("@m_name", material.m_name);
                        command.Parameters.AddWithValue("@m_state", material.m_state);
                        command.Parameters.AddWithValue("@m_quantity",  material.m_quantity);
                        command.Parameters.AddWithValue("@m_criticalQuantity", material.m_criticalQuantity);

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

            material.t_id = ""; material.m_name = ""; material.m_state = "";
            material.m_quantity = ""; material.m_criticalQuantity = "";
            successMessage = "Material Added Correctly";
        }
    }
}
