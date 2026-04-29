using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertySeedLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "Cairo", "Cairo Governorate", "11511" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "Cairo", "Cairo Governorate", "11512" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "Giza", "Giza Governorate", "12511" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "New Cairo", "Cairo Governorate", "11835" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "", "", "" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "", "", "" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "", "", "" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                columns: new[] { "City", "State", "ZipCode" },
                values: new object[] { "", "", "" });
        }
    }
}
