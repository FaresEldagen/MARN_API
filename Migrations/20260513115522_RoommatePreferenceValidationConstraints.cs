using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class RoommatePreferenceValidationConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_RoommatePreference_ImportanceRanges",
                table: "RoommatePreferences",
                sql: "[SmokingImportance] BETWEEN 1 AND 5 AND [PetsImportance] BETWEEN 1 AND 5 AND [SleepImportance] BETWEEN 1 AND 5 AND [EducationImportance] BETWEEN 1 AND 5 AND [FieldOfStudyImportance] BETWEEN 1 AND 5 AND [NoiseToleranceImportance] BETWEEN 1 AND 5 AND [GuestsFrequencyImportance] BETWEEN 1 AND 5 AND [WorkScheduleImportance] BETWEEN 1 AND 5 AND [SharingLevelImportance] BETWEEN 1 AND 5 AND [BudgetImportance] BETWEEN 1 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RoommatePreference_NoiseTolerance",
                table: "RoommatePreferences",
                sql: "[NoiseTolerance] IS NULL OR [NoiseTolerance] BETWEEN 1 AND 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_RoommatePreference_ImportanceRanges",
                table: "RoommatePreferences");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RoommatePreference_NoiseTolerance",
                table: "RoommatePreferences");
        }
    }
}
