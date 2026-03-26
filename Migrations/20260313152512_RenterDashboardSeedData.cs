using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class RenterDashboardSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1114503b-92fc-4768-9635-535bef7cdc95"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2d0929b0-d45f-4c1e-ade1-c0dc32656ea1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("997ff7b2-6752-4e75-a177-086a4bf5a68c"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Renter", "RENTER" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Owner", "OWNER" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, 2, null, null, null, "SEED-RENTER-A-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.a@example.com", true, "Renter", null, 1, 0, "Alpha", false, null, null, "RENTER.A@EXAMPLE.COM", "RENTER.A@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-A-SECURITY-STAMP", false, "renter.a@example.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, 2, null, null, null, "SEED-RENTER-B-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.b@example.com", true, "Renter", null, 2, 0, "Beta", false, null, null, "RENTER.B@EXAMPLE.COM", "RENTER.B@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-B-SECURITY-STAMP", false, "renter.b@example.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, 2, null, null, null, "SEED-RENTER-C-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.c@example.com", true, "Renter", null, 1, 0, "Gamma", false, null, null, "RENTER.C@EXAMPLE.COM", "RENTER.C@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-C-SECURITY-STAMP", false, "renter.c@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName", "WithdrawableEarnings" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, 2, null, null, null, "SEED-OWNER-X-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.x@example.com", true, "Owner", null, 1, 0, "X", false, null, null, "OWNER.X@EXAMPLE.COM", "OWNER.X@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-X-SECURITY-STAMP", false, "owner.x@example.com", 10000m },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, 2, null, null, null, "SEED-OWNER-Y-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.y@example.com", true, "Owner", null, 2, 0, "Y", false, null, null, "OWNER.Y@EXAMPLE.COM", "OWNER.Y@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Y-SECURITY-STAMP", false, "owner.y@example.com", 0m }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Body", "CreatedAt", "Data", "ReadAt", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 6001L, "Your next rent payment is due soon.", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Upcoming Payment Due", 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 6002L, "Your booking request has been accepted.", new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Request Update", 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 6003L, "Thanks for signing up!", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to the platform", 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 6004L, "Your booking request is pending owner approval.", new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Pending", 0, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 6005L, "Add more details to your profile to get better recommendations.", new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Complete Your Profile", 0, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Availability", "AverageRating", "Bathrooms", "Bedrooms", "Beds", "CreatedAt", "DeletedAt", "Description", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "RentalUnit", "Status", "Title", "Type", "Views" },
                values: new object[,]
                {
                    { 1001L, "123 Seed Street, Cairo", 0, 4.5f, 1, 2, 3, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A cozy seeded apartment suitable for testing active rentals.", true, false, 30.0444, 31.235700000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, 1, 1, "Cozy Seed Apartment", 0, 5 },
                    { 1002L, "456 Integration Avenue, Cairo", 0, 4f, 1, 1, 1, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "A modern loft used for pending booking and payments tests.", true, false, 30.050000000000001, 31.239999999999998, 2, new Guid("44444444-4444-4444-4444-444444444444"), 7500m, 1, 1, "Modern Seed Loft", 0, 3 },
                    { 1003L, "789 Scenario Road, Cairo", 0, 3.8f, 1, 1, 1, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "A small studio property used for saved properties and pending bookings.", true, false, 30.059999999999999, 31.245000000000001, 1, new Guid("44444444-4444-4444-4444-444444444444"), 3500m, 1, 1, "Seed Studio Flat", 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "BookingRequests",
                columns: new[] { "Id", "CreatedAt", "EndDate", "PropertyId", "RenterId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 5001L, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 5002L, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5003L, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1003L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "CancellationReason", "CancelledAt", "CreatedAt", "DocumentHash", "DocumentPath", "EndDate", "IPAddress", "IsLocked", "OwnerId", "OwnerSignature", "PaymentFrequency", "PropertyId", "RenterId", "RenterSignature", "SignedByOwnerAt", "SignedByRenterAt", "StartDate", "Status", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { 3001L, null, null, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-1-HASH", "/contracts/seed/contract1.pdf", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), "OwnerX-Signature", 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), "RenterA-Signature", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1 },
                    { 3002L, null, null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-2-HASH", "/contracts/seed/contract2.pdf", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), "OwnerX-Signature", 1, 1002L, new Guid("33333333-3333-3333-3333-333333333333"), "RenterC-Signature", null, null, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1 },
                    { 3003L, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-3-HASH", "/contracts/seed/contract3.pdf", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), null, 1, 1003L, new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 2001L, true, "/images/seed/property1-main.jpg", 1001L },
                    { 2002L, true, "/images/seed/property2-main.jpg", 1002L },
                    { 2003L, true, "/images/seed/property3-main.jpg", 1003L }
                });

            migrationBuilder.InsertData(
                table: "SavedProperties",
                columns: new[] { "PropertyId", "UserId" },
                values: new object[,]
                {
                    { 1001L, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 1002L, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 1003L, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "ContractId", "CreatedAt", "Currency", "DueDate", "PaidAt", "Status", "StripePaymentIntentId" },
                values: new object[,]
                {
                    { 4001L, 5000m, 3001L, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, null },
                    { 4002L, 5000m, 3001L, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, null },
                    { 4003L, 7500m, 3002L, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, null },
                    { 4004L, 7500m, 3002L, new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3003L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6002L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6003L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6004L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6005L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4001L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4002L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4003L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4004L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2001L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2002L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2003L);

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1001L, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1002L, new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1003L, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3001L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3002L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1114503b-92fc-4768-9635-535bef7cdc95"), null, "Renter", "RENTER" },
                    { new Guid("2d0929b0-d45f-4c1e-ade1-c0dc32656ea1"), null, "Admin", "ADMIN" },
                    { new Guid("997ff7b2-6752-4e75-a177-086a4bf5a68c"), null, "Owner", "OWNER" }
                });
        }
    }
}
