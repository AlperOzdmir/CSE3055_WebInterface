using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Product
{
    public class ProductEmployeeModel : PageModel
    {
        public List<PProductInfo> pproductInfos = new List<PProductInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Produced_Products";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PProductInfo pproduct = new PProductInfo();
                                pproduct.pi_id = reader["producedItemID"].ToString();
                                pproduct.e_id = reader["employeeID"].ToString();
                                pproduct.p_id = reader["productID"].ToString();
                                pproduct.p_date = reader["productionDate"].ToString();
                                pproductInfos.Add(pproduct);
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

    public class PProductInfo
    {
        public String pi_id;
        public String e_id;
        public String p_id;
        public String p_date;
    }
}
