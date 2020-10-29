using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightFinder.Pages
{
    public class TestModel : PageModel
    {
        public string User_ID { get; set; }

        public string User_Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ErrorText { get; set; }

        public void OnGet()
        {
            ErrorText = "";
        }

        bool Debug()
        {
           
            Username = Request.Form["Username"];
            Console.WriteLine(Username);
            User_Email = Request.Form["User_Email"];
            Console.WriteLine(User_Email);
            Password = Request.Form["Password"];
            Console.WriteLine(Password);
            return true;
        }

       
        public async Task<IActionResult> OnPost()
        {
            Debug();
            return Page();
        }
    }
}
