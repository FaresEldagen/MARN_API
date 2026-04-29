using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Property;

namespace MARN_API.Data.Seed
{
    public class PropertyAmenitySeed : IEntityTypeConfiguration<PropertyAmenity>
    {
        public void Configure(EntityTypeBuilder<PropertyAmenity> builder)
        {
            builder.HasData(
                // Property 1001
                new PropertyAmenity { Id = 1, PropertyId = 1001, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 2, PropertyId = 1001, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 3, PropertyId = 1001, Amenity = AmenityType.Kitchen },

                // Property 1002
                new PropertyAmenity { Id = 4, PropertyId = 1002, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 5, PropertyId = 1002, Amenity = AmenityType.Tv },
                new PropertyAmenity { Id = 6, PropertyId = 1002, Amenity = AmenityType.Elevator },

                // Property 1003
                new PropertyAmenity { Id = 7, PropertyId = 1003, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 8, PropertyId = 1003, Amenity = AmenityType.Washer },

                // Property 1004
                new PropertyAmenity { Id = 9, PropertyId = 1004, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 10, PropertyId = 1004, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 11, PropertyId = 1004, Amenity = AmenityType.Pool },
                new PropertyAmenity { Id = 12, PropertyId = 1004, Amenity = AmenityType.Gym },
                new PropertyAmenity { Id = 13, PropertyId = 1004, Amenity = AmenityType.Parking }
            );
        }
    }
}
