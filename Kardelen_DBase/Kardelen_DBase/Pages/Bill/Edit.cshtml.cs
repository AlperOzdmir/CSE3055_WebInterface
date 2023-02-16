using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Kardelen_DBase;

namespace Kardelen_DBase.Pages.Bill
{
    public class EditModel : PageModel
    {
        Connection conn = new Connection();
        public BillInfo bill = new BillInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["b_id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Bill WHERE billID = @b_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@b_id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bill.b_id = reader["billID"].ToString();
                                bill.payment = reader["paymentType"].ToString();
                                bill.instalment = reader["instalmentMonths"].ToString();
                                bill.amount = reader["transactionAmount"].ToString();
                                bill.kdv = reader["kdv"].ToString();
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
            bill.b_id = Request.Form["b_id"];
            bill.payment = Request.Form["payment"];
            bill.instalment = Request.Form["instalment"];
            bill.amount = Request.Form["amount"];
            bill.kdv = Request.Form["kdv"];

            if (bill.b_id.Length == 0 || bill.instalment.Length == 0
                || bill.amount.Length == 0 || bill.kdv.Length == 0
                || bill.payment.Length == 0)
            {
                errorMessage = "Please fill all the fields";
                return;
            }
            
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.connString))
                {
                    connection.Open();
                    String sql = "UPDATE Bill SET paymentType = @payment, instalmentMonths = @instalment, " +
                        "transactionAmount = @amount, kdv = @kdv WHERE billID = @b_id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@payment", bill.payment);
                        command.Parameters.AddWithValue("@instalment", bill.instalment);
                        command.Parameters.AddWithValue("@amount", bill.amount);
                        command.Parameters.AddWithValue("@kdv", bill.kdv);
                        command.Parameters.AddWithValue("@b_id", bill.b_id);

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
            Response.Redirect("/Bill/Index");
        }
    }
}
