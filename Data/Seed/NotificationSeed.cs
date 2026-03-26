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
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                // Renter A notifications (2 unread, 1 read)
                new Notification
                {
                    Id = 6001,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
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
                    UserType = NotificationUserType.Renter,
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
                    UserType = NotificationUserType.Renter,
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
                    UserType = NotificationUserType.Renter,
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
                    UserType = NotificationUserType.Renter,
                    Type = default(NotificationType),
                    Title = "Complete Your Profile",
                    Body = "Add more details to your profile to get better recommendations.",
                    CreatedAt = new DateTime(2025, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 4, 0, 0, 0, DateTimeKind.Utc)
                },

                // Owner X notifications (owner dashboard)
                new Notification
                {
                    Id = 6006,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = default(NotificationType),
                    Title = "New booking request",
                    Body = "A renter submitted a booking request for one of your properties.",
                    CreatedAt = new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6007,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = default(NotificationType),
                    Title = "Payment received",
                    Body = "A rent payment was successfully processed.",
                    CreatedAt = new DateTime(2025, 4, 9, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6008,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = default(NotificationType),
                    Title = "Welcome, property owner",
                    Body = "Complete your listing details to attract more renters.",
                    CreatedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc)
                },

                // Owner Z renter-type notifications (renter dashboard)
                new Notification
                {
                    Id = 6009,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Renter,
                    Type = default(NotificationType),
                    Title = "Rent Payment Due Soon",
                    Body = "Your next rent payment for Cozy Seed Apartment is due soon.",
                    CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6010,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Renter,
                    Type = default(NotificationType),
                    Title = "Booking Submitted",
                    Body = "Your booking request for Seed Studio Flat has been submitted.",
                    CreatedAt = new DateTime(2025, 4, 11, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6011,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Renter,
                    Type = default(NotificationType),
                    Title = "Welcome to MARN",
                    Body = "Thanks for joining MARN! Explore properties near you.",
                    CreatedAt = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc)
                },

                // Owner Z owner-type notifications (owner dashboard)
                new Notification
                {
                    Id = 6012,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = default(NotificationType),
                    Title = "Your property is live",
                    Body = "Luxury Seed Villa is now visible to renters.",
                    CreatedAt = new DateTime(2025, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6013,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = default(NotificationType),
                    Title = "Welcome, property owner",
                    Body = "Set up your payout details to start receiving rent payments.",
                    CreatedAt = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
