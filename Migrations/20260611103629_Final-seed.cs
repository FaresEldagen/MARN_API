using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class Finalseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), new Guid("10000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2001L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2002L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2003L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2004L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2005L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 2006L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-2222-3333-444444444444"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "ConcurrencyStamp", "CreatedAt", "FirstName", "Gender", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp" },
                values: new object[] { "مدينة نصر، القاهرة", "خالد قيد الانتظار", "SCENARIO-PENDING-CONCURRENCY-STAMP", new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Khaled", 1, "Pending", "30001010101010", false, "/images/profiles/pending-renter.png", "SCENARIO-PENDING-SECURITY-STAMP" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "ConcurrencyStamp", "CreatedAt", "FirstName", "Language", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan" },
                values: new object[] { "شبرا، القاهرة", "سيد محظور", "SCENARIO-BANNED-CONCURRENCY-STAMP", new DateTime(2026, 3, 5, 14, 0, 0, 0, DateTimeKind.Utc), "Sayed", 1, "Banned", "30002020202020", false, "/images/profiles/banned-renter.png", "SCENARIO-BANNED-SECURITY-STAMP", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp" },
                values: new object[] { "العجوزة، الجيزة", "رامي محذوف", "/images/idCards/deleted-renter-back.jpg", "SCENARIO-DELETED-CONCURRENCY-STAMP", new DateTime(2026, 2, 20, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Ramy", "/images/idCards/deleted-renter-front.jpg", 1, "Deleted", "30003030303030", false, "/images/profiles/deleted-renter.png", "SCENARIO-DELETED-SECURITY-STAMP" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "CreatedAt", "FirstName", "FrontIdPhoto", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp" },
                values: new object[] { "الشيخ زايد، الجيزة", "نور حديث", "/images/idCards/recent-renter-back.jpg", null, "SCENARIO-RECENT-CONCURRENCY-STAMP", new DateTime(2026, 5, 25, 16, 0, 0, 0, DateTimeKind.Utc), "Nour", "/images/idCards/recent-renter-front.jpg", "Recent", "30004040404040", false, "/images/profiles/recent-renter.png", "SCENARIO-RECENT-SECURITY-STAMP" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "المعادي، القاهرة", "كريم حسن", "/images/idCards/user-cairo-mid-back.jpg", "Karim", "/images/idCards/user-cairo-mid-front.jpg", 0, "Hassan", "30101010101010", "/images/profiles/user-cairo-mid.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "سيدي جابر، الإسكندرية", "مريم فؤاد", "/images/idCards/user-alex-low-back.jpg", "Mariam", "/images/idCards/user-alex-low-front.jpg", "Fouad", "30202020202020", "/images/profiles/user-alex-low.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Gender", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "الدقي، الجيزة", "منى أدمن", "/images/idCards/assistant-admin-back.jpg", "Mona", "/images/idCards/assistant-admin-front.jpg", 2, "29502020202020", "/images/profiles/assistant-admin.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "الزقازيق، الشرقية", "أحمد نبيل", "/images/idCards/user-delta-multi-back.jpg", "Ahmed", "/images/idCards/user-delta-multi-front.jpg", 1, "Nabil", "30303030303030", "/images/profiles/user-delta-multi.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "التجمع الخامس، القاهرة", "محمود فهمي", "/images/idCards/owner-x-back.jpg", "Mahmoud", "/images/idCards/owner-x-front.jpg", 1, "Fahmy", "28010101010101", "/images/profiles/owner-x.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "سموحة، الإسكندرية", "هبة يوسف", "/images/idCards/owner-y-back.jpg", "Heba", "/images/idCards/owner-y-front.jpg", "Youssef", "28502020202020", "/images/profiles/owner-y.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "المهندسين، الجيزة", "طارق زكي", "/images/idCards/owner-z-back.jpg", "Tarek", "/images/idCards/owner-z-front.jpg", 1, "Zaki", "27003030303030", "/images/profiles/owner-z.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Gender", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "القاهرة الجديدة، القاهرة", "سارة عادل", "/images/idCards/user-family-high-back.jpg", "Sara", "/images/idCards/user-family-high-front.jpg", 2, "Adel", "30404040404040", "/images/profiles/user-family-high.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "رأس البر، دمياط", "عمر سمير", "/images/idCards/user-coastal-flex-back.jpg", "Omar", "/images/idCards/user-coastal-flex-front.jpg", 1, "Samir", "30505050505050", "/images/profiles/user-coastal-flex.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Gender", "Language", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "مصر الجديدة، القاهرة", "ياسر أدمن", "/images/idCards/admin-back.jpg", "Yasser", "/images/idCards/admin-front.jpg", 1, 1, "29010101010101", "/images/profiles/admin.png" });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileName", "FilePath", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFilePath", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[] { 1000103L, new DateTime(2026, 5, 22, 9, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 5, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed-contract-1000103.pdf", null, "SEEDHASH1000103BANNEDRENTERDASHBOARD", new DateOnly(2026, 12, 1), new DateOnly(2026, 6, 1), null, null, 1, 1205L, new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 5, 21, 10, 0, 0, 0, DateTimeKind.Utc), 1, 30000m, null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                columns: new[] { "Address", "Description", "Latitude", "Longitude", "ProofOfOwnership", "Title", "ZipCode" },
                values: new object[] { "123 26th of July St, Zamalek, Cairo", "A cozy seeded apartment with a wonderful Nile view in Zamalek.", 30.0626, 31.222999999999999, "/images/documents/property1001-POO.jpg", "Zamalek Riverside Apartment", "11211" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                columns: new[] { "Address", "City", "Description", "Latitude", "Longitude", "Price", "ProofOfOwnership", "RentalUnit", "State", "Title", "ZipCode" },
                values: new object[] { "45 Tahrir St, Dokki, Giza", "Giza", "A modern loft in the heart of Dokki used for testing.", 30.038399999999999, 31.211400000000001, 9000m, "/images/documents/property1002-POO.jpg", 1, "GizaGovernorate", "Dokki Modern Loft", "12311" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                columns: new[] { "Address", "Description", "Latitude", "Longitude", "ProofOfOwnership", "Title", "ZipCode" },
                values: new object[] { "78 Arab League St, Mohandeseen, Giza", "A small studio property in Mohandeseen.", 30.055800000000001, 31.200099999999999, "/images/documents/property1003-POO.jpg", "Mohandeseen Studio Flat", "12411" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                columns: new[] { "Address", "City", "Description", "Latitude", "Longitude", "Price", "ProofOfOwnership", "State", "Title", "ZipCode" },
                values: new object[] { "Beverly Hills, Sheikh Zayed, Giza", "Sheikh Zayed", "A luxury villa in Sheikh Zayed owned by the dual-role Owner Z.", 30.052, 30.984999999999999, 35000m, "/images/documents/property1004-POO.jpg", "GizaGovernorate", "Sheikh Zayed Luxury Villa", "12588" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1100L,
                columns: new[] { "Address", "City", "Description", "Latitude", "Longitude", "ProofOfOwnership", "State", "Title", "ZipCode" },
                values: new object[] { "15 Nile Corniche, Agouza, Giza", "Giza", "A shared house in Agouza for testing roommate matching logic.", 30.046800000000001, 31.213100000000001, "/images/documents/property1100-POO.jpg", "GizaGovernorate", "Agouza Shared House", "12611" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L,
                column: "ProofOfOwnership",
                value: "/images/documents/property1201-POO.jpg");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1202L,
                column: "ProofOfOwnership",
                value: "/images/documents/property1202-POO.jpg");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1203L,
                column: "ProofOfOwnership",
                value: "/images/documents/property1203-POO.jpg");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1204L,
                column: "ProofOfOwnership",
                value: "/images/documents/property1204-POO.jpg");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1205L,
                column: "ProofOfOwnership",
                value: "/images/documents/property1205-POO.jpg");

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "ImagesDeletionJob", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[,]
                {
                    { 2001L, "15 Tahrir Square", 1, 2, 3, "Cairo", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A bright apartment near downtown Cairo for monthly stays", null, true, false, 30.0444, 31.235700000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 7200m, "/images/documents/property2001-POO.jpg", 1, 118.0, "CairoGovernorate", 1, "Nile View Apartment", 0, 14, "11511" },
                    { 2002L, "22 Al Ahram Street", 2, 3, 4, "Heliopolis", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "Comfortable family flat close to shops and transit", null, true, false, 30.091899999999999, 31.317399999999999, 4, new Guid("66666666-6666-6666-6666-666666666666"), 9800m, "/images/documents/property2002-POO.jpg", 1, 146.0, "CairoGovernorate", 1, "Heliopolis Family Flat", 0, 9, "11757" },
                    { 2003L, "8 Road 9", 1, 1, 1, "Maadi", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "Quiet studio with easy access to Maadi services", null, true, false, 29.9602, 31.256900000000002, 2, new Guid("44444444-4444-4444-4444-444444444444"), 5400m, "/images/documents/property2003-POO.jpg", 1, 62.0, "CairoGovernorate", 1, "Maadi Garden Studio", 4, 7, "11431" },
                    { 2004L, "41 Makram Ebeid Street", 1, 1, 3, "Nasr City", new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "Shared loft suited for students and young professionals", null, true, true, 30.0626, 31.330100000000002, 3, new Guid("66666666-6666-6666-6666-666666666666"), 3200m, "/images/documents/property2004-POO.jpg", 1, 88.0, "CairoGovernorate", 0, "Nasr City Shared Loft", 5, 5, "11765" },
                    { 2005L, "10 South 90 Street", 3, 4, 5, "New Cairo", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "Modern villa in a quiet New Cairo neighborhood", null, true, false, 30.030000000000001, 31.469999999999999, 6, new Guid("44444444-4444-4444-4444-444444444444"), 22000m, "/images/documents/property2005-POO.jpg", 1, 285.0, "CairoGovernorate", 1, "New Cairo Corner Villa", 3, 11, "11835" },
                    { 2006L, "33 Corniche Road", 1, 2, 2, "Alexandria", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sea facing apartment near the Alexandria Corniche", null, true, false, 31.200099999999999, 29.918700000000001, 3, new Guid("66666666-6666-6666-6666-666666666666"), 7600m, "/images/documents/property2006-POO.jpg", 1, 110.0, "AlexandriaGovernorate", 1, "Corniche Sea Apartment", 0, 18, "21519" },
                    { 2007L, "12 El Horreya Road", 2, 2, 3, "Sidi Gaber", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "Well placed flat near transport and universities", null, true, false, 31.215599999999998, 29.942, 4, new Guid("44444444-4444-4444-4444-444444444444"), 8100m, "/images/documents/property2007-POO.jpg", 1, 124.0, "AlexandriaGovernorate", 1, "Sidi Gaber Urban Flat", 0, 12, "21615" },
                    { 2008L, "27 Fawzy Moaz Street", 2, 3, 4, "Smouha", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Spacious residence overlooking a calm residential area", null, true, false, 31.215, 29.955300000000001, 5, new Guid("66666666-6666-6666-6666-666666666666"), 11500m, "/images/documents/property2008-POO.jpg", 1, 172.0, "AlexandriaGovernorate", 1, "Smouha Park Residence", 1, 10, "21646" },
                    { 2009L, "50 Khaled Ibn Al Walid Street", 1, 1, 1, "Miami", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, "Compact studio within walking distance of the beach", null, true, false, 31.267700000000001, 30.0046, 2, new Guid("44444444-4444-4444-4444-444444444444"), 4900m, "/images/documents/property2009-POO.jpg", 1, 58.0, "AlexandriaGovernorate", 0, "Miami Beach Studio", 4, 16, "21919" },
                    { 2010L, "6 Malek Hefny Street", 3, 4, 6, "Montaza", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "Large villa near the Montaza district gardens", null, true, false, 31.285399999999999, 30.017299999999999, 7, new Guid("66666666-6666-6666-6666-666666666666"), 24500m, "/images/documents/property2010-POO.jpg", 1, 310.0, "AlexandriaGovernorate", 1, "Montaza Family Villa", 3, 8, "21923" },
                    { 2011L, "14 Talat Harb Street", 1, 2, 2, "Zagazig", new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "Clean apartment close to central Zagazig amenities", null, true, false, 30.587700000000002, 31.501999999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5300m, "/images/documents/property2011-POO.jpg", 1, 102.0, "SharkiaGovernorate", 1, "Zagazig Central Apartment", 0, 6, "44511" },
                    { 2012L, "88 Industrial Zone Road", 1, 1, 1, "Tenth of Ramadan", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "Practical loft near business zones in 10th of Ramadan", null, true, false, 30.304500000000001, 31.742000000000001, 2, new Guid("66666666-6666-6666-6666-666666666666"), 4700m, "/images/documents/property2012-POO.jpg", 1, 76.0, "SharkiaGovernorate", 0, "Tenth District Loft", 0, 4, "44629" },
                    { 2013L, "19 Saad Zaghloul Street", 2, 3, 4, "Belbeis", new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "Traditional house with a private courtyard and storage", null, true, false, 30.420400000000001, 31.562200000000001, 5, new Guid("44444444-4444-4444-4444-444444444444"), 6900m, "/images/documents/property2013-POO.jpg", 1, 180.0, "SharkiaGovernorate", 1, "Belbeis Courtyard House", 1, 3, "44621" },
                    { 2014L, "9 El Geish Street", 2, 2, 4, "Minya Al Qamh", new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "Shared home designed for longer affordable stays", null, true, true, 30.422799999999999, 31.369700000000002, 4, new Guid("66666666-6666-6666-6666-666666666666"), 2600m, "/images/documents/property2014-POO.jpg", 1, 130.0, "SharkiaGovernorate", 1, "Minya Al Qamh Shared Home", 5, 2, "44661" },
                    { 2015L, "31 Port Said Street", 1, 2, 3, "Abu Hammad", new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bright flat near local markets and key roads", null, true, false, 30.536899999999999, 31.683499999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5100m, "/images/documents/property2015-POO.jpg", 1, 97.0, "SharkiaGovernorate", 1, "Abu Hammad Riverside Flat", 0, 5, "44671" },
                    { 2016L, "18 El Galaa Street", 1, 2, 2, "Damietta", new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "Modern apartment near the city center and port", null, true, false, 31.416499999999999, 31.813300000000002, 3, new Guid("66666666-6666-6666-6666-666666666666"), 6100m, "/images/documents/property2016-POO.jpg", 1, 108.0, "DamiettaGovernorate", 1, "Damietta Port Apartment", 0, 7, "34511" },
                    { 2017L, "5 Nile Street", 1, 1, 1, "Ras El Bar", new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, "Compact studio ideal for short coastal stays", null, true, false, 31.508500000000002, 31.840399999999999, 2, new Guid("44444444-4444-4444-4444-444444444444"), 3800m, "/images/documents/property2017-POO.jpg", 0, 54.0, "DamiettaGovernorate", 1, "Ras El Bar Summer Studio", 4, 13, "34711" },
                    { 2018L, "44 Al Gamea Street", 2, 3, 3, "New Damietta", new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, "Contemporary flat in a newer planned district", null, true, false, 31.445599999999999, 31.676400000000001, 4, new Guid("66666666-6666-6666-6666-666666666666"), 7900m, "/images/documents/property2018-POO.jpg", 1, 138.0, "DamiettaGovernorate", 0, "New Damietta Corner Flat", 0, 4, "34517" },
                    { 2019L, "11 Mostafa Kamel Street", 2, 3, 4, "Kafr Saad", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "Well sized family house with a practical layout", null, true, false, 31.355399999999999, 31.676300000000001, 5, new Guid("44444444-4444-4444-4444-444444444444"), 7300m, "/images/documents/property2019-POO.jpg", 1, 192.0, "DamiettaGovernorate", 1, "Kafr Saad Family House", 1, 3, "34614" },
                    { 2020L, "7 Omar Ibn El Khattab Street", 3, 4, 5, "Faraskur", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "Large home with generous indoor living space", null, true, false, 31.329000000000001, 31.715900000000001, 6, new Guid("66666666-6666-6666-6666-666666666666"), 8600m, "/images/documents/property2020-POO.jpg", 1, 230.0, "DamiettaGovernorate", 1, "Faraskur Riverside Home", 1, 2, "34631" }
                });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 3001L, true, "/images/properties/property1001-main.jpg", 1001L },
                    { 3002L, false, "/images/properties/property1001-sec.jpg", 1001L },
                    { 3003L, true, "/images/properties/property1002-main.jpg", 1002L },
                    { 3004L, false, "/images/properties/property1002-sec.jpg", 1002L },
                    { 3005L, true, "/images/properties/property1003-main.jpg", 1003L },
                    { 3006L, false, "/images/properties/property1003-sec.jpg", 1003L },
                    { 3007L, true, "/images/properties/property1004-main.jpg", 1004L },
                    { 3008L, false, "/images/properties/property1004-sec.jpg", 1004L },
                    { 3009L, true, "/images/properties/property1100-main.jpg", 1100L },
                    { 3010L, false, "/images/properties/property1100-sec.jpg", 1100L },
                    { 3051L, true, "/images/properties/property1201-main.jpg", 1201L },
                    { 3052L, false, "/images/properties/property1201-sec.jpg", 1201L },
                    { 3053L, true, "/images/properties/property1202-main.jpg", 1202L },
                    { 3054L, false, "/images/properties/property1202-sec.jpg", 1202L },
                    { 3055L, true, "/images/properties/property1203-main.jpg", 1203L },
                    { 3056L, false, "/images/properties/property1203-sec.jpg", 1203L },
                    { 3057L, true, "/images/properties/property1204-main.jpg", 1204L },
                    { 3058L, false, "/images/properties/property1204-sec.jpg", 1204L },
                    { 3059L, true, "/images/properties/property1205-main.jpg", 1205L },
                    { 3060L, false, "/images/properties/property1205-sec.jpg", 1205L }
                });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9105L,
                column: "ReportableGuidId",
                value: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.InsertData(
                table: "SavedProperties",
                columns: new[] { "PropertyId", "UserId" },
                values: new object[] { 1004L, new Guid("10000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "UserActivities",
                columns: new[] { "Id", "CreatedAt", "Metadata", "PropertyId", "UserActivityType", "UserId" },
                values: new object[,]
                {
                    { 4001L, new DateTime(2026, 5, 21, 9, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"cairo apartment\",\"governorate\":\"CairoGovernorate\",\"minPrice\":5000,\"maxPrice\":9000,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":30.0444,\"longitude\":31.2357,\"radiusKm\":15,\"page\":1,\"pageSize\":20}", null, "search", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4002L, new DateTime(2026, 5, 21, 9, 2, 0, 0, DateTimeKind.Utc), null, 2001L, "view", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4003L, new DateTime(2026, 5, 21, 9, 5, 0, 0, DateTimeKind.Utc), null, 2003L, "save", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4004L, new DateTime(2026, 5, 21, 9, 9, 0, 0, DateTimeKind.Utc), null, 2002L, "booking", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4005L, new DateTime(2026, 5, 22, 10, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex studio\",\"governorate\":\"AlexandriaGovernorate\",\"maxPrice\":6000,\"type\":\"Studio\",\"rentalUnit\":\"Monthly\",\"latitude\":31.2001,\"longitude\":29.9187,\"radiusKm\":12,\"page\":1,\"pageSize\":20}", null, "search", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4006L, new DateTime(2026, 5, 22, 10, 3, 0, 0, DateTimeKind.Utc), null, 2009L, "view", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4007L, new DateTime(2026, 5, 22, 10, 5, 0, 0, DateTimeKind.Utc), null, 2009L, "save", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4008L, new DateTime(2026, 5, 22, 10, 11, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex apartment\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":5000,\"maxPrice\":8000,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"page\":1,\"pageSize\":20}", null, "search", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4009L, new DateTime(2026, 5, 23, 11, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"zagazig apartment\",\"governorate\":\"SharkiaGovernorate\",\"minPrice\":4500,\"maxPrice\":7000,\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":30.5877,\"longitude\":31.5020,\"radiusKm\":20,\"page\":1,\"pageSize\":20}", null, "search", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4010L, new DateTime(2026, 5, 23, 11, 2, 0, 0, DateTimeKind.Utc), null, 2011L, "view", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4011L, new DateTime(2026, 5, 23, 11, 10, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"damietta apartment\",\"governorate\":\"DamiettaGovernorate\",\"minPrice\":4500,\"maxPrice\":8000,\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":31.4165,\"longitude\":31.8133,\"radiusKm\":25,\"page\":1,\"pageSize\":20}", null, "search", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4012L, new DateTime(2026, 5, 23, 11, 14, 0, 0, DateTimeKind.Utc), null, 2016L, "save", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4013L, new DateTime(2026, 5, 24, 12, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"cairo villa\",\"governorate\":\"CairoGovernorate\",\"minPrice\":15000,\"maxPrice\":26000,\"type\":\"Villa\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":4,\"minBathrooms\":3,\"latitude\":30.0300,\"longitude\":31.4700,\"radiusKm\":20,\"page\":1,\"pageSize\":20}", null, "search", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4014L, new DateTime(2026, 5, 24, 12, 4, 0, 0, DateTimeKind.Utc), null, 2005L, "view", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4015L, new DateTime(2026, 5, 24, 12, 10, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex villa\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":18000,\"maxPrice\":26000,\"type\":\"Villa\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":4,\"minBathrooms\":3,\"latitude\":31.2854,\"longitude\":30.0173,\"radiusKm\":18,\"page\":1,\"pageSize\":20}", null, "search", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4016L, new DateTime(2026, 5, 24, 12, 15, 0, 0, DateTimeKind.Utc), null, 2010L, "booking", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4017L, new DateTime(2026, 5, 25, 13, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"ras el bar studio\",\"governorate\":\"DamiettaGovernorate\",\"maxPrice\":5000,\"type\":\"Studio\",\"rentalUnit\":\"Daily\",\"latitude\":31.5085,\"longitude\":31.8404,\"radiusKm\":10,\"page\":1,\"pageSize\":20}", null, "search", new Guid("88888888-8888-8888-8888-888888888888") },
                    { 4018L, new DateTime(2026, 5, 25, 13, 3, 0, 0, DateTimeKind.Utc), null, 2017L, "view", new Guid("88888888-8888-8888-8888-888888888888") },
                    { 4019L, new DateTime(2026, 5, 25, 13, 12, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex coast apartment\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":6000,\"maxPrice\":8500,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"latitude\":31.2156,\"longitude\":29.9420,\"radiusKm\":15,\"page\":1,\"pageSize\":20}", null, "search", new Guid("88888888-8888-8888-8888-888888888888") },
                    { 4020L, new DateTime(2026, 5, 25, 13, 16, 0, 0, DateTimeKind.Utc), null, 2006L, "save", new Guid("88888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "PaymentSchedules",
                columns: new[] { "Id", "Amount", "ContractId", "Currency", "DueDate", "PaymentIntentId", "Status" },
                values: new object[] { 20108L, 5000m, 1000103L, "egp", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 3011L, true, "/images/properties/property2001-main.jpg", 2001L },
                    { 3012L, false, "/images/properties/property2001-sec.jpg", 2001L },
                    { 3013L, true, "/images/properties/property2002-main.jpg", 2002L },
                    { 3014L, false, "/images/properties/property2002-sec.jpg", 2002L },
                    { 3015L, true, "/images/properties/property2003-main.jpg", 2003L },
                    { 3016L, false, "/images/properties/property2003-sec.jpg", 2003L },
                    { 3017L, true, "/images/properties/property2004-main.jpg", 2004L },
                    { 3018L, false, "/images/properties/property2004-sec.jpg", 2004L },
                    { 3019L, true, "/images/properties/property2005-main.jpg", 2005L },
                    { 3020L, false, "/images/properties/property2005-sec.jpg", 2005L },
                    { 3021L, true, "/images/properties/property2006-main.jpg", 2006L },
                    { 3022L, false, "/images/properties/property2006-sec.jpg", 2006L },
                    { 3023L, true, "/images/properties/property2007-main.jpg", 2007L },
                    { 3024L, false, "/images/properties/property2007-sec.jpg", 2007L },
                    { 3025L, true, "/images/properties/property2008-main.jpg", 2008L },
                    { 3026L, false, "/images/properties/property2008-sec.jpg", 2008L },
                    { 3027L, true, "/images/properties/property2009-main.jpg", 2009L },
                    { 3028L, false, "/images/properties/property2009-sec.jpg", 2009L },
                    { 3029L, true, "/images/properties/property2010-main.jpg", 2010L },
                    { 3030L, false, "/images/properties/property2010-sec.jpg", 2010L },
                    { 3031L, true, "/images/properties/property2011-main.jpg", 2011L },
                    { 3032L, false, "/images/properties/property2011-sec.jpg", 2011L },
                    { 3033L, true, "/images/properties/property2012-main.jpg", 2012L },
                    { 3034L, false, "/images/properties/property2012-sec.jpg", 2012L },
                    { 3035L, true, "/images/properties/property2013-main.jpg", 2013L },
                    { 3036L, false, "/images/properties/property2013-sec.jpg", 2013L },
                    { 3037L, true, "/images/properties/property2014-main.jpg", 2014L },
                    { 3038L, false, "/images/properties/property2014-sec.jpg", 2014L },
                    { 3039L, true, "/images/properties/property2015-main.jpg", 2015L },
                    { 3040L, false, "/images/properties/property2015-sec.jpg", 2015L },
                    { 3041L, true, "/images/properties/property2016-main.jpg", 2016L },
                    { 3042L, false, "/images/properties/property2016-sec.jpg", 2016L },
                    { 3043L, true, "/images/properties/property2017-main.jpg", 2017L },
                    { 3044L, false, "/images/properties/property2017-sec.jpg", 2017L },
                    { 3045L, true, "/images/properties/property2018-main.jpg", 2018L },
                    { 3046L, false, "/images/properties/property2018-sec.jpg", 2018L },
                    { 3047L, true, "/images/properties/property2019-main.jpg", 2019L },
                    { 3048L, false, "/images/properties/property2019-sec.jpg", 2019L },
                    { 3049L, true, "/images/properties/property2020-main.jpg", 2020L },
                    { 3050L, false, "/images/properties/property2020-sec.jpg", 2020L }
                });

            migrationBuilder.InsertData(
                table: "SavedProperties",
                columns: new[] { "PropertyId", "UserId" },
                values: new object[] { 2001L, new Guid("11111111-1111-1111-1111-111111111111") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentSchedules",
                keyColumn: "Id",
                keyValue: 20108L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3001L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3002L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3003L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3004L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3005L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3006L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3007L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3008L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3009L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3010L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3011L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3012L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3013L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3014L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3015L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3016L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3017L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3018L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3019L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3020L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3021L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3022L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3023L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3024L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3025L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3026L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3027L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3028L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3029L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3030L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3031L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3032L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3033L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3034L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3035L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3036L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3037L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3038L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3039L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3040L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3041L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3042L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3043L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3044L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3045L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3046L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3047L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3048L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3049L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3050L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3051L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3052L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3053L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3054L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3055L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3056L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3057L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3058L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3059L);

            migrationBuilder.DeleteData(
                table: "PropertyMedia",
                keyColumn: "Id",
                keyValue: 3060L);

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 1004L, new Guid("10000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "SavedProperties",
                keyColumns: new[] { "PropertyId", "UserId" },
                keyValues: new object[] { 2001L, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4001L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4002L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4003L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4004L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4005L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4006L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4007L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4008L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4009L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4010L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4011L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4012L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4013L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4014L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4015L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4016L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4017L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4018L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4019L);

            migrationBuilder.DeleteData(
                table: "UserActivities",
                keyColumn: "Id",
                keyValue: 4020L);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1000103L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2001L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2002L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2003L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2004L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2005L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2006L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2007L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2008L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2009L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2010L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2011L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2012L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2013L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2014L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2015L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2016L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2017L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2018L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2019L);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2020L);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), null, "Moderator", "MODERATOR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "ConcurrencyStamp", "CreatedAt", "FirstName", "Gender", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp" },
                values: new object[] { "15 شارع التسعين، القاهرة الجديدة", "مستخدم قيد التحقق", "SCENARIO-PENDING-RENTER-CONCURRENCY-STAMP", new DateTime(2026, 5, 2, 9, 0, 0, 0, DateTimeKind.Utc), "Pending", 2, "Renter", "34567890123456", true, null, "SCENARIO-PENDING-RENTER-SECURITY-STAMP" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "ConcurrencyStamp", "CreatedAt", "FirstName", "Language", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan" },
                values: new object[] { "22 شارع النصر، مدينة نصر", "مستخدم موقوف", "SCENARIO-BANNED-RENTER-CONCURRENCY-STAMP", new DateTime(2026, 2, 14, 10, 0, 0, 0, DateTimeKind.Utc), "Banned", 0, "Renter", "45678901234567", true, null, "SCENARIO-BANNED-RENTER-SECURITY-STAMP", 1 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp" },
                values: new object[] { null, null, null, "SCENARIO-DELETED-RENTER-CONCURRENCY-STAMP", new DateTime(2026, 3, 3, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Deleted", null, 0, "Renter", null, true, null, "SCENARIO-DELETED-RENTER-SECURITY-STAMP" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "CreatedAt", "FirstName", "FrontIdPhoto", "LastName", "NationalIDNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp" },
                values: new object[] { null, null, null, "Fresh account created to validate the dashboard new-user metrics.", "SCENARIO-RECENT-RENTER-CONCURRENCY-STAMP", new DateTime(2026, 5, 10, 14, 30, 0, 0, DateTimeKind.Utc), "Recent", null, "Renter", null, true, null, "SCENARIO-RECENT-RENTER-SECURITY-STAMP" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "123 شارع النيل، القاهرة", "رينتر ألفا", "/images/idCards/b8ee0c84-7a46-457d-a6d5-9696166b3c87.jpg", "Renter", "/images/idCards/95c1567c-357c-4c0a-b711-e0ba27c1a96f.jpg", 1, "Alpha", "12345678901234", "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { "456 شارع المعادي، القاهرة", "رينتر بيتا", "/images/idCards/0b2b1890-82ff-4459-be9a-6dc65971849a.jpg", "Renter", "/images/idCards/f9797aa8-46ce-4dbb-ad14-2a521ed962fc.jpg", "Beta", "23456789012345", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Gender", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Assistant", null, 0, null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Renter", null, 0, "Gamma", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Owner", null, 0, "X", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Owner", null, "Y", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Owner", null, 0, "Z", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Gender", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Renter", null, 1, "Delta", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Language", "LastName", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "Renter", null, 0, "Epsilon", null, null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "ArabicAddress", "ArabicFullName", "BackIdPhoto", "FirstName", "FrontIdPhoto", "Gender", "Language", "NationalIDNumber", "ProfileImage" },
                values: new object[] { null, null, null, "System", null, 0, 0, null, null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "ImagesDeletionJob", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan", "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000005"), 0, 2, null, null, null, "Seeded moderator candidate for role-management testing.", "SCENARIO-MODERATOR-USER-CONCURRENCY-STAMP", 1, new DateTime(2026, 4, 20, 11, 0, 0, 0, DateTimeKind.Utc), null, null, "moderator.user@example.com", true, "Mona", null, 2, null, 0, "Moderator", false, null, null, "MODERATOR.USER@EXAMPLE.COM", "MODERATOR.USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-MODERATOR-USER-SECURITY-STAMP", null, null, false, false, false, "moderator.user@example.com" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001L,
                columns: new[] { "Address", "Description", "Latitude", "Longitude", "ProofOfOwnership", "Title", "ZipCode" },
                values: new object[] { "123 Seed Street, Cairo", "A cozy seeded apartment suitable for testing active rentals.", 30.0444, 31.235700000000001, "/images/documents/property1-POO.jpg", "Cozy Seed Apartment", "11511" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1002L,
                columns: new[] { "Address", "City", "Description", "Latitude", "Longitude", "Price", "ProofOfOwnership", "RentalUnit", "State", "Title", "ZipCode" },
                values: new object[] { "456 Integration Avenue, Cairo", "Cairo", "A modern loft used for pending booking and payments tests.", 30.050000000000001, 31.239999999999998, 90000m, "/images/documents/property2-POO.jpg", 2, "CairoGovernorate", "Modern Seed Loft", "11512" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1003L,
                columns: new[] { "Address", "Description", "Latitude", "Longitude", "ProofOfOwnership", "Title", "ZipCode" },
                values: new object[] { "789 Scenario Road, Cairo", "A small studio property used for saved properties and pending bookings.", 30.059999999999999, 31.245000000000001, "/images/documents/property3-POO.jpg", "Seed Studio Flat", "12511" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1004L,
                columns: new[] { "Address", "City", "Description", "Latitude", "Longitude", "Price", "ProofOfOwnership", "State", "Title", "ZipCode" },
                values: new object[] { "321 Elite Boulevard, Cairo", "New Cairo", "A luxury villa owned by the dual-role Owner Z for owner dashboard testing.", 30.07, 31.25, 15000m, "/images/documents/property4-POO.jpg", "CairoGovernorate", "Luxury Seed Villa", "11835" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1100L,
                columns: new[] { "Address", "City", "Description", "Latitude", "Longitude", "ProofOfOwnership", "State", "Title", "ZipCode" },
                values: new object[] { "555 Shared Lane, Cairo", "Cairo", "A shared house seeded for testing roommate matching logic.", 30.079999999999998, 31.260000000000002, "/images/documents/property100-POO.jpg", "CairoGovernorate", "Shared Seed House", "11513" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1201L,
                column: "ProofOfOwnership",
                value: "/docs/properties/pending-downtown-apartment.pdf");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1202L,
                column: "ProofOfOwnership",
                value: "/docs/properties/declined-garden-house.pdf");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1203L,
                column: "ProofOfOwnership",
                value: "/docs/properties/deleted-test-studio.pdf");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1204L,
                column: "ProofOfOwnership",
                value: "/docs/properties/recent-marina-flat.pdf");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1205L,
                column: "ProofOfOwnership",
                value: "/docs/properties/moderated-riverside-villa.pdf");

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 2001L, true, "/images/properties/property1-main.jpg", 1001L },
                    { 2002L, false, "/images/properties/property1-secondary.jpg", 1001L },
                    { 2003L, true, "/images/properties/property2-main.jpg", 1002L },
                    { 2004L, true, "/images/properties/property3-main.jpg", 1003L },
                    { 2005L, true, "/images/properties/property4-main.jpg", 1004L },
                    { 2006L, true, "/images/properties/property100-main.jpg", 1100L }
                });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 9105L,
                column: "ReportableGuidId",
                value: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), new Guid("10000000-0000-0000-0000-000000000005") }
                });
        }
    }
}
