namespace PriceBondAPI.Models.DTOS.BondDto
{
    public class AddBondDto
    {
        public string? BondNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? UserId { get; set; }

        public int? DenominationId { get; set; }
    }
}
