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

namespace FlightFinder.Pages {
    public class BookModel : PageModel
    {
        #region flight_table
        List<string> F_ID = new List<string>();
        static public string[] Flight_ID;
        List<string> D_City = new List<string>();
        static public string[] Departure_City;
        List<string> A_City = new List<string>();
        static public string[] Arrival_City;
        List<string> D_Time = new List<string>();
        static public string[] Departure_Time;
        List<string> A_Time = new List<string>();
        static public string[] Arrival_Time;
        List<string> F_Date = new List<string>();
        static public string[] Flight_Date;
        List<string> A_Name = new List<string>();
        public static string[] Airline_Name;
        List<string> A_Registration = new List<string>();
        static public string[] Aircraft_Reg_Num;
        List<string> A_Type = new List<string>();
        static public string[] Aircraft_Type;
        List<string> F_Distance = new List<string>();
        static public string[] Flight_Distance;
        List<string> E_F_Time = new List<string>();
        static public string[] E_Flight_Time;
        List<string> T_Seats = new List<string>();
        static public string[] Total_Seats;
        List<string> O_Seats = new List<string>();
        static public string[] Open_Seats;
        #endregion

        #region card_list
        public int ListSize = 0;
        List<string> C_Num = new List<string>();
        static public string[] Card_Number;
        #endregion

        static public string User_ID = Startup.CurrentUser.GetUser();
        public string Seats_Reserved;
        public string LastFour_Card;
        static public int MaxSeats = 0;

        public void OnGet() {
            Flight_ID = Request.Query["Flight_ID"];
            FlightTableFill();
            CardListFill();
            MaxSeats = Int32.Parse(Open_Seats[0]);
        }

        public void FlightTableFill() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM flight WHERE flight.flight_id = {Flight_ID[0]}", conn);
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
                }

                for (int i = 0; i < Flight_Date.Length; ++i) { // Remove "12:00:00 AM" from string
                    Flight_Date[i] = Flight_Date[i].Remove(Flight_Date[i].Length - 12, 12);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }

        public void CardListFill() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM payment_card WHERE payment_card.user_id = {User_ID}", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    C_Num.Add(string.Format("{0}", rdr["card_number"].ToString()));
                    Card_Number = C_Num.ToArray();
                    Card_Number[ListSize] = Card_Number[ListSize].Substring(Card_Number[ListSize].Length - 4); // Get last 4 digits of the card number

                    ++ListSize;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }

        public async Task<IActionResult> OnPost(string submit) { 
            if (submit[0] == 'C') { 
                Console.WriteLine("DEBUG - Confirm Booking");
                LastFour_Card = Request.Form["Cards"];
                Seats_Reserved = Request.Form["Seats"];
                return Redirect($"/ConfirmBook?Flight_ID={Flight_ID[0]}&Seats={Seats_Reserved}&Card_ID={LastFour_Card}");
            }
            else if (submit[0] == 'N') {
                Console.WriteLine("DEBUG - New Card");
                return Redirect($"/Payment?Flight_ID={Flight_ID[0]}");
            }
            else {
                Console.WriteLine("DEBUG - Error");
                return RedirectToPage();
            }
        }
    }
}
