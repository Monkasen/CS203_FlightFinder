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

        List<string> D_Port = new List<string>();
        static public string[] Departure_Airport;
        List<string> A_Port = new List<string>();
        static public string[] Arrival_Airport;

        List<string> D_Time = new List<string>();
        static public string[] Departure_Time;
        List<string> A_Time = new List<string>();
        static public string[] Arrival_Time;
        List<string> F_Date = new List<string>();
        static public string[] Flight_Date;
        List<string> Air_Name = new List<string>();
        static public string[] Airline_Name;
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

        List<string> U_Email = new List<string>();
        public string[] User_Email; 

        static public string User_ID;
        public string Seats_Reserved;
        public string LastFour_Card;

        static string StaticSeats;
        static string StaticCard;

        public double FlightPrice;

        public void OnGet() {
            User_ID = Startup.CurrentUser.GetUser();
            Flight_ID = Request.Query["Flight_ID"];
            Seats_Reserved = Request.Query["Seats"];
            LastFour_Card = Request.Query["Card_ID"];

            StaticSeats = Seats_Reserved;
            StaticCard = LastFour_Card;

            float seatNum = Int32.Parse(Seats_Reserved);
            FlightPrice = seatNum * 89.99;

            FlightTableFill();
        }

        private void FlightTableFill()
        {
            const string connectionString = "server=flightfinder.cwmrpa3cnct9.us-east-1.rds.amazonaws.com;user id=admin;password=flightfinder20;database=flightfinder;port=3306;persistsecurityinfo=True;";
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
                    D_Port.Add(string.Format("{0}", rdr["departure_airport"].ToString()));
                    Departure_Airport = D_Port.ToArray();
                    A_Port.Add(string.Format("{0}", rdr["arrival_airport"].ToString()));
                    Arrival_Airport = A_Port.ToArray();
                    F_Date.Add(string.Format("{0}", rdr["flight_date"].ToString()));
                    Flight_Date = F_Date.ToArray();
                    D_Time.Add(string.Format("{0}", rdr["departure_time"].ToString()));
                    Departure_Time = D_Time.ToArray();
                    A_Time.Add(string.Format("{0}", rdr["arrival_time"].ToString()));
                    Arrival_Time = A_Time.ToArray();
                    E_F_Time.Add(string.Format("{0}", rdr["estimated_flight_time"].ToString()));
                    E_Flight_Time = E_F_Time.ToArray();
                    Air_Name.Add(string.Format("{0}", rdr["airline"].ToString()));
                    Airline_Name = Air_Name.ToArray();
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

        public void SendConfirmationEmail() {
            if (FlightFinder.Startup.CurrentUser.GetNotification()) { // If notifications are enabled, send the email
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("flightfinder20@gmail.com");
                mail.To.Add($"{GetUserEmail()}");
                mail.Subject = "Test Mail";
                mail.Body = $"This is a confirmation email for your recent booking on FlightFinder.com. You have reserved a flight from {Departure_City[0]} to {Arrival_City[0]}. You have reserved {StaticSeats} seat(s) for this flight." +
                    $" You made this reservation using a card ending in {StaticCard}. The total cost was: ${FlightPrice}";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("flightfinder20@gmail.com", "Flightfinder20!");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
        }

        private string GetUserEmail() {
            const string connectionString = "server=flightfinder.cwmrpa3cnct9.us-east-1.rds.amazonaws.com;user id=admin;password=flightfinder20;database=flightfinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM user WHERE user_id = {User_ID}", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {
                    U_Email.Add(string.Format("{0}", rdr["user_email"].ToString()));
                    User_Email = U_Email.ToArray();
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();

            return User_Email[0];
        }

        public void BookFlight() {
            const string connectionString = "server=flightfinder.cwmrpa3cnct9.us-east-1.rds.amazonaws.com;user id=admin;password=flightfinder20;database=flightfinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string txtcmd = $"INSERT INTO booked_flights (User_ID, Flight_ID, Seats_Reserved, LastFour_Card) " +
                $"VALUES (@User_ID, @Flight_ID, @Seats_Reserved, @LastFour_Card )";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@User_ID", User_ID);
            cmd.Parameters.AddWithValue("@Flight_ID", Flight_ID[0]);
            cmd.Parameters.AddWithValue("@Seats_Reserved", StaticSeats);
            cmd.Parameters.AddWithValue("@LastFour_Card", StaticCard);
            cmd.Prepare();
            cmd.ExecuteReader();

            conn.Dispose();
        }
        public async Task<IActionResult> OnPost(string submit) {
            Console.WriteLine("DEBUG - Confirm Booking");
            BookFlight();
            SendConfirmationEmail();
            return Redirect($"/Flights");
        }
    }
}
