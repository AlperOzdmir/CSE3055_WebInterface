using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Employee
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> employeeInfos= new List<EmployeeInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Employee";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                EmployeeInfo employee = new EmployeeInfo();
                                employee.e_id = reader.GetInt32(0).ToString();
                                employee.hasReference = reader["hasReference"].ToString();
                                employee.firstName = reader.GetString(2);
                                employee.lastName = reader.GetString(3);
                                employee.age = reader.GetInt32(4).ToString();
                                employee.salary = reader.GetInt32(5).ToString();
                                employee.maritalStatus = reader["maritalStatus"].ToString();
                                employee.experience = reader.GetInt32(7).ToString();
                                employee.startDate = reader["startDate"].ToString();
                                employeeInfos.Add(employee);
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

    public class EmployeeInfo
    {
        public String e_id;
        public String hasReference;
        public String firstName;
        public String lastName;
        public String age;
        public String salary;
        public String maritalStatus;
        public String experience;
        public String startDate;
    }
}
