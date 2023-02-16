using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Transaction
{
    public class IndexModel : PageModel
    {
        public List<TransactionInfo> transactionInfos= new List<TransactionInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Work_Flow";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TransactionInfo transaction = new TransactionInfo();
                                transaction.t_id = reader["transactionID"].ToString();
                                transaction.c_id = reader["customerID"].ToString();
                                transaction.b_id = reader["billID"].ToString();
                                transaction.type = reader["transactionType"].ToString();
                                transaction.date = reader["transactionDate"].ToString();
                                transactionInfos.Add(transaction);
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

    public class TransactionInfo
    {
        public String t_id;
        public String c_id;
        public String b_id;
        public String type;
        public String date;
    }
}
