using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using FlightFinder.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


namespace FlightFinder.Pages
{
    public class RegistrationModel : PageModel
    {
        public int User_ID { get; }
        public string User_Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string ErrorText { get; set; }

        public DateTime Account_Creation_Date { get; set; }
        public void OnGet()
        {
            ErrorText = "";
        }

        public bool AddUserToDB()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            Username = Request.Form["Username"];
            User_Email = Request.Form["User_Email"];
            Password = Request.Form["Password"];
            First_Name = Request.Form["First_Name"];
            Last_Name = Request.Form["Last_Name"];
            Account_Creation_Date = DateTime.Today;

            if (Username == "" || User_Email == "" || Password == "") {
                ErrorText = "Not all required fields are completed. Please try again.";
                return false;
            }

            conn.Open();

            string txtcmd = $"INSERT INTO user (User_Email, Username, Password, First_Name, Last_Name, Account_Creation_Date) " +
                $"VALUES (@User_Email, @Username, @Password, @First_Name, @Last_Name, @Account_Creation_Date )";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;
            
            cmd.Parameters.AddWithValue("@User_Email", User_Email);
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@First_Name", First_Name);
            cmd.Parameters.AddWithValue("@Last_Name", Last_Name);
            cmd.Parameters.AddWithValue("@Account_Creation_Date", Account_Creation_Date);
            cmd.Prepare();
            cmd.ExecuteReader();
           
            conn.Dispose();
            return true;
        }

        public async Task<IActionResult> OnPost()
        {
            if (AddUserToDB()) {
                return Redirect("/Login");
            }
            else {
                return Page();
            }

        }
    }
}
