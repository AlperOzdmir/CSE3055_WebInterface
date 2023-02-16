using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using Kardelen_DBase;

namespace Kardelen_DBase.Pages.Bill
{
    public class AddBillModel : PageModel
    {
        Connection conn = new Connection();
        public BillInfo bill = new BillInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            bill.payment = Request.Form["payment"];
            bill.instalment = Request.Form["instalment"];
            bill.amount = Request.Form["amount"];

            if ( bill.payment.Length == 0
                || bill.instalment.Length == 0 || bill.amount.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            else if (bill.payment == "advance" && bill.instalment != "0")
            {
                errorMessage = "Conflict on payment type and instalment month";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "EXEC spa_EnterBill " + "@payment, @instalment, @amount";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@payment", bill.payment);
                        command.Parameters.AddWithValue("@instalment", bill.instalment);
                        command.Parameters.AddWithValue("@amount",  bill.amount);

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

            bill.payment = ""; bill.instalment = ""; bill.amount = "";
            successMessage = "Bill Added Correctly";
        }
    }
}
