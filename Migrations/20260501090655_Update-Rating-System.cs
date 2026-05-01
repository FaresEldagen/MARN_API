using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRatingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CancellationReason", "CancelledAt", "ContractNumber", "CreatedAt", "FileBytes", "FileName", "Hash", "IPAddress", "IsLocked", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFileBytes", "OwnerId", "OwnerSignature", "PaymentFrequency", "PropertyId", "RenterId", "RenterSignature", "SignedByOwnerAt", "SignedByRenterAt", "Status", "SubmittedAt", "TransactionId", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { 950001L, null, 0, null, null, "SEED-CONTRACT-1001-ACTIVE-A", new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1001-active-a.pdf", "SEEDHASH1001ACTIVEA", null, false, new DateOnly(2026, 3, 1), new DateOnly(2025, 3, 1), null, null, new Guid("44444444-4444-4444-4444-444444444444"), null, 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2025, 2, 27, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1 },
                    { 950002L, null, 0, null, null, "SEED-CONTRACT-1001-EXPIRED-B", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1001-expired-b.pdf", "SEEDHASH1001EXPIREDB", null, false, new DateOnly(2024, 12, 31), new DateOnly(2024, 1, 1), null, null, new Guid("44444444-4444-4444-4444-444444444444"), null, 1, 1001L, new Guid("22222222-2222-2222-2222-222222222222"), null, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1 },
                    { 950003L, null, 0, null, null, "SEED-CONTRACT-1004-EXPIRED-A", new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1004-expired-a.pdf", "SEEDHASH1004EXPIREDA", null, false, new DateOnly(2025, 1, 31), new DateOnly(2024, 6, 1), null, null, new Guid("66666666-6666-6666-6666-666666666666"), null, 1, 1004L, new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "PropertyComments",
                columns: new[] { "Id", "Content", "CreatedAt", "PropertyId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 900001L, "Great place! Very clean and quiet.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900002L, "Awesome location, but the neighbors were a bit noisy.", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 900003L, "Superb luxury villa. Highly recommend!", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, null, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "PropertyRatings",
                columns: new[] { "Id", "CreatedAt", "PropertyId", "Rating", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 900001L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 5, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900002L, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 4, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 900003L, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, 5, null, new Guid("11111111-1111-1111-1111-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950001L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950002L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 950003L);

            migrationBuilder.DeleteData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900001L);

            migrationBuilder.DeleteData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900002L);

            migrationBuilder.DeleteData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900003L);

            migrationBuilder.DeleteData(
                table: "PropertyRatings",
                keyColumn: "Id",
                keyValue: 900001L);

            migrationBuilder.DeleteData(
                table: "PropertyRatings",
                keyColumn: "Id",
                keyValue: 900002L);

            migrationBuilder.DeleteData(
                table: "PropertyRatings",
                keyColumn: "Id",
                keyValue: 900003L);

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.CheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5");
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "PropertyId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1L, "Great place! Very clean and quiet.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 5, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2L, "Awesome location, but the neighbors were a bit noisy.", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 4, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 3L, "Superb luxury villa. Highly recommend!", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, 5, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PropertyId_UserId",
                table: "Reviews",
                columns: new[] { "PropertyId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }
    }
}
