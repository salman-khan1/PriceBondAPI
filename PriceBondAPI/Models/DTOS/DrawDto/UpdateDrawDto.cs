namespace PriceBondAPI.Models.DTOS.DrawDto
{
    public class UpdateDrawDto
    {

        public DateTime? DrawDate { get; set; }

        public string? DrawLocation { get; set; }

        public int? Price { get; set; }
        public int? DenominationId { get; set; }

    }
}
