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
                new PropertyAmenity { PropertyId = 1001, Amenity = AmenityType.Wifi },
                new PropertyAmenity { PropertyId = 1001, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { PropertyId = 1001, Amenity = AmenityType.Kitchen },

                // Property 1002
                new PropertyAmenity { PropertyId = 1002, Amenity = AmenityType.Wifi },
                new PropertyAmenity { PropertyId = 1002, Amenity = AmenityType.Tv },
                new PropertyAmenity { PropertyId = 1002, Amenity = AmenityType.Elevator },

                // Property 1003
                new PropertyAmenity { PropertyId = 1003, Amenity = AmenityType.Wifi },
                new PropertyAmenity { PropertyId = 1003, Amenity = AmenityType.Washer },

                // Property 1004
                new PropertyAmenity { PropertyId = 1004, Amenity = AmenityType.Wifi },
                new PropertyAmenity { PropertyId = 1004, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { PropertyId = 1004, Amenity = AmenityType.Pool },
                new PropertyAmenity { PropertyId = 1004, Amenity = AmenityType.Gym },
                new PropertyAmenity { PropertyId = 1004, Amenity = AmenityType.Parking }
            );
        }
    }
}
