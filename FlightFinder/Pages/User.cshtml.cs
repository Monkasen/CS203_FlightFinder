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
    public class UserModel : PageModel
    {
        public string User_ID;
        List<string> F_ID = new List<string>();
        public string[] Flight_ID;
        List<string> D_City = new List<string>();
        public string[] Departure_City;
        List<string> A_City = new List<string>();
        public string[] Arrival_City;
        List<string> D_Time = new List<string>();
        public string[] Departure_Time;
        List<string> A_Time = new List<string>();
        public string[] Arrival_Time;
        List<string> F_Date = new List<string>();
        public string[] Flight_Date;
        List<string> A_Name = new List<string>();
        public string[] Airline_Name;
        List<string> A_Registration = new List<string>();
        public string[] Aircraft_Reg_Num;
        List<string> A_Type = new List<string>();
        public string[] Aircraft_Type;
        List<string> F_Distance = new List<string>();
        public string[] Flight_Distance;
        List<string> E_F_Time = new List<string>();
        public string[] E_Flight_Time;
        List<string> T_Seats = new List<string>();
        public string[] Total_Seats;
        List<string> O_Seats = new List<string>();
        public string[] Open_Seats;

        public int TableSize = 0;

        public void OnGet() {
            User_ID = Request.Query["User_ID"];
            TableFill();
            Console.WriteLine($"THIS IS ON USER PAGE: {User_ID}");
        }

        public void TableFill()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM flight, saved_flights WHERE user_id = {User_ID} AND saved_flights.flight_id = flight.flight_id", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    F_ID.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                    Flight_ID = F_ID.ToArray();
                    D_City.Add(string.Format("{0}", rdr["departure_city"].ToString()));
                    Departure_City = D_City.ToArray();
                    A_City.Add(string.Format("{0}", rdr["arrival_city"].ToString()));
                    Arrival_City = A_City.ToArray();
                    F_Date.Add(string.Format("{0}", rdr["flight_date"].ToString()));
                    Flight_Date = F_Date.ToArray();
                    D_Time.Add(string.Format("{0}", rdr["departure_time"].ToString()));
                    Departure_Time = D_Time.ToArray();
                    A_Time.Add(string.Format("{0}", rdr["arrival_time"].ToString()));
                    Arrival_Time = A_Time.ToArray();
                    E_F_Time.Add(string.Format("{0}", rdr["estimated_flight_time"].ToString()));
                    E_Flight_Time = E_F_Time.ToArray();
                    A_Name.Add(string.Format("{0}", rdr["airline"].ToString()));
                    Airline_Name = A_Name.ToArray();
                    A_Registration.Add(string.Format("{0}", rdr["aircraft_registration"].ToString()));
                    Aircraft_Reg_Num = A_Registration.ToArray();
                    A_Type.Add(string.Format("{0}", rdr["aircraft_type"].ToString()));
                    Aircraft_Type = A_Type.ToArray();
                    F_Distance.Add(string.Format("{0} miles", rdr["flight_distance"].ToString()));
                    Flight_Distance = F_Distance.ToArray();
                    T_Seats.Add(string.Format("{0}", rdr["total_seats"].ToString()));
                    Total_Seats = T_Seats.ToArray();
                    O_Seats.Add(string.Format("{0}", rdr["open_seats"].ToString()));
                    Open_Seats = O_Seats.ToArray();

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