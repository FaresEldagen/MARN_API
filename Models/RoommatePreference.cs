using System;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Enums.Property;

namespace MARN_API.Models
{
    public class RoommatePreference
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }

        public bool RoommatePreferencesEnabled { get; set; } = true;

        public Governorate Governorate { get; set; } = Governorate.CairoGovernorate;
        public RoommateSearchStatus SearchStatus { get; set; } = RoommateSearchStatus.Searching;

        public bool? Smoking { get; set; }
        public int SmokingImportance { get; set; } = 3;

        public bool? Pets { get; set; }
        public int PetsImportance { get; set; } = 3;

        public SleepSchedule SleepSchedule { get; set; } = SleepSchedule.Unknown;
        public int SleepImportance { get; set; } = 3;

        public EducationLevel EducationLevel { get; set; } = EducationLevel.Unknown;
        public int EducationImportance { get; set; } = 3;

        public FieldOfStudy FieldOfStudy { get; set; } = FieldOfStudy.Unknown;
        public int FieldOfStudyImportance { get; set; } = 3;

        public int? NoiseTolerance { get; set; }
        public int NoiseToleranceImportance { get; set; } = 3;

        public GuestsFrequency GuestsFrequency { get; set; } = GuestsFrequency.Unknown;
        public int GuestsFrequencyImportance { get; set; } = 3;

        public WorkSchedule WorkSchedule { get; set; } = WorkSchedule.Unknown;
        public int WorkScheduleImportance { get; set; } = 3;

        public SharingLevel SharingLevel { get; set; } = SharingLevel.Unknown;
        public int SharingLevelImportance { get; set; } = 3;

        public decimal? BudgetRangeMin { get; set; }
        public decimal? BudgetRangeMax { get; set; }
        public int BudgetImportance { get; set; } = 3;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}




