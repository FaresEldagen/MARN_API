using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRoommateMatchingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EducationImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FieldOfStudyImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Governorate",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuestsFrequencyImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoiseToleranceImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PetsImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SearchStatus",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SharingLevelImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SleepImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SmokingImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkScheduleImportance",
                table: "RoommatePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "PaymentFrequency",
                value: 1);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "PaymentFrequency",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "BudgetImportance", "EducationImportance", "FieldOfStudyImportance", "Governorate", "GuestsFrequencyImportance", "NoiseToleranceImportance", "PetsImportance", "SearchStatus", "SharingLevelImportance", "SleepImportance", "SmokingImportance", "WorkScheduleImportance" },
                values: new object[] { 2, 2, 2, 0, 2, 2, 2, 0, 2, 2, 2, 2 });

            migrationBuilder.UpdateData(
                table: "RoommatePreferences",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "BudgetImportance", "EducationImportance", "FieldOfStudyImportance", "Governorate", "GuestsFrequencyImportance", "NoiseToleranceImportance", "PetsImportance", "SearchStatus", "SharingLevelImportance", "SleepImportance", "SmokingImportance", "WorkScheduleImportance" },
                values: new object[] { 2, 2, 2, 0, 2, 2, 2, 0, 2, 2, 2, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "EducationImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "GuestsFrequencyImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "NoiseToleranceImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "PetsImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SearchStatus",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SharingLevelImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SleepImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "SmokingImportance",
                table: "RoommatePreferences");

            migrationBuilder.DropColumn(
                name: "WorkScheduleImportance",
                table: "RoommatePreferences");

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "PaymentFrequency",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookingRequests",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "PaymentFrequency",
                value: 0);
        }
    }
}
