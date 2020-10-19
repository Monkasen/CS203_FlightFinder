using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Model
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public DateTime Account_Creation_Date { get; set; }


    }
}
