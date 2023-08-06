using System;
using System.Collections.Generic;

namespace PriceBondAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();
}
