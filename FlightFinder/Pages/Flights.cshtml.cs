using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using FlightFinder.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FlightFinder.Pages
{
    public class FlightsModel : PageModel
    {
        #region flight table variables
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
        List<string> Air_Name = new List<string>();
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
        public int TableSize = 0;
        #region textbox variables
        public int NumNullBoxes;

        public string From_TextBox { get; set; }

        public string To_TextBox { get; set; }

        public string DepartDate_TextBox { get; set; }

        public string Airline_TextBox { get; set; }

        public DateTime? Depart_Time_TextBox { get; set; }

        public DateTime? Arrival_Time_TextBox { get; set; }

        public string Registration_Num_TextBox { get; set; }

        public string AircraftType_TextBox { get; set; }
        #endregion
        public string DebugText { get; set; }

        public string NotificationSuccesful = "success";
        public string NotificationText = "Flight succesfully saved!";
        #region saved flight check
        List<string> SF_U = new List<string>();
        private string[] SF_User;
        List<string> SF_F = new List<string>();
        private string[] SF_Flight;
        #endregion
        #region booked flight check
        List<string> BF_U = new List<string>();
        private string[] BF_User;
        List<string> BF_F = new List<string>();
        private string[] BF_Flight;
        #endregion
        public void OnGet() {
            TableFill();
        }

        public bool FilterResults()
        {
            TableSize = 0;
            ModelState.Clear();
            D_City.Clear();
            A_City.Clear();
            D_Time.Clear();
            A_Time.Clear();
            F_Date.Clear();
            Air_Name.Clear();
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataSet = new DataTable();


            string cmdtxt = CreateQuery();
            MySqlCommand cmd = new MySqlCommand(cmdtxt, conn);
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(dataSet);

            if (dataSet.Rows.Count < 1)
            {
                conn.Dispose();
                
                return false;
            }
            else
            { 
                MySqlDataReader rdr = cmd.ExecuteReader();

                for (int i = 0; i < dataSet.Rows.Count; i++)
                {
                    rdr.Read();
                    F_ID.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                    this.Flight_ID = F_ID.ToArray();
                    D_City.Add(string.Format("{0}", rdr["departure_city"].ToString()));
                    this.Departure_City = D_City.ToArray();
                    A_City.Add(string.Format("{0}", rdr["arrival_city"].ToString()));
                    this.Arrival_City = A_City.ToArray();
                    D_Port.Add(string.Format("{0}", rdr["departure_airport"].ToString()));
                    this.Departure_Airport = D_Port.ToArray();
                    A_Port.Add(string.Format("{0}", rdr["arrival_airport"].ToString()));
                    this.Arrival_Airport = A_Port.ToArray();
                    F_Date.Add(string.Format("{0}", rdr["flight_date"].ToString()));
                    this.Flight_Date = F_Date.ToArray();
                    D_Time.Add(string.Format("{0}", rdr["departure_time"].ToString()));
                    this.Departure_Time = D_Time.ToArray();
                    A_Time.Add(string.Format("{0}", rdr["arrival_time"].ToString()));
                    this.Arrival_Time = A_Time.ToArray();
                    E_F_Time.Add(string.Format("{0}", rdr["estimated_flight_time"].ToString()));
                    this.E_Flight_Time = E_F_Time.ToArray();
                    Air_Name.Add(string.Format("{0}", rdr["airline"].ToString()));
                    this.Airline_Name = Air_Name.ToArray();
                    A_Registration.Add(string.Format("{0}", rdr["aircraft_registration"].ToString()));
                    this.Aircraft_Reg_Num = A_Registration.ToArray();
                    A_Type.Add(string.Format("{0}", rdr["aircraft_type"].ToString()));
                    this.Aircraft_Type = A_Type.ToArray();
                    F_Distance.Add(string.Format("{0} miles", rdr["flight_distance"].ToString()));
                    this.Flight_Distance = F_Distance.ToArray();
                    T_Seats.Add(string.Format("{0}", rdr["total_seats"].ToString()));
                    this.Total_Seats = T_Seats.ToArray();
                    O_Seats.Add(string.Format("{0}", rdr["open_seats"].ToString()));
                    this.Open_Seats = O_Seats.ToArray();

                    TableSize++;
                }

                for (int i = 0; i < Flight_Date.Length; ++i) { // Remove "12:00:00 AM" from string
                    Flight_Date[i] = Flight_Date[i].Remove(Flight_Date[i].Length - 12, 12);
                }

                conn.Dispose();
                return true;
            }
        }

        public string CreateQuery()
        {
            string cmd = "SELECT * FROM flight WHERE Total_Seats > 0";
            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            string s5 = "";
            string s6 = "";
            string s7 = "";
            string s8 = "";

            if (!string.IsNullOrEmpty(Request.Form["From_TextBox"]))
            {
                From_TextBox = Request.Form["From_TextBox"];
                s1 += $" AND Departure_Airport = '{this.From_TextBox}' OR Departure_City = '{this.From_TextBox}'";
            }
            else
            {
                ++NumNullBoxes;
            }
            if (!string.IsNullOrEmpty(Request.Form["To_TextBox"]))
            {
                To_TextBox = Request.Form["To_TextBox"];
                s2 += $" AND Arrival_Airport = '{this.To_TextBox}' OR Arrival_City = '{this.To_TextBox}'";
            }
            else
            {
                ++NumNullBoxes;
            }
            if (!string.IsNullOrEmpty(Request.Form["DepartDate_TextBox"]))
            {
                DepartDate_TextBox = Request.Form["DepartDate_TextBox"];
                s3 += $" AND Flight_Date = '{this.DepartDate_TextBox}'";
            }
            else
            {
                ++NumNullBoxes;
            }
            if (!string.IsNullOrEmpty(Request.Form["Airline_TextBox"]))
            {
                Airline_TextBox = Request.Form["Airline_TextBox"];
                s4 += $" AND Airline = '{this.Airline_TextBox}'";
            }
            else
            { 
                ++NumNullBoxes;
            }
            if (!string.IsNullOrEmpty(Request.Form["Depart_Time_TextBox"]))
            {
                Depart_Time_TextBox = DateTime.Parse(Request.Form["Depart_Time_TextBox"]);
                string time = string.Format("{0:t}", Depart_Time_TextBox);
                s5 += $" AND Departure_Time = '{time}'";
            }
            else
            {
                ++NumNullBoxes; 
            }
            if (!string.IsNullOrEmpty(Request.Form["Arrival_Time_TextBox"]))
            {
                Arrival_Time_TextBox = DateTime.Parse(Request.Form["Arrival_Time_TextBox"]);
                string time = string.Format("{0:t}", Arrival_Time_TextBox);
                s6 += $" AND Arrival_Time = '{time}'";
            }
            else
            {
                ++NumNullBoxes;
            }
            if (!string.IsNullOrEmpty(Request.Form["Registration_Num_TextBox"]))
            {
                Registration_Num_TextBox = Request.Form["Registration_Num_TextBox"];
                s6 += $" AND Aircraft_Registration = '{this.Registration_Num_TextBox}'";
            }
            else
            {
                ++NumNullBoxes;
            }
            if (!string.IsNullOrEmpty(Request.Form["AircraftType_TextBox"]))
            {
                AircraftType_TextBox = Request.Form["AircraftType_TextBox"];
                s6 += $" AND Aircraft_Type = '{this.AircraftType_TextBox}'";
            }
            else
            {
                ++NumNullBoxes;
            }


            if (NumNullBoxes == 8)
            {
                cmd += ";";
                return cmd;
            }
            else
            {
                if (!string.IsNullOrEmpty(s1))
                {
                    cmd += s1;
                }
                if(!string.IsNullOrEmpty(s2))
                {
                    cmd += s2;
                }
                if (!string.IsNullOrEmpty(s3))
                {
                    cmd += s3;
                }
                if (!string.IsNullOrEmpty(s4))
                {
                    cmd += s4;
                }
                if (!string.IsNullOrEmpty(s5))
                {
                    cmd += s5;
                }
                if (!string.IsNullOrEmpty(s6))
                {
                    cmd += s6;
                }
                if (!string.IsNullOrEmpty(s7))
                {
                    cmd += s7;
                }
                if (!string.IsNullOrEmpty(s8))
                {
                    cmd += s8;
                }
                cmd += ";";
                return cmd;
            }
            
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
                    
                    F_ID.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                    this.Flight_ID = F_ID.ToArray();

                    D_City.Add(string.Format("{0}", rdr["departure_city"].ToString()));
                    this.Departure_City = D_City.ToArray();
                    A_City.Add(string.Format("{0}", rdr["arrival_city"].ToString()));
                    this.Arrival_City = A_City.ToArray();
                    D_Port.Add(string.Format("{0}", rdr["departure_airport"].ToString()));
                    this.Departure_Airport = D_Port.ToArray();
                    A_Port.Add(string.Format("{0}", rdr["arrival_airport"].ToString()));
                    this.Arrival_Airport = A_Port.ToArray();
                    F_Date.Add(string.Format("{0}", rdr["flight_date"].ToString()));
                    this.Flight_Date = F_Date.ToArray();
                    D_Time.Add(string.Format("{0}", rdr["departure_time"].ToString()));
                    this.Departure_Time = D_Time.ToArray();
                    A_Time.Add(string.Format("{0}", rdr["arrival_time"].ToString()));
                    this.Arrival_Time = A_Time.ToArray(); 
                    E_F_Time.Add(string.Format("{0}", rdr["estimated_flight_time"].ToString()));
                    this.E_Flight_Time = E_F_Time.ToArray();
                    Air_Name.Add(string.Format("{0}", rdr["airline"].ToString()));
                    this.Airline_Name = Air_Name.ToArray();
                    A_Registration.Add(string.Format("{0}", rdr["aircraft_registration"].ToString()));
                    this.Aircraft_Reg_Num = A_Registration.ToArray();
                    A_Type.Add(string.Format("{0}", rdr["aircraft_type"].ToString()));
                    this.Aircraft_Type = A_Type.ToArray();
                    F_Distance.Add(string.Format("{0} miles", rdr["flight_distance"].ToString()));
                    this.Flight_Distance = F_Distance.ToArray();
                    T_Seats.Add(string.Format("{0}", rdr["total_seats"].ToString()));
                    this.Total_Seats = T_Seats.ToArray();
                    O_Seats.Add(string.Format("{0}", rdr["open_seats"].ToString()));
                    this.Open_Seats = O_Seats.ToArray();

                    TableSize++;
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

        private void SaveFlightToDB(string ButtonValue) { // Saves specified flight ID to specified user ID   
            if (!CheckSaved(Startup.CurrentUser.GetUser(), ButtonValue)) { 
                return;
            }

            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string txtcmd = $"INSERT INTO saved_flights (User_ID, Flight_ID) " +
                $"VALUES (@User_ID, @Flight_ID )";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@User_ID", Startup.CurrentUser.GetUser()); 
            cmd.Parameters.AddWithValue("@Flight_ID", ButtonValue);
            cmd.Prepare();
            cmd.ExecuteReader();

            conn.Dispose();
        }

        private bool CheckSaved(string U_ID, string F_ID) {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM saved_flights", conn);
            cmd.CommandType = CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read()) {
                SF_U.Add(string.Format("{0}", rdr["user_id"].ToString()));
                this.SF_User = SF_U.ToArray();
                SF_F.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                this.SF_Flight = SF_F.ToArray();
            }

            for (int i = 0; i < SF_User.Length; ++i) { // Check to see if the flight is already saved
                if (SF_User[i] == U_ID && SF_Flight[i] == F_ID) {
                    return false;
                }
            }

            conn.Dispose();
            return true;
        }

        private bool CheckBooked(string U_ID, string F_ID) {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM booked_flights", conn);
            cmd.CommandType = CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read()) {
                BF_U.Add(string.Format("{0}", rdr["user_id"].ToString()));
                this.BF_User = BF_U.ToArray();
                BF_F.Add(string.Format("{0}", rdr["flight_id"].ToString()));
                this.BF_Flight = BF_F.ToArray();
            }

            for (int i = 0; i < BF_User.Length; ++i) { // Check to see if the flight is already booked
                if (BF_User[i] == U_ID && BF_Flight[i] == F_ID) {
                    return false;
                }
            }

            return true;
        }

        public string ConfigureNotification(string FlightValue, string InputType) {
            if (InputType == "Save") {
                if (Startup.CurrentUser.GetUser() == "0") {
                    return "'You must be logged in to save a flight!','error'";
                }
                if (!CheckSaved(Startup.CurrentUser.GetUser(), FlightValue)) { 
                    return "'Flight already saved!','error'";
                }

                return "'Flight succesfully saved!','success'";
            }
            else {
                if (Startup.CurrentUser.GetUser() == "0") {
                    return "'You must be logged in to book a flight!','error'";
                }
                else if (!CheckBooked(Startup.CurrentUser.GetUser(), FlightValue)) {
                    return "'Flight already booked!','error'";
                }
                else {
                    return "'default text','error'";
                }
            }
        }

        public async Task<IActionResult> OnPost(string submit) {
            string parsedID = null;

            for (int i = 0; i < submit.Length; i++) { // Parse Flight_ID from button value
                if (Char.IsDigit(submit[i]))
                    parsedID += submit[i];
            }

            if (submit[0] == 'B') { // Determine if the flight is to be booked...
                Console.WriteLine("DEBUG - Book");
                if (Startup.CurrentUser.GetUser() == "0" || !CheckBooked(Startup.CurrentUser.GetUser(), parsedID)) {
                    TableFill();
                    Thread.Sleep(3000);
                    return Page();
                }
                return Redirect($"/Book?Flight_ID={parsedID}");
            }
            else if (submit[0] == 'S') { // ...saved...
                Console.WriteLine("DEBUG - Save");
                if (Startup.CurrentUser.GetUser() != "0") {
                    SaveFlightToDB(parsedID);
                }
                TableFill();
                Thread.Sleep(3000);
                return Page();
            }
            else if (submit[0] == 'F') { // ...or filtered through.
                Console.WriteLine("DEBUG - Filter");
                FilterResults();
                return Page();
            }
            else {
                Console.WriteLine("DEBUG - Error");
                return Redirect("/Error");
            }
        }
    }     
}
