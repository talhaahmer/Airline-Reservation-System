namespace airlines.Models
{
    public class BookingViewModell
    {
        public int ReservationId { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
        public string TripType { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
}
