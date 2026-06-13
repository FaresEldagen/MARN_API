using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDummyPropertiesAndFixedNotificationSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "Data", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserType" },
                values: new object[] { 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim\"]", new DateTime(2025, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Karim!", "NOTIFICATION_WELCOME_TITLE", 0, 0 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6002L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey" },
                values: new object[] { null, "The owner of \"Dokki Modern Loft\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "[\"Dokki Modern Loft\"]", new DateTime(2023, 12, 19, 12, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6003L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type" },
                values: new object[] { 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-03-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-03-31\"]", new DateTime(2024, 4, 5, 14, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6004L,
                columns: new[] { "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type" },
                values: new object[] { null, 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-06-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\"]", new DateTime(2024, 6, 30, 11, 5, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6005L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-09-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 10, 3, 10, 0, 0, 0, DateTimeKind.Utc), "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-09-30\"]", new DateTime(2024, 10, 3, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6006L,
                columns: new[] { "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { null, 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-12-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 12, 31, 9, 0, 0, 0, DateTimeKind.Utc), "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\"]", new DateTime(2024, 12, 31, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L,
                columns: new[] { "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "Data", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { "44444444-4444-4444-4444-444444444444", 2, "You have a new message from Mahmoud Fahmy", "NOTIFICATION_NEW_MESSAGE_BODY", new DateTime(2025, 3, 20, 11, 0, 0, 0, DateTimeKind.Utc), "{\"SenderId\":\"44444444-4444-4444-4444-444444444444\",\"SenderName\":\"Mahmoud Fahmy\",\"Content\":\"Hello Karim! Welcome to the property.\"}", "[\"Mahmoud Fahmy\"]", new DateTime(2025, 3, 20, 11, 5, 0, 0, DateTimeKind.Utc), "New Message", "NOTIFICATION_NEW_MESSAGE_TITLE", 1, new Guid("11111111-1111-1111-1111-111111111111"), 0 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6008L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { null, "The owner of \"Agouza Shared House\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), "[\"Agouza Shared House\"]", new DateTime(2025, 5, 24, 10, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { null, "The owner of \"Zamalek Riverside Apartment\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "[\"Zamalek Riverside Apartment\"]", new DateTime(2025, 12, 27, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6010L,
                columns: new[] { "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-01-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-31\"]", new DateTime(2026, 1, 29, 12, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6011L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-02-28.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 2, 28, 10, 0, 0, 0, DateTimeKind.Utc), "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\"]", new DateTime(2026, 2, 28, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6012L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-03-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-03-31\"]", new DateTime(2026, 4, 5, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6013L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-04-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-30\"]", new DateTime(2026, 5, 5, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "Data", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[,]
                {
                    { 6014L, null, 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-05-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-31\"]", new DateTime(2026, 6, 5, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6015L, null, null, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-30.", "NOTIFICATION_UPCOMING_PAYMENT_BODY", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"7\",\"2026-06-30\"]", null, "Upcoming Payment Available", "NOTIFICATION_UPCOMING_PAYMENT_TITLE", 10, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6101L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam\"]", new DateTime(2025, 1, 2, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Mariam!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("22222222-2222-2222-2222-222222222222"), 0 },
                    { 6102L, null, null, "The owner of \"Sheikh Zayed Luxury Villa\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6103L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-05-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\"]", new DateTime(2025, 5, 31, 10, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6104L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-06-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\"]", new DateTime(2025, 6, 30, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6105L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-07-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 8, 4, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-07-31\"]", new DateTime(2025, 8, 4, 9, 20, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6106L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-08-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 8, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-31\"]", new DateTime(2025, 8, 29, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6107L, null, null, "The owner of \"Agouza Shared House\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Agouza Shared House\"]", new DateTime(2026, 1, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6108L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-02-28.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 2, 22, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-02-28\"]", new DateTime(2026, 2, 22, 10, 12, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6109L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-03-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 3, 31, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-03-31\"]", new DateTime(2026, 3, 31, 11, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6110L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-04-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-04-30\"]", new DateTime(2026, 5, 8, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6111L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-05-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-05-31\"]", new DateTime(2026, 5, 31, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6112L, null, null, "Your payment of 4000 egp for \"Agouza Shared House\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-30.", "NOTIFICATION_UPCOMING_PAYMENT_BODY", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"7\",\"2026-06-30\"]", null, "Upcoming Payment Available", "NOTIFICATION_UPCOMING_PAYMENT_TITLE", 10, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6113L, null, null, "The owner of \"Dokki Modern Loft\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\"]", new DateTime(2025, 12, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6114L, null, 4, "An admin has cancelled contract #1000006 for \"Sheikh Zayed Luxury Villa\".", "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY", new DateTime(2026, 4, 26, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"1000006\",\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 26, 12, 30, 0, 0, DateTimeKind.Utc), "Contract Cancelled", "NOTIFICATION_CONTRACT_CANCELLED_TITLE", 7, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6201L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Ahmed\"]", new DateTime(2025, 1, 3, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Ahmed!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("33333333-3333-3333-3333-333333333333"), 0 },
                    { 6301L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sara\"]", new DateTime(2025, 1, 4, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Sara!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("77777777-7777-7777-7777-777777777777"), 0 },
                    { 6401L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Omar\"]", new DateTime(2025, 1, 5, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Omar!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("88888888-8888-8888-8888-888888888888"), 0 },
                    { 6402L, null, null, "The owner of \"Mohandeseen Studio Flat\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\"]", new DateTime(2025, 11, 28, 13, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("88888888-8888-8888-8888-888888888888"), 1 },
                    { 6501L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mahmoud\"]", new DateTime(2025, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Mahmoud!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("44444444-4444-4444-4444-444444444444"), 0 },
                    { 6502L, null, 5, "Your Stripe Connect account has been activated and is now ready to withdraw your payments.", "NOTIFICATION_CONNECT_SUCCESS_BODY", new DateTime(2025, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 1, 10, 12, 5, 0, 0, DateTimeKind.Utc), "Connect Account Activated", "NOTIFICATION_CONNECT_SUCCESS_TITLE", 17, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6503L, "1001", 1, "Your property \"Zamalek Riverside Apartment\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Zamalek Riverside Apartment\"]", new DateTime(2024, 2, 1, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6504L, "1002", 1, "Your property \"Dokki Modern Loft\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\"]", new DateTime(2023, 2, 2, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6505L, "1003", 1, "Your property \"Mohandeseen Studio Flat\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\"]", new DateTime(2025, 2, 3, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6506L, "1100", 1, "Your property \"Agouza Shared House\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Agouza Shared House\"]", new DateTime(2024, 2, 5, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6507L, null, null, "You have received a new booking request for \"Dokki Modern Loft\" from Karim Hassan.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\",\"Karim Hassan\"]", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6508L, null, null, "You have received a new booking request for \"Dokki Modern Loft\" from Karim Hassan.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\",\"Karim Hassan\"]", new DateTime(2025, 3, 10, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6509L, null, null, "You have received a new booking request for \"Mohandeseen Studio Flat\" from Mariam Fouad.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\",\"Mariam Fouad\"]", new DateTime(2025, 4, 2, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6510L, null, null, "You have received a new booking request for \"Mohandeseen Studio Flat\" from Tarek Owner.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\",\"Tarek Owner\"]", new DateTime(2025, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6511L, "1000005", 6, "The renter Karim Hassan has signed the contract for \"Dokki Modern Loft\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim Hassan\",\"Dokki Modern Loft\"]", new DateTime(2023, 12, 20, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6512L, "1000003", 6, "The renter Karim Hassan has signed the contract for \"Agouza Shared House\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim Hassan\",\"Agouza Shared House\"]", new DateTime(2025, 5, 25, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6513L, "1000102", 6, "The renter Omar Samir has signed the contract for \"Mohandeseen Studio Flat\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 11, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Omar Samir\",\"Mohandeseen Studio Flat\"]", new DateTime(2025, 11, 29, 11, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6514L, "1000001", 6, "The renter Karim Hassan has signed the contract for \"Zamalek Riverside Apartment\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim Hassan\",\"Zamalek Riverside Apartment\"]", new DateTime(2025, 12, 28, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6515L, "1000004", 6, "The renter Mariam Fouad has signed the contract for \"Agouza Shared House\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam Fouad\",\"Agouza Shared House\"]", new DateTime(2026, 1, 28, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6516L, "1000002", 6, "The renter Mariam Fouad has signed the contract for \"Dokki Modern Loft\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam Fouad\",\"Dokki Modern Loft\"]", new DateTime(2025, 12, 29, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6517L, "11111111-1111-1111-1111-111111111111", 2, "You have a new message from Karim Hassan", "NOTIFICATION_NEW_MESSAGE_BODY", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Utc), "{\"SenderId\":\"11111111-1111-1111-1111-111111111111\",\"SenderName\":\"Karim Hassan\",\"Content\":\"Hello Mahmoud! I am interested in your property.\"}", "[\"Karim Hassan\"]", new DateTime(2025, 3, 20, 10, 5, 0, 0, DateTimeKind.Utc), "New Message", "NOTIFICATION_NEW_MESSAGE_TITLE", 1, new Guid("44444444-4444-4444-4444-444444444444"), 0 },
                    { 6518L, "10000000-0000-0000-0000-000000000002", 2, "You have a new message from Sayed Banned", "NOTIFICATION_NEW_MESSAGE_BODY", new DateTime(2026, 4, 12, 19, 30, 0, 0, DateTimeKind.Utc), "{\"SenderId\":\"10000000-0000-0000-0000-000000000002\",\"SenderName\":\"Sayed Banned\",\"Content\":\"Can you lower the rent?\"}", "[\"Sayed Banned\"]", new DateTime(2026, 4, 12, 19, 35, 0, 0, DateTimeKind.Utc), "New Message", "NOTIFICATION_NEW_MESSAGE_TITLE", 1, new Guid("44444444-4444-4444-4444-444444444444"), 0 },
                    { 6519L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-03-31.\n\nYou can withdraw this amount after 2024-04-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-03-31\",\"2024-04-15\"]", new DateTime(2024, 4, 5, 14, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6520L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-04-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2024, 4, 15, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-04-05\"]", new DateTime(2024, 4, 15, 14, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6521L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-06-30.\n\nYou can withdraw this amount after 2024-07-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\",\"2024-07-10\"]", new DateTime(2024, 6, 30, 11, 5, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6522L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-06-30 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2024, 7, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\"]", new DateTime(2024, 7, 10, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6523L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-09-30.\n\nYou can withdraw this amount after 2024-10-13.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 10, 3, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-09-30\",\"2024-10-13\"]", new DateTime(2024, 10, 3, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6524L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-10-03 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2024, 10, 13, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-10-03\"]", new DateTime(2024, 10, 13, 10, 20, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6525L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-12-31.\n\nYou can withdraw this amount after 2025-01-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 12, 31, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\",\"2025-01-10\"]", new DateTime(2024, 12, 31, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6526L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-12-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\"]", new DateTime(2025, 1, 10, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6527L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-01-31.\n\nYou can withdraw this amount after 2026-02-08.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-31\",\"2026-02-08\"]", new DateTime(2026, 1, 29, 12, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6528L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-01-29 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 2, 8, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-29\"]", new DateTime(2026, 2, 8, 12, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6529L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-02-28.\n\nYou can withdraw this amount after 2026-03-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 2, 28, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\",\"2026-03-10\"]", new DateTime(2026, 2, 28, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6530L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-02-28 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\"]", new DateTime(2026, 3, 10, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6531L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-03-31.\n\nYou can withdraw this amount after 2026-04-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-03-31\",\"2026-04-15\"]", new DateTime(2026, 4, 5, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6532L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-04-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 4, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-05\"]", new DateTime(2026, 4, 15, 9, 20, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6533L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-04-30.\n\nYou can withdraw this amount after 2026-05-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-30\",\"2026-05-15\"]", new DateTime(2026, 5, 5, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6534L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-05-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 5, 15, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-05\"]", new DateTime(2026, 5, 15, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6535L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-05-31.\n\nYou can withdraw this amount after 2026-06-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-31\",\"2026-06-15\"]", new DateTime(2026, 6, 5, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6536L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-06-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 6, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-06-05\"]", new DateTime(2026, 6, 15, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6537L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-02-28.\n\nYou can withdraw this amount after 2026-03-04.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 2, 22, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-02-28\",\"2026-03-04\"]", new DateTime(2026, 2, 22, 10, 12, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6538L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-02-22 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 3, 4, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-02-22\"]", new DateTime(2026, 3, 4, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6539L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-03-31.\n\nYou can withdraw this amount after 2026-04-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 3, 31, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-03-31\",\"2026-04-10\"]", new DateTime(2026, 3, 31, 11, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6540L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-03-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 4, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-03-31\"]", new DateTime(2026, 4, 10, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6541L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-04-30.\n\nYou can withdraw this amount after 2026-05-18.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-04-30\",\"2026-05-18\"]", new DateTime(2026, 5, 8, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6542L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-05-08 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 5, 18, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-08\"]", new DateTime(2026, 5, 18, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6543L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-05-31.\n\nYou can withdraw this amount after 2026-06-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-31\",\"2026-06-10\"]", new DateTime(2026, 5, 31, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6544L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-05-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-31\"]", new DateTime(2026, 6, 10, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6601L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sherif\"]", new DateTime(2025, 1, 2, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Sherif!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("55555555-5555-5555-5555-555555555555"), 0 },
                    { 6602L, "1201", 1, "Your property \"Pending Downtown Apartment\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"Pending Downtown Apartment\"]", new DateTime(2026, 5, 3, 9, 30, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6603L, "1202", 1, "Your property \"Declined Garden House\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"Declined Garden House\"]", new DateTime(2026, 4, 18, 13, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6604L, "1203", 1, "Your property \"Soft Deleted Test Studio\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 3, 8, 16, 0, 0, 0, DateTimeKind.Utc), null, "[\"Soft Deleted Test Studio\"]", new DateTime(2026, 3, 8, 17, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6605L, "1203", 1, "Your property \"Soft Deleted Test Studio\" has been deleted.", "NOTIFICATION_PROPERTY_DELETED_BODY", new DateTime(2026, 4, 4, 13, 0, 0, 0, DateTimeKind.Utc), null, "[\"Soft Deleted Test Studio\"]", new DateTime(2026, 4, 4, 13, 10, 0, 0, DateTimeKind.Utc), "Property Deleted", "NOTIFICATION_PROPERTY_DELETED_TITLE", 23, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6606L, "1204", 1, "Your property \"Recent Marina Flat\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Recent Marina Flat\"]", new DateTime(2026, 5, 5, 10, 30, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6607L, "1205", 1, "Your property \"Moderated Riverside Villa\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 5, 7, 15, 0, 0, 0, DateTimeKind.Utc), null, "[\"Moderated Riverside Villa\"]", new DateTime(2026, 5, 7, 15, 30, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6608L, "1000103", 6, "The renter Sayed Banned has signed the contract for \"Moderated Riverside Villa\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2026, 5, 21, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sayed Banned\",\"Moderated Riverside Villa\"]", new DateTime(2026, 5, 21, 10, 30, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6701L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Tarek\"]", new DateTime(2025, 1, 3, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Tarek!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("66666666-6666-6666-6666-666666666666"), 0 },
                    { 6702L, null, 5, "Your Stripe Connect account has been activated and is now ready to withdraw your payments.", "NOTIFICATION_CONNECT_SUCCESS_BODY", new DateTime(2025, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 2, 10, 12, 5, 0, 0, DateTimeKind.Utc), "Connect Account Activated", "NOTIFICATION_CONNECT_SUCCESS_TITLE", 17, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6703L, "1004", 1, "Your property \"Sheikh Zayed Luxury Villa\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sheikh Zayed Luxury Villa\"]", new DateTime(2025, 2, 4, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6704L, "1000006", 6, "The renter Mariam Fouad has signed the contract for \"Sheikh Zayed Luxury Villa\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam Fouad\",\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 25, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6705L, null, 5, "An admin has cancelled contract #1000006 for \"Sheikh Zayed Luxury Villa\".", "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY", new DateTime(2026, 4, 26, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"1000006\",\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 26, 12, 30, 0, 0, DateTimeKind.Utc), "Contract Cancelled", "NOTIFICATION_CONTRACT_CANCELLED_TITLE", 7, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6706L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-05-31.\n\nYou can withdraw this amount after 2025-06-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\",\"2025-06-10\"]", new DateTime(2025, 5, 31, 10, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6707L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-05-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\"]", new DateTime(2025, 6, 10, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6708L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-06-30.\n\nYou can withdraw this amount after 2025-07-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\",\"2025-07-10\"]", new DateTime(2025, 6, 30, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6709L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-06-30 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 7, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\"]", new DateTime(2025, 7, 10, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6710L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-07-31.\n\nYou can withdraw this amount after 2025-08-14.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 8, 4, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-07-31\",\"2025-08-14\"]", new DateTime(2025, 8, 4, 9, 20, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6711L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-08-04 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 8, 14, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-04\"]", new DateTime(2025, 8, 14, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6712L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-08-31.\n\nYou can withdraw this amount after 2025-09-08.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 8, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-31\",\"2025-09-08\"]", new DateTime(2025, 8, 29, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6713L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-08-29 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 9, 8, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-29\"]", new DateTime(2025, 9, 8, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6801L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Khaled\"]", new DateTime(2026, 5, 10, 10, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Khaled!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000001"), 0 },
                    { 6802L, null, null, "Your profile has been updated successfully. Our team will review your information, and your account is expected to be verified within approximately 24 hours.\n\nOnce verified, you’ll be able to start renting properties, listing your own, and connecting with compatible roommates.", "NOTIFICATION_PROFILE_UPDATED_BODY", new DateTime(2026, 5, 10, 10, 5, 0, 0, DateTimeKind.Utc), null, null, null, "Profile Updated Successfully!", "NOTIFICATION_PROFILE_UPDATED_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000001"), 0 },
                    { 6901L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 3, 5, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sayed\"]", new DateTime(2026, 3, 5, 14, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Sayed!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000002"), 0 },
                    { 6902L, null, null, "An admin has banned your account. You can no longer use MARN until the ban is removed. If you believe this is a mistake, please contact support.", "NOTIFICATION_ACCOUNT_BANNED_BODY", new DateTime(2026, 4, 13, 9, 1, 0, 0, DateTimeKind.Utc), null, null, null, "Account Banned", "NOTIFICATION_ACCOUNT_BANNED_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000002"), 0 },
                    { 6903L, null, null, "An admin has banned your account. You can no longer use MARN until the ban is removed. If you believe this is a mistake, please contact support.", "NOTIFICATION_ACCOUNT_BANNED_BODY", new DateTime(2026, 4, 14, 12, 1, 0, 0, DateTimeKind.Utc), null, null, null, "Account Banned", "NOTIFICATION_ACCOUNT_BANNED_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000002"), 0 },
                    { 6904L, null, null, "The owner of \"Moderated Riverside Villa\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 5, 20, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"Moderated Riverside Villa\"]", new DateTime(2026, 5, 20, 12, 30, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("10000000-0000-0000-0000-000000000002"), 1 },
                    { 6905L, null, null, "Your payment of 5000 egp for \"Moderated Riverside Villa\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-23.", "NOTIFICATION_UPCOMING_PAYMENT_BODY", new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Moderated Riverside Villa\",\"7\",\"2026-06-23\"]", null, "Upcoming Payment Available", "NOTIFICATION_UPCOMING_PAYMENT_TITLE", 10, new Guid("10000000-0000-0000-0000-000000000002"), 1 },
                    { 7001L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 2, 20, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"Ramy\"]", new DateTime(2026, 2, 20, 11, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Ramy!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000003"), 0 },
                    { 7101L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 5, 25, 16, 0, 0, 0, DateTimeKind.Utc), null, "[\"Nour\"]", new DateTime(2026, 5, 25, 16, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Nour!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000004"), 0 },
                    { 7102L, null, null, "The owner of \"Recent Marina Flat\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 5, 26, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Recent Marina Flat\"]", null, "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("10000000-0000-0000-0000-000000000004"), 1 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "ImagesDeletionJob", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[,]
                {
                    { 3001L, "Dummy Address 1", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 1", null, true, false, 30.010000000000002, 31.010000000000002, 2, new Guid("44444444-4444-4444-4444-444444444444"), 1000m, "/images/documents/dummy.jpg", 1, 80.0, "CairoGovernorate", 1, "Dummy Property 1", 0, 0, "11111" },
                    { 3002L, "Dummy Address 2", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 2", null, true, false, 30.02, 31.02, 1, new Guid("44444444-4444-4444-4444-444444444444"), 1500m, "/images/documents/dummy.jpg", 1, 50.0, "GizaGovernorate", 1, "Dummy Property 2", 4, 0, "11112" },
                    { 3003L, "Dummy Address 3", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 3", null, true, false, 30.030000000000001, 31.030000000000001, 4, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, "/images/documents/dummy.jpg", 1, 150.0, "AlexandriaGovernorate", 1, "Dummy Property 3", 1, 0, "11113" },
                    { 3004L, "Dummy Address 4", 3, 4, 5, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 4", null, true, false, 30.039999999999999, 31.039999999999999, 6, new Guid("44444444-4444-4444-4444-444444444444"), 10000m, "/images/documents/dummy.jpg", 1, 250.0, "SharkiaGovernorate", 1, "Dummy Property 4", 3, 0, "11114" },
                    { 3005L, "Dummy Address 5", 1, 1, 2, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 5", null, true, true, 30.050000000000001, 31.050000000000001, 2, new Guid("44444444-4444-4444-4444-444444444444"), 800m, "/images/documents/dummy.jpg", 1, 60.0, "DamiettaGovernorate", 1, "Dummy Property 5", 5, 0, "11115" },
                    { 3006L, "Dummy Address 6", 1, 2, 3, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 6", null, true, false, 30.059999999999999, 31.059999999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 1200m, "/images/documents/dummy.jpg", 1, 90.0, "CairoGovernorate", 1, "Dummy Property 6", 0, 0, "11116" },
                    { 3007L, "Dummy Address 7", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 7", null, true, false, 30.07, 31.07, 1, new Guid("44444444-4444-4444-4444-444444444444"), 1100m, "/images/documents/dummy.jpg", 1, 45.0, "GizaGovernorate", 1, "Dummy Property 7", 4, 0, "11117" },
                    { 3008L, "Dummy Address 8", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 8", null, true, false, 30.079999999999998, 31.079999999999998, 5, new Guid("44444444-4444-4444-4444-444444444444"), 4800m, "/images/documents/dummy.jpg", 1, 160.0, "AlexandriaGovernorate", 1, "Dummy Property 8", 1, 0, "11118" },
                    { 3009L, "Dummy Address 9", 4, 5, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 9", null, true, false, 30.09, 31.09, 8, new Guid("44444444-4444-4444-4444-444444444444"), 15000m, "/images/documents/dummy.jpg", 1, 300.0, "SharkiaGovernorate", 1, "Dummy Property 9", 3, 0, "11119" },
                    { 3010L, "Dummy Address 10", 1, 1, 3, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 10", null, true, true, 30.100000000000001, 31.100000000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 900m, "/images/documents/dummy.jpg", 1, 70.0, "DamiettaGovernorate", 1, "Dummy Property 10", 5, 0, "11120" },
                    { 3011L, "Dummy Address 11", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 11", null, true, false, 30.109999999999999, 31.109999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 1300m, "/images/documents/dummy.jpg", 1, 85.0, "CairoGovernorate", 1, "Dummy Property 11", 0, 0, "11121" },
                    { 3012L, "Dummy Address 12", 1, 1, 2, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 12", null, true, false, 30.120000000000001, 31.120000000000001, 2, new Guid("55555555-5555-5555-5555-555555555555"), 1600m, "/images/documents/dummy.jpg", 1, 55.0, "GizaGovernorate", 1, "Dummy Property 12", 4, 0, "11122" },
                    { 3013L, "Dummy Address 13", 2, 3, 5, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 13", null, true, false, 30.129999999999999, 31.129999999999999, 6, new Guid("55555555-5555-5555-5555-555555555555"), 5200m, "/images/documents/dummy.jpg", 1, 170.0, "AlexandriaGovernorate", 1, "Dummy Property 13", 1, 0, "11123" },
                    { 3014L, "Dummy Address 14", 4, 4, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 14", null, true, false, 30.140000000000001, 31.140000000000001, 8, new Guid("55555555-5555-5555-5555-555555555555"), 12000m, "/images/documents/dummy.jpg", 1, 280.0, "SharkiaGovernorate", 1, "Dummy Property 14", 3, 0, "11124" },
                    { 3015L, "Dummy Address 15", 1, 1, 2, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 15", null, true, true, 30.149999999999999, 31.149999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 850m, "/images/documents/dummy.jpg", 1, 65.0, "DamiettaGovernorate", 1, "Dummy Property 15", 5, 0, "11125" },
                    { 3016L, "Dummy Address 16", 2, 3, 4, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 16", null, true, false, 30.16, 31.16, 4, new Guid("55555555-5555-5555-5555-555555555555"), 2500m, "/images/documents/dummy.jpg", 1, 120.0, "CairoGovernorate", 1, "Dummy Property 16", 0, 0, "11126" },
                    { 3017L, "Dummy Address 17", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 17", null, true, false, 30.170000000000002, 31.170000000000002, 1, new Guid("55555555-5555-5555-5555-555555555555"), 1000m, "/images/documents/dummy.jpg", 1, 40.0, "GizaGovernorate", 1, "Dummy Property 17", 4, 0, "11127" },
                    { 3018L, "Dummy Address 18", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 18", null, true, false, 30.18, 31.18, 5, new Guid("55555555-5555-5555-5555-555555555555"), 4600m, "/images/documents/dummy.jpg", 1, 155.0, "AlexandriaGovernorate", 1, "Dummy Property 18", 1, 0, "11128" },
                    { 3019L, "Dummy Address 19", 5, 5, 8, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 19", null, true, false, 30.190000000000001, 31.190000000000001, 10, new Guid("55555555-5555-5555-5555-555555555555"), 18000m, "/images/documents/dummy.jpg", 1, 350.0, "SharkiaGovernorate", 1, "Dummy Property 19", 3, 0, "11129" },
                    { 3020L, "Dummy Address 20", 2, 2, 4, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 20", null, true, true, 30.199999999999999, 31.199999999999999, 4, new Guid("55555555-5555-5555-5555-555555555555"), 950m, "/images/documents/dummy.jpg", 1, 80.0, "DamiettaGovernorate", 1, "Dummy Property 20", 5, 0, "11130" },
                    { 3021L, "Dummy Address 21", 1, 2, 3, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 21", null, true, false, 30.210000000000001, 31.210000000000001, 3, new Guid("66666666-6666-6666-6666-666666666666"), 1800m, "/images/documents/dummy.jpg", 1, 95.0, "CairoGovernorate", 1, "Dummy Property 21", 0, 0, "11131" },
                    { 3022L, "Dummy Address 22", 1, 1, 2, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 22", null, true, false, 30.219999999999999, 31.219999999999999, 2, new Guid("66666666-6666-6666-6666-666666666666"), 1400m, "/images/documents/dummy.jpg", 1, 48.0, "GizaGovernorate", 1, "Dummy Property 22", 4, 0, "11132" },
                    { 3023L, "Dummy Address 23", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 23", null, true, false, 30.23, 31.23, 5, new Guid("66666666-6666-6666-6666-666666666666"), 4900m, "/images/documents/dummy.jpg", 1, 165.0, "AlexandriaGovernorate", 1, "Dummy Property 23", 1, 0, "11133" },
                    { 3024L, "Dummy Address 24", 3, 4, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 24", null, true, false, 30.239999999999998, 31.239999999999998, 8, new Guid("66666666-6666-6666-6666-666666666666"), 11000m, "/images/documents/dummy.jpg", 1, 270.0, "SharkiaGovernorate", 1, "Dummy Property 24", 3, 0, "11134" },
                    { 3025L, "Dummy Address 25", 1, 2, 3, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 25", null, true, true, 30.25, 31.25, 3, new Guid("66666666-6666-6666-6666-666666666666"), 880m, "/images/documents/dummy.jpg", 1, 72.0, "DamiettaGovernorate", 1, "Dummy Property 25", 5, 0, "11135" },
                    { 3026L, "Dummy Address 26", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 26", null, true, false, 30.260000000000002, 31.260000000000002, 2, new Guid("66666666-6666-6666-6666-666666666666"), 1700m, "/images/documents/dummy.jpg", 1, 78.0, "CairoGovernorate", 1, "Dummy Property 26", 0, 0, "11136" },
                    { 3027L, "Dummy Address 27", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 27", null, true, false, 30.27, 31.27, 1, new Guid("66666666-6666-6666-6666-666666666666"), 1250m, "/images/documents/dummy.jpg", 1, 42.0, "GizaGovernorate", 1, "Dummy Property 27", 4, 0, "11137" },
                    { 3028L, "Dummy Address 28", 2, 3, 5, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 28", null, true, false, 30.280000000000001, 31.280000000000001, 6, new Guid("66666666-6666-6666-6666-666666666666"), 5500m, "/images/documents/dummy.jpg", 1, 180.0, "AlexandriaGovernorate", 1, "Dummy Property 28", 1, 0, "11138" },
                    { 3029L, "Dummy Address 29", 6, 6, 10, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 29", null, true, false, 30.289999999999999, 31.289999999999999, 12, new Guid("66666666-6666-6666-6666-666666666666"), 22000m, "/images/documents/dummy.jpg", 1, 400.0, "SharkiaGovernorate", 1, "Dummy Property 29", 3, 0, "11139" },
                    { 3030L, "Dummy Address 30", 2, 2, 4, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 30", null, true, true, 30.300000000000001, 31.300000000000001, 4, new Guid("66666666-6666-6666-6666-666666666666"), 920m, "/images/documents/dummy.jpg", 1, 85.0, "DamiettaGovernorate", 1, "Dummy Property 30", 5, 0, "11140" }
                });

            migrationBuilder.InsertData(
                table: "PropertyAmenities",
                columns: new[] { "Id", "Amenity", "PropertyId" },
                values: new object[,]
                {
                    { 14L, 0, 1100L },
                    { 15L, 8, 1100L },
                    { 16L, 12, 1100L },
                    { 17L, 14, 1100L },
                    { 18L, 0, 2001L },
                    { 19L, 2, 2001L },
                    { 20L, 8, 2001L },
                    { 21L, 11, 2001L },
                    { 22L, 14, 2001L },
                    { 23L, 5, 2001L },
                    { 24L, 0, 2002L },
                    { 25L, 2, 2002L },
                    { 26L, 8, 2002L },
                    { 27L, 11, 2002L },
                    { 28L, 12, 2002L },
                    { 29L, 14, 2002L },
                    { 30L, 0, 2003L },
                    { 31L, 2, 2003L },
                    { 32L, 8, 2003L },
                    { 33L, 4, 2003L },
                    { 34L, 10, 2003L },
                    { 35L, 0, 2004L },
                    { 36L, 2, 2004L },
                    { 37L, 8, 2004L },
                    { 38L, 12, 2004L },
                    { 39L, 0, 2005L },
                    { 40L, 2, 2005L },
                    { 41L, 6, 2005L },
                    { 42L, 7, 2005L },
                    { 43L, 1, 2005L },
                    { 44L, 17, 2005L },
                    { 45L, 8, 2005L },
                    { 46L, 0, 2006L },
                    { 47L, 2, 2006L },
                    { 48L, 14, 2006L },
                    { 49L, 11, 2006L },
                    { 50L, 4, 2006L },
                    { 51L, 0, 2007L },
                    { 52L, 2, 2007L },
                    { 53L, 8, 2007L },
                    { 54L, 5, 2007L },
                    { 55L, 14, 2007L },
                    { 56L, 0, 2008L },
                    { 57L, 2, 2008L },
                    { 58L, 8, 2008L },
                    { 59L, 1, 2008L },
                    { 60L, 17, 2008L },
                    { 61L, 0, 2009L },
                    { 62L, 2, 2009L },
                    { 63L, 8, 2009L },
                    { 64L, 14, 2009L },
                    { 65L, 11, 2009L },
                    { 66L, 0, 2010L },
                    { 67L, 2, 2010L },
                    { 68L, 6, 2010L },
                    { 69L, 1, 2010L },
                    { 70L, 14, 2010L },
                    { 71L, 17, 2010L },
                    { 72L, 0, 2011L },
                    { 73L, 8, 2011L },
                    { 74L, 11, 2011L },
                    { 75L, 14, 2011L },
                    { 76L, 0, 2012L },
                    { 77L, 2, 2012L },
                    { 78L, 8, 2012L },
                    { 79L, 11, 2012L },
                    { 80L, 4, 2012L },
                    { 81L, 0, 2013L },
                    { 82L, 8, 2013L },
                    { 83L, 11, 2013L },
                    { 84L, 1, 2013L },
                    { 85L, 0, 2014L },
                    { 86L, 8, 2014L },
                    { 87L, 12, 2014L },
                    { 88L, 11, 2014L },
                    { 89L, 0, 2015L },
                    { 90L, 8, 2015L },
                    { 91L, 14, 2015L },
                    { 92L, 0, 2016L },
                    { 93L, 2, 2016L },
                    { 94L, 8, 2016L },
                    { 95L, 5, 2016L },
                    { 96L, 0, 2017L },
                    { 97L, 14, 2017L },
                    { 98L, 8, 2017L },
                    { 99L, 11, 2017L },
                    { 100L, 0, 2018L },
                    { 101L, 2, 2018L },
                    { 102L, 8, 2018L },
                    { 103L, 14, 2018L },
                    { 104L, 0, 2019L },
                    { 105L, 8, 2019L },
                    { 106L, 11, 2019L },
                    { 107L, 1, 2019L },
                    { 108L, 0, 2020L },
                    { 109L, 8, 2020L },
                    { 110L, 11, 2020L },
                    { 111L, 14, 2020L },
                    { 112L, 1, 2020L },
                    { 113L, 0, 1201L },
                    { 114L, 8, 1201L },
                    { 115L, 5, 1201L },
                    { 116L, 14, 1201L },
                    { 117L, 0, 1202L },
                    { 118L, 8, 1202L },
                    { 119L, 1, 1202L },
                    { 120L, 14, 1202L },
                    { 121L, 0, 1203L },
                    { 122L, 8, 1203L },
                    { 123L, 2, 1203L },
                    { 124L, 0, 1204L },
                    { 125L, 2, 1204L },
                    { 126L, 14, 1204L },
                    { 127L, 11, 1204L },
                    { 128L, 1, 1204L },
                    { 129L, 17, 1204L },
                    { 130L, 0, 1205L },
                    { 131L, 2, 1205L },
                    { 132L, 6, 1205L },
                    { 133L, 1, 1205L },
                    { 134L, 17, 1205L },
                    { 135L, 8, 1205L }
                });

            migrationBuilder.InsertData(
                table: "PropertyRules",
                columns: new[] { "Id", "PropertyId", "Rule" },
                values: new object[,]
                {
                    { 7L, 1100L, "Keep shared spaces clean" },
                    { 8L, 1100L, "No guests overnight without roommate approval" },
                    { 9L, 2001L, "Annual maintenance fees are split" },
                    { 10L, 2001L, "Quiet hours after 10 PM" },
                    { 11L, 2002L, "Families only" },
                    { 12L, 2002L, "Small pets allowed with prior consent" },
                    { 13L, 2003L, "Turn off AC when leaving the studio" },
                    { 14L, 2003L, "Checkout is at 12 PM" },
                    { 15L, 2004L, "Share chores weekly" },
                    { 16L, 2004L, "No smoking indoors" },
                    { 17L, 2005L, "Maintain the garden weekly" },
                    { 18L, 2005L, "No events or commercial filming" },
                    { 19L, 2006L, "Clean feet from sand before entering" },
                    { 20L, 2006L, "No loud music on balcony" },
                    { 21L, 2007L, "Maximum of 4 overnight occupants" },
                    { 22L, 2007L, "Inform owner before having visitors" },
                    { 23L, 2008L, "Respect neighbors' parking spaces" },
                    { 24L, 2008L, "No sub-leasing allowed" },
                    { 25L, 2009L, "No smoking in the studio" },
                    { 26L, 2009L, "Check-out by 11 AM" },
                    { 27L, 2010L, "Pool usage only until 8 PM" },
                    { 28L, 2010L, "No loud outdoor activities late at night" },
                    { 29L, 2011L, "Quiet hours after 10 PM" },
                    { 30L, 2012L, "Commercial use of the loft is prohibited" },
                    { 31L, 2012L, "Ideal for students or professionals" },
                    { 32L, 2013L, "No modification to the courtyard structure" },
                    { 33L, 2013L, "Pets allowed in the courtyard only" },
                    { 34L, 2014L, "Shared kitchen duties should be respected" },
                    { 35L, 2014L, "No loud gatherings" },
                    { 36L, 2015L, "Do not leave water taps running" },
                    { 37L, 2016L, "Daily trash disposal is required" },
                    { 38L, 2016L, "Turn off air conditioning when out" },
                    { 39L, 2017L, "Beach wear not allowed inside the living room" },
                    { 40L, 2017L, "Maximum 2 occupants" },
                    { 41L, 2018L, "Key return to the doorman on check-out" },
                    { 42L, 2018L, "No loud parties" },
                    { 43L, 2019L, "Respect local residential rules" },
                    { 44L, 2020L, "No smoking inside the house" },
                    { 45L, 2020L, "No pets allowed" },
                    { 46L, 1201L, "No smoking inside" },
                    { 47L, 1201L, "Respect the historic building rules" },
                    { 48L, 1202L, "Keep the garden area tidy" },
                    { 49L, 1202L, "No noisy gatherings after midnight" },
                    { 50L, 1203L, "For single occupants only" },
                    { 51L, 1204L, "Beach access cards must not be shared" },
                    { 52L, 1204L, "No pets" },
                    { 53L, 1205L, "Pool rules must be strictly followed" },
                    { 54L, 1205L, "Respect neighbors' privacy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6014L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6015L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6101L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6102L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6103L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6104L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6105L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6106L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6107L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6108L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6109L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6110L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6111L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6112L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6113L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6114L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6201L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6301L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6401L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6402L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6501L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6502L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6503L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6504L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6505L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6506L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6507L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6508L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6509L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6510L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6511L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6512L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6513L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6514L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6515L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6516L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6517L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6518L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6519L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6520L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6521L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6522L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6523L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6524L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6525L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6526L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6527L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6528L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6529L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6530L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6531L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6532L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6533L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6534L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6535L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6536L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6537L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6538L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6539L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6540L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6541L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6542L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6543L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6544L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6601L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6602L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6603L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6604L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6605L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6606L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6607L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6608L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6701L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6702L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6703L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6704L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6705L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6706L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6707L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6708L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6709L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6710L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6711L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6712L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6713L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6801L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6802L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6901L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6902L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6903L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6904L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6905L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 7001L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 7101L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 7102L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3001L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3002L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3003L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3004L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3005L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3006L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3007L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3008L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3009L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3010L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3011L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3012L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3013L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3014L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3015L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3016L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3017L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3018L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3019L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3020L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3021L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3022L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3023L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3024L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3025L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3026L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3027L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3028L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3029L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3030L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 55L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 57L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 60L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 61L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 62L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 63L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 64L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 65L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 66L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 67L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 68L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 69L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 70L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 73L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 74L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 75L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 76L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 77L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 78L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 80L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 81L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 82L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 83L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 84L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 85L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 86L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 88L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 89L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 90L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 91L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 94L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 96L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 97L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 98L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 99L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 106L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 107L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 108L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 109L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 110L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 111L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 112L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 113L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 114L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 115L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 116L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 117L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 118L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 119L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 120L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 121L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 122L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 123L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 124L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 125L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 126L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 127L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 128L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 129L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 130L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 131L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 132L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 133L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 134L);

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 135L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "Data", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserType" },
                values: new object[] { 4, "Your next rent payment is due soon.", null, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), "{\"propertyName\":\"Cozy Seed Apartment\"}", null, null, "Upcoming Payment Due", null, 10, 1 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6002L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey" },
                values: new object[] { 4, "Your booking request has been accepted.", null, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Request Update", null });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6003L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type" },
                values: new object[] { null, "Thanks for signing up!", null, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to the platform", null, 0 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6004L,
                columns: new[] { "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type" },
                values: new object[] { "44444444-4444-4444-4444-444444444444", 2, "You have a new message from the owner.", null, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "New Message", null, 1 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6005L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { 3, "Add more details to your profile to get better recommendations.", null, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Complete Your Profile", null, 0, new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6006L,
                columns: new[] { "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { "1002", 1, "A renter submitted a booking request for one of your properties.", null, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "New booking request", null, 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L,
                columns: new[] { "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "Data", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { null, 5, "A rent payment was successfully processed.", null, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), "{\"amount\":\"1200\", \"currency\":\"USD\"}", null, null, "Payment received", null, 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6008L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { 3, "Complete your listing details to attract more renters.", null, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", null, 0, new Guid("44444444-4444-4444-4444-444444444444"), 2 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { 4, "Your next rent payment for Cozy Seed Apartment is due soon.", null, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Rent Payment Due Soon", null, 10, new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6010L,
                columns: new[] { "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { "Your booking request for Seed Studio Flat has been submitted.", null, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Submitted", null, 6, new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6011L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId" },
                values: new object[] { null, "Thanks for joining MARN! Explore properties near you.", null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to MARN", null, 0, new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6012L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { 5, "Luxury Seed Villa is now visible to renters.", null, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Your property is live", null, 0, new Guid("66666666-6666-6666-6666-666666666666"), 2 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6013L,
                columns: new[] { "ActionType", "Body", "BodyKey", "CreatedAt", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[] { 3, "Set up your payout details to start receiving rent payments.", null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", null, 0, new Guid("66666666-6666-6666-6666-666666666666"), 2 });
        }
    }
}
