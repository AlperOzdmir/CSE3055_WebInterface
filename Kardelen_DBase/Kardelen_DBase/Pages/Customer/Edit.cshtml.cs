using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Customer
{
    public class EditModel : PageModel
    {
        Connection conn = new Connection();
        public CustomerInfo customer = new CustomerInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["c_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Customer WHERE customerID = @c_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@c_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer.c_id = reader.GetInt32(0).ToString();
                                customer.hasReference = reader["hasReference"].ToString();
                                customer.firstName = reader.GetString(2);
                                customer.lastName = reader.GetString(3);
                                customer.contactNumber = reader.GetString(4);
                                customer.address = reader.GetString(5);
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
            customer.c_id = Request.Form["c_id"];
            customer.hasReference = Request.Form["hasReference"];
            customer.firstName = Request.Form["firstName"];
            customer.lastName = Request.Form["lastName"];
            customer.contactNumber = Request.Form["contactNumber"];
            customer.address = Request.Form["address"];

            if (customer.c_id.Length == 0 || customer.hasReference.Length == 0 || customer.firstName.Length == 0
                || customer.lastName.Length == 0 || customer.contactNumber.Length == 0
                || customer.address.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Customer SET hasReference = @hasReference, firstName = @firstName, " +
                        "lastName = @lastName, contactNumber = @contactNumber, address = @address" +
                        " WHERE customerID = @c_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@c_id", customer.c_id);
                        command.Parameters.AddWithValue("@hasReference", customer.hasReference);
                        command.Parameters.AddWithValue("@firstName", customer.firstName);
                        command.Parameters.AddWithValue("@lastName", customer.lastName);
                        command.Parameters.AddWithValue("@contactNumber", customer.contactNumber);
                        command.Parameters.AddWithValue("@address", customer.address);

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
            Response.Redirect("/Customer/Index");
        }
    }
}
