using System;
using System.Collections.Generic;

namespace airlines.Models;

public partial class City
{
    public int Id { get; set; }

    public string? Namee { get; set; }

    public long? Latitude { get; set; }

    public long? Longitude { get; set; }

    public virtual ICollection<Flight> FlightDestinationCityNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightOriginCityNavigations { get; set; } = new List<Flight>();
}
