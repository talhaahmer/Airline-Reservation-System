namespace airlines.Models
{
  
        public class BookingViewModel
        {
            public int SelectedRouteId { get; set; }
            public string PassengerClass { get; set; }
            public string TripType { get; set; }
            public DateTime DepartureDate { get; set; }
            public DateTime? ReturnDate { get; set; }
            public int Adults { get; set; }
            public int Children { get; set; }
            public int Seniors { get; set; }
        
    }
}
