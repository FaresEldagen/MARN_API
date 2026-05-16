using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminDashboardScenarioSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), null, "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), 0, 1, "15 شارع التسعين، القاهرة الجديدة", "مستخدم قيد التحقق", "/images/idCards/pending-renter-back.jpg", null, "SCENARIO-PENDING-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 5, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "pending.renter@example.com", true, "Pending", "/images/idCards/pending-renter-front.jpg", 2, 1, "Renter", false, null, "34567890123456", "PENDING.RENTER@EXAMPLE.COM", "PENDING.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-PENDING-RENTER-SECURITY-STAMP", null, false, "pending.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), 0, 4, "22 شارع النصر، مدينة نصر", "مستخدم موقوف", "/images/idCards/banned-renter-back.jpg", null, "SCENARIO-BANNED-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 2, 14, 10, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "banned.renter@example.com", true, "Banned", "/images/idCards/banned-renter-front.jpg", 1, 0, "Renter", false, null, "45678901234567", "BANNED.RENTER@EXAMPLE.COM", "BANNED.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-BANNED-RENTER-SECURITY-STAMP", 1, false, "banned.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), 0, 2, null, null, null, null, "SCENARIO-DELETED-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 3, 3, 8, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Renter", "deleted.renter@example.com", true, "Deleted", null, 1, 0, "Renter", false, null, null, "DELETED.RENTER@EXAMPLE.COM", "DELETED.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-DELETED-RENTER-SECURITY-STAMP", null, false, "deleted.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), 0, 2, null, null, null, "Fresh account created to validate the dashboard new-user metrics.", "SCENARIO-RECENT-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 5, 10, 14, 30, 0, 0, DateTimeKind.Utc), null, null, "Renter", "recent.renter@example.com", true, "Recent", null, 2, 0, "Renter", false, null, null, "RECENT.RENTER@EXAMPLE.COM", "RECENT.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-RECENT-RENTER-SECURITY-STAMP", null, false, "recent.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), 0, 2, null, null, null, "Seeded moderator candidate for role-management testing.", "SCENARIO-MODERATOR-USER-CONCURRENCY-STAMP", 1, new DateTime(2026, 4, 20, 11, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "moderator.user@example.com", true, "Mona", null, 2, 0, "Moderator", false, null, null, "MODERATOR.USER@EXAMPLE.COM", "MODERATOR.USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-MODERATOR-USER-SECURITY-STAMP", null, false, "moderator.user@example.com" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), 0, 2, null, null, null, null, "SCENARIO-SECOND-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, null, "Admin", "assistant.admin@marn.com", true, "Assistant", null, 0, 0, "Admin", false, null, null, "ASSISTANT.ADMIN@MARN.COM", "ASSISTANT.ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-SECOND-ADMIN-SECURITY-STAMP", null, false, "assistant.admin@marn.com" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileBytes", "FileName", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFileBytes", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[] { 1000102L, new DateTime(2025, 11, 30, 9, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000102.pdf", "SEEDHASH1000102REVENUEGRAPHADMINDASHBOARD", new DateOnly(2026, 6, 30), new DateOnly(2025, 12, 1), null, null, 1, 1003L, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2025, 11, 29, 10, 0, 0, 0, DateTimeKind.Utc), 1, 42000m, null });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[,]
                {
                    { 1201L, "10 Tahrir Square", 1, 1, 1, "Cairo", new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, "Ownership documents are uploaded and waiting for admin review.", true, false, 30.044, 31.234999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 6200m, "/docs/properties/pending-downtown-apartment.pdf", 1, 85.0, "Cairo Governorate", 0, "Pending Downtown Apartment", 0, 0, "11511" },
                    { 1202L, "88 Palm Street", 2, 3, 4, "Giza", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Utc), null, "A property with rejected ownership documentation for verification testing.", true, false, 30.010999999999999, 31.207999999999998, 5, new Guid("55555555-5555-5555-5555-555555555555"), 11000m, "/docs/properties/declined-garden-house.pdf", 1, 180.0, "Giza Governorate", 2, "Declined Garden House", 1, 4, "12511" },
                    { 1203L, "34 Sunset Alley", 1, 1, 1, "Alexandria", new DateTime(2026, 3, 8, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 4, 13, 0, 0, 0, DateTimeKind.Utc), "Soft deleted property used to validate include-deleted admin filters.", false, false, 31.199999999999999, 29.918700000000001, 1, new Guid("55555555-5555-5555-5555-555555555555"), 4300m, "/docs/properties/deleted-test-studio.pdf", 1, 55.0, "Alexandria Governorate", 1, "Soft Deleted Test Studio", 4, 1, "21511" },
                    { 1204L, "5 Marina Walk", 2, 2, 2, "North Coast", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Fresh verified property created this month for dashboard trend checks.", true, false, 30.899999999999999, 28.899999999999999, 3, new Guid("55555555-5555-5555-5555-555555555555"), 7800m, "/docs/properties/recent-marina-flat.pdf", 1, 110.0, "Matrouh Governorate", 1, "Recent Marina Flat", 0, 9, "51711" },
                    { 1205L, "77 Corniche View", 3, 4, 5, "Luxor", new DateTime(2026, 5, 7, 15, 0, 0, 0, DateTimeKind.Utc), null, "Property already deactivated through a seeded moderation outcome.", false, false, 25.687200000000001, 32.639600000000002, 6, new Guid("55555555-5555-5555-5555-555555555555"), 16000m, "/docs/properties/moderated-riverside-villa.pdf", 1, 240.0, "Luxor Governorate", 1, "Moderated Riverside Villa", 3, 22, "85951" }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "ActionTaken", "CreatedAt", "Reason", "ReportableGuidId", "ReportableId", "ReportableType", "ReporterId", "ReviewedAt", "ReviewerId", "ReviewerNote", "Status" },
                values: new object[,]
                {
                    { 9101L, null, new DateTime(2026, 5, 11, 9, 30, 0, 0, DateTimeKind.Utc), "Profile details look inconsistent and need manual review.", new Guid("10000000-0000-0000-0000-000000000004"), null, 0, new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, 0 },
                    { 9102L, 2, new DateTime(2026, 5, 8, 10, 0, 0, 0, DateTimeKind.Utc), "Listing contains misleading availability details.", null, 1205L, 1, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 5, 8, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Property deactivated until the owner corrects the listing.", 1 },
                    { 9103L, 3, new DateTime(2026, 4, 13, 8, 0, 0, 0, DateTimeKind.Utc), "Abusive language in chat.", new Guid("00000000-0000-0000-0000-000000000101"), null, 2, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Message hidden and sender banned.", 1 },
                    { 9104L, 4, new DateTime(2026, 4, 14, 10, 0, 0, 0, DateTimeKind.Utc), "Comment includes harassment.", null, 900101L, 3, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Comment hidden and the commenter was banned.", 1 }
                });

            migrationBuilder.InsertData(
                table: "AdminActionLogs",
                columns: new[] { "Id", "ActionType", "AdminId", "CreatedAt", "MetadataJson", "Reason", "ReportId", "TargetGuidId", "TargetLongId", "TargetType" },
                values: new object[,]
                {
                    { 8101L, "DeactivateProperty", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 5, 8, 12, 0, 0, 0, DateTimeKind.Utc), null, "Property deactivated until listing details are corrected.", 9102L, null, 1205L, "Property" },
                    { 8102L, "HideMessage", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), null, "Hidden abusive message.", 9103L, new Guid("00000000-0000-0000-0000-000000000101"), null, "Message" },
                    { 8103L, "BanUser", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 13, 9, 1, 0, 0, DateTimeKind.Utc), null, "Banned sender after abusive chat message.", 9103L, new Guid("10000000-0000-0000-0000-000000000002"), null, "Message" },
                    { 8104L, "HidePropertyComment", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, "Hidden harassing property comment.", 9104L, null, 900101L, "PropertyComment" },
                    { 8105L, "BanUser", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 14, 12, 1, 0, 0, DateTimeKind.Utc), null, "Banned commenter after repeated harassment.", 9104L, new Guid("10000000-0000-0000-0000-000000000002"), null, "PropertyComment" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("30000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileBytes", "FileName", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFileBytes", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[] { 1000101L, null, 0, new DateTime(2026, 5, 8, 13, 0, 0, 0, DateTimeKind.Utc), null, "seed-contract-1000101.pdf", "SEEDHASH1000101PENDINGADMINDASHBOARD", new DateOnly(2026, 7, 31), new DateOnly(2026, 6, 1), null, null, 1, 1204L, new Guid("10000000-0000-0000-0000-000000000004"), null, 0, 15600m, null });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration", "ReadAt", "ReceiverId", "SenderId", "SentAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000101"), "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=", new DateTime(2026, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Seeded moderation example for admin dashboard testing.", true, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 4, 12, 19, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "PaymentSchedules",
                columns: new[] { "Id", "Amount", "ContractId", "Currency", "DueDate", "PaymentIntentId", "Status" },
                values: new object[,]
                {
                    { 20101L, 6000m, 1000102L, "egp", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20101", 4 },
                    { 20102L, 6000m, 1000102L, "egp", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20102", 4 },
                    { 20103L, 6000m, 1000102L, "egp", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20103", 4 },
                    { 20104L, 6000m, 1000102L, "egp", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20104", 4 },
                    { 20105L, 6000m, 1000102L, "egp", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20105", 4 },
                    { 20106L, 6000m, 1000102L, "egp", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20106", 4 },
                    { 20107L, 6000m, 1000102L, "egp", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 }
                });

            migrationBuilder.InsertData(
                table: "PropertyComments",
                columns: new[] { "Id", "Content", "CreatedAt", "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration", "PropertyId", "UpdatedAt", "UserId" },
                values: new object[] { 900101L, "This seeded comment was hidden by moderation for admin review testing.", new DateTime(2026, 4, 14, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Seeded moderation example for admin dashboard testing.", true, 1001L, null, new Guid("10000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "ActionTaken", "CreatedAt", "Reason", "ReportableGuidId", "ReportableId", "ReportableType", "ReporterId", "ReviewedAt", "ReviewerId", "ReviewerNote", "Status" },
                values: new object[] { 9105L, null, new DateTime(2026, 5, 9, 9, 0, 0, 0, DateTimeKind.Utc), "Suspicious behavior, but without evidence.", new Guid("10000000-0000-0000-0000-000000000005"), null, 0, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 5, 9, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "Insufficient evidence after review.", 2 });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AmountTotal", "ApplicationUserId", "AvailableAt", "Currency", "OwnerAmount", "PaidAt", "PaymentIntentId", "PaymentScheduleId", "PlatformFee", "Status" },
                values: new object[,]
                {
                    { 30101L, 6000m, null, new DateTime(2025, 12, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2025, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20101", 20101L, 600m, 1 },
                    { 30102L, 6000m, null, new DateTime(2026, 1, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20102", 20102L, 600m, 1 },
                    { 30103L, 6000m, null, new DateTime(2026, 2, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20103", 20103L, 600m, 1 },
                    { 30104L, 6000m, null, new DateTime(2026, 3, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20104", 20104L, 600m, 1 },
                    { 30105L, 6000m, null, new DateTime(2026, 4, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20105", 20105L, 600m, 1 },
                    { 30106L, 6000m, null, new DateTime(2026, 5, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 5, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20106", 20106L, 600m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminActionLogs",
                keyColumn: "Id",
                keyValue: 8101L);

            migrationBuilder.DeleteData(
                table: "AdminActionLogs",
                keyColumn: "Id",
                keyValue: 8102L);

            migrationBuilder.DeleteData(
                table: "AdminActionLogs",
                keyColumn: "Id",
                keyValue: 8103L);

            migrationBuilder.DeleteData(
                table: "AdminActionLogs",
                keyColumn: "Id",
                keyValue: 8104L);

            migrationBuilder.DeleteData(
                table: "AdminActionLogs",
                keyColumn: "Id",
                keyValue: 8105L);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), new Guid("10000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("30000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000101L);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20107L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30101L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30102L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30103L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30104L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30105L);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 30106L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1202L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1203L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1205L);

            migrationBuilder.DeleteData(
                table: "PropertyComments",
                keyColumn: "Id",
                keyValue: 900101L);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9101L);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9105L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-2222-3333-444444444444"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20101L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20102L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20103L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20104L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20105L);

            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20106L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1204L);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9102L);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9103L);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9104L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000102L);
        }
    }
}
