using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Enums.Property;

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
                    BudgetRangeMax = 6000,
                    Governorate = Governorate.CairoGovernorate
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
                    BudgetRangeMax = 4500,
                    Governorate = Governorate.CairoGovernorate
                },
                new RoommatePreference
                {
                    Id = 3,
                    UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    RoommatePreferencesEnabled = true,
                    Governorate = Governorate.CairoGovernorate,
                    SearchStatus = RoommateSearchStatus.Searching,
                    Smoking = false, 
                    Pets = false, 
                    SleepSchedule = SleepSchedule.EarlyBird, 
                    EducationLevel = EducationLevel.Bachelor, 
                    FieldOfStudy = FieldOfStudy.Business, 
                    NoiseTolerance = 2, 
                    GuestsFrequency = GuestsFrequency.Often, 
                    WorkSchedule = WorkSchedule.Remote, 
                    SharingLevel = SharingLevel.Medium, 
                    BudgetRangeMin = 2000, 
                    BudgetRangeMax = 3500
                },
                new RoommatePreference
                {
                    Id = 4,
                    UserId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    RoommatePreferencesEnabled = true,
                    Governorate = Governorate.CairoGovernorate,
                    SearchStatus = RoommateSearchStatus.Offering,
                    Smoking = false, 
                    Pets = true, 
                    SleepSchedule = SleepSchedule.Flexible, 
                    EducationLevel = EducationLevel.Master, 
                    FieldOfStudy = FieldOfStudy.Engineering, 
                    NoiseTolerance = 4, 
                    GuestsFrequency = GuestsFrequency.Rarely, 
                    WorkSchedule = WorkSchedule.DayShift, 
                    SharingLevel = SharingLevel.High, 
                    BudgetRangeMin = 4000, 
                    BudgetRangeMax = 5500
                },
                new RoommatePreference
                {
                    Id = 5,
                    UserId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    RoommatePreferencesEnabled = true,
                    Governorate = Governorate.CairoGovernorate,
                    SearchStatus = RoommateSearchStatus.Searching,
                    Smoking = true, 
                    Pets = false, 
                    SleepSchedule = SleepSchedule.NightOwl, 
                    EducationLevel = EducationLevel.HighSchool, 
                    FieldOfStudy = FieldOfStudy.Arts, 
                    NoiseTolerance = 5, 
                    GuestsFrequency = GuestsFrequency.Often, 
                    WorkSchedule = WorkSchedule.NightShift, 
                    SharingLevel = SharingLevel.Low, 
                    BudgetRangeMin = 7000, 
                    BudgetRangeMax = 10000
                }
            );
        }
    }
}
