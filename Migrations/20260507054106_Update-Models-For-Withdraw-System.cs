using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsForWithdrawSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StripeAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StripeChargesEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StripePayoutsEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30001L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30002L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30003L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30010L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30020L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30030L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30031L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30032L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30033L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30040L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30050L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30051L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30052L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30053L,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Amenity",
                value: 8);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Amenity",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Amenity",
                value: 5);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Amenity",
                value: 12);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Amenity",
                value: 6);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Amenity",
                value: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripeChargesEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripePayoutsEnabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Amenity",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Amenity",
                value: 7);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Amenity",
                value: 8);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Amenity",
                value: 5);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Amenity",
                value: 9);

            migrationBuilder.UpdateData(
                table: "PropertyAmenities",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Amenity",
                value: 10);
        }
    }
}
