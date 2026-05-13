using MARN_API.Enums.RoommatePreferences;
using MARN_API.Enums.Property;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Profile
{
    public class UpdateRoommatePreferencesDto : IValidatableObject
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool RoommatePreferencesEnabled { get; set; } = true;

        public Governorate Governorate { get; set; } = Governorate.CairoGovernorate;
        public RoommateSearchStatus SearchStatus { get; set; } = RoommateSearchStatus.Searching;

        public bool? Smoking { get; set; }
        [Range(1, 5)]
        public int SmokingImportance { get; set; } = 3;

        public bool? Pets { get; set; }
        [Range(1, 5)]
        public int PetsImportance { get; set; } = 3;

        public SleepSchedule SleepSchedule { get; set; } = SleepSchedule.Unknown;
        [Range(1, 5)]
        public int SleepImportance { get; set; } = 3;

        public EducationLevel EducationLevel { get; set; } = EducationLevel.Unknown;
        [Range(1, 5)]
        public int EducationImportance { get; set; } = 3;

        public FieldOfStudy FieldOfStudy { get; set; } = FieldOfStudy.Unknown;
        [Range(1, 5)]
        public int FieldOfStudyImportance { get; set; } = 3;

        [Range(1, 5)]
        public int? NoiseTolerance { get; set; }
        [Range(1, 5)]
        public int NoiseToleranceImportance { get; set; } = 3;

        public GuestsFrequency GuestsFrequency { get; set; } = GuestsFrequency.Unknown;
        [Range(1, 5)]
        public int GuestsFrequencyImportance { get; set; } = 3;

        public WorkSchedule WorkSchedule { get; set; } = WorkSchedule.Unknown;
        [Range(1, 5)]
        public int WorkScheduleImportance { get; set; } = 3;

        public SharingLevel SharingLevel { get; set; } = SharingLevel.Unknown;
        [Range(1, 5)]
        public int SharingLevelImportance { get; set; } = 3;

        public decimal? BudgetRangeMin { get; set; }
        public decimal? BudgetRangeMax { get; set; }
        [Range(1, 5)]
        public int BudgetImportance { get; set; } = 3;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BudgetRangeMin.HasValue && BudgetRangeMax.HasValue && BudgetRangeMax.Value < BudgetRangeMin.Value)
            {
                yield return new ValidationResult(
                    "BudgetRangeMax must be greater than or equal to BudgetRangeMin.",
                    new[] { nameof(BudgetRangeMax), nameof(BudgetRangeMin) });
            }
        }
    }
}

