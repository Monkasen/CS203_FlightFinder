using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;



namespace FlightFinder
{
   
    public class ForgotPasswordModel : PageModel
    {
        public string User_Email { get; set; }

        public string ErrorText;



        public void OnGet()
        {
        }

        bool VerifyEmail()
        {
            const string connectionString = "server=flightfinder.cwmrpa3cnct9.us-east-1.rds.amazonaws.com;user id=admin;password=flightfinder20;database=flightfinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataSet = new DataTable();

            User_Email = Request.Form["User_Email"];

            conn.Open();

            string cmdText = $"SELECT User_Email FROM user WHERE User_Email = '{User_Email}'";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(dataSet);

            if (dataSet.Rows.Count < 1)
            {
                conn.Dispose();

                return false;
            }
            else
            {
                conn.Dispose();

                return true;
            }
        }

        void GetUserEmail()
        {
            User_Email = Request.Form["User_Email"];

            const string connectionString = "server=flightfinder.cwmrpa3cnct9.us-east-1.rds.amazonaws.com;user id=admin;password=flightfinder20;database=flightfinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string cmdText = $"SELECT User_ID FROM user WHERE User_Email = '{this.User_Email}'";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                User_Email = (string.Format("{0}", rdr["User_Email"].ToString()));
            }

            conn.Dispose();
        }

        public async Task<IActionResult> OnPost()
        {
            if (VerifyEmail())
            {
                Startup.userEmail.SetEmail(User_Email);
                Startup.userEmail.GetEmail();
                return Redirect($"/PIN?email={User_Email}");
            }
            else
            {
                ErrorText = "An account with this email does not exist";
                return Page();
            }
        }
    }
}