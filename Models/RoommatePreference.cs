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
        public PreferenceImportance SmokingImportance { get; set; } = PreferenceImportance.Important;

        public bool? Pets { get; set; }
        public PreferenceImportance PetsImportance { get; set; } = PreferenceImportance.Important;

        public SleepSchedule SleepSchedule { get; set; } = SleepSchedule.Unknown;
        public PreferenceImportance SleepImportance { get; set; } = PreferenceImportance.Important;

        public EducationLevel EducationLevel { get; set; } = EducationLevel.Unknown;
        public PreferenceImportance EducationImportance { get; set; } = PreferenceImportance.Important;

        public FieldOfStudy FieldOfStudy { get; set; } = FieldOfStudy.Unknown;
        public PreferenceImportance FieldOfStudyImportance { get; set; } = PreferenceImportance.Important;

        public int? NoiseTolerance { get; set; }
        public PreferenceImportance NoiseToleranceImportance { get; set; } = PreferenceImportance.Important;

        public GuestsFrequency GuestsFrequency { get; set; } = GuestsFrequency.Unknown;
        public PreferenceImportance GuestsFrequencyImportance { get; set; } = PreferenceImportance.Important;

        public WorkSchedule WorkSchedule { get; set; } = WorkSchedule.Unknown;
        public PreferenceImportance WorkScheduleImportance { get; set; } = PreferenceImportance.Important;

        public SharingLevel SharingLevel { get; set; } = SharingLevel.Unknown;
        public PreferenceImportance SharingLevelImportance { get; set; } = PreferenceImportance.Important;

        public decimal? BudgetRangeMin { get; set; }
        public decimal? BudgetRangeMax { get; set; }
        public PreferenceImportance BudgetImportance { get; set; } = PreferenceImportance.Important;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}



