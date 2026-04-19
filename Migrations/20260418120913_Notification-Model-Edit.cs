using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class NotificationModelEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("99999999-9999-9999-9999-999999999999") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                columns: new[] { "ActionId", "ActionType", "Data" },
                values: new object[] { null, 4, "{\"propertyName\":\"Cozy Seed Apartment\"}" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6002L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 4 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6003L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6004L,
                columns: new[] { "ActionId", "ActionType", "Body", "Title", "Type", "UserId" },
                values: new object[] { "44444444-4444-4444-4444-444444444444", 2, "You have a new message from the owner.", "New Message", 1, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6005L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 3 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6006L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { "1002", 1 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L,
                columns: new[] { "ActionId", "ActionType", "Data" },
                values: new object[] { null, 5, "{\"amount\":\"1200\", \"currency\":\"USD\"}" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6008L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 3 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 4 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6010L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 4 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6011L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6012L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 5 });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6013L,
                columns: new[] { "ActionId", "ActionType" },
                values: new object[] { null, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("99999999-9999-9999-9999-999999999999") });

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                column: "Data",
                value: null);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6004L,
                columns: new[] { "Body", "Title", "Type", "UserId" },
                values: new object[] { "Your booking request is pending owner approval.", "Booking Pending", 0, new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L,
                column: "Data",
                value: null);
        }
    }
}
