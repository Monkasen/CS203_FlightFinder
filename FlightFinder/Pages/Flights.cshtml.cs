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

        public int TableSize = 0;

        public string From_TextBox { get; set; }

        public string To_TextBox { get; set; }

        public DateTime? DepartDate_TextBox { get; set; }

        public string Airline_TextBox { get; set; }

        public string Depart_Time_TextBox { get; set; }

        public string Arrival_Time_TextBox { get; set; }

        public void OnGet() {
            TableFill();
        }

        public bool FilterResults()
        {
            TableSize = 0;
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


            if (!String.IsNullOrEmpty(Request.Form["From"]))
            {
                From_TextBox = Request.Form["From"];
            }

            if (!String.IsNullOrEmpty(Request.Form["To"]))
            {
                To_TextBox = Request.Form["To"];
            }
            if (!String.IsNullOrEmpty(Request.Form["DepartDate"]))
            {
                DepartDate_TextBox = DateTime.Parse(Request.Form["DepartDate"]);
            }
            if (!String.IsNullOrEmpty(Request.Form["Airline"]))
            {
                Airline_TextBox = Request.Form["Airline"];
            }
            

            string cmdtxt = $"SELECT * FROM flight WHERE Departure_Airport = '{this.From_TextBox}' OR Departure_City = '{this.From_TextBox}' OR Arrival_City = '{this.To_TextBox}' OR Arrival_Airport = '{this.To_TextBox}';";
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
                while (rdr.Read())
                {

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

                    TableSize++;
                }
                conn.Dispose();
                return true;
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

                    TableSize++;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose(); 
        }

        private void SaveFlightToDB(string ButtonValue) { // Saves specified flight ID to specified user ID         
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string txtcmd = $"INSERT INTO saved_flights (User_ID, Flight_ID) " +
                $"VALUES (@User_ID, @Flight_ID )";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@User_ID", 1); // THIS IS WHERE WE ASSIGN WHAT USER GETS THE SAVED FLIGHT, RIGHT NOW IT DEFAULTS TO USER 1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            cmd.Parameters.AddWithValue("@Flight_ID", ButtonValue);
            cmd.Prepare();
            cmd.ExecuteReader();

            conn.Dispose();
        }

        public async Task<IActionResult> OnPost(string submit) {
            string parsedID = null;

            for (int i = 0; i < submit.Length; i++) { // Parse Flight_ID from button value
                if (Char.IsDigit(submit[i]))
                    parsedID += submit[i];
            }
            Console.WriteLine($"DEBUG - OUTPUT OF PARSE = {parsedID}");

            if (submit[0] == 'B') { // Determine if the flight is to be booked...
                Console.WriteLine("DEBUG - Book");
                return Redirect($"/Book?Flight_ID={parsedID}");
            }
            else if (submit[0] == 'S') { // ...saved...
                Console.WriteLine("DEBUG - Save");
                SaveFlightToDB(parsedID);
            }
            else if(submit[0] == 'F') {//... or filtered through
                Console.WriteLine("Filter results");
                FilterResults();
                return RedirectToPage();
            }
            else {
                Console.WriteLine("DEBUG - Error");
            }
            
            TableFill();
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostFilter()
        {
            if(FilterResults())
            {
                return RedirectToPage();
            }
            else
            {
                Console.WriteLine("No Flights match your search");
                return RedirectToPage();
            }
        }
    }     


}
