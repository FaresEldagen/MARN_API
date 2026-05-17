using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<bool>(
                name: "StripePayoutsEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "StripeChargesEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled" },
                values: new object[] { null, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.AlterColumn<bool>(
                name: "StripePayoutsEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "StripeChargesEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

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
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, 2, "123 شارع النيل، القاهرة", "رينتر ألفا", "/images/idCards/b8ee0c84-7a46-457d-a6d5-9696166b3c87.jpg", null, "SEED-RENTER-A-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.a@example.com", true, "Renter", "/images/idCards/95c1567c-357c-4c0a-b711-e0ba27c1a96f.jpg", 1, 1, "Alpha", false, null, "12345678901234", "RENTER.A@EXAMPLE.COM", "RENTER.A@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png", "SEED-RENTER-A-SECURITY-STAMP", null, false, "renter.a@example.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, 2, "456 شارع المعادي، القاهرة", "رينتر بيتا", "/images/idCards/0b2b1890-82ff-4459-be9a-6dc65971849a.jpg", null, "SEED-RENTER-B-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.b@example.com", true, "Renter", "/images/idCards/f9797aa8-46ce-4dbb-ad14-2a521ed962fc.jpg", 2, 0, "Beta", false, null, "23456789012345", "RENTER.B@EXAMPLE.COM", "RENTER.B@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-B-SECURITY-STAMP", null, false, "renter.b@example.com" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), 0, 2, null, null, null, null, "SCENARIO-SECOND-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, null, "Admin", "assistant.admin@marn.com", true, "Assistant", null, 0, 0, "Admin", false, null, null, "ASSISTANT.ADMIN@MARN.COM", "ASSISTANT.ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-SECOND-ADMIN-SECURITY-STAMP", null, false, "assistant.admin@marn.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, 2, null, null, null, null, "SEED-RENTER-C-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.c@example.com", true, "Renter", null, 1, 0, "Gamma", false, null, null, "RENTER.C@EXAMPLE.COM", "RENTER.C@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-C-SECURITY-STAMP", null, false, "renter.c@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan", "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, 2, null, null, null, null, "SEED-OWNER-X-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.x@example.com", true, "Owner", null, 1, 0, "X", false, null, null, "OWNER.X@EXAMPLE.COM", "OWNER.X@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-X-SECURITY-STAMP", null, null, false, false, false, "owner.x@example.com" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, 2, null, null, null, null, "SEED-OWNER-Y-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.y@example.com", true, "Owner", null, 2, 0, "Y", false, null, null, "OWNER.Y@EXAMPLE.COM", "OWNER.Y@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Y-SECURITY-STAMP", null, null, false, false, false, "owner.y@example.com" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 0, 2, null, null, null, null, "SEED-OWNER-Z-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.z@example.com", true, "Owner", null, 1, 0, "Z", false, null, null, "OWNER.Z@EXAMPLE.COM", "OWNER.Z@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Z-SECURITY-STAMP", null, null, false, false, false, "owner.z@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), 0, 2, null, null, null, null, "SEED-RENTER-D-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.d@example.com", true, "Renter", null, 1, 0, "Delta", false, null, null, "RENTER.D@EXAMPLE.COM", "RENTER.D@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-D-SECURITY-STAMP", null, false, "renter.d@example.com" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 0, 2, null, null, null, null, "SEED-RENTER-E-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.e@example.com", true, "Renter", null, 1, 0, "Epsilon", false, null, null, "RENTER.E@EXAMPLE.COM", "RENTER.E@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-E-SECURITY-STAMP", null, false, "renter.e@example.com" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 0, 2, null, null, null, null, "SEED-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Admin", "admin@marn.com", true, "System", null, 0, 0, "Admin", false, null, null, "ADMIN@MARN.COM", "ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SEED-ADMIN-SECURITY-STAMP", null, false, "admin@marn.com" }
                });
        }
    }
}
