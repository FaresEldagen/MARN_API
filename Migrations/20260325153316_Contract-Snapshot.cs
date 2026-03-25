using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class ContractSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Snapshot",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3001L,
                column: "Snapshot",
                value: "{\"RenterSnapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"FullName\":\"Renter Alpha\",\"Email\":\"renter.a@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":\"12345678901234\",\"ArabicFullName\":\"\\u0631\\u064A\\u0646\\u062A\\u0631 \\u0623\\u0644\\u0641\\u0627\",\"ArabicAddress\":\"123 \\u0634\\u0627\\u0631\\u0639 \\u0627\\u0644\\u0646\\u064A\\u0644\\u060C \\u0627\\u0644\\u0642\\u0627\\u0647\\u0631\\u0629\",\"ProfileImage\":\"/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png\"},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1001,\"Title\":\"Cozy Seed Apartment\",\"Description\":\"A cozy seeded apartment suitable for testing active rentals.\",\"Address\":\"123 Seed Street, Cairo\",\"Price\":5000,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Apartment\",\"Bedrooms\":2,\"Bathrooms\":1,\"Beds\":3}}");

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3002L,
                column: "Snapshot",
                value: "{\"RenterSnapshot\":{\"Id\":\"33333333-3333-3333-3333-333333333333\",\"FullName\":\"Renter Gamma\",\"Email\":\"renter.c@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1002,\"Title\":\"Modern Seed Loft\",\"Description\":\"A modern loft used for pending booking and payments tests.\",\"Address\":\"456 Integration Avenue, Cairo\",\"Price\":7500,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Apartment\",\"Bedrooms\":1,\"Bathrooms\":1,\"Beds\":1}}");

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3003L,
                column: "Snapshot",
                value: "{\"RenterSnapshot\":{\"Id\":\"11111111-1111-1111-1111-111111111111\",\"FullName\":\"Renter Alpha\",\"Email\":\"renter.a@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":\"12345678901234\",\"ArabicFullName\":\"\\u0631\\u064A\\u0646\\u062A\\u0631 \\u0623\\u0644\\u0641\\u0627\",\"ArabicAddress\":\"123 \\u0634\\u0627\\u0631\\u0639 \\u0627\\u0644\\u0646\\u064A\\u0644\\u060C \\u0627\\u0644\\u0642\\u0627\\u0647\\u0631\\u0629\",\"ProfileImage\":\"/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png\"},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1003,\"Title\":\"Seed Studio Flat\",\"Description\":\"A small studio property used for saved properties and pending bookings.\",\"Address\":\"789 Scenario Road, Cairo\",\"Price\":3500,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Studio\",\"Bedrooms\":1,\"Bathrooms\":1,\"Beds\":1}}");

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3004L,
                column: "Snapshot",
                value: "{\"RenterSnapshot\":{\"Id\":\"66666666-6666-6666-6666-666666666666\",\"FullName\":\"Owner Z\",\"Email\":\"owner.z@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"OwnerSnapshot\":{\"Id\":\"44444444-4444-4444-4444-444444444444\",\"FullName\":\"Owner X\",\"Email\":\"owner.x@example.com\",\"PhoneNumber\":null,\"NationalIDNumber\":null,\"ArabicFullName\":null,\"ArabicAddress\":null,\"ProfileImage\":null},\"PropertySnapshot\":{\"Id\":1001,\"Title\":\"Cozy Seed Apartment\",\"Description\":\"A cozy seeded apartment suitable for testing active rentals.\",\"Address\":\"123 Seed Street, Cairo\",\"Price\":5000,\"RentalUnit\":\"Monthly\",\"PropertyType\":\"Apartment\",\"Bedrooms\":2,\"Bathrooms\":1,\"Beds\":3}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Snapshot",
                table: "Contracts");
        }
    }
}
