using System.ComponentModel.DataAnnotations;

namespace MARN_API.Validation
{
    /// <summary>
    /// Validates that a DateTime value is in the past and not unreasonably old.
    /// </summary>
    public class PastDateAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public PastDateAttribute(int minAge = 0, int maxAge = 120)
        {
            _minAge = minAge;
            _maxAge = maxAge;
            ErrorMessage = $"Date of birth must represent an age between {_minAge} and {_maxAge} years.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime date)
                return ValidationResult.Success; // nullable — let [Required] handle presence

            var today = DateTime.UtcNow.Date;

            if (date > today)
                return new ValidationResult("Date of birth cannot be in the future.");

            var age = today.Year - date.Year;
            if (date > today.AddYears(-age)) age--;

            if (age < _minAge || age > _maxAge)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
