using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class Removingoldpaymentsystemandupdatecontracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Properties_PropertyId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "ConnectedAccounts");

            migrationBuilder.DropTable(
                name: "RentalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId_RenterId_OwnerId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OwnerSignature",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "RenterSignature",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SignedByOwnerAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Contracts");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LeaseStartDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LeaseEndDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalContractAmount",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950001L,
                column: "TotalContractAmount",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950002L,
                column: "TotalContractAmount",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950003L,
                column: "TotalContractAmount",
                value: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_RenterId",
                table: "Contracts",
                columns: new[] { "PropertyId", "RenterId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Properties_PropertyId",
                table: "Payments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Properties_PropertyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId_RenterId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "TotalContractAmount",
                table: "Contracts");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LeaseStartDate",
                table: "Contracts",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LeaseEndDate",
                table: "Contracts",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Contracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Contracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OwnerSignature",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RenterSignature",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignedByOwnerAt",
                table: "Contracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Contracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ConnectedAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsOnboardingComplete = table.Column<bool>(type: "bit", nullable: false),
                    StripeAccountId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedAccounts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalTransactions_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950001L,
                columns: new[] { "CancellationReason", "CancelledAt", "IPAddress", "IsLocked", "OwnerId", "OwnerSignature", "RenterSignature", "SignedByOwnerAt", "SubmittedAt", "UpdatedAt", "Version" },
                values: new object[] { null, null, null, false, new Guid("44444444-4444-4444-4444-444444444444"), null, null, new DateTime(2025, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 });

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950002L,
                columns: new[] { "CancellationReason", "CancelledAt", "IPAddress", "IsLocked", "OwnerId", "OwnerSignature", "RenterSignature", "SignedByOwnerAt", "SubmittedAt", "UpdatedAt", "Version" },
                values: new object[] { null, null, null, false, new Guid("44444444-4444-4444-4444-444444444444"), null, null, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 });

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950003L,
                columns: new[] { "CancellationReason", "CancelledAt", "IPAddress", "IsLocked", "OwnerId", "OwnerSignature", "RenterSignature", "SignedByOwnerAt", "SubmittedAt", "UpdatedAt", "Version" },
                values: new object[] { null, null, null, false, new Guid("66666666-6666-6666-6666-666666666666"), null, null, new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_RenterId_OwnerId",
                table: "Contracts",
                columns: new[] { "PropertyId", "RenterId", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedAccounts_ApplicationUserId",
                table: "ConnectedAccounts",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedAccounts_StripeAccountId",
                table: "ConnectedAccounts",
                column: "StripeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_ContractId",
                table: "RentalTransactions",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_PaymentId",
                table: "RentalTransactions",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_PropertyId",
                table: "RentalTransactions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_RenterId_PropertyId_CreatedAt",
                table: "RentalTransactions",
                columns: new[] { "RenterId", "PropertyId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_StripeSessionId",
                table: "RentalTransactions",
                column: "StripeSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Properties_PropertyId",
                table: "Payments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
