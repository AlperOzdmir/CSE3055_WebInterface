using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Statistics
{
    public class ProductionPerEmployee : PageModel
    {
        public List<ProductionInfo> listProductionInfo = new List<ProductionInfo>();
        Connection conn = new Connection();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "select * from Production_Per_Employee";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductionInfo productionInfo = new ProductionInfo();
                                productionInfo.id = "" + reader.GetInt32(0);
                                productionInfo.fullName = reader.GetString(1);
                                productionInfo.productCount = "" + reader.GetInt32(2);

                                listProductionInfo.Add(productionInfo);
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

    public class ProductionInfo
    {
        public String id;
        public String fullName;
        public String productCount;

    }
}
