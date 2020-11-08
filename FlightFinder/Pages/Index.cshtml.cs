using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightFinder.Pages
{
    public class IndexModel : PageModel
    {
        public string Search { get; set; }
        List<string> SearchList = new List<string>();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("DEBUG - Search");
            return Redirect("/Flights");
        }
    }
}
