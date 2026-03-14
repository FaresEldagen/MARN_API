namespace MARN_API.DTOs.Profile
{
    public class UpdateProfileDto
    {
        public long id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
