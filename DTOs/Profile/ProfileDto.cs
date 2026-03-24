using MARN_API.DTOs.Property;
using MARN_API.Enums.Account;
using MARN_API.Enums.RoommatePrefrences;

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
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Country Country { get; set; }
        public DateTime MemberSince { get; set; }
        public string? Bio { get; set; }


        // Owner Data
        public bool IsOwner { get; set; }
        public float AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public int OwnedPropertiesCount { get; set; }
        public ICollection<PropertyCardDto>? OwnedProperties { get; set; }


        // Roommate Preferences
        public bool RoommatePrefrencesEnabled { get; set; } = false;
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
