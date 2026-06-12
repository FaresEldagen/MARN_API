using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class userpropertyseeddataedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Fresh graduate looking to relocate for work in Nasr City.", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+201298765430" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Banned user account for terms of service violations.", new DateTime(2000, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "+201598765429" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Inactive account deleted by the user.", new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), "+201098765428" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Looking for a roommate in Sheikh Zayed area. Friendly and outgoing.", new DateTime(2000, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), "+201198765427" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Student at Cairo University, loves football and reading.", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+201012345671" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Graphic designer looking for a shared apartment in Alexandria.", new DateTime(2002, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), "+201112345672" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Assistant Administrator managing compliance and user verifications.", new DateTime(1995, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "+201198765431" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Software engineer looking for a room in Delta region near transport.", new DateTime(2003, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), "+201212345673" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Property owner offering premium apartments in Fifth Settlement, Cairo.", new DateTime(1980, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), "+201123456786" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Real estate investor with multiple listings in Alexandria and Giza.", new DateTime(1985, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), "+201223456787" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Providing high-quality rental options in Mohandessin and Dokki.", new DateTime(1970, 3, 30, 0, 0, 0, 0, DateTimeKind.Utc), "+201523456788" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Marketing specialist, quiet and clean, looking for a roommate in New Cairo.", new DateTime(2004, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), "+201512345674" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Engineering student looking for a cozy place in Damietta.", new DateTime(2005, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), "+201023456785" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { "Lead System Administrator for MARN platform.", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+201098765432" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 320m, 0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2001L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 85000m, 2 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2003L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 280m, 0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2005L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 250000m, 2 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2006L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 350m, 0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2008L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 130000m, 2 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2011L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 55000m, 2 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2013L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 72000m, 2 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2016L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 300m, 0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2018L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 420m, 0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2020L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 96000m, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "Bio", "DateOfBirth", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 6200m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2001L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 7200m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2003L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 5400m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2005L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 22000m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2006L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 7600m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2008L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 11500m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2011L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 5300m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2013L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 6900m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2016L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 6100m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2018L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 7900m, 1 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2020L,
                columns: new[] { "Price", "RentalUnit" },
                values: new object[] { 8600m, 1 });
        }
    }
}
