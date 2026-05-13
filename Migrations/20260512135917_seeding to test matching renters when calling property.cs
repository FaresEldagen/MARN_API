using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class seedingtotestmatchingrenterswhencallingproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[] { 1100L, "555 Shared Lane, Cairo", 2, 3, 4, "Cairo", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "A shared house seeded for testing roommate matching logic.", true, true, 30.079999999999998, 31.260000000000002, 4, new Guid("44444444-4444-4444-4444-444444444444"), 4000m, "", 1, 0.0, "Cairo Governorate", 1, "Shared Seed House", 1, 10, "11513" });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileBytes", "FileName", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFileBytes", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[,]
                {
                    { 1000008L, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000008.pdf", "SEEDHASH1000008SHARED", new DateOnly(2026, 1, 15), new DateOnly(2025, 1, 15), null, null, 1, 1100L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 48000m, null },
                    { 1000009L, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000009.pdf", "SEEDHASH1000009SHARED", new DateOnly(2026, 2, 1), new DateOnly(2025, 2, 1), null, null, 1, 1100L, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, 48000m, null },
                    { 1000010L, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000010.pdf", "SEEDHASH1000010SHARED", new DateOnly(2026, 2, 1), new DateOnly(2025, 2, 1), null, null, 1, 1100L, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, 48000m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000008L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000009L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000010L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1100L);
        }
    }
}
