using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class deletedummyproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3001L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3002L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3003L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3004L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3005L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3006L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3007L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3008L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3009L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3010L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3011L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3012L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3013L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3014L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3015L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3016L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3017L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3018L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3019L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3020L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3021L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3022L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3023L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3024L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3025L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3026L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3027L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3028L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3029L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3030L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "ImagesDeletionJob", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[,]
                {
                    { 3001L, "Dummy Address 1", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 1", null, true, false, 30.010000000000002, 31.010000000000002, 2, new Guid("44444444-4444-4444-4444-444444444444"), 1000m, "/images/documents/dummy.jpg", 1, 80.0, "CairoGovernorate", 1, "Dummy Property 1", 0, 0, "11111" },
                    { 3002L, "Dummy Address 2", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 2", null, true, false, 30.02, 31.02, 1, new Guid("44444444-4444-4444-4444-444444444444"), 1500m, "/images/documents/dummy.jpg", 1, 50.0, "GizaGovernorate", 1, "Dummy Property 2", 4, 0, "11112" },
                    { 3003L, "Dummy Address 3", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 3", null, true, false, 30.030000000000001, 31.030000000000001, 4, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, "/images/documents/dummy.jpg", 1, 150.0, "AlexandriaGovernorate", 1, "Dummy Property 3", 1, 0, "11113" },
                    { 3004L, "Dummy Address 4", 3, 4, 5, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 4", null, true, false, 30.039999999999999, 31.039999999999999, 6, new Guid("44444444-4444-4444-4444-444444444444"), 10000m, "/images/documents/dummy.jpg", 1, 250.0, "SharkiaGovernorate", 1, "Dummy Property 4", 3, 0, "11114" },
                    { 3005L, "Dummy Address 5", 1, 1, 2, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 5", null, true, true, 30.050000000000001, 31.050000000000001, 2, new Guid("44444444-4444-4444-4444-444444444444"), 800m, "/images/documents/dummy.jpg", 1, 60.0, "DamiettaGovernorate", 1, "Dummy Property 5", 5, 0, "11115" },
                    { 3006L, "Dummy Address 6", 1, 2, 3, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 6", null, true, false, 30.059999999999999, 31.059999999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 1200m, "/images/documents/dummy.jpg", 1, 90.0, "CairoGovernorate", 1, "Dummy Property 6", 0, 0, "11116" },
                    { 3007L, "Dummy Address 7", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 7", null, true, false, 30.07, 31.07, 1, new Guid("44444444-4444-4444-4444-444444444444"), 1100m, "/images/documents/dummy.jpg", 1, 45.0, "GizaGovernorate", 1, "Dummy Property 7", 4, 0, "11117" },
                    { 3008L, "Dummy Address 8", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 8", null, true, false, 30.079999999999998, 31.079999999999998, 5, new Guid("44444444-4444-4444-4444-444444444444"), 4800m, "/images/documents/dummy.jpg", 1, 160.0, "AlexandriaGovernorate", 1, "Dummy Property 8", 1, 0, "11118" },
                    { 3009L, "Dummy Address 9", 4, 5, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 9", null, true, false, 30.09, 31.09, 8, new Guid("44444444-4444-4444-4444-444444444444"), 15000m, "/images/documents/dummy.jpg", 1, 300.0, "SharkiaGovernorate", 1, "Dummy Property 9", 3, 0, "11119" },
                    { 3010L, "Dummy Address 10", 1, 1, 3, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 10", null, true, true, 30.100000000000001, 31.100000000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 900m, "/images/documents/dummy.jpg", 1, 70.0, "DamiettaGovernorate", 1, "Dummy Property 10", 5, 0, "11120" },
                    { 3011L, "Dummy Address 11", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 11", null, true, false, 30.109999999999999, 31.109999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 1300m, "/images/documents/dummy.jpg", 1, 85.0, "CairoGovernorate", 1, "Dummy Property 11", 0, 0, "11121" },
                    { 3012L, "Dummy Address 12", 1, 1, 2, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 12", null, true, false, 30.120000000000001, 31.120000000000001, 2, new Guid("55555555-5555-5555-5555-555555555555"), 1600m, "/images/documents/dummy.jpg", 1, 55.0, "GizaGovernorate", 1, "Dummy Property 12", 4, 0, "11122" },
                    { 3013L, "Dummy Address 13", 2, 3, 5, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 13", null, true, false, 30.129999999999999, 31.129999999999999, 6, new Guid("55555555-5555-5555-5555-555555555555"), 5200m, "/images/documents/dummy.jpg", 1, 170.0, "AlexandriaGovernorate", 1, "Dummy Property 13", 1, 0, "11123" },
                    { 3014L, "Dummy Address 14", 4, 4, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 14", null, true, false, 30.140000000000001, 31.140000000000001, 8, new Guid("55555555-5555-5555-5555-555555555555"), 12000m, "/images/documents/dummy.jpg", 1, 280.0, "SharkiaGovernorate", 1, "Dummy Property 14", 3, 0, "11124" },
                    { 3015L, "Dummy Address 15", 1, 1, 2, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 15", null, true, true, 30.149999999999999, 31.149999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 850m, "/images/documents/dummy.jpg", 1, 65.0, "DamiettaGovernorate", 1, "Dummy Property 15", 5, 0, "11125" },
                    { 3016L, "Dummy Address 16", 2, 3, 4, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 16", null, true, false, 30.16, 31.16, 4, new Guid("55555555-5555-5555-5555-555555555555"), 2500m, "/images/documents/dummy.jpg", 1, 120.0, "CairoGovernorate", 1, "Dummy Property 16", 0, 0, "11126" },
                    { 3017L, "Dummy Address 17", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 17", null, true, false, 30.170000000000002, 31.170000000000002, 1, new Guid("55555555-5555-5555-5555-555555555555"), 1000m, "/images/documents/dummy.jpg", 1, 40.0, "GizaGovernorate", 1, "Dummy Property 17", 4, 0, "11127" },
                    { 3018L, "Dummy Address 18", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 18", null, true, false, 30.18, 31.18, 5, new Guid("55555555-5555-5555-5555-555555555555"), 4600m, "/images/documents/dummy.jpg", 1, 155.0, "AlexandriaGovernorate", 1, "Dummy Property 18", 1, 0, "11128" },
                    { 3019L, "Dummy Address 19", 5, 5, 8, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 19", null, true, false, 30.190000000000001, 31.190000000000001, 10, new Guid("55555555-5555-5555-5555-555555555555"), 18000m, "/images/documents/dummy.jpg", 1, 350.0, "SharkiaGovernorate", 1, "Dummy Property 19", 3, 0, "11129" },
                    { 3020L, "Dummy Address 20", 2, 2, 4, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 20", null, true, true, 30.199999999999999, 31.199999999999999, 4, new Guid("55555555-5555-5555-5555-555555555555"), 950m, "/images/documents/dummy.jpg", 1, 80.0, "DamiettaGovernorate", 1, "Dummy Property 20", 5, 0, "11130" },
                    { 3021L, "Dummy Address 21", 1, 2, 3, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 21", null, true, false, 30.210000000000001, 31.210000000000001, 3, new Guid("66666666-6666-6666-6666-666666666666"), 1800m, "/images/documents/dummy.jpg", 1, 95.0, "CairoGovernorate", 1, "Dummy Property 21", 0, 0, "11131" },
                    { 3022L, "Dummy Address 22", 1, 1, 2, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 22", null, true, false, 30.219999999999999, 31.219999999999999, 2, new Guid("66666666-6666-6666-6666-666666666666"), 1400m, "/images/documents/dummy.jpg", 1, 48.0, "GizaGovernorate", 1, "Dummy Property 22", 4, 0, "11132" },
                    { 3023L, "Dummy Address 23", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 23", null, true, false, 30.23, 31.23, 5, new Guid("66666666-6666-6666-6666-666666666666"), 4900m, "/images/documents/dummy.jpg", 1, 165.0, "AlexandriaGovernorate", 1, "Dummy Property 23", 1, 0, "11133" },
                    { 3024L, "Dummy Address 24", 3, 4, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 24", null, true, false, 30.239999999999998, 31.239999999999998, 8, new Guid("66666666-6666-6666-6666-666666666666"), 11000m, "/images/documents/dummy.jpg", 1, 270.0, "SharkiaGovernorate", 1, "Dummy Property 24", 3, 0, "11134" },
                    { 3025L, "Dummy Address 25", 1, 2, 3, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 25", null, true, true, 30.25, 31.25, 3, new Guid("66666666-6666-6666-6666-666666666666"), 880m, "/images/documents/dummy.jpg", 1, 72.0, "DamiettaGovernorate", 1, "Dummy Property 25", 5, 0, "11135" },
                    { 3026L, "Dummy Address 26", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 26", null, true, false, 30.260000000000002, 31.260000000000002, 2, new Guid("66666666-6666-6666-6666-666666666666"), 1700m, "/images/documents/dummy.jpg", 1, 78.0, "CairoGovernorate", 1, "Dummy Property 26", 0, 0, "11136" },
                    { 3027L, "Dummy Address 27", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 27", null, true, false, 30.27, 31.27, 1, new Guid("66666666-6666-6666-6666-666666666666"), 1250m, "/images/documents/dummy.jpg", 1, 42.0, "GizaGovernorate", 1, "Dummy Property 27", 4, 0, "11137" },
                    { 3028L, "Dummy Address 28", 2, 3, 5, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 28", null, true, false, 30.280000000000001, 31.280000000000001, 6, new Guid("66666666-6666-6666-6666-666666666666"), 5500m, "/images/documents/dummy.jpg", 1, 180.0, "AlexandriaGovernorate", 1, "Dummy Property 28", 1, 0, "11138" },
                    { 3029L, "Dummy Address 29", 6, 6, 10, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 29", null, true, false, 30.289999999999999, 31.289999999999999, 12, new Guid("66666666-6666-6666-6666-666666666666"), 22000m, "/images/documents/dummy.jpg", 1, 400.0, "SharkiaGovernorate", 1, "Dummy Property 29", 3, 0, "11139" },
                    { 3030L, "Dummy Address 30", 2, 2, 4, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 30", null, true, true, 30.300000000000001, 31.300000000000001, 4, new Guid("66666666-6666-6666-6666-666666666666"), 920m, "/images/documents/dummy.jpg", 1, 85.0, "DamiettaGovernorate", 1, "Dummy Property 30", 5, 0, "11140" }
                });
        }
    }
}
