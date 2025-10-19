namespace airlines.Models
{
    public class FlightSearchViewModel
    {
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public string Class { get; set; }
        public string TripType { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int Adults { get; set; }
        public int Children { get; set; }
        public int Seniors { get; set; }

        public List<Flight> SearchResults { get; set; } = new List<Flight>(); // ✅ Important
    }
}