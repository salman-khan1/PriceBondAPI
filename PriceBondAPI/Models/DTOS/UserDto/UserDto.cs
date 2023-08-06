using System.ComponentModel.DataAnnotations.Schema;

namespace PriceBondAPI.Models.DTOS.UserDto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();
    }
}
