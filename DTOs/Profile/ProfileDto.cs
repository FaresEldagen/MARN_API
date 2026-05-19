using MARN_API.DTOs.Property;
using MARN_API.Enums.Account;
using MARN_API.Enums.RoommatePreferences;

namespace MARN_API.DTOs.Profile
{
    public class ProfileDto
    {
        // Basic Info
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string GenderDisplayName { get; set; } = string.Empty;
        public Country Country { get; set; }
        public string CountryDisplayName { get; set; } = string.Empty;
        public DateTime MemberSince { get; set; }
        public string? Bio { get; set; }


        // Owner Data
        public bool IsOwner { get; set; }
        public float? AverageRating { get; set; } = null;
        public int? RatingsCount { get; set; } = null;
        public int? OwnedPropertiesCount { get; set; } = null;
        public ICollection<PropertyCardDto>? OwnedProperties { get; set; } = null;


        // Roommate Preferences
        public bool RoommatePreferencesEnabled { get; set; } = false;
        public bool? Smoking { get; set; } = null;
        public bool? Pets { get; set; } = null;
        public string? SleepSchedule { get; set; } = null;
        public string SleepScheduleDisplayName { get; set; } = string.Empty;
        public EducationLevel? EducationLevel { get; set; } = null;
        public string EducationLevelDisplayName { get; set; } = string.Empty;
        public FieldOfStudy? FieldOfStudy { get; set; } = null;
        public string FieldOfStudyDisplayName { get; set; } = string.Empty;
        public int? NoiseTolerance { get; set; } = null;
        public GuestsFrequency? GuestsFrequency { get; set; } = null;
        public string GuestsFrequencyDisplayName { get; set; } = string.Empty;
        public WorkSchedule? WorkSchedule { get; set; } = null;
        public string WorkScheduleDisplayName { get; set; } = string.Empty;
        public SharingLevel? SharingLevel { get; set; } = null;
        public string SharingLevelDisplayName { get; set; } = string.Empty;
        public decimal? BudgetRangeMin { get; set; } = null;
        public decimal? BudgetRangeMax { get; set; } = null;
        // Roommate Matches
        public double? MatchingPercentage { get; set; } = null;
        public List<string>? TopMatchingTraits { get; set; } = null;
        public List<string>? MismatchedTraits { get; set; } = null;
        public List<string>? DealbreakersFound { get; set; } = null;
    }
}
