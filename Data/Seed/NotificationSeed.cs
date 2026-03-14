using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class NotificationSeed : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            builder.HasData(
                // Renter A notifications (2 unread, 1 read)
                new Notification
                {
                    Id = 6001,
                    UserId = renterAId,
                    Type = default(NotificationType),
                    Title = "Upcoming Payment Due",
                    Body = "Your next rent payment is due soon.",
                    CreatedAt = new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6002,
                    UserId = renterAId,
                    Type = default(NotificationType),
                    Title = "Booking Request Update",
                    Body = "Your booking request has been accepted.",
                    CreatedAt = new DateTime(2025, 4, 6, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6003,
                    UserId = renterAId,
                    Type = default(NotificationType),
                    Title = "Welcome to the platform",
                    Body = "Thanks for signing up!",
                    CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 2, 0, 0, 0, DateTimeKind.Utc)
                },

                // Renter B notifications
                new Notification
                {
                    Id = 6004,
                    UserId = renterBId,
                    Type = default(NotificationType),
                    Title = "Booking Pending",
                    Body = "Your booking request is pending owner approval.",
                    CreatedAt = new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6005,
                    UserId = renterBId,
                    Type = default(NotificationType),
                    Title = "Complete Your Profile",
                    Body = "Add more details to your profile to get better recommendations.",
                    CreatedAt = new DateTime(2025, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 4, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

