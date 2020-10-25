﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using MySqlX.XDevAPI;

namespace FlightFinder
{
    public class LoginModel : PageModel
    {
       
        public string User_ID { get; set; }
      
        public string User_Email { get; set; }
       
        public string Username { get; set; }
       
        public string Password { get; set; }

        public string ErrorText { get; set; }

        public void OnGet()
        {
            ErrorText = "";
        }

        bool VerifyUser()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataSet = new DataTable();
            
            Username = Request.Form["Username"];
            User_Email = Request.Form["User_Email"];
            Password = Request.Form["Password"];

            conn.Open();

            string cmdText = $"SELECT * FROM user WHERE Username = '{this.Username}' AND Password = '{this.Password}' AND User_Email = '{this.User_Email}'";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(dataSet);

            if (dataSet.Rows.Count < 1) {
                conn.Dispose();
                return false;
            }
            else {
                conn.Dispose();
                return true;
            }
         }

        void GetUserID() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataSet = new DataTable();

            Username = Request.Form["Username"];
            User_Email = Request.Form["User_Email"];
            Password = Request.Form["Password"];

            conn.Open();

            string cmdText = $"SELECT User_ID FROM user WHERE Username = '{this.Username}' AND Password = '{this.Password}' AND User_Email = '{this.User_Email}'";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()) {
                User_ID = (string.Format("{0}", rdr["user_id"].ToString()));
            }
            Console.WriteLine(User_ID);
        }

        public async Task<IActionResult> OnPost() 
        {
            if (VerifyUser()) {
                GetUserID();
                return Redirect($"/User?User_ID={this.User_ID}");
            }
            else 
            {
                ErrorText = "Login failed. Please try again.";
                return Page();
            }
                
        }
    }
}