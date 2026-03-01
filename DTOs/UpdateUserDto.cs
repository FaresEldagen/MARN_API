namespace MARN_API.DTOs
{
    public class UpdateUserDto
    {
        public long id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
