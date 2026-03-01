using System;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class RoommatePreference
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }

        public bool? Smoking { get; set; }
        public bool? Pets { get; set; }
        public string? SleepSchedule { get; set; }
        public EducationLevel EducationLevel { get; set; } = EducationLevel.Unknown;
        public FieldOfStudy FieldOfStudy { get; set; } = FieldOfStudy.Unknown;
        public int? NoiseTolerance { get; set; }
        public GuestsFrequency GuestsFrequency { get; set; } = GuestsFrequency.Unknown;
        public WorkSchedule WorkSchedule { get; set; } = WorkSchedule.Unknown;
        public SharingLevel SharingLevel { get; set; } = SharingLevel.Unknown;
        public decimal? BudgetRangeMin { get; set; }
        public decimal? BudgetRangeMax { get; set; }
        public string? Bio { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}



