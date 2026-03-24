using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SleepSchedule",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FrontIdPhoto", "Language", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "123 شارع النيل، القاهرة", "رينتر ألفا", "/images/idCards/b8ee0c84-7a46-457d-a6d5-9696166b3c87.jpg", "/images/idCards/95c1567c-357c-4c0a-b711-e0ba27c1a96f.jpg", 1, "12345678901234", "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FrontIdPhoto", "NationalIDNumber" },
                values: new object[] { "456 شارع المعادي، القاهرة", "رينتر بيتا", "/images/idCards/0b2b1890-82ff-4459-be9a-6dc65971849a.jpg", "/images/idCards/f9797aa8-46ce-4dbb-ad14-2a521ed962fc.jpg", "23456789012345" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), 0, 2, null, null, null, null, "SEED-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Admin", "admin@marn.com", true, "System", null, 0, 0, "Admin", false, null, null, "ADMIN@MARN.COM", "ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SEED-ADMIN-SECURITY-STAMP", false, "admin@marn.com" });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "IsRead", "ReadAt", "ReceiverId", "SenderId", "SentAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Hi, is the apartment still available for next month?", true, new DateTime(2025, 3, 20, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Yes, it is! Would you like to schedule a visit?", false, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 3, 20, 11, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PropertyAmenities",
                columns: new[] { "Amenity", "PropertyId" },
                values: new object[,]
                {
                    { 0, 1001L },
                    { 2, 1001L },
                    { 4, 1001L },
                    { 0, 1002L },
                    { 7, 1002L },
                    { 8, 1002L },
                    { 0, 1003L },
                    { 5, 1003L },
                    { 0, 1004L },
                    { 1, 1004L },
                    { 2, 1004L },
                    { 9, 1004L },
                    { 10, 1004L }
                });

            migrationBuilder.InsertData(
                table: "PropertyRules",
                columns: new[] { "Id", "PropertyId", "Rule" },
                values: new object[,]
                {
                    { 1L, 1001L, "No Smoking inside the apartment." },
                    { 2L, 1001L, "No parties or loud music after 11 PM." },
                    { 3L, 1002L, "Pets are not allowed." },
                    { 4L, 1003L, "Single occupancy only." },
                    { 5L, 1004L, "Respect the neighbors." },
                    { 6L, 1004L, "Smoking allowed only in the balcony." }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "PropertyId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1L, "Great place! Very clean and quiet.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 5, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2L, "Awesome location, but the neighbors were a bit noisy.", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 4, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 3L, "Superb luxury villa. Highly recommend!", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, 5, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "RoommatePreferences",
                columns: new[] { "Id", "BudgetRangeMax", "BudgetRangeMin", "EducationLevel", "FieldOfStudy", "GuestsFrequency", "NoiseTolerance", "Pets", "RoommatePrefrencesEnabled", "SharingLevel", "SleepSchedule", "Smoking", "UserId", "WorkSchedule" },
                values: new object[,]
                {
                    { 1L, 6000m, 3000m, 2, 1, 2, 3, true, true, 3, 1, false, new Guid("11111111-1111-1111-1111-111111111111"), 2 },
                    { 2L, 4500m, 2000m, 2, 5, 4, 5, false, true, 3, 2, true, new Guid("22222222-2222-2222-2222-222222222222"), 5 }
                });

            migrationBuilder.InsertData(
                table: "UserActivities",
                columns: new[] { "Id", "CreatedAt", "Description", "IPAddress", "Metadata", "Type", "UserId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 3, 24, 10, 0, 0, 0, DateTimeKind.Utc), "User logged in.", "127.0.0.1", null, 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2L, new DateTime(2025, 3, 24, 10, 5, 0, 0, DateTimeKind.Utc), "User viewed property 1001.", null, "{\"PropertyId\": 1001}", 9, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "UserDevices",
                columns: new[] { "Id", "FcmToken", "LastUpdated", "UserId" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddd01"), "fcm-token-renter-a-device-1", new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Utc), "11111111-1111-1111-1111-111111111111" });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreatedAt", "Reason", "ReportableId", "ReportableType", "ReporterId", "ReviewedAt", "ReviewerId", "Status" },
                values: new object[] { 1L, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Misleading information about the property.", 1001L, 1, new Guid("11111111-1111-1111-1111-111111111111"), null, new Guid("99999999-9999-9999-9999-999999999999"), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 0, 1001L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 2, 1001L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 4, 1001L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 0, 1002L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 7, 1002L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 8, 1002L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 0, 1003L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 5, 1003L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 0, 1004L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 1, 1004L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 2, 1004L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 9, 1004L });

            migrationBuilder.DeleteData(
                table: "PropertyAmenities",
                keyColumns: new[] { "Amenity", "PropertyId" },
                keyValues: new object[] { 10, 1004L });

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "PropertyRules",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "UserDevices",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddd01"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.AlterColumn<string>(
                name: "SleepSchedule",
                table: "RoommatePreferences",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FrontIdPhoto", "Language", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, null, 0, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FrontIdPhoto", "NationalIDNumber" },
                values: new object[] { null, null, null, null, null });
        }
    }
}
