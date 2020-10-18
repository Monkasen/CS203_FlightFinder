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


namespace FlightFinder.Pages
{
    public class FlightsModel : PageModel
    {
        public void OnGet() 
        {
            TableFill();
        }

        void TableFill()
        {
            using var connection = new MySqlConnection("server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;");
            connection.Open();
            MySqlDataAdapter sqlDa = new MySqlDataAdapter("FlightViewAll", connection);
            sqlDa.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            
        }
    }  
}
