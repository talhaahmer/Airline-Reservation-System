using System;
using System.Collections.Generic;

namespace airlines.Models;

public partial class Class
{
    public int Id { get; set; }

    public string? Namee { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
