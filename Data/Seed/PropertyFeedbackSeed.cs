using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyFeedbackSeed : IEntityTypeConfiguration<PropertyFeedback>
    {
        public void Configure(EntityTypeBuilder<PropertyFeedback> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");

            builder.HasData(
                new PropertyFeedback
                {
                    Id = 900001,
                    PropertyId = 1001,
                    UserId = renterAId,
                    Rating = 5,
                    Content = "Great place! Very clean and quiet.",
                    CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyFeedback
                {
                    Id = 900002,
                    PropertyId = 1001,
                    UserId = renterBId,
                    Rating = 4,
                    Content = "Awesome location, but the neighbors were a bit noisy.",
                    CreatedAt = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyFeedback
                {
                    Id = 900003,
                    PropertyId = 1004,
                    UserId = renterAId,
                    Rating = 5,
                    Content = "Superb luxury villa. Highly recommend!",
                    CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyFeedback
                {
                    Id = 900101,
                    PropertyId = 1001,
                    UserId = bannedRenterId,
                    Rating = 1,
                    Content = "This seeded comment was hidden by moderation for admin review testing.",
                    CreatedAt = new DateTime(2026, 4, 14, 8, 0, 0, DateTimeKind.Utc),
                    IsHiddenByModeration = true,
                    HiddenAt = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc),
                    HiddenByAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    HiddenReason = "Seeded moderation example for admin dashboard testing."
                });
        }
    }
}
