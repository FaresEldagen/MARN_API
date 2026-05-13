using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class hotfixseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000008L,
                column: "LeaseEndDate",
                value: new DateOnly(2027, 1, 15));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000009L,
                column: "LeaseEndDate",
                value: new DateOnly(2027, 2, 1));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000010L,
                column: "LeaseEndDate",
                value: new DateOnly(2027, 2, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000008L,
                column: "LeaseEndDate",
                value: new DateOnly(2026, 1, 15));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000009L,
                column: "LeaseEndDate",
                value: new DateOnly(2026, 2, 1));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000010L,
                column: "LeaseEndDate",
                value: new DateOnly(2026, 2, 1));
        }
    }
}
