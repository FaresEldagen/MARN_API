using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class Removebookingrequeststatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BookingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5002L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5004L,
                column: "Status",
                value: 0);
        }
    }
}
