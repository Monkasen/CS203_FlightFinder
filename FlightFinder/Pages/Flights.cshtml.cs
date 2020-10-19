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
    public class FlightsModel : PageModel
    {
        public string YourText;

         public void OnGet() {
            TableFill();
            YourText = "test";
        }
        
        void TableFill() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open(); 

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM flight", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) { // remove console.writeline and set each equal to variable to pass them to the html
                    Console.WriteLine(string.Format("flight_id = {0}", rdr["flight_id"].ToString()));
                    Console.WriteLine(string.Format("departure_city = {0}", rdr["departure_city"].ToString()));
                    Console.WriteLine(string.Format("arrival_city = {0}", rdr["arrival_city"].ToString()));
                    Console.WriteLine(string.Format("flight_date = {0}", rdr["flight_date"].ToString()));
                    Console.WriteLine(string.Format("departure_time = {0}", rdr["departure_time"].ToString()));
                    Console.WriteLine(string.Format("arrival_time = {0}", rdr["arrival_time"].ToString()));
                    Console.WriteLine(string.Format("estimated_flight_time = {0}", rdr["estimated_flight_time"].ToString()));
                    Console.WriteLine(string.Format("airline = {0}", rdr["airline"].ToString()));
                    Console.WriteLine(string.Format("aircraft_registration = {0}", rdr["aircraft_registration"].ToString()));
                    Console.WriteLine(string.Format("aircraft_type = {0}", rdr["aircraft_type"].ToString()));
                    Console.WriteLine(string.Format("flight_distance = {0}", rdr["flight_distance"].ToString()));
                    Console.WriteLine(string.Format("total_seats = {0}", rdr["total_seats"].ToString()));
                    Console.WriteLine(string.Format("open_seats = {0}", rdr["open_seats"].ToString()));
                }

            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose(); 
        }
    }  
}
