using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Bill
{
    public class IndexModel : PageModel
    {
        public List<BillInfo> billInfos= new List<BillInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Bill";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BillInfo bill = new BillInfo();
                                bill.b_id = reader["billID"].ToString();
                                bill.payment = reader["paymentType"].ToString();
                                bill.instalment = reader["instalmentMonths"].ToString();
                                bill.amount = reader["transactionAmount"].ToString();
                                bill.kdv = reader["kdv"].ToString();
                                billInfos.Add(bill);
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

    public class BillInfo
    {
        public String b_id;
        public String payment;
        public String instalment;
        public String amount;
        public String kdv;
    }
}
