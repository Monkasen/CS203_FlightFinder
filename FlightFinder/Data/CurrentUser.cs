using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder {
    public class CurrentUser {
        public string LoggedInUser = "0";

        public void SetUser(string input) {
            LoggedInUser = input;
        }

        public string GetUser() {
            return LoggedInUser;
        }

        public string ConfigureLayout(){
            if (LoggedInUser == "0") {
                return "/Login";
            }
            else {
                return "/User";
            }
        }
    }
}
