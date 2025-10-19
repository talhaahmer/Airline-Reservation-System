using System;
using System.Collections.Generic;

namespace airlines.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string? Cstatus { get; set; }

    public int? TotalPrice { get; set; }

    public DateTime? ReservationDate { get; set; }

    public string? CreditCardUsed { get; set; }

    public int? Fid { get; set; }

    public int? PesCount { get; set; }

    public string? TripType { get; set; }

    public DateTime? DepartureDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Flight? FidNavigation { get; set; }

    public virtual Login? User { get; set; }
}
