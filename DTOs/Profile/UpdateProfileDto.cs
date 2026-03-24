using MARN_API.Enums.Account;
using MARN_API.Validation;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Profile
{
    public class UpdateProfileDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [PastDate(13, 120)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; } = Gender.Unknown;

        [Required(ErrorMessage = "Language is required.")]
        [EnumDataType(typeof(Language))]
        public Language Language { get; set; } = Language.English;

        [Required(ErrorMessage = "Country is required.")]
        [EnumDataType(typeof(Country))]
        public Country Country { get; set; } = Country.Unknown;

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        public string? Bio { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ProfileImage { get; set; }
    }
}
