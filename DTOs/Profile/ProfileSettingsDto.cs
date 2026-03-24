using MARN_API.DTOs.Property;
using MARN_API.Enums.Account;
using MARN_API.Enums.RoommatePreferences;

namespace MARN_API.DTOs.Profile
{
    public class ProfileSettingsDto
    {
        // Basic Info
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public Language Language { get; set; } = Language.English;
        public string? ProfileImage { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public Country Country { get; set; } = Country.Unknown;
        public string? Bio { get; set; }


        // Verification Info
        public string? FrontIdPhoto { get; set; }
        public string? BackIdPhoto { get; set; }
        public string? ArabicAddress { get; set; }
        public string? ArabicFullName { get; set; }
        public string? NationalIDNumber { get; set; }


        // Roommate Preferences
        public bool RoommatePreferencesEnabled { get; set; } = false;
        public bool? Smoking { get; set; } = null;
        public bool? Pets { get; set; } = null;
        public string? SleepSchedule { get; set; } = null;
        public EducationLevel? EducationLevel { get; set; } = null;
        public FieldOfStudy? FieldOfStudy { get; set; } = null;
        public int? NoiseTolerance { get; set; } = null;
        public GuestsFrequency? GuestsFrequency { get; set; } = null;
        public WorkSchedule? WorkSchedule { get; set; } = null;
        public SharingLevel? SharingLevel { get; set; } = null;
        public decimal? BudgetRangeMin { get; set; } = null;
        public decimal? BudgetRangeMax { get; set; } = null;
    }
}
