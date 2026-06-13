using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Notification;

namespace MARN_API.Data.Seed
{
    public class NotificationSeed : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var renterDId = Guid.Parse("77777777-7777-7777-7777-777777777777");
            var renterEId = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var pendingRenterId = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");
            var deletedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000003");
            var recentRenterId = Guid.Parse("10000000-0000-0000-0000-000000000004");

            builder.HasData(
                // ==========================================
                // RENTER A (Karim Hassan) - 11111111-1111...
                // ==========================================
                new Notification
                {
                    Id = 6001,
                    UserId = renterAId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Karim\"]",
                    Title = "Welcome to Your New Home Journey Karim!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 1, 0, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6002,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Dokki Modern Loft\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Dokki Modern Loft\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2023, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2023, 12, 19, 12, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6003,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-03-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-03-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2024, 4, 5, 14, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 4, 5, 14, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6004,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-06-30.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2024, 6, 30, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 6, 30, 11, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6005,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-09-30\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-09-30.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2024, 10, 3, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 10, 3, 10, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6006,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-12-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2024, 12, 31, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 12, 31, 9, 12, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6007,
                    UserId = renterAId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.NewMessage,
                    TitleKey = "NOTIFICATION_NEW_MESSAGE_TITLE",
                    BodyKey = "NOTIFICATION_NEW_MESSAGE_BODY",
                    LocalizationArgumentsJson = "[\"Mahmoud Fahmy\"]",
                    Title = "New Message",
                    Body = "You have a new message from Mahmoud Fahmy",
                    Data = "{\"SenderId\":\"44444444-4444-4444-4444-444444444444\",\"SenderName\":\"Mahmoud Fahmy\",\"Content\":\"Hello Karim! Welcome to the property.\"}",
                    ActionType = NotificationActionType.ChatUser,
                    ActionId = "44444444-4444-4444-4444-444444444444",
                    CreatedAt = new DateTime(2025, 3, 20, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 20, 11, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6008,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Agouza Shared House\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Agouza Shared House\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2025, 5, 24, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 5, 24, 10, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6009,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Zamalek Riverside Apartment\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Zamalek Riverside Apartment\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2025, 12, 27, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 12, 27, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6010,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-01-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 1, 29, 12, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6011,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-02-28.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 2, 28, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 2, 28, 10, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6012,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-03-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-03-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 4, 5, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 5, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6013,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-30\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-04-30.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 5, 5, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 5, 10, 8, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6014,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-05-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 6, 5, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 6, 5, 9, 12, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6015,
                    UserId = renterAId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.UpcomingPayment,
                    TitleKey = "NOTIFICATION_UPCOMING_PAYMENT_TITLE",
                    BodyKey = "NOTIFICATION_UPCOMING_PAYMENT_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"7\",\"2026-06-30\"]",
                    Title = "Upcoming Payment Available",
                    Body = "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-30.",
                    CreatedAt = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },

                // ==========================================
                // RENTER B (Mariam Fouad) - 22222222-2222...
                // ==========================================
                new Notification
                {
                    Id = 6101,
                    UserId = renterBId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Mariam\"]",
                    Title = "Welcome to Your New Home Journey Mariam!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 2, 0, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6102,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Sheikh Zayed Luxury Villa\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Sheikh Zayed Luxury Villa\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2026, 4, 20, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 20, 10, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6103,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-05-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2025, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 5, 31, 10, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6104,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-06-30.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2025, 6, 30, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 6, 30, 11, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6105,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-07-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-07-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2025, 8, 4, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 8, 4, 9, 20, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6106,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-08-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2025, 8, 29, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 8, 29, 10, 8, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6107,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Agouza Shared House\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Agouza Shared House\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2026, 1, 27, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 1, 27, 9, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6108,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-02-28\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-02-28.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 2, 22, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 2, 22, 10, 12, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6109,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-03-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-03-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 3, 31, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 3, 31, 11, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6110,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-04-30\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-04-30.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 8, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6111,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentSuccessful,
                    TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                    LocalizationArgumentsJson = "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-05-31\"]",
                    Title = "Payment Successful",
                    Body = "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-05-31.",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 31, 10, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6112,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.UpcomingPayment,
                    TitleKey = "NOTIFICATION_UPCOMING_PAYMENT_TITLE",
                    BodyKey = "NOTIFICATION_UPCOMING_PAYMENT_BODY",
                    LocalizationArgumentsJson = "[\"4000\",\"egp\",\"Agouza Shared House\",\"7\",\"2026-06-30\"]",
                    Title = "Upcoming Payment Available",
                    Body = "Your payment of 4000 egp for \"Agouza Shared House\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-30.",
                    CreatedAt = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6113,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Dokki Modern Loft\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Dokki Modern Loft\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2025, 12, 28, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 12, 28, 9, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6114,
                    UserId = renterBId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractCanceled,
                    TitleKey = "NOTIFICATION_CONTRACT_CANCELLED_TITLE",
                    BodyKey = "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY",
                    LocalizationArgumentsJson = "[\"1000006\",\"Sheikh Zayed Luxury Villa\"]",
                    Title = "Contract Cancelled",
                    Body = "An admin has cancelled contract #1000006 for \"Sheikh Zayed Luxury Villa\".",
                    ActionType = NotificationActionType.RenterDashboard,
                    CreatedAt = new DateTime(2026, 4, 26, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 26, 12, 30, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // RENTER C (Ahmed Nabil) - 33333333-3333...
                // ==========================================
                new Notification
                {
                    Id = 6201,
                    UserId = renterCId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Ahmed\"]",
                    Title = "Welcome to Your New Home Journey Ahmed!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 3, 0, 5, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // RENTER D (Sara Adel) - 33333333-3333...4
                // ==========================================
                new Notification
                {
                    Id = 6301,
                    UserId = renterDId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Sara\"]",
                    Title = "Welcome to Your New Home Journey Sara!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 4, 0, 5, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // RENTER E (Omar Samir) - 33333333-3333...5
                // ==========================================
                new Notification
                {
                    Id = 6401,
                    UserId = renterEId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Omar\"]",
                    Title = "Welcome to Your New Home Journey Omar!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 5, 0, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6402,
                    UserId = renterEId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Mohandeseen Studio Flat\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Mohandeseen Studio Flat\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2025, 11, 28, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 11, 28, 13, 0, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // OWNER X (Mahmoud Fahmy) - 44444444-4444...
                // ==========================================
                new Notification
                {
                    Id = 6501,
                    UserId = ownerXId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Mahmoud\"]",
                    Title = "Welcome to Your New Home Journey Mahmoud!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 1, 0, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6502,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ConnectAccountSuccess,
                    TitleKey = "NOTIFICATION_CONNECT_SUCCESS_TITLE",
                    BodyKey = "NOTIFICATION_CONNECT_SUCCESS_BODY",
                    Title = "Connect Account Activated",
                    Body = "Your Stripe Connect account has been activated and is now ready to withdraw your payments.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2025, 1, 10, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 10, 12, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6503,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Zamalek Riverside Apartment\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Zamalek Riverside Apartment\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1001",
                    CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 2, 1, 1, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6504,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Dokki Modern Loft\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Dokki Modern Loft\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1002",
                    CreatedAt = new DateTime(2023, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2023, 2, 2, 1, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6505,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Mohandeseen Studio Flat\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Mohandeseen Studio Flat\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1003",
                    CreatedAt = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 2, 3, 1, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6506,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Agouza Shared House\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Agouza Shared House\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1100",
                    CreatedAt = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 2, 5, 1, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6507,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.NewBookingRequest,
                    TitleKey = "NOTIFICATION_BOOKING_REQUEST_TITLE",
                    BodyKey = "NOTIFICATION_BOOKING_REQUEST_BODY",
                    LocalizationArgumentsJson = "[\"Dokki Modern Loft\",\"Karim Hassan\"]",
                    Title = "New Booking Request",
                    Body = "You have received a new booking request for \"Dokki Modern Loft\" from Karim Hassan.",
                    CreatedAt = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 4, 1, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6508,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.NewBookingRequest,
                    TitleKey = "NOTIFICATION_BOOKING_REQUEST_TITLE",
                    BodyKey = "NOTIFICATION_BOOKING_REQUEST_BODY",
                    LocalizationArgumentsJson = "[\"Dokki Modern Loft\",\"Karim Hassan\"]",
                    Title = "New Booking Request",
                    Body = "You have received a new booking request for \"Dokki Modern Loft\" from Karim Hassan.",
                    CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 10, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6509,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.NewBookingRequest,
                    TitleKey = "NOTIFICATION_BOOKING_REQUEST_TITLE",
                    BodyKey = "NOTIFICATION_BOOKING_REQUEST_BODY",
                    LocalizationArgumentsJson = "[\"Mohandeseen Studio Flat\",\"Mariam Fouad\"]",
                    Title = "New Booking Request",
                    Body = "You have received a new booking request for \"Mohandeseen Studio Flat\" from Mariam Fouad.",
                    CreatedAt = new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 4, 2, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6510,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.NewBookingRequest,
                    TitleKey = "NOTIFICATION_BOOKING_REQUEST_TITLE",
                    BodyKey = "NOTIFICATION_BOOKING_REQUEST_BODY",
                    LocalizationArgumentsJson = "[\"Mohandeseen Studio Flat\",\"Tarek Owner\"]",
                    Title = "New Booking Request",
                    Body = "You have received a new booking request for \"Mohandeseen Studio Flat\" from Tarek Owner.",
                    CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 4, 10, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6511,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Karim Hassan\",\"Dokki Modern Loft\"]",
                    Title = "Contract Signed",
                    Body = "The renter Karim Hassan has signed the contract for \"Dokki Modern Loft\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000005",
                    CreatedAt = new DateTime(2023, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2023, 12, 20, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6512,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Karim Hassan\",\"Agouza Shared House\"]",
                    Title = "Contract Signed",
                    Body = "The renter Karim Hassan has signed the contract for \"Agouza Shared House\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000003",
                    CreatedAt = new DateTime(2025, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 5, 25, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6513,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Omar Samir\",\"Mohandeseen Studio Flat\"]",
                    Title = "Contract Signed",
                    Body = "The renter Omar Samir has signed the contract for \"Mohandeseen Studio Flat\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000102",
                    CreatedAt = new DateTime(2025, 11, 29, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 11, 29, 11, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6514,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Karim Hassan\",\"Zamalek Riverside Apartment\"]",
                    Title = "Contract Signed",
                    Body = "The renter Karim Hassan has signed the contract for \"Zamalek Riverside Apartment\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000001",
                    CreatedAt = new DateTime(2025, 12, 28, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 12, 28, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6515,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Mariam Fouad\",\"Agouza Shared House\"]",
                    Title = "Contract Signed",
                    Body = "The renter Mariam Fouad has signed the contract for \"Agouza Shared House\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000004",
                    CreatedAt = new DateTime(2026, 1, 28, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 1, 28, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6516,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Mariam Fouad\",\"Dokki Modern Loft\"]",
                    Title = "Contract Signed",
                    Body = "The renter Mariam Fouad has signed the contract for \"Dokki Modern Loft\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000002",
                    CreatedAt = new DateTime(2025, 12, 29, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 12, 29, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6517,
                    UserId = ownerXId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.NewMessage,
                    TitleKey = "NOTIFICATION_NEW_MESSAGE_TITLE",
                    BodyKey = "NOTIFICATION_NEW_MESSAGE_BODY",
                    LocalizationArgumentsJson = "[\"Karim Hassan\"]",
                    Title = "New Message",
                    Body = "You have a new message from Karim Hassan",
                    Data = "{\"SenderId\":\"11111111-1111-1111-1111-111111111111\",\"SenderName\":\"Karim Hassan\",\"Content\":\"Hello Mahmoud! I am interested in your property.\"}",
                    ActionType = NotificationActionType.ChatUser,
                    ActionId = "11111111-1111-1111-1111-111111111111",
                    CreatedAt = new DateTime(2025, 3, 20, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 20, 10, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6518,
                    UserId = ownerXId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.NewMessage,
                    TitleKey = "NOTIFICATION_NEW_MESSAGE_TITLE",
                    BodyKey = "NOTIFICATION_NEW_MESSAGE_BODY",
                    LocalizationArgumentsJson = "[\"Sayed Banned\"]",
                    Title = "New Message",
                    Body = "You have a new message from Sayed Banned",
                    Data = "{\"SenderId\":\"10000000-0000-0000-0000-000000000002\",\"SenderName\":\"Sayed Banned\",\"Content\":\"Can you lower the rent?\"}",
                    ActionType = NotificationActionType.ChatUser,
                    ActionId = "10000000-0000-0000-0000-000000000002",
                    CreatedAt = new DateTime(2026, 4, 12, 19, 30, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 12, 19, 35, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6519,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-03-31\",\"2024-04-15\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-03-31.\n\nYou can withdraw this amount after 2024-04-15.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2024, 4, 5, 14, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 4, 5, 14, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6520,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-04-05\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-04-05 is now available for withdrawal.",
                    CreatedAt = new DateTime(2024, 4, 15, 14, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 4, 15, 14, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6521,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\",\"2024-07-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-06-30.\n\nYou can withdraw this amount after 2024-07-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2024, 6, 30, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 6, 30, 11, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6522,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-06-30 is now available for withdrawal.",
                    CreatedAt = new DateTime(2024, 7, 10, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 7, 10, 11, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6523,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-09-30\",\"2024-10-13\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-09-30.\n\nYou can withdraw this amount after 2024-10-13.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2024, 10, 3, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 10, 3, 10, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6524,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-10-03\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-10-03 is now available for withdrawal.",
                    CreatedAt = new DateTime(2024, 10, 13, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 10, 13, 10, 20, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6525,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\",\"2025-01-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-12-31.\n\nYou can withdraw this amount after 2025-01-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2024, 12, 31, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2024, 12, 31, 9, 12, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6526,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-12-31 is now available for withdrawal.",
                    CreatedAt = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 10, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6527,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-31\",\"2026-02-08\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-01-31.\n\nYou can withdraw this amount after 2026-02-08.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 1, 29, 12, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6528,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-29\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-01-29 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 2, 8, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 2, 8, 12, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6529,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\",\"2026-03-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-02-28.\n\nYou can withdraw this amount after 2026-03-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 2, 28, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 2, 28, 10, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6530,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-02-28 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 3, 10, 10, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6531,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-03-31\",\"2026-04-15\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-03-31.\n\nYou can withdraw this amount after 2026-04-15.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 4, 5, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 5, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6532,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-05\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-04-05 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 4, 15, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 15, 9, 20, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6533,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-30\",\"2026-05-15\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-04-30.\n\nYou can withdraw this amount after 2026-05-15.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 5, 5, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 5, 10, 08, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6534,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-05\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-05-05 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 5, 15, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 15, 10, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6535,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-31\",\"2026-06-15\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-05-31.\n\nYou can withdraw this amount after 2026-06-15.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 6, 5, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 6, 5, 9, 12, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6536,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-06-05\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-06-05 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 6, 15, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 6, 15, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6537,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-02-28\",\"2026-03-04\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-02-28.\n\nYou can withdraw this amount after 2026-03-04.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 2, 22, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 2, 22, 10, 12, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6538,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-02-22\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-02-22 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 3, 4, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 3, 4, 10, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6539,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-03-31\",\"2026-04-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-03-31.\n\nYou can withdraw this amount after 2026-04-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 3, 31, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 3, 31, 11, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6540,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-03-31\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-03-31 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 4, 10, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 10, 11, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6541,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-04-30\",\"2026-05-18\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-04-30.\n\nYou can withdraw this amount after 2026-05-18.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 8, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6542,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-08\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-05-08 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 5, 18, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 18, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6543,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-31\",\"2026-06-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-05-31.\n\nYou can withdraw this amount after 2026-06-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 31, 10, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6544,
                    UserId = ownerXId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-31\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-05-31 is now available for withdrawal.",
                    CreatedAt = new DateTime(2026, 6, 10, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 6, 10, 10, 15, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // OWNER Y (Sherif Owner) - 55555555-5555...
                // ==========================================
                new Notification
                {
                    Id = 6601,
                    UserId = ownerYId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Sherif\"]",
                    Title = "Welcome to Your New Home Journey Sherif!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 2, 0, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6602,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Pending Downtown Apartment\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Pending Downtown Apartment\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1201",
                    CreatedAt = new DateTime(2026, 5, 3, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 3, 9, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6603,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Declined Garden House\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Declined Garden House\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1202",
                    CreatedAt = new DateTime(2026, 4, 18, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 18, 13, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6604,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Soft Deleted Test Studio\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Soft Deleted Test Studio\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1203",
                    CreatedAt = new DateTime(2026, 3, 8, 16, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 3, 8, 17, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6605,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyDeleted,
                    TitleKey = "NOTIFICATION_PROPERTY_DELETED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_DELETED_BODY",
                    LocalizationArgumentsJson = "[\"Soft Deleted Test Studio\"]",
                    Title = "Property Deleted",
                    Body = "Your property \"Soft Deleted Test Studio\" has been deleted.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1203",
                    CreatedAt = new DateTime(2026, 4, 4, 13, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 4, 13, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6606,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Recent Marina Flat\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Recent Marina Flat\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1204",
                    CreatedAt = new DateTime(2026, 5, 5, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 5, 10, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6607,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Moderated Riverside Villa\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Moderated Riverside Villa\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1205",
                    CreatedAt = new DateTime(2026, 5, 7, 15, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 7, 15, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6608,
                    UserId = ownerYId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Sayed Banned\",\"Moderated Riverside Villa\"]",
                    Title = "Contract Signed",
                    Body = "The renter Sayed Banned has signed the contract for \"Moderated Riverside Villa\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000103",
                    CreatedAt = new DateTime(2026, 5, 21, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 21, 10, 30, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // OWNER Z (Tarek Owner) - 66666666-6666...
                // ==========================================
                new Notification
                {
                    Id = 6701,
                    UserId = ownerZId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Tarek\"]",
                    Title = "Welcome to Your New Home Journey Tarek!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 1, 3, 0, 5, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6702,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ConnectAccountSuccess,
                    TitleKey = "NOTIFICATION_CONNECT_SUCCESS_TITLE",
                    BodyKey = "NOTIFICATION_CONNECT_SUCCESS_BODY",
                    Title = "Connect Account Activated",
                    Body = "Your Stripe Connect account has been activated and is now ready to withdraw your payments.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2025, 2, 10, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 2, 10, 12, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6703,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,
                    TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                    LocalizationArgumentsJson = "[\"Sheikh Zayed Luxury Villa\"]",
                    Title = "Property Submitted for Review",
                    Body = "Your property \"Sheikh Zayed Luxury Villa\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = "1004",
                    CreatedAt = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 2, 4, 1, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6704,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractSigned,
                    TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                    LocalizationArgumentsJson = "[\"Mariam Fouad\",\"Sheikh Zayed Luxury Villa\"]",
                    Title = "Contract Signed",
                    Body = "The renter Mariam Fouad has signed the contract for \"Sheikh Zayed Luxury Villa\".",
                    ActionType = NotificationActionType.Contract,
                    ActionId = "1000006",
                    CreatedAt = new DateTime(2026, 4, 25, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 25, 8, 0, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6705,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractCanceled,
                    TitleKey = "NOTIFICATION_CONTRACT_CANCELLED_TITLE",
                    BodyKey = "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY",
                    LocalizationArgumentsJson = "[\"1000006\",\"Sheikh Zayed Luxury Villa\"]",
                    Title = "Contract Cancelled",
                    Body = "An admin has cancelled contract #1000006 for \"Sheikh Zayed Luxury Villa\".",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2026, 4, 26, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 4, 26, 12, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6706,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\",\"2025-06-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-05-31.\n\nYou can withdraw this amount after 2025-06-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2025, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 5, 31, 10, 10, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6707,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-05-31 is now available for withdrawal.",
                    CreatedAt = new DateTime(2025, 6, 10, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 6, 10, 10, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6708,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\",\"2025-07-10\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-06-30.\n\nYou can withdraw this amount after 2025-07-10.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2025, 6, 30, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 6, 30, 11, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6709,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-06-30 is now available for withdrawal.",
                    CreatedAt = new DateTime(2025, 7, 10, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 7, 10, 11, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6710,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-07-31\",\"2025-08-14\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-07-31.\n\nYou can withdraw this amount after 2025-08-14.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2025, 8, 4, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 8, 4, 9, 20, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6711,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-04\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-08-04 is now available for withdrawal.",
                    CreatedAt = new DateTime(2025, 8, 14, 9, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 8, 14, 9, 15, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6712,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PaymentReceived,
                    TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-31\",\"2025-09-08\"]",
                    Title = "Payment Received",
                    Body = "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-08-31.\n\nYou can withdraw this amount after 2025-09-08.",
                    ActionType = NotificationActionType.OwnerDashboard,
                    CreatedAt = new DateTime(2025, 8, 29, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 8, 29, 10, 08, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6713,
                    UserId = ownerZId,
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.AvailableForWithdrawal,
                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                    LocalizationArgumentsJson = "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-29\"]",
                    Title = "Payment Available for Withdrawal",
                    Body = "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-08-29 is now available for withdrawal.",
                    CreatedAt = new DateTime(2025, 9, 8, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 9, 8, 10, 15, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // PENDING RENTER (Khaled Pending) - 10000000-0000...1
                // ==========================================
                new Notification
                {
                    Id = 6801,
                    UserId = pendingRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Khaled\"]",
                    Title = "Welcome to Your New Home Journey Khaled!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2026, 5, 10, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 10, 10, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6802,
                    UserId = pendingRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_PROFILE_UPDATED_TITLE",
                    BodyKey = "NOTIFICATION_PROFILE_UPDATED_BODY",
                    Title = "Profile Updated Successfully!",
                    Body = "Your profile has been updated successfully. Our team will review your information, and your account is expected to be verified within approximately 24 hours.\n\nOnce verified, you’ll be able to start renting properties, listing your own, and connecting with compatible roommates.",
                    CreatedAt = new DateTime(2026, 5, 10, 10, 05, 0, DateTimeKind.Utc),
                    ReadAt = null
                },

                // ==========================================
                // BANNED RENTER (Sayed Banned) - 10000000-0000...2
                // ==========================================
                new Notification
                {
                    Id = 6901,
                    UserId = bannedRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Sayed\"]",
                    Title = "Welcome to Your New Home Journey Sayed!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2026, 3, 5, 14, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 3, 5, 14, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6902,
                    UserId = bannedRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_ACCOUNT_BANNED_TITLE",
                    BodyKey = "NOTIFICATION_ACCOUNT_BANNED_BODY",
                    Title = "Account Banned",
                    Body = "An admin has banned your account. You can no longer use MARN until the ban is removed. If you believe this is a mistake, please contact support.",
                    CreatedAt = new DateTime(2026, 4, 13, 9, 01, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6903,
                    UserId = bannedRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_ACCOUNT_BANNED_TITLE",
                    BodyKey = "NOTIFICATION_ACCOUNT_BANNED_BODY",
                    Title = "Account Banned",
                    Body = "An admin has banned your account. You can no longer use MARN until the ban is removed. If you believe this is a mistake, please contact support.",
                    CreatedAt = new DateTime(2026, 4, 14, 12, 01, 0, DateTimeKind.Utc),
                    ReadAt = null
                },
                new Notification
                {
                    Id = 6904,
                    UserId = bannedRenterId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Moderated Riverside Villa\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Moderated Riverside Villa\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2026, 5, 20, 12, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 20, 12, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 6905,
                    UserId = bannedRenterId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.UpcomingPayment,
                    TitleKey = "NOTIFICATION_UPCOMING_PAYMENT_TITLE",
                    BodyKey = "NOTIFICATION_UPCOMING_PAYMENT_BODY",
                    LocalizationArgumentsJson = "[\"5000\",\"egp\",\"Moderated Riverside Villa\",\"7\",\"2026-06-23\"]",
                    Title = "Upcoming Payment Available",
                    Body = "Your payment of 5000 egp for \"Moderated Riverside Villa\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-23.",
                    CreatedAt = new DateTime(2026, 6, 16, 0, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                },

                // ==========================================
                // DELETED RENTER (Ramy Deleted) - 10000000-0000...3
                // ==========================================
                new Notification
                {
                    Id = 7001,
                    UserId = deletedRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Ramy\"]",
                    Title = "Welcome to Your New Home Journey Ramy!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2026, 2, 20, 11, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 2, 20, 11, 05, 0, DateTimeKind.Utc)
                },

                // ==========================================
                // RECENT RENTER (Nour Recent) - 10000000-0000...4
                // ==========================================
                new Notification
                {
                    Id = 7101,
                    UserId = recentRenterId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,
                    TitleKey = "NOTIFICATION_WELCOME_TITLE",
                    BodyKey = "NOTIFICATION_WELCOME_BODY",
                    LocalizationArgumentsJson = "[\"Nour\"]",
                    Title = "Welcome to Your New Home Journey Nour!",
                    Body = "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.",
                    ActionType = NotificationActionType.EditProfile,
                    CreatedAt = new DateTime(2026, 5, 25, 16, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2026, 5, 25, 16, 05, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 7102,
                    UserId = recentRenterId,
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractStarted,
                    TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                    LocalizationArgumentsJson = "[\"Recent Marina Flat\"]",
                    Title = "Contract Ready for Signature",
                    Body = "The owner of \"Recent Marina Flat\" has generated a contract for you. Please review and sign it.",
                    CreatedAt = new DateTime(2026, 5, 26, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = null
                }
            );
        }
    }
}
