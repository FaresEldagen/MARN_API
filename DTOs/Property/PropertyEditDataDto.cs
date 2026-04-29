using MARN_API.Enums.Property;
using System.Collections.Generic;

namespace MARN_API.DTOs.Property
{
    public class PropertyEditDataDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public bool IsShared { get; set; }
        public int MaxOccupants { get; set; }
        public int Bedrooms { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double SquareMeters { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<PropertyAmenityDto> Amenities { get; set; } = new List<PropertyAmenityDto>();
        public List<PropertyRuleDto> Rules { get; set; } = new List<PropertyRuleDto>();
        public List<PropertyMediaDto> Media { get; set; } = new List<PropertyMediaDto>();
    }

    public class PropertyAmenityDto
    {
        public long Id { get; set; }
        public AmenityType Amenity { get; set; }
    }

    public class PropertyRuleDto
    {
        public long Id { get; set; }
        public string Rule { get; set; } = string.Empty;
    }

    public class PropertyMediaDto
    {
        public long Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}
