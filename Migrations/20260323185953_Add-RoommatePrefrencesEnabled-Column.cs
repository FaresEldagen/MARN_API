using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRoommatePrefrencesEnabledColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RoommatePrefrencesEnabled",
                table: "RoommatePreferences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoommatePrefrencesEnabled",
                table: "RoommatePreferences");
        }
    }
}
