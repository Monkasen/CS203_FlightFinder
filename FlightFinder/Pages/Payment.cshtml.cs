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
using FlightFinder.Model;
using System.Security.Cryptography.X509Certificates;

namespace FlightFinder.Pages
{
    public class PaymentModel : PageModel
    {
        public static string User_ID { get; set; }
        public static string Flight_ID { get; set; }
        public string Card_Name { get; set; }
        public string Card_Number { get; set; }
        public string Expir_Month { get; set; }
        public string Expir_Year { get; set; }
        public string CVV_Number { get; set; }
        public string ErrorText { get; set; }

        public void OnGet() {
            User_ID = "1"; // CHANGE THIS TO DYNAMIC USER ID!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ErrorText = "";
            Flight_ID = Request.Query["Flight_ID"];
            Console.WriteLine(Flight_ID);
        }

        private void AddCardToDB() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            Card_Name = Request.Form["Username"];
            Card_Number = Request.Form["User_Email"];
            Expir_Month = Request.Form["Password"];
            Expir_Year = Request.Form["First_Name"];
            CVV_Number = Request.Form["Last_Name"];

            conn.Open();

            string txtcmd = $"INSERT INTO payment_card (User_ID, Card_Name, Card_Number, Expir_Month, Expir_Year, CVV_Number) " +
                $"VALUES (@User_ID, @Card_Name, @Card_Number, @Expir_Month, @Expir_Year, @CVV_Number )";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@User_ID", User_ID);
            cmd.Parameters.AddWithValue("@Card_Name", Card_Name);
            cmd.Parameters.AddWithValue("@Card_Number", Card_Number);
            cmd.Parameters.AddWithValue("@Expir_Month", Expir_Month);
            cmd.Parameters.AddWithValue("@Expir_Year", Expir_Year);
            cmd.Parameters.AddWithValue("@CVV_Number", CVV_Number);
            cmd.Prepare();
            cmd.ExecuteReader();

            conn.Dispose();
        }

        public async Task<IActionResult> OnPost() {
            AddCardToDB();
            return Redirect($"/Book?Flight_ID={Flight_ID}");
        }
    }
}
