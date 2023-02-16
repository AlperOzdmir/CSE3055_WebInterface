using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Product
{
    public class ProductFormulaModel : PageModel
    {
        public List<FormulaInfo> formulaInfos = new List<FormulaInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product_Formula";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FormulaInfo formula = new FormulaInfo();
                                formula.p_id = reader["productID"].ToString();
                                formula.m_id = reader["materialID"].ToString();
                                formula.quantity = reader["materialQuantity"].ToString();
                                formulaInfos.Add(formula);
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

    public class FormulaInfo
    {
        public String p_id;
        public String m_id;
        public String quantity;
    }
}
