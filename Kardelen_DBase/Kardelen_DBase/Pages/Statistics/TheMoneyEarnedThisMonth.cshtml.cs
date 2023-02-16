using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Statistics
{
    public class TheMoneyEarnedThisMonth : PageModel
    {
        public List<EarnedMoneyInfo> listEarnedMoneyInfo = new List<EarnedMoneyInfo>();

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
                                EarnedMoneyInfo earnedMoneyInfo = new EarnedMoneyInfo();
                                if(reader.IsDBNull(0))
                                {
                                    earnedMoneyInfo.earnedMoney = "0";
                                }
                                else
                                {
                                    earnedMoneyInfo.earnedMoney = "" + reader.GetInt32(0);
                                }
                                listEarnedMoneyInfo.Add(earnedMoneyInfo);
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

    public class EarnedMoneyInfo
    {
        public String earnedMoney;
    }
}
