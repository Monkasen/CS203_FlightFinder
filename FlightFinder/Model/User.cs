using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Model
{
    public class User
    {
        [Required]
        public int User_ID { get; set; }
        [Required]
        public string User_Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }
        [Required]
        public DateTime Account_Creation_Date { get; set; }


    }
}
