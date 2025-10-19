using System;
using System.Collections.Generic;

namespace airlines.Models;

public partial class Flight
{
    public int Id { get; set; }

    public string? FlightNumber { get; set; }

    public int? OriginCity { get; set; }

    public int? DestinationCity { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public int? Duration { get; set; }

    public int? BasePrice { get; set; }

    public int? AvailableSeats { get; set; }

    public DateTime? Date { get; set; }

    public virtual City? DestinationCityNavigation { get; set; }

    public virtual City? OriginCityNavigation { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
