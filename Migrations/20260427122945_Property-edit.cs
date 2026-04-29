using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class Propertyedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProofOfOwnership",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "SquareMeters",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                columns: new[] { "City", "ProofOfOwnership", "SquareMeters", "State", "ZipCode" },
                values: new object[] { "", "", 0.0, "", "" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                columns: new[] { "City", "ProofOfOwnership", "SquareMeters", "State", "ZipCode" },
                values: new object[] { "", "", 0.0, "", "" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                columns: new[] { "City", "ProofOfOwnership", "SquareMeters", "State", "ZipCode" },
                values: new object[] { "", "", 0.0, "", "" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                columns: new[] { "City", "ProofOfOwnership", "SquareMeters", "State", "ZipCode" },
                values: new object[] { "", "", 0.0, "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ProofOfOwnership",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SquareMeters",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Properties");
        }
    }
}
