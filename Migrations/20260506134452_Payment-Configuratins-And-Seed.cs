using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class PaymentConfiguratinsAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentScheduleId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Status_AvailableAt",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950002L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950003L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950001L);

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "PaymentSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentIntentId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileBytes", "FileName", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFileBytes", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[,]
                {
                    { 1000001L, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000001.pdf", "SEEDHASH1000001ACTIVEMONTHLY", new DateOnly(2026, 1, 1), new DateOnly(2025, 1, 1), null, null, 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 60000m, null },
                    { 1000002L, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000002.pdf", "SEEDHASH1000002ACTIVEQUARTERLY", new DateOnly(2026, 1, 1), new DateOnly(2025, 1, 1), null, null, 2, 1002L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, 90000m, null },
                    { 1000003L, new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000003.pdf", "SEEDHASH1000003ACTIVEYEARLY", new DateOnly(2026, 6, 1), new DateOnly(2024, 6, 1), null, null, 3, 1003L, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 84000m, null },
                    { 1000004L, new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000004.pdf", "SEEDHASH1000004OWNERZMONTHLY", new DateOnly(2026, 2, 1), new DateOnly(2025, 2, 1), null, null, 1, 1004L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 180000m, null },
                    { 1000005L, null, 0, new DateTime(2025, 2, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000005.pdf", "SEEDHASH1000005ONETIME", new DateOnly(2025, 9, 1), new DateOnly(2025, 3, 1), null, null, 0, 1001L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 30000m, null },
                    { 1000006L, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000006.pdf", "SEEDHASH1000006EXPIRED", new DateOnly(2024, 12, 31), new DateOnly(2024, 1, 1), null, null, 2, 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, 36000m, null },
                    { 1000007L, null, 0, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000007.pdf", "SEEDHASH1000007CANCELLED", new DateOnly(2026, 5, 1), new DateOnly(2025, 5, 1), null, null, 1, 1004L, new Guid("22222222-2222-2222-2222-222222222222"), null, 2, 180000m, null }
                });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                column: "Type",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                column: "Type",
                value: 10);

            migrationBuilder.InsertData(
                table: "PaymentSchedules",
                columns: new[] { "Id", "Amount", "ContractId", "Currency", "DueDate", "PaymentIntentId", "Status" },
                values: new object[,]
                {
                    { 20001L, 5000m, 1000001L, "egp", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20001", 3 },
                    { 20002L, 5000m, 1000001L, "egp", new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20002", 4 },
                    { 20003L, 5000m, 1000001L, "egp", new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20003", 5 },
                    { 20004L, 5000m, 1000001L, "egp", new DateTime(2026, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 6 },
                    { 20005L, 5000m, 1000001L, "egp", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 2 },
                    { 20006L, 5000m, 1000001L, "egp", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 },
                    { 20007L, 5000m, 1000001L, "egp", new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20008L, 5000m, 1000001L, "egp", new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20009L, 5000m, 1000001L, "egp", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20010L, 22500m, 1000002L, "egp", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20010", 3 },
                    { 20011L, 22500m, 1000002L, "egp", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, 6 },
                    { 20012L, 22500m, 1000002L, "egp", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 },
                    { 20013L, 22500m, 1000002L, "egp", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20020L, 42000m, 1000003L, "egp", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20020", 4 },
                    { 20021L, 42000m, 1000003L, "egp", new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20030L, 15000m, 1000004L, "egp", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20030", 3 },
                    { 20031L, 15000m, 1000004L, "egp", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20031", 4 },
                    { 20032L, 15000m, 1000004L, "egp", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20032", 5 },
                    { 20033L, 15000m, 1000004L, "egp", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20033", 4 },
                    { 20034L, 15000m, 1000004L, "egp", new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 6 },
                    { 20035L, 15000m, 1000004L, "egp", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 },
                    { 20036L, 15000m, 1000004L, "egp", new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20037L, 15000m, 1000004L, "egp", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20040L, 30000m, 1000005L, "egp", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20040", 5 },
                    { 20050L, 9000m, 1000006L, "egp", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20050", 4 },
                    { 20051L, 9000m, 1000006L, "egp", new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20051", 4 },
                    { 20052L, 9000m, 1000006L, "egp", new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20052", 5 },
                    { 20053L, 9000m, 1000006L, "egp", new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20053", 3 },
                    { 20060L, 15000m, 1000007L, "egp", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AmountTotal", "ApplicationUserId", "AvailableAt", "Currency", "OwnerAmount", "PaidAt", "PaymentIntentId", "PaymentScheduleId", "PlatformFee" },
                values: new object[,]
                {
                    { 30001L, 5000m, null, new DateTime(2025, 2, 8, 12, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2025, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20001", 20001L, 500m },
                    { 30002L, 5000m, null, new DateTime(2025, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2025, 2, 28, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20002", 20002L, 500m },
                    { 30003L, 5000m, null, new DateTime(2025, 4, 15, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2025, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20003", 20003L, 500m },
                    { 30010L, 22500m, null, new DateTime(2025, 4, 1, 14, 0, 0, 0, DateTimeKind.Utc), "egp", 20250m, new DateTime(2025, 3, 22, 14, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20010", 20010L, 2250m },
                    { 30020L, 42000m, null, new DateTime(2025, 6, 11, 8, 0, 0, 0, DateTimeKind.Utc), "egp", 37800m, new DateTime(2025, 6, 1, 8, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20020", 20020L, 4200m },
                    { 30030L, 15000m, null, new DateTime(2025, 3, 8, 11, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 2, 26, 11, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20030", 20030L, 1500m },
                    { 30031L, 15000m, null, new DateTime(2025, 4, 11, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 4, 1, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20031", 20031L, 1500m },
                    { 30032L, 15000m, null, new DateTime(2025, 5, 18, 16, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 5, 8, 16, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20032", 20032L, 1500m },
                    { 30033L, 15000m, null, new DateTime(2025, 8, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 8, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20033", 20033L, 1500m },
                    { 30040L, 30000m, null, new DateTime(2025, 4, 21, 13, 0, 0, 0, DateTimeKind.Utc), "egp", 27000m, new DateTime(2025, 4, 11, 13, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20040", 20040L, 3000m },
                    { 30050L, 9000m, null, new DateTime(2024, 4, 10, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 8100m, new DateTime(2024, 3, 31, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20050", 20050L, 900m },
                    { 30051L, 9000m, null, new DateTime(2024, 7, 10, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 8100m, new DateTime(2024, 6, 30, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20051", 20051L, 900m },
                    { 30052L, 9000m, null, new DateTime(2024, 10, 17, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 8100m, new DateTime(2024, 10, 7, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20052", 20052L, 900m },
                    { 30053L, 9000m, null, new DateTime(2025, 1, 4, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 8100m, new DateTime(2024, 12, 25, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20053", 20053L, 900m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AvailableAt",
                table: "Payments",
                column: "AvailableAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentIntentId",
                table: "Payments",
                column: "PaymentIntentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentScheduleId",
                table: "Payments",
                column: "PaymentScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_AvailableAt",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentIntentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentScheduleId",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20004L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20005L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20006L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20007L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20008L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20009L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20011L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20012L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20013L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20021L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20034L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20035L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20036L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20037L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20060L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30001L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30002L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30003L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30010L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30020L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30030L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30031L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30032L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30033L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30040L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30050L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30051L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30052L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30053L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000007L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20001L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20002L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20003L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20010L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20020L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20030L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20031L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20032L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20033L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20040L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20050L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20051L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20052L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20053L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000001L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000002L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000003L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000004L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000005L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000006L);

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "PaymentSchedules");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileBytes", "FileName", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFileBytes", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[,]
                {
                    { 950001L, null, 0, new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1001-active-a.pdf", "SEEDHASH1001ACTIVEA", new DateOnly(2026, 3, 1), new DateOnly(2025, 3, 1), null, null, 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, 0m, null },
                    { 950002L, null, 0, new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1001-expired-b.pdf", "SEEDHASH1001EXPIREDB", new DateOnly(2024, 12, 31), new DateOnly(2024, 1, 1), null, null, 1, 1001L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, 0m, null },
                    { 950003L, null, 0, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1004-expired-a.pdf", "SEEDHASH1004EXPIREDA", new DateOnly(2025, 1, 31), new DateOnly(2024, 6, 1), null, null, 1, 1004L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, 0m, null }
                });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6001L,
                column: "Type",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 6009L,
                column: "Type",
                value: 11);

            migrationBuilder.InsertData(
                table: "PaymentSchedules",
                columns: new[] { "Id", "Amount", "ContractId", "Currency", "DueDate", "Status" },
                values: new object[,]
                {
                    { 1L, 5000m, 950001L, "egp", new DateTime(2026, 5, 8, 8, 41, 33, 437, DateTimeKind.Utc).AddTicks(2657), 0 },
                    { 2L, 5000m, 950001L, "egp", new DateTime(2026, 6, 3, 8, 41, 33, 437, DateTimeKind.Utc).AddTicks(2665), 0 },
                    { 3L, 5000m, 950001L, "egp", new DateTime(2026, 4, 3, 8, 41, 33, 437, DateTimeKind.Utc).AddTicks(2671), 2 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AmountTotal", "ApplicationUserId", "AvailableAt", "Currency", "OwnerAmount", "PaidAt", "PaymentIntentId", "PaymentScheduleId", "PlatformFee", "Status" },
                values: new object[] { 1L, 5000m, null, new DateTime(2026, 4, 10, 8, 41, 33, 437, DateTimeKind.Utc).AddTicks(2940), "egp", 4500m, new DateTime(2026, 4, 3, 8, 41, 33, 437, DateTimeKind.Utc).AddTicks(2938), "pi_seed_1", 3L, 500m, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentScheduleId",
                table: "Payments",
                column: "PaymentScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status_AvailableAt",
                table: "Payments",
                columns: new[] { "Status", "AvailableAt" });
        }
    }
}
