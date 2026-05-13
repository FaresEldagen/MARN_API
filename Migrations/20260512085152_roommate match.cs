using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class roommatematch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EducationImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FieldOfStudyImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Governorate",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuestsFrequencyImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoiseToleranceImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PetsImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SearchStatus",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SharingLevelImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SleepImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SmokingImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkScheduleImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), 0, 2, null, null, null, null, "SEED-RENTER-D-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.d@example.com", true, "Renter", null, 1, 0, "Delta", false, null, null, "RENTER.D@EXAMPLE.COM", "RENTER.D@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-D-SECURITY-STAMP", false, "renter.d@example.com" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 0, 2, null, null, null, null, "SEED-RENTER-E-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.e@example.com", true, "Renter", null, 1, 0, "Epsilon", false, null, null, "RENTER.E@EXAMPLE.COM", "RENTER.E@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-E-SECURITY-STAMP", false, "renter.e@example.com" }
                });

            migrationBuilder.UpdateData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "BudgetImportance", "EducationImportance", "FieldOfStudyImportance", "Governorate", "GuestsFrequencyImportance", "NoiseToleranceImportance", "PetsImportance", "SearchStatus", "SharingLevelImportance", "SleepImportance", "SmokingImportance", "WorkScheduleImportance" },
                values: new object[] { 3, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3 });

            migrationBuilder.UpdateData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "BudgetImportance", "EducationImportance", "FieldOfStudyImportance", "Governorate", "GuestsFrequencyImportance", "NoiseToleranceImportance", "PetsImportance", "SearchStatus", "SharingLevelImportance", "SleepImportance", "SmokingImportance", "WorkScheduleImportance" },
                values: new object[] { 3, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3 });

            migrationBuilder.InsertData(
                table: "RoommatePreferences",
                columns: new[] { "Id", "BudgetImportance", "BudgetRangeMax", "BudgetRangeMin", "EducationImportance", "EducationLevel", "FieldOfStudy", "FieldOfStudyImportance", "Governorate", "GuestsFrequency", "GuestsFrequencyImportance", "NoiseTolerance", "NoiseToleranceImportance", "Pets", "PetsImportance", "RoommatePreferencesEnabled", "SearchStatus", "SharingLevel", "SharingLevelImportance", "SleepImportance", "SleepSchedule", "Smoking", "SmokingImportance", "UserId", "WorkSchedule", "WorkScheduleImportance" },
                values: new object[] { 3L, 3, 3500m, 2000m, 3, 2, 3, 3, 0, 4, 3, 2, 3, false, 3, true, 0, 2, 3, 3, 1, false, 3, new Guid("33333333-3333-3333-3333-333333333333"), 5, 3 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("88888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "RoommatePreferences",
                columns: new[] { "Id", "BudgetImportance", "BudgetRangeMax", "BudgetRangeMin", "EducationImportance", "EducationLevel", "FieldOfStudy", "FieldOfStudyImportance", "Governorate", "GuestsFrequency", "GuestsFrequencyImportance", "NoiseTolerance", "NoiseToleranceImportance", "Pets", "PetsImportance", "RoommatePreferencesEnabled", "SearchStatus", "SharingLevel", "SharingLevelImportance", "SleepImportance", "SleepSchedule", "Smoking", "SmokingImportance", "UserId", "WorkSchedule", "WorkScheduleImportance" },
                values: new object[,]
                {
                    { 4L, 3, 5500m, 4000m, 3, 3, 1, 3, 0, 2, 3, 4, 3, true, 3, true, 1, 3, 3, 3, 3, false, 3, new Guid("77777777-7777-7777-7777-777777777777"), 2, 3 },
                    { 5L, 3, 10000m, 7000m, 3, 1, 5, 3, 0, 4, 3, 5, 3, false, 3, true, 0, 1, 3, 3, 2, true, 3, new Guid("88888888-8888-8888-8888-888888888888"), 3, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-777777777777") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("88888888-8888-8888-8888-888888888888") });

            migrationBuilder.DeleteData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DropColumn(
                name: "BudgetImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "EducationImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "GuestsFrequencyImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "NoiseToleranceImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "PetsImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SearchStatus",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SharingLevelImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SleepImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SmokingImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "WorkScheduleImportance",
                table: "RoommatePreferences");
        }
    }
}
