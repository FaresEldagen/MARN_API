using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Property;
using MARN_API.Models;
using System;

namespace MARN_API.Data.Seed
{
    public class AdminDashboardScenarioPropertySeed : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            builder.HasData(
                new Property
                {
                    Id = AdminDashboardScenarioIds.PendingPropertyId,
                    OwnerId = ownerYId,
                    Title = "Pending Downtown Apartment",
                    Description = "Ownership documents are uploaded and waiting for admin review.",
                    Type = PropertyType.Apartment,
                    ProofOfOwnership = "/docs/properties/pending-downtown-apartment.pdf",
                    IsShared = false,
                    MaxOccupants = 2,
                    Bedrooms = 1,
                    Beds = 1,
                    Bathrooms = 1,
                    SquareMeters = 85,
                    Views = 0,
                    Price = 6200m,
                    RentalUnit = RentalUnit.Monthly,
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
                    Id = AdminDashboardScenarioIds.DeclinedPropertyId,
                    OwnerId = ownerYId,
                    Title = "Declined Garden House",
                    Description = "A property with rejected ownership documentation for verification testing.",
                    Type = PropertyType.House,
                    ProofOfOwnership = "/docs/properties/declined-garden-house.pdf",
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
                    Id = AdminDashboardScenarioIds.DeletedPropertyId,
                    OwnerId = ownerYId,
                    Title = "Soft Deleted Test Studio",
                    Description = "Soft deleted property used to validate include-deleted admin filters.",
                    Type = PropertyType.Studio,
                    ProofOfOwnership = "/docs/properties/deleted-test-studio.pdf",
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
                    Id = AdminDashboardScenarioIds.RecentPropertyId,
                    OwnerId = ownerYId,
                    Title = "Recent Marina Flat",
                    Description = "Fresh verified property created this month for dashboard trend checks.",
                    Type = PropertyType.Apartment,
                    ProofOfOwnership = "/docs/properties/recent-marina-flat.pdf",
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
                    Id = AdminDashboardScenarioIds.ModeratedInactivePropertyId,
                    OwnerId = ownerYId,
                    Title = "Moderated Riverside Villa",
                    Description = "Property already deactivated through a seeded moderation outcome.",
                    Type = PropertyType.Villa,
                    ProofOfOwnership = "/docs/properties/moderated-riverside-villa.pdf",
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
                });
        }
    }
}
