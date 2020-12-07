using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder
{
    public class Email
    {
        public string User_Email { get; set; }

        public void SetEmail(string input)
        {
            User_Email = input;
        }

        public string GetEmail()
        {
            return User_Email;
        }
    }
}
