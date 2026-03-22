using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class OwnerDashboardSeedUpdates : Migration
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
