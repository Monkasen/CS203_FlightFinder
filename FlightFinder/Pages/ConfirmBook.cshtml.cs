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
using System.Net.Mail;

namespace FlightFinder.Pages
{
    public class ConfirmBookModel : PageModel
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

        static public string User_ID;
        static public string Seats_Reserved;
        static public string LastFour_Card;

        static public string User_Email;

        public void OnGet() {
            User_ID = "1"; // TEMPORARY VALUE TO STORE USER ID, DEFAULTS TO NULL USER, LATER CHANGE TO WHATEVER USER IS BOOKING THE FLIGHT!!!!!!!!!!!!!!!!!!!
            Flight_ID = Request.Query["Flight_ID"];
            Seats_Reserved = Request.Query["Seats"];
            LastFour_Card = Request.Query["Card_ID"];

            FlightTableFill();
        }

        public void SendConfirmationEmail() {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("flightfinder20@gmail.com");
            mail.To.Add("ndougan23@gmail.com"); // TEMPORARY VALUE TO STORE EMAIL, DEFAULTS TO MINE, LATER CHANGE TO WHATEVER USER IS BOOKING THE FLIGHT!!!!!!!!!!!!!!!!!!!
            mail.Subject = "Test Mail";
            mail.Body = $"This is a confirmation email for your recent booking on FlightFinder.com. You have reserved a flight from {Departure_City[0]} to {Arrival_City[0]}. You have reserved {Seats_Reserved} seat(s) for this flight." +
                $" You made this reservation using a card ending in {LastFour_Card}.";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("flightfinder20@gmail.com", "Flightfinder20!");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public async Task<IActionResult> OnPost(string submit) {
            Console.WriteLine("DEBUG - Confirm Booking");
            SendConfirmationEmail();
            return Redirect($"/Flights");
        }

        public void FlightTableFill()
        {
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
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }
    }
}
