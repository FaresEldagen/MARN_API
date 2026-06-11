using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Utilities;

namespace MARN_API.Data.Seed
{
    /// <summary>
    /// User activity seed data (merged from 20_user_activity_seed_corrected.csv).
    /// Covers search, view, save, and booking activities across 5 renters and multiple governorates.
    /// </summary>
    public class UserActivitySeed : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            var userCairoMid = Guid.Parse("11111111-1111-1111-1111-111111111111"); // renterAId
            var userAlexLow = Guid.Parse("22222222-2222-2222-2222-222222222222"); // renterBId
            var userDeltaMulti = Guid.Parse("33333333-3333-3333-3333-333333333333"); // renterCId
            var userFamilyHigh = Guid.Parse("77777777-7777-7777-7777-777777777777"); // renterDId
            var userCoastalFlex = Guid.Parse("88888888-8888-8888-8888-888888888888"); // renterEId

            builder.HasData(
                // ── User: Karim Hassan (Cairo Mid) ──
                new UserActivity { Id = 4001, UserId = userCairoMid, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"cairo apartment\",\"governorate\":\"CairoGovernorate\",\"minPrice\":5000,\"maxPrice\":9000,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":30.0444,\"longitude\":31.2357,\"radiusKm\":15,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 21, 9, 0, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4002, UserId = userCairoMid, PropertyId = 2001, UserActivityType = UserActivityTypes.View, CreatedAt = new DateTime(2026, 5, 21, 9, 2, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4003, UserId = userCairoMid, PropertyId = 2003, UserActivityType = UserActivityTypes.Save, CreatedAt = new DateTime(2026, 5, 21, 9, 5, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4004, UserId = userCairoMid, PropertyId = 2002, UserActivityType = UserActivityTypes.Booking, CreatedAt = new DateTime(2026, 5, 21, 9, 9, 0, DateTimeKind.Utc) },

                // ── User: Mariam Fouad (Alex Low) ──
                new UserActivity { Id = 4005, UserId = userAlexLow, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"alex studio\",\"governorate\":\"AlexandriaGovernorate\",\"maxPrice\":6000,\"type\":\"Studio\",\"rentalUnit\":\"Monthly\",\"latitude\":31.2001,\"longitude\":29.9187,\"radiusKm\":12,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 22, 10, 0, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4006, UserId = userAlexLow, PropertyId = 2009, UserActivityType = UserActivityTypes.View, CreatedAt = new DateTime(2026, 5, 22, 10, 3, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4007, UserId = userAlexLow, PropertyId = 2009, UserActivityType = UserActivityTypes.Save, CreatedAt = new DateTime(2026, 5, 22, 10, 5, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4008, UserId = userAlexLow, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"alex apartment\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":5000,\"maxPrice\":8000,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 22, 10, 11, 0, DateTimeKind.Utc) },

                // ── User: Ahmed Nabil (Delta Multi) ──
                new UserActivity { Id = 4009, UserId = userDeltaMulti, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"zagazig apartment\",\"governorate\":\"SharkiaGovernorate\",\"minPrice\":4500,\"maxPrice\":7000,\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":30.5877,\"longitude\":31.5020,\"radiusKm\":20,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 23, 11, 0, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4010, UserId = userDeltaMulti, PropertyId = 2011, UserActivityType = UserActivityTypes.View, CreatedAt = new DateTime(2026, 5, 23, 11, 2, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4011, UserId = userDeltaMulti, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"damietta apartment\",\"governorate\":\"DamiettaGovernorate\",\"minPrice\":4500,\"maxPrice\":8000,\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":31.4165,\"longitude\":31.8133,\"radiusKm\":25,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 23, 11, 10, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4012, UserId = userDeltaMulti, PropertyId = 2016, UserActivityType = UserActivityTypes.Save, CreatedAt = new DateTime(2026, 5, 23, 11, 14, 0, DateTimeKind.Utc) },

                // ── User: Sara Adel (Family High) ──
                new UserActivity { Id = 4013, UserId = userFamilyHigh, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"cairo villa\",\"governorate\":\"CairoGovernorate\",\"minPrice\":15000,\"maxPrice\":26000,\"type\":\"Villa\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":4,\"minBathrooms\":3,\"latitude\":30.0300,\"longitude\":31.4700,\"radiusKm\":20,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 24, 12, 0, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4014, UserId = userFamilyHigh, PropertyId = 2005, UserActivityType = UserActivityTypes.View, CreatedAt = new DateTime(2026, 5, 24, 12, 4, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4015, UserId = userFamilyHigh, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"alex villa\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":18000,\"maxPrice\":26000,\"type\":\"Villa\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":4,\"minBathrooms\":3,\"latitude\":31.2854,\"longitude\":30.0173,\"radiusKm\":18,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 24, 12, 10, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4016, UserId = userFamilyHigh, PropertyId = 2010, UserActivityType = UserActivityTypes.Booking, CreatedAt = new DateTime(2026, 5, 24, 12, 15, 0, DateTimeKind.Utc) },

                // ── User: Omar Samir (Coastal Flex) ──
                new UserActivity { Id = 4017, UserId = userCoastalFlex, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"ras el bar studio\",\"governorate\":\"DamiettaGovernorate\",\"maxPrice\":5000,\"type\":\"Studio\",\"rentalUnit\":\"Daily\",\"latitude\":31.5085,\"longitude\":31.8404,\"radiusKm\":10,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 25, 13, 0, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4018, UserId = userCoastalFlex, PropertyId = 2017, UserActivityType = UserActivityTypes.View, CreatedAt = new DateTime(2026, 5, 25, 13, 3, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4019, UserId = userCoastalFlex, PropertyId = null, UserActivityType = UserActivityTypes.Search, Metadata = "{\"keyword\":\"alex coast apartment\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":6000,\"maxPrice\":8500,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"latitude\":31.2156,\"longitude\":29.9420,\"radiusKm\":15,\"page\":1,\"pageSize\":20}", CreatedAt = new DateTime(2026, 5, 25, 13, 12, 0, DateTimeKind.Utc) },
                new UserActivity { Id = 4020, UserId = userCoastalFlex, PropertyId = 2006, UserActivityType = UserActivityTypes.Save, CreatedAt = new DateTime(2026, 5, 25, 13, 16, 0, DateTimeKind.Utc) }
            );
        }
    }
}
