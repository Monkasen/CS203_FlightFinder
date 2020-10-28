using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightFinder.Pages
{
    public class BookModel : PageModel
    {
        public string Flight_ID;
        public void OnGet() {
            Flight_ID = Request.Query["Flight_ID"];
            Console.WriteLine($"You are reserving a flight with internal ID: {Flight_ID}");
        }
    }
}
