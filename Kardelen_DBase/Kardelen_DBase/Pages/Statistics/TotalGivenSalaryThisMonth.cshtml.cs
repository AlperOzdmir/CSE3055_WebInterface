using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Statistics
{
    public class TotalGivenSalaryThisMonth : PageModel
    {
        public List<SalaryInfo> listSalaryInfo = new List<SalaryInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    string sql = "select * from Total_Given_Salary_This_Month";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SalaryInfo salaryInfo = new SalaryInfo();

                                salaryInfo.totalSalary = "" + reader.GetInt32(0);
                                salaryInfo.employeeCount = "" + reader.GetInt32(1);

                                listSalaryInfo.Add(salaryInfo);
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

    public class SalaryInfo
    {
        public String totalSalary;
        public String employeeCount;
    }
}
