using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Property;

namespace MARN_API.Data.Seed
{
    public class PropertySeed : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                // ── Core Properties ──
                new Property
                {
                    Id = 1001,
                    OwnerId = ownerXId,
                    Title = "Zamalek Riverside Apartment",
                    Description = "A cozy seeded apartment with a wonderful Nile view in Zamalek.",
                    Type = PropertyType.Apartment,
                    IsShared = false,
                    MaxOccupants = 3,
                    Bedrooms = 2,
                    Beds = 3,
                    Bathrooms = 1,
                    Views = 5,
                    Price = 5000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "123 26th of July St, Zamalek, Cairo",
                    City = "Cairo",
                    State = Governorate.CairoGovernorate.ToString(),
                    ZipCode = "11211",
                    Latitude = 30.0626,
                    Longitude = 31.2230,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property1001-POO.jpg"
                },

                new Property
                {
                    Id = 1002,
                    OwnerId = ownerXId,
                    Title = "Dokki Modern Loft",
                    Description = "A modern loft in the heart of Dokki used for testing.",
                    Type = PropertyType.Apartment,
                    IsShared = false,
                    MaxOccupants = 2,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    Views = 3,
                    Price = 9000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "45 Tahrir St, Dokki, Giza",
                    City = "Giza",
                    State = Governorate.GizaGovernorate.ToString(),
                    ZipCode = "12311",
                    Latitude = 30.0384,
                    Longitude = 31.2114,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2023, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property1002-POO.jpg"
                },

                new Property
                {
                    Id = 1003,
                    OwnerId = ownerXId,
                    Title = "Mohandeseen Studio Flat",
                    Description = "A small studio property in Mohandeseen.",
                    Type = PropertyType.Studio,
                    IsShared = false,
                    MaxOccupants = 1,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    Views = 1,
                    Price = 3500m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "78 Arab League St, Mohandeseen, Giza",
                    City = "Giza",
                    State = Governorate.GizaGovernorate.ToString(),
                    ZipCode = "12411",
                    Latitude = 30.0558,
                    Longitude = 31.2001,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property1003-POO.jpg"
                },

                // Property owned by Owner Z (for owner dashboard)
                new Property
                {
                    Id = 1004,
                    OwnerId = ownerZId,
                    Title = "Sheikh Zayed Luxury Villa",
                    Description = "A luxury villa in Sheikh Zayed owned by the dual-role Owner Z.",
                    Type = PropertyType.Villa,
                    IsShared = false,
                    MaxOccupants = 6,
                    Bedrooms = 4,
                    Beds = 5,
                    Bathrooms = 3,
                    Views = 12,
                    Price = 35000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "Beverly Hills, Sheikh Zayed, Giza",
                    City = "Sheikh Zayed",
                    State = Governorate.GizaGovernorate.ToString(),
                    ZipCode = "12588",
                    Latitude = 30.0520,
                    Longitude = 30.9850,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property1004-POO.jpg"
                },

                // Shared property for roommate matching tests
                new Property
                {
                    Id = 1100,
                    OwnerId = ownerXId,
                    Title = "Agouza Shared House",
                    Description = "A shared house in Agouza for testing roommate matching logic.",
                    Type = PropertyType.House,
                    IsShared = true,
                    MaxOccupants = 4,
                    Bedrooms = 3,
                    Beds = 4,
                    Bathrooms = 2,
                    Views = 10,
                    Price = 4000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "15 Nile Corniche, Agouza, Giza",
                    City = "Giza",
                    State = Governorate.GizaGovernorate.ToString(),
                    ZipCode = "12611",
                    Latitude = 30.0468,
                    Longitude = 31.2131,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property1100-POO.jpg"
                },

                // ── CSV-imported properties (merged from 20_property_seed.csv) ──
                // Cairo
                new Property { Id = 2001, OwnerId = ownerXId, Title = "Nile View Apartment", Description = "A bright apartment near downtown Cairo for monthly stays", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 3, Bedrooms = 2, Beds = 3, Bathrooms = 1, SquareMeters = 118, Views = 14, Price = 85000m, RentalUnit = RentalUnit.Yearly, Address = "15 Tahrir Square", City = "Cairo", State = Governorate.CairoGovernorate.ToString(), ZipCode = "11511", Latitude = 30.0444, Longitude = 31.2357, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2001-POO.jpg" },
                new Property { Id = 2002, OwnerId = ownerZId, Title = "Heliopolis Family Flat", Description = "Comfortable family flat close to shops and transit", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 4, Bedrooms = 3, Beds = 4, Bathrooms = 2, SquareMeters = 146, Views = 9, Price = 9800m, RentalUnit = RentalUnit.Monthly, Address = "22 Al Ahram Street", City = "Heliopolis", State = Governorate.CairoGovernorate.ToString(), ZipCode = "11757", Latitude = 30.0919, Longitude = 31.3174, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 2, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2002-POO.jpg" },
                new Property { Id = 2003, OwnerId = ownerXId, Title = "Maadi Garden Studio", Description = "Quiet studio with easy access to Maadi services", Type = PropertyType.Studio, IsShared = false, MaxOccupants = 2, Bedrooms = 1, Beds = 1, Bathrooms = 1, SquareMeters = 62, Views = 7, Price = 280m, RentalUnit = RentalUnit.Daily, Address = "8 Road 9", City = "Maadi", State = Governorate.CairoGovernorate.ToString(), ZipCode = "11431", Latitude = 29.9602, Longitude = 31.2569, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2003-POO.jpg" },
                new Property { Id = 2004, OwnerId = ownerZId, Title = "Nasr City Shared Loft", Description = "Shared loft suited for students and young professionals", Type = PropertyType.SharedRoom, IsShared = true, MaxOccupants = 3, Bedrooms = 1, Beds = 3, Bathrooms = 1, SquareMeters = 88, Views = 5, Price = 3200m, RentalUnit = RentalUnit.Monthly, Address = "41 Makram Ebeid Street", City = "Nasr City", State = Governorate.CairoGovernorate.ToString(), ZipCode = "11765", Latitude = 30.0626, Longitude = 31.3301, IsActive = true, Status = PropertyStatus.Pending, CreatedAt = new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2004-POO.jpg" },
                new Property { Id = 2005, OwnerId = ownerXId, Title = "New Cairo Corner Villa", Description = "Modern villa in a quiet New Cairo neighborhood", Type = PropertyType.Villa, IsShared = false, MaxOccupants = 6, Bedrooms = 4, Beds = 5, Bathrooms = 3, SquareMeters = 285, Views = 11, Price = 250000m, RentalUnit = RentalUnit.Yearly, Address = "10 South 90 Street", City = "New Cairo", State = Governorate.CairoGovernorate.ToString(), ZipCode = "11835", Latitude = 30.0300, Longitude = 31.4700, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2005-POO.jpg" },
                // Alexandria
                new Property { Id = 2006, OwnerId = ownerZId, Title = "Corniche Sea Apartment", Description = "Sea facing apartment near the Alexandria Corniche", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 3, Bedrooms = 2, Beds = 2, Bathrooms = 1, SquareMeters = 110, Views = 18, Price = 350m, RentalUnit = RentalUnit.Daily, Address = "33 Corniche Road", City = "Alexandria", State = Governorate.AlexandriaGovernorate.ToString(), ZipCode = "21519", Latitude = 31.2001, Longitude = 29.9187, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2006-POO.jpg" },
                new Property { Id = 2007, OwnerId = ownerXId, Title = "Sidi Gaber Urban Flat", Description = "Well placed flat near transport and universities", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 4, Bedrooms = 2, Beds = 3, Bathrooms = 2, SquareMeters = 124, Views = 12, Price = 8100m, RentalUnit = RentalUnit.Monthly, Address = "12 El Horreya Road", City = "Sidi Gaber", State = Governorate.AlexandriaGovernorate.ToString(), ZipCode = "21615", Latitude = 31.2156, Longitude = 29.9420, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 7, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2007-POO.jpg" },
                new Property { Id = 2008, OwnerId = ownerZId, Title = "Smouha Park Residence", Description = "Spacious residence overlooking a calm residential area", Type = PropertyType.House, IsShared = false, MaxOccupants = 5, Bedrooms = 3, Beds = 4, Bathrooms = 2, SquareMeters = 172, Views = 10, Price = 130000m, RentalUnit = RentalUnit.Yearly, Address = "27 Fawzy Moaz Street", City = "Smouha", State = Governorate.AlexandriaGovernorate.ToString(), ZipCode = "21646", Latitude = 31.2150, Longitude = 29.9553, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2008-POO.jpg" },
                new Property { Id = 2009, OwnerId = ownerXId, Title = "Miami Beach Studio", Description = "Compact studio within walking distance of the beach", Type = PropertyType.Studio, IsShared = false, MaxOccupants = 2, Bedrooms = 1, Beds = 1, Bathrooms = 1, SquareMeters = 58, Views = 16, Price = 4900m, RentalUnit = RentalUnit.Monthly, Address = "50 Khaled Ibn Al Walid Street", City = "Miami", State = Governorate.AlexandriaGovernorate.ToString(), ZipCode = "21919", Latitude = 31.2677, Longitude = 30.0046, IsActive = true, Status = PropertyStatus.Pending, CreatedAt = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2009-POO.jpg" },
                new Property { Id = 2010, OwnerId = ownerZId, Title = "Montaza Family Villa", Description = "Large villa near the Montaza district gardens", Type = PropertyType.Villa, IsShared = false, MaxOccupants = 7, Bedrooms = 4, Beds = 6, Bathrooms = 3, SquareMeters = 310, Views = 8, Price = 24500m, RentalUnit = RentalUnit.Monthly, Address = "6 Malek Hefny Street", City = "Montaza", State = Governorate.AlexandriaGovernorate.ToString(), ZipCode = "21923", Latitude = 31.2854, Longitude = 30.0173, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2010-POO.jpg" },
                // Sharkia
                new Property { Id = 2011, OwnerId = ownerXId, Title = "Zagazig Central Apartment", Description = "Clean apartment close to central Zagazig amenities", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 3, Bedrooms = 2, Beds = 2, Bathrooms = 1, SquareMeters = 102, Views = 6, Price = 55000m, RentalUnit = RentalUnit.Yearly, Address = "14 Talat Harb Street", City = "Zagazig", State = Governorate.SharkiaGovernorate.ToString(), ZipCode = "44511", Latitude = 30.5877, Longitude = 31.5020, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 11, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2011-POO.jpg" },
                new Property { Id = 2012, OwnerId = ownerZId, Title = "Tenth District Loft", Description = "Practical loft near business zones in 10th of Ramadan", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 2, Bedrooms = 1, Beds = 1, Bathrooms = 1, SquareMeters = 76, Views = 4, Price = 4700m, RentalUnit = RentalUnit.Monthly, Address = "88 Industrial Zone Road", City = "Tenth of Ramadan", State = Governorate.SharkiaGovernorate.ToString(), ZipCode = "44629", Latitude = 30.3045, Longitude = 31.7420, IsActive = true, Status = PropertyStatus.Pending, CreatedAt = new DateTime(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2012-POO.jpg" },
                new Property { Id = 2013, OwnerId = ownerXId, Title = "Belbeis Courtyard House", Description = "Traditional house with a private courtyard and storage", Type = PropertyType.House, IsShared = false, MaxOccupants = 5, Bedrooms = 3, Beds = 4, Bathrooms = 2, SquareMeters = 180, Views = 3, Price = 72000m, RentalUnit = RentalUnit.Yearly, Address = "19 Saad Zaghloul Street", City = "Belbeis", State = Governorate.SharkiaGovernorate.ToString(), ZipCode = "44621", Latitude = 30.4204, Longitude = 31.5622, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 13, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2013-POO.jpg" },
                new Property { Id = 2014, OwnerId = ownerZId, Title = "Minya Al Qamh Shared Home", Description = "Shared home designed for longer affordable stays", Type = PropertyType.SharedRoom, IsShared = true, MaxOccupants = 4, Bedrooms = 2, Beds = 4, Bathrooms = 2, SquareMeters = 130, Views = 2, Price = 2600m, RentalUnit = RentalUnit.Monthly, Address = "9 El Geish Street", City = "Minya Al Qamh", State = Governorate.SharkiaGovernorate.ToString(), ZipCode = "44661", Latitude = 30.4228, Longitude = 31.3697, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 14, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2014-POO.jpg" },
                new Property { Id = 2015, OwnerId = ownerXId, Title = "Abu Hammad Riverside Flat", Description = "Bright flat near local markets and key roads", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 3, Bedrooms = 2, Beds = 3, Bathrooms = 1, SquareMeters = 97, Views = 5, Price = 5100m, RentalUnit = RentalUnit.Monthly, Address = "31 Port Said Street", City = "Abu Hammad", State = Governorate.SharkiaGovernorate.ToString(), ZipCode = "44671", Latitude = 30.5369, Longitude = 31.6835, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2015-POO.jpg" },
                // Damietta
                new Property { Id = 2016, OwnerId = ownerZId, Title = "Damietta Port Apartment", Description = "Modern apartment near the city center and port", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 3, Bedrooms = 2, Beds = 2, Bathrooms = 1, SquareMeters = 108, Views = 7, Price = 300m, RentalUnit = RentalUnit.Daily, Address = "18 El Galaa Street", City = "Damietta", State = Governorate.DamiettaGovernorate.ToString(), ZipCode = "34511", Latitude = 31.4165, Longitude = 31.8133, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2016-POO.jpg" },
                new Property { Id = 2017, OwnerId = ownerXId, Title = "Ras El Bar Summer Studio", Description = "Compact studio ideal for short coastal stays", Type = PropertyType.Studio, IsShared = false, MaxOccupants = 2, Bedrooms = 1, Beds = 1, Bathrooms = 1, SquareMeters = 54, Views = 13, Price = 3800m, RentalUnit = RentalUnit.Daily, Address = "5 Nile Street", City = "Ras El Bar", State = Governorate.DamiettaGovernorate.ToString(), ZipCode = "34711", Latitude = 31.5085, Longitude = 31.8404, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2017-POO.jpg" },
                new Property { Id = 2018, OwnerId = ownerZId, Title = "New Damietta Corner Flat", Description = "Contemporary flat in a newer planned district", Type = PropertyType.Apartment, IsShared = false, MaxOccupants = 4, Bedrooms = 3, Beds = 3, Bathrooms = 2, SquareMeters = 138, Views = 4, Price = 420m, RentalUnit = RentalUnit.Daily, Address = "44 Al Gamea Street", City = "New Damietta", State = Governorate.DamiettaGovernorate.ToString(), ZipCode = "34517", Latitude = 31.4456, Longitude = 31.6764, IsActive = true, Status = PropertyStatus.Pending, CreatedAt = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2018-POO.jpg" },
                new Property { Id = 2019, OwnerId = ownerXId, Title = "Kafr Saad Family House", Description = "Well sized family house with a practical layout", Type = PropertyType.House, IsShared = false, MaxOccupants = 5, Bedrooms = 3, Beds = 4, Bathrooms = 2, SquareMeters = 192, Views = 3, Price = 7300m, RentalUnit = RentalUnit.Monthly, Address = "11 Mostafa Kamel Street", City = "Kafr Saad", State = Governorate.DamiettaGovernorate.ToString(), ZipCode = "34614", Latitude = 31.3554, Longitude = 31.6763, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 19, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2019-POO.jpg" },
                new Property { Id = 2020, OwnerId = ownerZId, Title = "Faraskur Riverside Home", Description = "Large home with generous indoor living space", Type = PropertyType.House, IsShared = false, MaxOccupants = 6, Bedrooms = 4, Beds = 5, Bathrooms = 3, SquareMeters = 230, Views = 2, Price = 96000m, RentalUnit = RentalUnit.Yearly, Address = "7 Omar Ibn El Khattab Street", City = "Faraskur", State = Governorate.DamiettaGovernorate.ToString(), ZipCode = "34631", Latitude = 31.3290, Longitude = 31.7159, IsActive = true, Status = PropertyStatus.Verified, CreatedAt = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), ProofOfOwnership = "/images/documents/property2020-POO.jpg" },

                // ── Admin Dashboard Scenario Properties ──
                new Property
                {
                    Id = 1201,
                    OwnerId = ownerYId,
                    Title = "Pending Downtown Apartment",
                    Description = "Ownership documents are uploaded and waiting for admin review.",
                    Type = PropertyType.Apartment,
                    ProofOfOwnership = "/images/documents/property1201-POO.jpg",
                    IsShared = false,
                    MaxOccupants = 2,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    SquareMeters = 85,
                    Views = 0,
                    Price = 320m,
                    RentalUnit = RentalUnit.Daily,
                    Address = "10 Tahrir Square",
                    City = "Cairo",
                    State = Governorate.CairoGovernorate.ToString(),
                    ZipCode = "11511",
                    Latitude = 30.0440,
                    Longitude = 31.2350,
                    IsActive = true,
                    Status = PropertyStatus.Pending,
                    CreatedAt = new DateTime(2026, 5, 3, 9, 0, 0, DateTimeKind.Utc)
                },
                new Property
                {
                    Id = 1202,
                    OwnerId = ownerYId,
                    Title = "Declined Garden House",
                    Description = "A property with rejected ownership documentation for verification testing.",
                    Type = PropertyType.House,
                    ProofOfOwnership = "/images/documents/property1202-POO.jpg",
                    IsShared = false,
                    MaxOccupants = 5,
                    Bedrooms = 3,
                    Beds = 4,
                    Bathrooms = 2,
                    SquareMeters = 180,
                    Views = 4,
                    Price = 11000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "88 Palm Street",
                    City = "Giza",
                    State = Governorate.GizaGovernorate.ToString(),
                    ZipCode = "12511",
                    Latitude = 30.0110,
                    Longitude = 31.2080,
                    IsActive = true,
                    Status = PropertyStatus.Declined,
                    CreatedAt = new DateTime(2026, 4, 18, 12, 0, 0, DateTimeKind.Utc)
                },
                new Property
                {
                    Id = 1203,
                    OwnerId = ownerYId,
                    Title = "Soft Deleted Test Studio",
                    Description = "Soft deleted property used to validate include-deleted admin filters.",
                    Type = PropertyType.Studio,
                    ProofOfOwnership = "/images/documents/property1203-POO.jpg",
                    IsShared = false,
                    MaxOccupants = 1,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    SquareMeters = 55,
                    Views = 1,
                    Price = 4300m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "34 Sunset Alley",
                    City = "Alexandria",
                    State = Governorate.AlexandriaGovernorate.ToString(),
                    ZipCode = "21511",
                    Latitude = 31.2000,
                    Longitude = 29.9187,
                    IsActive = false,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2026, 3, 8, 16, 0, 0, DateTimeKind.Utc),
                    DeletedAt = new DateTime(2026, 4, 4, 13, 0, 0, DateTimeKind.Utc)
                },
                new Property
                {
                    Id = 1204,
                    OwnerId = ownerYId,
                    Title = "Recent Marina Flat",
                    Description = "Fresh verified property created this month for dashboard trend checks.",
                    Type = PropertyType.Apartment,
                    ProofOfOwnership = "/images/documents/property1204-POO.jpg",
                    IsShared = false,
                    MaxOccupants = 3,
                    Bedrooms = 2,
                    Beds = 2,
                    Bathrooms = 2,
                    SquareMeters = 110,
                    Views = 9,
                    Price = 7800m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "5 Marina Walk",
                    City = "North Coast",
                    State = Governorate.MarsaMatruhGovernorate.ToString(),
                    ZipCode = "51711",
                    Latitude = 30.9000,
                    Longitude = 28.9000,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2026, 5, 5, 10, 0, 0, DateTimeKind.Utc)
                },
                new Property
                {
                    Id = 1205,
                    OwnerId = ownerYId,
                    Title = "Moderated Riverside Villa",
                    Description = "Property already deactivated through a seeded moderation outcome.",
                    Type = PropertyType.Villa,
                    ProofOfOwnership = "/images/documents/property1205-POO.jpg",
                    IsShared = false,
                    MaxOccupants = 6,
                    Bedrooms = 4,
                    Beds = 5,
                    Bathrooms = 3,
                    SquareMeters = 240,
                    Views = 22,
                    Price = 16000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "77 Corniche View",
                    City = "Luxor",
                    State = Governorate.LuxorGovernorate.ToString(),
                    ZipCode = "85951",
                    Latitude = 25.6872,
                    Longitude = 32.6396,
                    IsActive = false,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2026, 5, 7, 15, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
