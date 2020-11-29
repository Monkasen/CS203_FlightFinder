using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using MySqlX.XDevAPI;

namespace FlightFinder.Pages
{
    public class UserSettingsModel : PageModel
    {
        public static string User_ID { get; set; }
        #region user variables
        List<string> U_Email = new List<string>();
        public string[] User_Email;
        List<string> U_Uname = new List<string>();
        public string[] User_Username;
        List<string> U_Pword = new List<string>();
        public string[] User_Password;
        List<string> A_C_Date = new List<string>();
        public string[] Account_Creation_Date;
        List<string> N_Set = new List<string>();
        public string[] Notif_Setting;
        #endregion

        List<string> O_Password = new List<string>();
        public string[] Old_Password { get; set; }

        public string Pass_Check { get; set; }
        public string N_Password { get; set; }
        public string C_N_Password { get; set; }

        public string NotificationSet;
        public string ErrorText = "";

        public void OnGet() {
            User_ID = Startup.CurrentUser.GetUser();
            UserTableFill();
        }

        public void UserTableFill()
        {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM user WHERE user_id = {User_ID}", conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {
                    U_Email.Add(string.Format("{0}", rdr["user_email"].ToString()));
                    User_Email = U_Email.ToArray();
                    U_Uname.Add(string.Format("{0}", rdr["username"].ToString()));
                    User_Username = U_Uname.ToArray();
                    U_Pword.Add(string.Format("{0}", rdr["password"].ToString()));
                    User_Password = U_Pword.ToArray();
                    A_C_Date.Add(string.Format("{0}", rdr["account_creation_date"].ToString()));
                    Account_Creation_Date = A_C_Date.ToArray();
                    N_Set.Add(string.Format("{0}", rdr["notification_setting"].ToString()));
                    Notif_Setting = N_Set.ToArray();
                }

                Account_Creation_Date[0] = Account_Creation_Date[0].Remove(Account_Creation_Date[0].Length - 12, 12); // Remove "12:00:00 AM" from string
            }
            catch (Exception ex) {
                Console.WriteLine("{oops - {0}", ex.Message);
            }
            conn.Dispose();
        }

        bool CheckOldPassword() {
            Pass_Check = Request.Form["Pass_Check"];
            N_Password = Request.Form["N_Password"];
            C_N_Password = Request.Form["C_N_Password"];
            if (Pass_Check == "" && N_Password == "" && C_N_Password == "") {
                return true;
            }

            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            conn.Open();

            string cmdText = $"SELECT password FROM user WHERE User_ID = '{User_ID}'";
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read()) {
                O_Password.Add(string.Format("{0}", rdr["password"].ToString()));
                this.Old_Password = O_Password.ToArray();
            }

            if (Old_Password[0] != Pass_Check || Pass_Check == null) {
                ErrorText = "Old password is incorrect. Please try again.";
                return false;
            }
            return true;
        }

        bool SetNewPassword() {
            const string connectionString = "server=73.249.227.33;user id=admin;password=flightfinder20;database=FlightFinder;port=3306;persistsecurityinfo=True;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            N_Password = Request.Form["N_Password"];
            C_N_Password = Request.Form["C_N_Password"];

            if (N_Password != C_N_Password) {
                ErrorText = "Passwords do not match. Please try again.";
                return false;
            }

            conn.Open();

            string txtcmd = $"UPDATE user SET Password='{N_Password}' WHERE User_ID={User_ID}";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Prepare();
            cmd.ExecuteReader();

            conn.Dispose();
            return true;
        }

        void ChangeNotificationSettings() {
            NotificationSet = Request.Form["Notif"];
            if (NotificationSet == "1") {
                FlightFinder.Startup.CurrentUser.SetNotification(true);
            }
            else {
                FlightFinder.Startup.CurrentUser.SetNotification(false);
            }
        }

        public async Task<IActionResult> OnPost() {
            ChangeNotificationSettings();
            if (CheckOldPassword()) {
                if (!SetNewPassword()) {
                    return Page();
                }
                return Redirect($"/User?User_ID={User_ID}&Notification=0");
            }
            return Page();
        }
    }
}
