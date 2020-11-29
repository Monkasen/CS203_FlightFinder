using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder {
    public class CurrentUser {
        public string LoggedInUser = "0";
        public bool NotificationSetting = true; // True = notifications on, false = notifications off.

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

        public void SetNotification(bool input) {
            NotificationSetting = input;
        }

        public bool GetNotification() {
            return NotificationSetting;
        }
    }
}
