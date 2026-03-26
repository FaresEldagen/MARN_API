using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class OwnerDashboardSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), 0, 2, null, null, null, "SEED-OWNER-Z-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.z@example.com", true, "Owner", null, 1, 0, "Z", false, null, null, "OWNER.Z@EXAMPLE.COM", "OWNER.Z@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Z-SECURITY-STAMP", false, "owner.z@example.com" });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Body", "CreatedAt", "Data", "ReadAt", "Title", "Type", "UserId", "UserType" },
                values: new object[,]
                {
                    { 6006L, "A renter submitted a booking request for one of your properties.", new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "New booking request", 0, new Guid("44444444-4444-4444-4444-444444444444"), 1 },
                    { 6007L, "A rent payment was successfully processed.", new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Payment received", 0, new Guid("44444444-4444-4444-4444-444444444444"), 1 },
                    { 6008L, "Complete your listing details to attract more renters.", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", 0, new Guid("44444444-4444-4444-4444-444444444444"), 1 }
                });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4002L,
                column: "AvailableAt",
                value: new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4003L,
                column: "AvailableAt",
                value: new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4004L,
                column: "AvailableAt",
                value: new DateTime(2035, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("66666666-6666-6666-6666-666666666666") }
                });

            migrationBuilder.InsertData(
                table: "BookingRequests",
                columns: new[] { "Id", "CreatedAt", "EndDate", "PropertyId", "RenterId", "StartDate", "Status" },
                values: new object[] { 5004L, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1003L, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "CancellationReason", "CancelledAt", "CreatedAt", "DocumentHash", "DocumentPath", "EndDate", "IPAddress", "IsLocked", "OwnerId", "OwnerSignature", "PaymentFrequency", "PropertyId", "RenterId", "RenterSignature", "SignedByOwnerAt", "SignedByRenterAt", "StartDate", "Status", "UpdatedAt", "Version" },
                values: new object[] { 3004L, null, null, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-4-HASH", "/contracts/seed/contract4.pdf", new DateTime(2027, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), "OwnerX-Signature", 1, 1001L, new Guid("66666666-6666-6666-6666-666666666666"), "OwnerZ-AsRenter-Signature", null, null, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Body", "CreatedAt", "Data", "ReadAt", "Title", "Type", "UserId", "UserType" },
                values: new object[,]
                {
                    { 6009L, "Your next rent payment for Cozy Seed Apartment is due soon.", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Rent Payment Due Soon", 0, new Guid("66666666-6666-6666-6666-666666666666"), 0 },
                    { 6010L, "Your booking request for Seed Studio Flat has been submitted.", new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Submitted", 0, new Guid("66666666-6666-6666-6666-666666666666"), 0 },
                    { 6011L, "Thanks for joining MARN! Explore properties near you.", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to MARN", 0, new Guid("66666666-6666-6666-6666-666666666666"), 0 },
                    { 6012L, "Luxury Seed Villa is now visible to renters.", new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Your property is live", 0, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6013L, "Set up your payout details to start receiving rent payments.", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", 0, new Guid("66666666-6666-6666-6666-666666666666"), 1 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Availability", "AverageRating", "Bathrooms", "Bedrooms", "Beds", "CreatedAt", "DeletedAt", "Description", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "RentalUnit", "Status", "Title", "Type", "Views" },
                values: new object[] { 1004L, "321 Elite Boulevard, Cairo", 0, 4.8f, 3, 4, 5, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "A luxury villa owned by the dual-role Owner Z for owner dashboard testing.", true, false, 30.07, 31.25, 6, new Guid("66666666-6666-6666-6666-666666666666"), 15000m, 1, 1, "Luxury Seed Villa", 3, 12 });

            migrationBuilder.InsertData(
                table: "SavedProperties",
                columns: new[] { "PropertyId", "UserId" },
                values: new object[,]
                {
                    { 1001L, new Guid("66666666-6666-6666-6666-666666666666") },
                    { 1002L, new Guid("66666666-6666-6666-6666-666666666666") }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AvailableAt", "ContractId", "CreatedAt", "Currency", "DueDate", "OwnerAmount", "PaidAt", "PlatformFee", "Status", "StripePaymentIntentId", "TotalAmount" },
                values: new object[,]
                {
                    { 4005L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3004L, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4500m, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), 500m, 1, null, 5000m },
                    { 4006L, null, 3004L, new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4500m, null, 500m, 0, null, 5000m }
                });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[] { 2004L, true, "/images/seed/property4-main.jpg", 1004L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6006L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6008L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6010L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6011L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6012L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6013L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4005L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4006L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2004L);

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1001L, new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1002L, new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3004L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4002L,
                column: "AvailableAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4003L,
                column: "AvailableAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4004L,
                column: "AvailableAt",
                value: null);
        }
    }
}
