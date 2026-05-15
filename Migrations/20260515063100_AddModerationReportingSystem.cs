using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class AddModerationReportingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ReportableId",
                table: "Reports",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ActionTaken",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportableGuidId",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerNote",
                table: "Reports",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HiddenAt",
                table: "PropertyComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HiddenByAdminId",
                table: "PropertyComments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HiddenReason",
                table: "PropertyComments",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHiddenByModeration",
                table: "PropertyComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "HiddenAt",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HiddenByAdminId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HiddenReason",
                table: "Messages",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHiddenByModeration",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AdminActionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<long>(type: "bigint", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TargetGuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetLongId = table.Column<long>(type: "bigint", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminActionLogs_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminActionLogs_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration" },
                values: new object[] { null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration" },
                values: new object[] { null, null, null, false });

            migrationBuilder.UpdateData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900001L,
                columns: new[] { "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration" },
                values: new object[] { null, null, null, false });

            migrationBuilder.UpdateData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900002L,
                columns: new[] { "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration" },
                values: new object[] { null, null, null, false });

            migrationBuilder.UpdateData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900003L,
                columns: new[] { "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration" },
                values: new object[] { null, null, null, false });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ActionTaken", "ReportableGuidId", "ReviewerId", "ReviewerNote" },
                values: new object[] { null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportableType",
                table: "Reports",
                column: "ReportableType");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Status",
                table: "Reports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActionLogs_AdminId",
                table: "AdminActionLogs",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActionLogs_ReportId",
                table: "AdminActionLogs",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminActionLogs");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportableType",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_Status",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportableGuidId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReviewerNote",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "HiddenAt",
                table: "PropertyComments");

            migrationBuilder.DropColumn(
                name: "HiddenByAdminId",
                table: "PropertyComments");

            migrationBuilder.DropColumn(
                name: "HiddenReason",
                table: "PropertyComments");

            migrationBuilder.DropColumn(
                name: "IsHiddenByModeration",
                table: "PropertyComments");

            migrationBuilder.DropColumn(
                name: "HiddenAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "HiddenByAdminId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "HiddenReason",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsHiddenByModeration",
                table: "Messages");

            migrationBuilder.AlterColumn<long>(
                name: "ReportableId",
                table: "Reports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ReviewerId",
                value: new Guid("99999999-9999-9999-9999-999999999999"));
        }
    }
}
