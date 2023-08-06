using System;
using System.Collections.Generic;

namespace PriceBondAPI.Models;

public partial class Draw
{
    public int Id { get; set; }

    public DateTime? DrawDate { get; set; }

    public string? DrawLocation { get; set; }

    public int? Price { get; set; }

    public int? DenominationId { get; set; }

    public virtual Denomination? Denomination { get; set; }
}
