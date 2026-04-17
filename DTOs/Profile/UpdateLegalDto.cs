using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Profile
{
    public class UpdateLegalDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile FrontIdPhoto { get; set; } = null!;

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile BackIdPhoto { get; set; } = null!;

        [Required(ErrorMessage = "Arabic address is required.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Arabic address must be between 10 and 200 characters.")]
        public string ArabicAddress { get; set; } = null!;

        [Required(ErrorMessage = "Arabic Full Name is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Arabic Full Name must be between 10 and 100 characters.")]
        public string ArabicFullName { get; set; } = null!;

        [Required(ErrorMessage = "National ID Number is required.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID Number must be 14 characters.")]
        public string NationalIDNumber { get; set; } = null!;
    }
}
