using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentFrequency",
                table: "BookingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "PaymentFrequency",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5002L,
                column: "PaymentFrequency",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "PaymentFrequency",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5004L,
                column: "PaymentFrequency",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                column: "Type",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6002L,
                column: "Type",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L,
                column: "Type",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                column: "Type",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6010L,
                column: "Type",
                value: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentFrequency",
                table: "BookingRequests");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                column: "Type",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6002L,
                column: "Type",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6007L,
                column: "Type",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                column: "Type",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6010L,
                column: "Type",
                value: 4);
        }
    }
}
