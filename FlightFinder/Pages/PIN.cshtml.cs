using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;

namespace FlightFinder
{
    public class PINModel : PageModel
    {
        public static string PIN { get; set; }

        public string UserPIN { get; set; }

        public string NewPassword;

        public string OldPassword;

        public string userEmail;

        public string Error;
        public void OnGet()
        {
            SetPIN();
            SendPINEmail();
        }

        public void SetPIN()
        {
            for (int i = 0; i < 5; i++)
            {
                PIN += Convert.ToString(new Random().Next(0, 9));
            }
        }

        public string GetPIN()
        {
            return PIN;
        }

        public void SendPINEmail()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("flightfinder20@gmail.com");
            mail.To.Add($"{Startup.userEmail.GetEmail()}");
            mail.Subject = "Test Mail";
            mail.Body = $"This email is to reset your forgotten password on FlightFinder.com. Your PIN is {GetPIN()}";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("flightfinder20@gmail.com", "Flightfinder20!");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public bool VerifyPIN()
        {
            UserPIN = Request.Form["UserPIN"];
            if (UserPIN == GetPIN())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerifyPasswords()
        {
            if (OldPassword == NewPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangePassword()
        {
            const string connectionString = "server=flightfinder.cwmrpa3cnct9.us-east-1.rds.amazonaws.com;user id=admin;password=flightfinder20;database=flightfinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            userEmail = Startup.userEmail.GetEmail();
            NewPassword = Request.Form["NewPassword"];

            conn.Open();
            string txtcmd = $"INSERT INTO user (User_Email, Password) " +
                $"VALUES (@User_Email, @Password)";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;


            cmd.Parameters.AddWithValue("@User_Email", userEmail);
            cmd.Parameters.AddWithValue("@Password", NewPassword);
            cmd.Prepare();
            cmd.ExecuteReader();

            conn.Dispose();
        }

        public async Task<IActionResult> OnPost()
        {
            if (VerifyPIN() && VerifyPasswords())
            {
                //database connection and update
                //return to sign in page
                ChangePassword();
                return Redirect("/Login");
            }
            else
            {
                Error = "Something is wrong with your passwords or your PIN";
                return Page();
            }
        }

    }
}