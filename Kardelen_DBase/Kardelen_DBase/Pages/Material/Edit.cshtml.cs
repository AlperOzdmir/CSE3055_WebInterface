using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Transactions;

namespace Kardelen_DBase.Pages.Material
{
    public class EditModel : PageModel
    {
        Connection conn = new Connection();
        public List<SelectListItem> transactionOptions = new List<SelectListItem>();
        public MaterialInfo material = new MaterialInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["m_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Material WHERE materialID = @m_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@m_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                material.m_id = reader.GetInt32(0).ToString();
                                material.t_id = reader.GetInt32(1).ToString();
                                material.m_name = reader.GetString(2);
                                material.m_state = reader.GetString(3);
                                material.m_quantity = reader.GetDouble(4).ToString();
                                material.m_criticalQuantity = reader.GetDouble(5).ToString();
                            }
                        }
                    }
                    sql = "SELECT transactionID FROM Work_Flow";
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
            material.m_id = Request.Form["m_id"];
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
                    String sql = "UPDATE Material SET transactionID = @t_id, materialName = @m_name, " +
                        "matterState = @m_state, quantity = @m_quantity, criticalQuantity = @m_criticalQuantity" +
                        " WHERE materialID = @m_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@t_id", material.t_id);
                        command.Parameters.AddWithValue("@m_name", material.m_name);
                        command.Parameters.AddWithValue("@m_state", material.m_state);
                        command.Parameters.AddWithValue("@m_quantity", material.m_quantity);
                        command.Parameters.AddWithValue("@m_criticalQuantity", material.m_criticalQuantity);
                        command.Parameters.AddWithValue("@m_id", material.m_id);

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
            Response.Redirect("/Material/Index");
        }
    }
}
