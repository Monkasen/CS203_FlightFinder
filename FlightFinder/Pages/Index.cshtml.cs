using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace FlightFinder.Pages
{
    public class IndexModel : PageModel
    {
        public string Search { get; set; }
        List<string> SearchList = new List<string>();

        public void OnGet()
        {
        }

        public string GetSearch()
        {
            Search = Request.Form["Search"];
            return Search;
        }

        public bool VerifySearch()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataSet = new DataTable();

            string cmdText = $"SELECT * FROM flight WHERE Total_Seats > 0 AND Departure_Airport = '{GetSearch()}' OR Departure_City = '{GetSearch()}' OR Arrival_Airport = '{GetSearch()}' OR Arrival_City = '{GetSearch()}'";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
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
                conn.Dispose();
                return true;
            }
        }



        public async Task<IActionResult> OnPost()
        {
            if (VerifySearch())
            {
                Startup.userSearch.SetSearch(GetSearch());
                Console.WriteLine("DEBUG - Search");
                return Redirect($"/Flights?Search={GetSearch()}");
            }
            else
            {//Add a message for when the search is invalid
                return Redirect("/Flights");
            }
            
        }
    }
}
