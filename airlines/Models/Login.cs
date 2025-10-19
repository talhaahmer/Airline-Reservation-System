using System;
using System.Collections.Generic;

namespace airlines.Models;

public partial class Login
{
    internal int roleid;

    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Role? Role { get; set; }
}
