using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.Enums;

namespace MARN_API.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!; // to do remote
        
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirming The Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Gender is Required")]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; } = Gender.Unknown;
    }
}