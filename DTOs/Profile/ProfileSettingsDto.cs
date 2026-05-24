using MARN_API.DTOs.Property;
using MARN_API.Enums.Account;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Enums.Property;

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
        public string LanguageDisplayName { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public string GenderDisplayName { get; set; } = string.Empty;
        public Country Country { get; set; } = Country.Unknown;
        public string CountryDisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public bool TwoFactorEnabled { get; set; } = false;


        // Verification Info
        public string? FrontIdPhoto { get; set; }
        public string? BackIdPhoto { get; set; }
        public string? ArabicAddress { get; set; }
        public string? ArabicFullName { get; set; }
        public string? NationalIDNumber { get; set; }


        // Roommate Preferences
        public Governorate Governorate { get; set; } = Governorate.CairoGovernorate;
        public string GovernorateDisplayName { get; set; } = string.Empty;
        public RoommateSearchStatus SearchStatus { get; set; } = RoommateSearchStatus.Searching;
        public string SearchStatusDisplayName { get; set; } = string.Empty;

        public bool RoommatePreferencesEnabled { get; set; } = false;
        public bool? Smoking { get; set; } = null;
        public int SmokingImportance { get; set; } = 3;

        public bool? Pets { get; set; } = null;
        public int PetsImportance { get; set; } = 3;

        public string? SleepSchedule { get; set; } = null;
        public string SleepScheduleDisplayName { get; set; } = string.Empty;
        public int SleepImportance { get; set; } = 3;

        public EducationLevel? EducationLevel { get; set; } = null;
        public string EducationLevelDisplayName { get; set; } = string.Empty;
        public int EducationImportance { get; set; } = 3;

        public FieldOfStudy? FieldOfStudy { get; set; } = null;
        public string FieldOfStudyDisplayName { get; set; } = string.Empty;
        public int FieldOfStudyImportance { get; set; } = 3;

        public int? NoiseTolerance { get; set; } = null;
        public int NoiseToleranceImportance { get; set; } = 3;

        public GuestsFrequency? GuestsFrequency { get; set; } = null;
        public string GuestsFrequencyDisplayName { get; set; } = string.Empty;
        public int GuestsFrequencyImportance { get; set; } = 3;

        public WorkSchedule? WorkSchedule { get; set; } = null;
        public string WorkScheduleDisplayName { get; set; } = string.Empty;
        public int WorkScheduleImportance { get; set; } = 3;

        public SharingLevel? SharingLevel { get; set; } = null;
        public string SharingLevelDisplayName { get; set; } = string.Empty;
        public int SharingLevelImportance { get; set; } = 3;

        public decimal? BudgetRangeMin { get; set; } = null;
        public decimal? BudgetRangeMax { get; set; } = null;
        public int BudgetImportance { get; set; } = 3;
    }
}

