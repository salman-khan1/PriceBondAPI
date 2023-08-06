using System;
using System.Collections.Generic;

namespace PriceBondAPI.Models;

public partial class Bond
{
    public int Id { get; set; }

    public string? BondNumber { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public int? UserId { get; set; }

    public int? DenominationId { get; set; }

    public virtual Denomination? Denomination { get; set; }

    public virtual User? User { get; set; }
}
