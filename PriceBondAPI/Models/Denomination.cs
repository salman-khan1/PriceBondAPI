using System;
using System.Collections.Generic;

namespace PriceBondAPI.Models;

public partial class Denomination
{
    public int Id { get; set; }

    public int? Value { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual ICollection<Draw> Draws { get; set; } = new List<Draw>();
}
