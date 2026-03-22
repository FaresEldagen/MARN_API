using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

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
                    Latitude = 30.0444,
                    Longitude = 31.2357,
                    IsActive = true,
                    Availability = PropertyAvailability.Available,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc)
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
                    Price = 7500m,
                    RentalUnit = RentalUnit.Monthly,
                    Address = "456 Integration Avenue, Cairo",
                    Latitude = 30.0500,
                    Longitude = 31.2400,
                    IsActive = true,
                    Availability = PropertyAvailability.Available,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc)
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
                    Latitude = 30.0600,
                    Longitude = 31.2450,
                    IsActive = true,
                    Availability = PropertyAvailability.Available,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc)
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
                    Latitude = 30.0700,
                    Longitude = 31.2500,
                    IsActive = true,
                    Availability = PropertyAvailability.Available,
                    Status = PropertyStatus.Verified,
                    CreatedAt = new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

