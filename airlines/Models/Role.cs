using System;
using System.Collections.Generic;

namespace airlines.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Namee { get; set; }

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();
}
