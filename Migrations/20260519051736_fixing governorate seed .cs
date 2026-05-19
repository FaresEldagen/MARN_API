using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class fixinggovernorateseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                column: "State",
                value: "CairoGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                column: "State",
                value: "CairoGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                column: "State",
                value: "GizaGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                column: "State",
                value: "CairoGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1100L,
                column: "State",
                value: "CairoGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L,
                column: "State",
                value: "CairoGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1202L,
                column: "State",
                value: "GizaGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1203L,
                column: "State",
                value: "AlexandriaGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1204L,
                column: "State",
                value: "MarsaMatruhGovernorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1205L,
                column: "State",
                value: "LuxorGovernorate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                column: "State",
                value: "Cairo Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                column: "State",
                value: "Cairo Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                column: "State",
                value: "Giza Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                column: "State",
                value: "Cairo Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1100L,
                column: "State",
                value: "Cairo Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L,
                column: "State",
                value: "Cairo Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1202L,
                column: "State",
                value: "Giza Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1203L,
                column: "State",
                value: "Alexandria Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1204L,
                column: "State",
                value: "Matrouh Governorate");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1205L,
                column: "State",
                value: "Luxor Governorate");
        }
    }
}
