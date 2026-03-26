using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class EditPropertyModelAndRemoveDuplicationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_AspNetUsers_OwnerId",
                table: "BookingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_AspNetUsers_OwnerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_OwnerId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_BookingRequests_OwnerId",
                table: "BookingRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0b5ac964-270b-4bb4-b532-f68f73a205ba"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a176a86-8fdc-4a49-9a49-3fe393ec965e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b8f8a2de-05b2-4853-a6cb-d72dad79f259"));

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RentalUnit",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentFrequency",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_Payments_DueDate",
                table: "Payments",
                column: "DueDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_DueDate",
                table: "Payments");

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
                name: "IsShared",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "RentalUnit",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentFrequency",
                table: "Contracts");

            migrationBuilder.AlterColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0b5ac964-270b-4bb4-b532-f68f73a205ba"), null, "Admin", "ADMIN" },
                    { new Guid("6a176a86-8fdc-4a49-9a49-3fe393ec965e"), null, "Owner", "OWNER" },
                    { new Guid("b8f8a2de-05b2-4853-a6cb-d72dad79f259"), null, "Renter", "RENTER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_OwnerId",
                table: "Contracts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_OwnerId",
                table: "BookingRequests",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_AspNetUsers_OwnerId",
                table: "BookingRequests",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_AspNetUsers_OwnerId",
                table: "Contracts",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
