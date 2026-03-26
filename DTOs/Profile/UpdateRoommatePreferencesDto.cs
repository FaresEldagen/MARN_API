using MARN_API.Enums.RoommatePreferences;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Profile
{
    public class UpdateRoommatePreferencesDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool RoommatePreferencesEnabled { get; set; } = true;

        public bool? Smoking { get; set; }
        public bool? Pets { get; set; }
        public SleepSchedule SleepSchedule { get; set; } = SleepSchedule.Unknown;
        public EducationLevel EducationLevel { get; set; } = EducationLevel.Unknown;
        public FieldOfStudy FieldOfStudy { get; set; } = FieldOfStudy.Unknown;
        public int? NoiseTolerance { get; set; }
        public GuestsFrequency GuestsFrequency { get; set; } = GuestsFrequency.Unknown;
        public WorkSchedule WorkSchedule { get; set; } = WorkSchedule.Unknown;
        public SharingLevel SharingLevel { get; set; } = SharingLevel.Unknown;
        public decimal? BudgetRangeMin { get; set; }
        public decimal? BudgetRangeMax { get; set; }
    }
}
