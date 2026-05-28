using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class UserActivityRecommendationsIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserActivities_UserId_Type_CreatedAt",
                table: "UserActivities");

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserActivities");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "UserActivities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserActivities");

            migrationBuilder.AddColumn<long>(
                name: "PropertyId",
                table: "UserActivities",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserActivityType",
                table: "UserActivities",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId_UserActivityType_CreatedAt",
                table: "UserActivities",
                columns: new[] { "UserId", "UserActivityType", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserActivities_UserId_UserActivityType_CreatedAt",
                table: "UserActivities");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "UserActivities");

            migrationBuilder.DropColumn(
                name: "UserActivityType",
                table: "UserActivities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "UserActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "UserActivities",
                columns: new[] { "Id", "CreatedAt", "Description", "IPAddress", "Metadata", "Type", "UserId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 3, 24, 10, 0, 0, 0, DateTimeKind.Utc), "User logged in.", "127.0.0.1", null, 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2L, new DateTime(2025, 3, 24, 10, 5, 0, 0, DateTimeKind.Utc), "User viewed property 1001.", null, "{\"PropertyId\": 1001}", 9, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId_Type_CreatedAt",
                table: "UserActivities",
                columns: new[] { "UserId", "Type", "CreatedAt" });
        }
    }
}
