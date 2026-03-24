using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.RoommatePreferences;

namespace MARN_API.Data.Seed
{
    public class RoommatePreferenceSeed : IEntityTypeConfiguration<RoommatePreference>
    {
        public void Configure(EntityTypeBuilder<RoommatePreference> builder)
        {
            builder.HasData(
                new RoommatePreference
                {
                    Id = 1,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    RoommatePreferencesEnabled = true,
                    Smoking = false,
                    Pets = true,
                    SleepSchedule = SleepSchedule.EarlyBird,
                    EducationLevel = EducationLevel.Bachelor,
                    FieldOfStudy = FieldOfStudy.Engineering,
                    NoiseTolerance = 3,
                    GuestsFrequency = GuestsFrequency.Rarely,
                    WorkSchedule = WorkSchedule.DayShift,
                    SharingLevel = SharingLevel.High,
                    BudgetRangeMin = 3000,
                    BudgetRangeMax = 6000
                },
                new RoommatePreference
                {
                    Id = 2,
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    RoommatePreferencesEnabled = true,
                    Smoking = true,
                    Pets = false,
                    SleepSchedule = SleepSchedule.NightOwl,
                    EducationLevel = EducationLevel.Bachelor,
                    FieldOfStudy = FieldOfStudy.Arts,
                    NoiseTolerance = 5,
                    GuestsFrequency = GuestsFrequency.Often,
                    WorkSchedule = WorkSchedule.Remote,
                    SharingLevel = SharingLevel.High,
                    BudgetRangeMin = 2000,
                    BudgetRangeMax = 4500
                }
            );
        }
    }
}
