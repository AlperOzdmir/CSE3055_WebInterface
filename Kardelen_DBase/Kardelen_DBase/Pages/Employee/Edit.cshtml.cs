using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Employee
{
    public class EditModel : PageModel
    {
        Connection conn = new Connection();
        public EmployeeInfo employee = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["e_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Employee WHERE employeeID = @e_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@e_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employee.e_id = reader.GetInt32(0).ToString();
                                employee.hasReference = reader["hasReference"].ToString();
                                employee.firstName = reader.GetString(2);
                                employee.lastName = reader.GetString(3);
                                employee.age = reader.GetInt32(4).ToString();
                                employee.salary = reader.GetInt32(5).ToString();
                                employee.maritalStatus = reader["maritalStatus"].ToString();
                                employee.experience = reader["experience"].ToString();
                                employee.startDate = reader["startDate"].ToString();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage= ex.Message;
                return;
            }
        }
        public void OnPost()
        {
            employee.e_id = Request.Form["e_id"];
            employee.hasReference = Request.Form["hasReference"];
            employee.firstName = Request.Form["firstName"];
            employee.lastName = Request.Form["lastName"];
            employee.age = Request.Form["age"];
            employee.salary = Request.Form["salary"];
            employee.maritalStatus = Request.Form["maritalStatus"];
            employee.experience = Request.Form["experience"];
            employee.startDate = Request.Form["startDate"];

            if (employee.e_id.Length == 0 || employee.hasReference.Length == 0
                || employee.firstName.Length == 0 || employee.lastName.Length == 0
                || employee.age.Length == 0 || employee.salary.Length == 0
                || employee.maritalStatus.Length == 0 || employee.experience.Length == 0
                || employee.startDate.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Employee SET hasReference = @hasReference, firstName = @firstName, " +
                        "lastName = @lastName, age = @age, salary = @salary, maritalStatus = @maritalStatus, " +
                        "experience = @experience, startDate = @startDate" +
                        " WHERE employeeID = @e_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@e_id", employee.e_id);
                        command.Parameters.AddWithValue("@hasReference", employee.hasReference);
                        command.Parameters.AddWithValue("@firstName", employee.firstName);
                        command.Parameters.AddWithValue("@lastName", employee.lastName);
                        command.Parameters.AddWithValue("@age", employee.age);
                        command.Parameters.AddWithValue("@salary", employee.salary);
                        command.Parameters.AddWithValue("@maritalStatus", employee.maritalStatus);
                        command.Parameters.AddWithValue("@experience", employee.experience);
                        command.Parameters.AddWithValue("@startDate", employee.startDate);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Employee/Index");
        }
    }
}
