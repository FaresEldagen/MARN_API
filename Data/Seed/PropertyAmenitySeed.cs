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
                new PropertyAmenity { Id = 13, PropertyId = 1004, Amenity = AmenityType.Parking },

                // Property 1100
                new PropertyAmenity { Id = 14, PropertyId = 1100, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 15, PropertyId = 1100, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 16, PropertyId = 1100, Amenity = AmenityType.Washer },
                new PropertyAmenity { Id = 17, PropertyId = 1100, Amenity = AmenityType.Balcony },

                // Property 2001
                new PropertyAmenity { Id = 18, PropertyId = 2001, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 19, PropertyId = 2001, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 20, PropertyId = 2001, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 21, PropertyId = 2001, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 22, PropertyId = 2001, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 23, PropertyId = 2001, Amenity = AmenityType.Elevator },

                // Property 2002
                new PropertyAmenity { Id = 24, PropertyId = 2002, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 25, PropertyId = 2002, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 26, PropertyId = 2002, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 27, PropertyId = 2002, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 28, PropertyId = 2002, Amenity = AmenityType.Washer },
                new PropertyAmenity { Id = 29, PropertyId = 2002, Amenity = AmenityType.Balcony },

                // Property 2003
                new PropertyAmenity { Id = 30, PropertyId = 2003, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 31, PropertyId = 2003, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 32, PropertyId = 2003, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 33, PropertyId = 2003, Amenity = AmenityType.Tv },
                new PropertyAmenity { Id = 34, PropertyId = 2003, Amenity = AmenityType.Microwave },

                // Property 2004
                new PropertyAmenity { Id = 35, PropertyId = 2004, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 36, PropertyId = 2004, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 37, PropertyId = 2004, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 38, PropertyId = 2004, Amenity = AmenityType.Washer },

                // Property 2005
                new PropertyAmenity { Id = 39, PropertyId = 2005, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 40, PropertyId = 2005, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 41, PropertyId = 2005, Amenity = AmenityType.Pool },
                new PropertyAmenity { Id = 42, PropertyId = 2005, Amenity = AmenityType.Gym },
                new PropertyAmenity { Id = 43, PropertyId = 2005, Amenity = AmenityType.Parking },
                new PropertyAmenity { Id = 44, PropertyId = 2005, Amenity = AmenityType.SecuritySystem },
                new PropertyAmenity { Id = 45, PropertyId = 2005, Amenity = AmenityType.Kitchen },

                // Property 2006
                new PropertyAmenity { Id = 46, PropertyId = 2006, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 47, PropertyId = 2006, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 48, PropertyId = 2006, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 49, PropertyId = 2006, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 50, PropertyId = 2006, Amenity = AmenityType.Tv },

                // Property 2007
                new PropertyAmenity { Id = 51, PropertyId = 2007, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 52, PropertyId = 2007, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 53, PropertyId = 2007, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 54, PropertyId = 2007, Amenity = AmenityType.Elevator },
                new PropertyAmenity { Id = 55, PropertyId = 2007, Amenity = AmenityType.Balcony },

                // Property 2008
                new PropertyAmenity { Id = 56, PropertyId = 2008, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 57, PropertyId = 2008, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 58, PropertyId = 2008, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 59, PropertyId = 2008, Amenity = AmenityType.Parking },
                new PropertyAmenity { Id = 60, PropertyId = 2008, Amenity = AmenityType.SecuritySystem },

                // Property 2009
                new PropertyAmenity { Id = 61, PropertyId = 2009, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 62, PropertyId = 2009, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 63, PropertyId = 2009, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 64, PropertyId = 2009, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 65, PropertyId = 2009, Amenity = AmenityType.Refrigerator },

                // Property 2010
                new PropertyAmenity { Id = 66, PropertyId = 2010, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 67, PropertyId = 2010, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 68, PropertyId = 2010, Amenity = AmenityType.Pool },
                new PropertyAmenity { Id = 69, PropertyId = 2010, Amenity = AmenityType.Parking },
                new PropertyAmenity { Id = 70, PropertyId = 2010, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 71, PropertyId = 2010, Amenity = AmenityType.SecuritySystem },

                // Property 2011
                new PropertyAmenity { Id = 72, PropertyId = 2011, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 73, PropertyId = 2011, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 74, PropertyId = 2011, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 75, PropertyId = 2011, Amenity = AmenityType.Balcony },

                // Property 2012
                new PropertyAmenity { Id = 76, PropertyId = 2012, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 77, PropertyId = 2012, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 78, PropertyId = 2012, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 79, PropertyId = 2012, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 80, PropertyId = 2012, Amenity = AmenityType.Tv },

                // Property 2013
                new PropertyAmenity { Id = 81, PropertyId = 2013, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 82, PropertyId = 2013, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 83, PropertyId = 2013, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 84, PropertyId = 2013, Amenity = AmenityType.Parking },

                // Property 2014
                new PropertyAmenity { Id = 85, PropertyId = 2014, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 86, PropertyId = 2014, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 87, PropertyId = 2014, Amenity = AmenityType.Washer },
                new PropertyAmenity { Id = 88, PropertyId = 2014, Amenity = AmenityType.Refrigerator },

                // Property 2015
                new PropertyAmenity { Id = 89, PropertyId = 2015, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 90, PropertyId = 2015, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 91, PropertyId = 2015, Amenity = AmenityType.Balcony },

                // Property 2016
                new PropertyAmenity { Id = 92, PropertyId = 2016, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 93, PropertyId = 2016, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 94, PropertyId = 2016, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 95, PropertyId = 2016, Amenity = AmenityType.Elevator },

                // Property 2017
                new PropertyAmenity { Id = 96, PropertyId = 2017, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 97, PropertyId = 2017, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 98, PropertyId = 2017, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 99, PropertyId = 2017, Amenity = AmenityType.Refrigerator },

                // Property 2018
                new PropertyAmenity { Id = 100, PropertyId = 2018, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 101, PropertyId = 2018, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 102, PropertyId = 2018, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 103, PropertyId = 2018, Amenity = AmenityType.Balcony },

                // Property 2019
                new PropertyAmenity { Id = 104, PropertyId = 2019, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 105, PropertyId = 2019, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 106, PropertyId = 2019, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 107, PropertyId = 2019, Amenity = AmenityType.Parking },

                // Property 2020
                new PropertyAmenity { Id = 108, PropertyId = 2020, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 109, PropertyId = 2020, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 110, PropertyId = 2020, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 111, PropertyId = 2020, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 112, PropertyId = 2020, Amenity = AmenityType.Parking },

                // Property 1201
                new PropertyAmenity { Id = 113, PropertyId = 1201, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 114, PropertyId = 1201, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 115, PropertyId = 1201, Amenity = AmenityType.Elevator },
                new PropertyAmenity { Id = 116, PropertyId = 1201, Amenity = AmenityType.Balcony },

                // Property 1202
                new PropertyAmenity { Id = 117, PropertyId = 1202, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 118, PropertyId = 1202, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 119, PropertyId = 1202, Amenity = AmenityType.Parking },
                new PropertyAmenity { Id = 120, PropertyId = 1202, Amenity = AmenityType.Balcony },

                // Property 1203
                new PropertyAmenity { Id = 121, PropertyId = 1203, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 122, PropertyId = 1203, Amenity = AmenityType.Kitchen },
                new PropertyAmenity { Id = 123, PropertyId = 1203, Amenity = AmenityType.AirConditioning },

                // Property 1204
                new PropertyAmenity { Id = 124, PropertyId = 1204, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 125, PropertyId = 1204, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 126, PropertyId = 1204, Amenity = AmenityType.Balcony },
                new PropertyAmenity { Id = 127, PropertyId = 1204, Amenity = AmenityType.Refrigerator },
                new PropertyAmenity { Id = 128, PropertyId = 1204, Amenity = AmenityType.Parking },
                new PropertyAmenity { Id = 129, PropertyId = 1204, Amenity = AmenityType.SecuritySystem },

                // Property 1205
                new PropertyAmenity { Id = 130, PropertyId = 1205, Amenity = AmenityType.Wifi },
                new PropertyAmenity { Id = 131, PropertyId = 1205, Amenity = AmenityType.AirConditioning },
                new PropertyAmenity { Id = 132, PropertyId = 1205, Amenity = AmenityType.Pool },
                new PropertyAmenity { Id = 133, PropertyId = 1205, Amenity = AmenityType.Parking },
                new PropertyAmenity { Id = 134, PropertyId = 1205, Amenity = AmenityType.SecuritySystem },
                new PropertyAmenity { Id = 135, PropertyId = 1205, Amenity = AmenityType.Kitchen }
            );
        }
    }
}
