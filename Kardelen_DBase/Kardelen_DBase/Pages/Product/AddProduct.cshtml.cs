using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;

namespace Kardelen_DBase.Pages.Product
{
    public class AddProductModel : PageModel
    {
        Connection conn = new Connection();
        public ProductInfo product = new ProductInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            product.p_name = Request.Form["p_name"];
            product.p_price = Request.Form["p_price"];
            product.p_installingPrice = Request.Form["p_installingPrice"];

            if (product.p_name.Length == 0 || product.p_price.Length == 0
                || product.p_installingPrice.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_EnterProduct " + "@p_name, @p_price, @p_installingPrice";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@p_name", product.p_name);
                        command.Parameters.AddWithValue("@p_price", product.p_price);
                        command.Parameters.AddWithValue("@p_installingPrice", product.p_installingPrice);

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

            product.p_name = ""; product.p_price = ""; product.p_installingPrice = "";
            successMessage = "New Product Type Added Correctly";
        }
    }
}
