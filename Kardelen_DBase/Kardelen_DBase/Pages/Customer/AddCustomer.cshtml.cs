using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;

namespace Kardelen_DBase.Pages.Customer
{
    public class AddCustomerModel : PageModel
    {
        Connection conn = new Connection();
        public CustomerInfo customer = new CustomerInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            customer.hasReference = Request.Form["hasReference"];
            customer.firstName = Request.Form["firstName"];
            customer.lastName = Request.Form["lastName"];
            customer.contactNumber = Request.Form["contactNumber"];
            customer.address = Request.Form["address"];

            if (customer.hasReference.Length == 0 || customer.firstName.Length == 0
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
                    String sql = "EXEC spa_EnterCustomer " + "@hasReference, @firstName, @lastName, @contactNumber, @address";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@hasReference", customer.hasReference);
                        command.Parameters.AddWithValue("@firstName", customer.firstName);
                        command.Parameters.AddWithValue("@lastName", customer.lastName);
                        command.Parameters.AddWithValue("@contactNumber",  customer.contactNumber);
                        command.Parameters.AddWithValue("@address", customer.address);

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

            customer.hasReference = ""; customer.firstName = ""; customer.lastName = "";
            customer.contactNumber = ""; customer.address = "";
            successMessage = "Customer Added Correctly";
        }
    }
}
