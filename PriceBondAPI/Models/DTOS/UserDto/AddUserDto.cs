namespace PriceBondAPI.Models.DTOS.UserDto
{
    public class AddUserDto
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime? RegistrationDate { get; set; }

    }
}
