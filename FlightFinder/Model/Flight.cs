using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Model {
    public class Flight {
        [Key]
        public int FlightID { get; set; }

        [Required]
        public int AirlineID { get; set; }
        [Required]
        public string DepartureAirport { get; set; }
        [Required]
        public string ArrivalAirport { get; set; }
        [Required]
        public string FlightDate { get; set; }
        [Required]
        public string DepartureTime { get; set; }
        [Required]
        public string ArrivalTime { get; set; }
        [Required]
        public string FlightDuration { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string AircraftType { get; set; }
        [Required]
        public int FlightDistance { get; set; }
        [Required]
        public int TotalSeats { get; set; }
        [Required]
        public int OpenSeats { get; set; }
    }
}
