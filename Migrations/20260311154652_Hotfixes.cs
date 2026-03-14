using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class Hotfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_BookingRequests_PropertyId_OwnerId_RenterId",
                table: "BookingRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3ccb056a-48da-4f79-b911-9635a74c7b07"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a3584465-3a0f-4e55-9f91-23c3ab1b638e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f6cd14ed-1b3d-4398-b6e0-e23103694119"));

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "BookingRequests");

            migrationBuilder.RenameColumn(
                name: "RentalDuration",
                table: "Properties",
                newName: "Beds");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "PropertyMedia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "AverageRating",
                table: "Properties",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1114503b-92fc-4768-9635-535bef7cdc95"), null, "Renter", "RENTER" },
                    { new Guid("2d0929b0-d45f-4c1e-ade1-c0dc32656ea1"), null, "Admin", "ADMIN" },
                    { new Guid("997ff7b2-6752-4e75-a177-086a4bf5a68c"), null, "Owner", "OWNER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_PropertyId_RenterId",
                table: "BookingRequests",
                columns: new[] { "PropertyId", "RenterId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_BookingRequests_PropertyId_RenterId",
                table: "BookingRequests");

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

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "PropertyMedia");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "Beds",
                table: "Properties",
                newName: "RentalDuration");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "BookingRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3ccb056a-48da-4f79-b911-9635a74c7b07"), null, "Admin", "ADMIN" },
                    { new Guid("a3584465-3a0f-4e55-9f91-23c3ab1b638e"), null, "Owner", "OWNER" },
                    { new Guid("f6cd14ed-1b3d-4398-b6e0-e23103694119"), null, "Renter", "RENTER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_PropertyId_OwnerId_RenterId",
                table: "BookingRequests",
                columns: new[] { "PropertyId", "OwnerId", "RenterId" });
        }
    }
}
