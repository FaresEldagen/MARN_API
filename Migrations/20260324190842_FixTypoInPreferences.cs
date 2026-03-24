using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoommatePrefrencesEnabled",
                table: "RoommatePreferences",
                newName: "RoommatePreferencesEnabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoommatePreferencesEnabled",
                table: "RoommatePreferences",
                newName: "RoommatePrefrencesEnabled");
        }
    }
}
