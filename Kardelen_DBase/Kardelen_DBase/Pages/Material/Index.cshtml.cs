using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Material
{
    public class IndexModel : PageModel
    {
        public List<MaterialInfo> materialInfos= new List<MaterialInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Material";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                MaterialInfo material = new MaterialInfo();
                                material.m_id = reader.GetInt32(0).ToString();
                                material.t_id = reader.GetInt32(1).ToString();
                                material.m_name = reader.GetString(2);
                                material.m_state = reader.GetString(3);
                                material.m_quantity = reader.GetDouble(4).ToString();
                                material.m_criticalQuantity = reader.GetDouble(5).ToString();
                                materialInfos.Add(material);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class MaterialInfo
    {
        public String m_id;
        public String t_id;
        public String m_name;
        public String m_state;
        public String m_quantity;
        public String m_criticalQuantity;

    }
}
