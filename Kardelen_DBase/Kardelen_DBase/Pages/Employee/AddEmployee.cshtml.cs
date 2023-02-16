using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;

namespace Kardelen_DBase.Pages.Employee
{
    public class AddEmployeeModel : PageModel
    {
        Connection conn = new Connection();
        public EmployeeInfo employee = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            employee.hasReference = Request.Form["hasReference"];
            employee.firstName = Request.Form["firstName"];
            employee.lastName = Request.Form["lastName"];
            employee.age = Request.Form["age"];
            employee.salary = Request.Form["salary"];
            employee.maritalStatus = Request.Form["maritalStatus"];
            employee.experience = Request.Form["experience"];

            if (employee.hasReference.Length == 0 || employee.firstName.Length == 0
                || employee.salary.Length == 0 || employee.age.Length == 0
                || employee.salary.Length == 0 ||employee.maritalStatus.Length == 0
                || employee.experience.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_EmployEmployee " + "@hasReference, @firstName, @lastName, @age, @salary, @maritalStatus, @experience";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@hasReference", employee.hasReference);
                        command.Parameters.AddWithValue("@firstName", employee.firstName);
                        command.Parameters.AddWithValue("@lastName", employee.lastName);
                        command.Parameters.AddWithValue("@age",  employee.age);
                        command.Parameters.AddWithValue("@salary", employee.salary);
                        command.Parameters.AddWithValue("@maritalStatus", employee.maritalStatus);
                        command.Parameters.AddWithValue("@experience", employee.experience);

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

            employee.hasReference = ""; employee.firstName = ""; employee.lastName = "";
            employee.age = ""; employee.salary = ""; employee.maritalStatus = ""; employee.experience = "";
            successMessage = "Employee Added Correctly";
        }
    }
}
