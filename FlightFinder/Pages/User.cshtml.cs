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
    public class UserModel : PageModel
    {
        public string User_ID;
        public string NotificationStatus;
        public int NotificationTimeout;

        #region user variables
        List<string> U_Email = new List<string>();
        public string[] User_Email;
        List<string> U_Uname = new List<string>();
        public string[] User_Username;
        List<string> U_Pword = new List<string>();
        public string[] User_Password;
        List<string> A_C_Date = new List<string>();
        public string[] Account_Creation_Date;
        List<string> N_Set = new List<string>();
        public string[] Notif_Setting;
        #endregion
        #region booked_flights variables
        public int B_TableSize = 0;
        List<string> B_F_ID = new List<string>();
        public string[] B_Flight_ID;

        List<string> B_D_City = new List<string>();
        public string[] B_Departure_City;
        List<string> B_A_City = new List<string>();
        public string[] B_Arrival_City;

        List<string> B_D_Port = new List<string>();
        public string[] B_Departure_Airport;
        List<string> B_A_Port = new List<string>();
        public string[] B_Arrival_Airport;

        List<string> B_D_Time = new List<string>();
        public string[] B_Departure_Time;
        List<string> B_A_Time = new List<string>();
        public string[] B_Arrival_Time;
        List<string> B_F_Date = new List<string>();
        public string[] B_Flight_Date;
        List<string> B_A_Name = new List<string>();
        public string[] B_Airline_Name;
        List<string> B_A_Registration = new List<string>();
        public string[] B_Aircraft_Reg_Num;
        List<string> B_A_Type = new List<string>();
        public string[] B_Aircraft_Type;
        List<string> B_F_Distance = new List<string>();
        public string[] B_Flight_Distance;
        List<string> B_E_F_Time = new List<string>();
        public string[] B_E_Flight_Time;
        List<string> B_T_Seats = new List<string>();
        public string[] B_Total_Seats;
        List<string> B_O_Seats = new List<string>();
        public string[] B_Open_Seats;
        #endregion      
        #region saved_flights variables
        public int TableSize = 0;
        List<string> F_ID = new List<string>();
        public string[] Flight_ID;

        List<string> D_City = new List<string>();
        public string[] Departure_City;
        List<string> A_City = new List<string>();
        public string[] Arrival_City;

        List<string> D_Port = new List<string>();
        public string[] Departure_Airport;
        List<string> A_Port = new List<string>();
        public string[] Arrival_Airport;

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
        #endregion
        

        public void OnGet() {
            User_ID = Startup.CurrentUser.GetUser();
            NotificationStatus = Request.Query["Notification"];
            if (NotificationStatus == null) {
                NotificationStatus = "0";
            }

            UserTableFill();
            BookedTableFill();
            SavedTableFill();
        }

        public void UserTableFill()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM user WHERE user_id = {User_ID}", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {
                    U_Email.Add(string.Format("{0}", rdr["user_email"].ToString()));
                    User_Email = U_Email.ToArray();
                    U_Uname.Add(string.Format("{0}", rdr["username"].ToString()));
                    User_Username = U_Uname.ToArray();
                    U_Pword.Add(string.Format("{0}", rdr["password"].ToString()));
                    User_Password = U_Pword.ToArray();
                    A_C_Date.Add(string.Format("{0}", rdr["account_creation_date"].ToString()));
                    Account_Creation_Date = A_C_Date.ToArray();
                    N_Set.Add(string.Format("{0}", rdr["notification_setting"].ToString()));
                    Notif_Setting = N_Set.ToArray();
                }

                Account_Creation_Date[0] = Account_Creation_Date[0].Remove(Account_Creation_Date[0].Length - 12, 12); // Remove "12:00:00 AM" from string
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }

        public void BookedTableFill()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM flight, booked_flights WHERE user_id = {User_ID} AND booked_flights.flight_id = flight.flight_id", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    B_F_ID.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                    B_Flight_ID = B_F_ID.ToArray();
                    B_D_City.Add(string.Format("{0}", rdr["departure_city"].ToString()));
                    this.B_Departure_City = B_D_City.ToArray();
                    B_A_City.Add(string.Format("{0}", rdr["arrival_city"].ToString()));
                    this.B_Arrival_City = B_A_City.ToArray();
                    B_D_Port.Add(string.Format("{0}", rdr["departure_airport"].ToString()));
                    this.B_Departure_Airport = B_D_Port.ToArray();
                    B_A_Port.Add(string.Format("{0}", rdr["arrival_airport"].ToString()));
                    this.B_Arrival_Airport = B_A_Port.ToArray();
                    B_F_Date.Add(string.Format("{0}", rdr["flight_date"].ToString()));
                    B_Flight_Date = B_F_Date.ToArray();
                    B_D_Time.Add(string.Format("{0}", rdr["departure_time"].ToString()));
                    B_Departure_Time = B_D_Time.ToArray();
                    B_A_Time.Add(string.Format("{0}", rdr["arrival_time"].ToString()));
                    B_Arrival_Time = B_A_Time.ToArray();
                    B_E_F_Time.Add(string.Format("{0}", rdr["estimated_flight_time"].ToString()));
                    B_E_Flight_Time = B_E_F_Time.ToArray();
                    B_A_Name.Add(string.Format("{0}", rdr["airline"].ToString()));
                    B_Airline_Name = B_A_Name.ToArray();
                    B_A_Registration.Add(string.Format("{0}", rdr["aircraft_registration"].ToString()));
                    B_Aircraft_Reg_Num = B_A_Registration.ToArray();
                    B_A_Type.Add(string.Format("{0}", rdr["aircraft_type"].ToString()));
                    B_Aircraft_Type = B_A_Type.ToArray();
                    B_F_Distance.Add(string.Format("{0} miles", rdr["flight_distance"].ToString()));
                    B_Flight_Distance = B_F_Distance.ToArray();
                    B_T_Seats.Add(string.Format("{0}", rdr["total_seats"].ToString()));
                    B_Total_Seats = B_T_Seats.ToArray();
                    B_O_Seats.Add(string.Format("{0}", rdr["open_seats"].ToString()));
                    B_Open_Seats = B_O_Seats.ToArray();

                    B_TableSize++;
                }
                if (B_TableSize > 0)
                {
                    for (int i = 0; i < B_Flight_Date.Length; ++i)
                    { // Remove "12:00:00 AM" from string
                        B_Flight_Date[i] = B_Flight_Date[i].Remove(B_Flight_Date[i].Length - 12, 12);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }

        public void SavedTableFill()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM flight, saved_flights WHERE user_id = {this.User_ID} AND saved_flights.flight_id = flight.flight_id", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    F_ID.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                    Flight_ID = F_ID.ToArray();
                    D_City.Add(string.Format("{0}", rdr["departure_city"].ToString()));
                    this.Departure_City = D_City.ToArray();
                    A_City.Add(string.Format("{0}", rdr["arrival_city"].ToString()));
                    this.Arrival_City = A_City.ToArray();
                    D_Port.Add(string.Format("{0}", rdr["departure_airport"].ToString()));
                    this.Departure_Airport = D_Port.ToArray();
                    A_Port.Add(string.Format("{0}", rdr["arrival_airport"].ToString()));
                    this.Arrival_Airport = A_Port.ToArray();
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
                if (TableSize > 0)
                {
                    for (int i = 0; i < TableSize; ++i)
                    { // Remove "12:00:00 AM" from string
                        Flight_Date[i] = Flight_Date[i].Remove(Flight_Date[i].Length - 12, 12);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }

        public string ConfigureNotification() {
            if (NotificationStatus == "1") {
                NotificationTimeout = 3000;
                return "'Login successful!','success'";
            }
            else {
                NotificationTimeout = 0;
                return "'Welcome back!','success'";
            }
        }

        public async Task<IActionResult> OnPost(string submit) {
            if (submit == "L") {
                Console.WriteLine("DEBUG - Log Out");
                Startup.CurrentUser.SetUser("0");
                return Redirect($"/Flights");
            }
            else if (submit == "S") {
                Console.WriteLine("DEBUG - User Settings");
                return Redirect($"/UserSettings");
            }
            return Page();
        }
    }
}