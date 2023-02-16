using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Statistics
{
    public class TheMoneySpentOnMaterialsThisMonth : PageModel
    {
        public List<SpentMoneyInfo> listSpentMoneyInfo = new List<SpentMoneyInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "select * from The_Money_Earned_This_Month";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SpentMoneyInfo spentMoneyInfo = new SpentMoneyInfo();
                                if(reader.IsDBNull(0))
                                {
                                    spentMoneyInfo.spentMoney = "0";
                                }
                                else
                                {
                                    spentMoneyInfo.spentMoney = "" + reader.GetInt32(0);
                                }


                                listSpentMoneyInfo.Add(spentMoneyInfo);
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

    public class SpentMoneyInfo
    {
        public String spentMoney;
    }
}
