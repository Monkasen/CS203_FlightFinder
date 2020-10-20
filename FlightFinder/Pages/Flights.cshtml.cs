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
        //public Flight Flight;
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
                    //StringBuilder sb = new StringBuilder();
                    string ID = (string.Format("flight_id = {0}", rdr["flight_id"].ToString()));
                   // Flight.FlightID = Convert.ToInt32(ID);
                    
                    string D_City = (string.Format("departure_city = {0}", rdr["departure_city"].ToString()));
                    //string D_City = rdr["departure_ciy"].ToString();
                    //Departure_City = D_City.ToString();
                    //sb.AppendFormat("{0}", Departure_City);
                    Departure_City = D_City;
                    string A_City = (string.Format("arrival_city = {0}", rdr["arrival_city"].ToString()));
                    Arrival_City = A_City;
                    string F_Date = (string.Format("flight_date = {0}", rdr["flight_date"].ToString()));
                    Flight_Date = F_Date;
                    string D_Time = (string.Format("departure_time = {0}", rdr["departure_time"].ToString()));
                    Departure_Time = D_Time;
                    string A_Time = (string.Format("arrival_time = {0}", rdr["arrival_time"].ToString()));
                    Arrival_Time = A_Time;
                    string E_F_Time = (string.Format("estimated_flight_time = {0}", rdr["estimated_flight_time"].ToString()));
                    E_Flight_Time = E_F_Time;                  
                    string Airline = (string.Format("airline = {0}", rdr["airline"].ToString()));
                    Airline_Name = Airline;
                    string A_Registration = (string.Format("aircraft_registration = {0}", rdr["aircraft_registration"].ToString()));
                    Aircraft_Reg_Num = A_Registration;
                    string A_Type = (string.Format("aircraft_type = {0}", rdr["aircraft_type"].ToString()));
                    Aircraft_Type = A_Type;
                    string F_Distance = (string.Format("flight_distance = {0}", rdr["flight_distance"].ToString()));
                    Flight_Distance = F_Distance;
                    string T_Seats = (string.Format("total_seats = {0}", rdr["total_seats"].ToString()));
                    Total_Seats = T_Seats;
                    string O_Seats = (string.Format("open_seats = {0}", rdr["open_seats"].ToString()));
                    Open_Seats = O_Seats;
                }

            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose(); 
        }
    }  
}
