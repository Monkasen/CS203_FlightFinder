using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;

namespace FlightFinder
{
    public class LoginModel : PageModel
    {
        List<string> U_ID = new List<string>();
        public string User_ID { get; set; }
        List<string> U_Email = new List<string>();
        public string User_Email { get; set; }
        List<string> U_Name = new List<string>();
        public string Username { get; set; }
        List<string> pswrd = new List<string>();
        public string Password { get; set; }
        List<string> F_Name = new List<string>();
        public string[] First_Name;
        List<string> L_Name = new List<string>();
        public string[] Last_Name;
        List<string> Acc_C_Date = new List<string>();
        public string[] Account_Creation_Date;

        public void OnGet()
        {
            SearchTable();
        }

        void SearchTable()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataSet = new DataTable();
            // MySqlConnection conn = new MySqlConnection(connectionString);
            //Username = Request.Form["Username"];
            //User_Email = Request.Form["Email"];
            //Password = Request.Form["Password"];
            

            try
            {
                conn.Open();

                string cmdText = $"SELECT * FROM user WHERE Username = {this.Username} AND Password = {this.Password} AND User_Email = {this.User_Email}";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                cmd.CommandType = CommandType.Text;
                adapter.SelectCommand = cmd;
                adapter.Fill(dataSet);
                // MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (dataSet.Rows.Count < 1)
                {
                    RedirectToPage();
                }
                else
                {
                    Redirect("/Flights");
                }
                //conn.Dispose();
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            conn.Dispose();
            //return true;
        }
    }
}