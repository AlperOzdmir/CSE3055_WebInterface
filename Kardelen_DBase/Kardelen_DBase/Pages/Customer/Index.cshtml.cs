using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Kardelen_DBase.Pages.Customer
{
    public class IndexModel : PageModel
    {
        public List<CustomerInfo> customerInfos= new List<CustomerInfo>();

        public void OnGet()
        {
            try
            {
                Connection conn = new Connection();
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Customer";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                CustomerInfo customer = new CustomerInfo();
                                customer.c_id = reader.GetInt32(0).ToString();
                                customer.hasReference = reader["hasReference"].ToString();
                                customer.firstName = reader.GetString(2);
                                customer.lastName = reader.GetString(3);
                                customer.contactNumber = reader.GetString(4);
                                customer.address = reader.GetString(5);
                                customerInfos.Add(customer);
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

    public class CustomerInfo
    {
        public String c_id;
        public String hasReference;
        public String firstName;
        public String lastName;
        public String contactNumber;
        public String address;

    }
}
