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
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                new Property
                {
                    Id = 1001,
                    OwnerId = ownerXId,
                    Title = "Cozy Seed Apartment",
                    Description = "A cozy seeded apartment suitable for testing active rentals.",
                    Type = PropertyType.Apartment,
                    IsShared = false,
                    MaxOccupants = 3,
                    Bedrooms = 2,
                    Beds = 3,
                    Bathrooms = 1,
                    Views = 5,
                    Price = 5000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "123 Seed Street, Cairo",
                    City = "Cairo",
                    State = Governorate.CairoGovernorate.ToString(),
                    ZipCode = "11511",
                    Latitude = 30.0444,
                    Longitude = 31.2357,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property1-POO.jpg"
                },

                new Property
                {
                    Id = 1002,
                    OwnerId = ownerXId,
                    Title = "Modern Seed Loft",
                    Description = "A modern loft used for pending booking and payments tests.",
                    Type = PropertyType.Apartment,
                    IsShared = false,
                    MaxOccupants = 2,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    Views = 3,
                    Price = 90000m,
                    RentalUnit = RentalUnit.Yearly,
                    Address = "456 Integration Avenue, Cairo",
                    City = "Cairo",
                    State = Governorate.CairoGovernorate.ToString(),
                    ZipCode = "11512",
                    Latitude = 30.0500,
                    Longitude = 31.2400,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2023, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property2-POO.jpg"
                },

                new Property
                {
                    Id = 1003,
                    OwnerId = ownerXId,
                    Title = "Seed Studio Flat",
                    Description = "A small studio property used for saved properties and pending bookings.",
                    Type = PropertyType.Studio,
                    IsShared = false,
                    MaxOccupants = 1,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    Views = 1,
                    Price = 3500m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "789 Scenario Road, Cairo",
                    City = "Giza",
                    State = Governorate.GizaGovernorate.ToString(),
                    ZipCode = "12511",
                    Latitude = 30.0600,
                    Longitude = 31.2450,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property3-POO.jpg"
                },

                // Property owned by Owner Z (for owner dashboard)
                new Property
                {
                    Id = 1004,
                    OwnerId = ownerZId,
                    Title = "Luxury Seed Villa",
                    Description = "A luxury villa owned by the dual-role Owner Z for owner dashboard testing.",
                    Type = PropertyType.Villa,
                    IsShared = false,
                    MaxOccupants = 6,
                    Bedrooms = 4,
                    Beds = 5,
                    Bathrooms = 3,
                    Views = 12,
                    Price = 15000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "321 Elite Boulevard, Cairo",
                    City = "New Cairo",
                    State = Governorate.CairoGovernorate.ToString(),
                    ZipCode = "11835",
                    Latitude = 30.0700,
                    Longitude = 31.2500,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property4-POO.jpg"
                },

                // Shared property for roommate matching tests
                new Property
                {
                    Id = 1100,
                    OwnerId = ownerXId,
                    Title = "Shared Seed House",
                    Description = "A shared house seeded for testing roommate matching logic.",
                    Type = PropertyType.House,
                    IsShared = true,
                    MaxOccupants = 4,
                    Bedrooms = 3,
                    Beds = 4,
                    Bathrooms = 2,
                    Views = 10,
                    Price = 4000m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "555 Shared Lane, Cairo",
                    City = "Cairo",
                    State = Governorate.CairoGovernorate.ToString(),
                    ZipCode = "11513",
                    Latitude = 30.0800,
                    Longitude = 31.2600,
                    IsActive = true,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                    ProofOfOwnership = "/images/documents/property100-POO.jpg"
                }
            );
        }
    }
}

