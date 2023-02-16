using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Product
{
    public class IndexModel : PageModel
    {
        public List<ProductInfo> productInfos= new List<ProductInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                ProductInfo product = new ProductInfo();
                                product.p_id = reader.GetInt32(0).ToString();
                                if(!reader.IsDBNull(1))
                                {
                                    product.t_id = reader.GetInt32(1).ToString();
                                }
                                product.p_name = reader.GetString(2);
                                product.p_quantity = reader.GetInt32(3).ToString();
                                product.p_price = reader.GetDouble(4).ToString();
                                product.p_installingPrice = reader.GetDouble(5).ToString();
                                productInfos.Add(product);
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

    public class ProductInfo
    {
        public String p_id;
        public String t_id;
        public String p_name;
        public String p_quantity;
        public String p_price;
        public String p_installingPrice;
    }
}
