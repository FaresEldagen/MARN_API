using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceContractPaymentWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Contract_Dates",
                table: "Contracts");

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3003L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4001L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4002L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4003L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4004L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4005L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4006L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3001L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3002L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3004L);

            migrationBuilder.DropColumn(
                name: "DocumentHash",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DocumentPath",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Payments",
                newName: "AmountTotal");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Snapshot",
                table: "Contracts");

            migrationBuilder.AlterColumn<long>(
                name: "ContractId",
                table: "Payments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OwnerStripeAccountId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PropertyId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptUrl",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RenterId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "RenterEmail",
                table: "Payments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnchoredAt",
                table: "Contracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnchoringStatus",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContractNumber",
                table: "Contracts",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileBytes",
                table: "Contracts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Contracts",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Contracts",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LeaseEndDate",
                table: "Contracts",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LeaseStartDate",
                table: "Contracts",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MerkleRoot",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "OtsFileBytes",
                table: "Contracts",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateTable(
                name: "ConnectedAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StripeAccountId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsOnboardingComplete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
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
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PaymentId = table.Column<long>(type: "bigint", nullable: true),
                    ContractId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PropertyId",
                table: "Payments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RenterId",
                table: "Payments",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StripeSessionId",
                table: "Payments",
                column: "StripeSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractNumber",
                table: "Contracts",
                column: "ContractNumber",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Contract_Dates",
                table: "Contracts",
                sql: "[LeaseEndDate] IS NULL OR [LeaseStartDate] IS NULL OR [LeaseEndDate] > [LeaseStartDate]");

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
                name: "FK_Payments_AspNetUsers_RenterId",
                table: "Payments",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Properties_PropertyId",
                table: "Payments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_RenterId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Properties_PropertyId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "ConnectedAccounts");

            migrationBuilder.DropTable(
                name: "RentalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PropertyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RenterId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_StripeSessionId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractNumber",
                table: "Contracts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Contract_Dates",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OwnerStripeAccountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReceiptUrl",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RenterId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RenterEmail",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AnchoredAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AnchoringStatus",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FileBytes",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "LeaseEndDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "LeaseStartDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "MerkleRoot",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "OtsFileBytes",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "AmountTotal",
                table: "Payments",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<long>(
                name: "ContractId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentHash",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DocumentPath",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Snapshot",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "CancellationReason", "CancelledAt", "CreatedAt", "DocumentHash", "DocumentPath", "EndDate", "IPAddress", "IsLocked", "OwnerId", "OwnerSignature", "PaymentFrequency", "PropertyId", "RenterId", "RenterSignature", "SignedByOwnerAt", "SignedByRenterAt", "Snapshot", "StartDate", "Status", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { 3001L, null, null, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-1-HASH", "/contracts/seed/contract1.pdf", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), "OwnerX-Signature", 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), "RenterA-Signature", null, null, "{\"RenterSnapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"FullName\":\"Renter Alpha\",\"Email\":\"renter.a@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":\"12345678901234\",\"ArabicFullName\":\"\\u0631\\u064A\\u0646\\u062A\\u0631 \\u0623\\u0644\\u0641\\u0627\",\"ArabicAddress\":\"123 \\u0634\\u0627\\u0631\\u0639 \\u0627\\u0644\\u0646\\u064A\\u0644\\u060C \\u0627\\u0644\\u0642\\u0627\\u0647\\u0631\\u0629\",\"ProfileImage\":\"/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png\"},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1001,\"Title\":\"Cozy Seed Apartment\",\"Description\":\"A cozy seeded apartment suitable for testing active rentals.\",\"Address\":\"123 Seed Street, Cairo\",\"Price\":5000,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Apartment\",\"Bedrooms\":2,\"Bathrooms\":1,\"Beds\":3}}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1 },
                    { 3002L, null, null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-2-HASH", "/contracts/seed/contract2.pdf", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), "OwnerX-Signature", 1, 1002L, new Guid("33333333-3333-3333-3333-333333333333"), "RenterC-Signature", null, null, "{\"RenterSnapshot\":{\"Id\":\"33333333-3333-3333-3333-333333333333\",\"FullName\":\"Renter Gamma\",\"Email\":\"renter.c@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1002,\"Title\":\"Modern Seed Loft\",\"Description\":\"A modern loft used for pending booking and payments tests.\",\"Address\":\"456 Integration Avenue, Cairo\",\"Price\":7500,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Apartment\",\"Bedrooms\":1,\"Bathrooms\":1,\"Beds\":1}}", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1 },
                    { 3003L, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-3-HASH", "/contracts/seed/contract3.pdf", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), null, 1, 1003L, new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, "{\"RenterSnapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"FullName\":\"Renter Alpha\",\"Email\":\"renter.a@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":\"12345678901234\",\"ArabicFullName\":\"\\u0631\\u064A\\u0646\\u062A\\u0631 \\u0623\\u0644\\u0641\\u0627\",\"ArabicAddress\":\"123 \\u0634\\u0627\\u0631\\u0639 \\u0627\\u0644\\u0646\\u064A\\u0644\\u060C \\u0627\\u0644\\u0642\\u0627\\u0647\\u0631\\u0629\",\"ProfileImage\":\"/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png\"},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1003,\"Title\":\"Seed Studio Flat\",\"Description\":\"A small studio property used for saved properties and pending bookings.\",\"Address\":\"789 Scenario Road, Cairo\",\"Price\":3500,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Studio\",\"Bedrooms\":1,\"Bathrooms\":1,\"Beds\":1}}", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, 1 },
                    { 3004L, null, null, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SEED-CONTRACT-4-HASH", "/contracts/seed/contract4.pdf", new DateTime(2027, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("44444444-4444-4444-4444-444444444444"), "OwnerX-Signature", 1, 1001L, new Guid("66666666-6666-6666-6666-666666666666"), "OwnerZ-AsRenter-Signature", null, null, "{\"RenterSnapshot\":{\"Id\":\"66666666-6666-6666-6666-666666666666\",\"FullName\":\"Owner Z\",\"Email\":\"owner.z@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1001,\"Title\":\"Cozy Seed Apartment\",\"Description\":\"A cozy seeded apartment suitable for testing active rentals.\",\"Address\":\"123 Seed Street, Cairo\",\"Price\":5000,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Apartment\",\"Bedrooms\":2,\"Bathrooms\":1,\"Beds\":3}}", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AvailableAt", "ContractId", "CreatedAt", "Currency", "DueDate", "OwnerAmount", "PaidAt", "PlatformFee", "Status", "StripePaymentIntentId", "TotalAmount" },
                values: new object[,]
                {
                    { 4001L, null, 3001L, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4500m, null, 500m, 0, null, 5000m },
                    { 4002L, new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3001L, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4500m, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 500m, 1, null, 5000m },
                    { 4003L, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3002L, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6750m, new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 750m, 1, null, 7500m },
                    { 4004L, new DateTime(2035, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3002L, new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6750m, new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), 750m, 1, null, 7500m },
                    { 4005L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3004L, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4500m, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), 500m, 1, null, 5000m },
                    { 4006L, null, 3004L, new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "EGP", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4500m, null, 500m, 0, null, 5000m }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Contract_Dates",
                table: "Contracts",
                sql: "[EndDate] > [StartDate]");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
