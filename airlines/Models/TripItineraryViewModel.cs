namespace airlines.Models
{
    public class TripItineraryViewModel
    {
        public List<Flight> Flights { get; set; }
        public int TotalDuration { get; set; }
        public int TotalPrice { get; set; }
    }
}
