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
        public string Flight_ID;
        public string Departure_City;
        public string Arrival_City;
        public string Departure_Time;
        public string Arrival_Time;
        public string Flight_Date;
        public string Airline_Name;
        public string Aircraft_Reg_Num;
        public string Aircraft_Type;
        public string Flight_Distance;
        public string E_Flight_Time;
        public string Total_Seats;
        public string Open_Seats;
        public string YourText;
        public int TableSize = 0;

         public void OnGet() {
            TableFill();
            Console.WriteLine(TableSize);
        }
        
        public void TableFill() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open(); 

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM flight", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read()) { 
                    string F_ID = (string.Format("{0}", rdr["flight_id"].ToString()));
                    Flight_ID = F_ID;                    
                    string D_City = (string.Format("{0}", rdr["departure_city"].ToString()));
                    Departure_City = D_City;
                    string A_City = (string.Format("{0}", rdr["arrival_city"].ToString()));
                    Arrival_City = A_City;
                    string F_Date = (string.Format("{0}", rdr["flight_date"].ToString()));
                    Flight_Date = F_Date;
                    string D_Time = (string.Format("{0}", rdr["departure_time"].ToString()));
                    Departure_Time = D_Time;
                    string A_Time = (string.Format("{0}", rdr["arrival_time"].ToString()));
                    Arrival_Time = A_Time;
                    string E_F_Time = (string.Format("{0}", rdr["estimated_flight_time"].ToString()));
                    E_Flight_Time = E_F_Time;                  
                    string Airline = (string.Format("{0}", rdr["airline"].ToString()));
                    Airline_Name = Airline;
                    string A_Registration = (string.Format("{0}", rdr["aircraft_registration"].ToString()));
                    Aircraft_Reg_Num = A_Registration;
                    string A_Type = (string.Format("{0}", rdr["aircraft_type"].ToString()));
                    Aircraft_Type = A_Type;
                    string F_Distance = (string.Format("{0} miles", rdr["flight_distance"].ToString()));
                    Flight_Distance = F_Distance;
                    string T_Seats = (string.Format("{0}", rdr["total_seats"].ToString()));
                    Total_Seats = T_Seats;
                    string O_Seats = (string.Format("{0}", rdr["open_seats"].ToString()));
                    Open_Seats = O_Seats;

                    TableSize++;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose(); 
        }
    }  
}
