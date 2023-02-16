using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Log
{
    public class IndexModel : PageModel
    {
        public List<LogInfo> logInfos= new List<LogInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Log_Table";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                LogInfo log = new LogInfo();
                                log.op = reader["operation"].ToString();
                                log.r_id = reader["employeeID"].ToString();
                                log.op_date = reader["operationDate"].ToString();
                                logInfos.Add(log);
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

    public class LogInfo
    {
        public String op;
        public String r_id;
        public String op_date;
    }
}
