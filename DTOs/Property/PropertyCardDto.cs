using MARN_API.Enums.Property;

namespace MARN_API.DTOs.Property
{
    public class PropertyCardDto
    {
        public long Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int MaxOccupants { get; set; }

        public PropertyType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public float AverageRating { get; set; }
        public int Ratings { get; set; }

        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }
        public string RentalUnitDisplayName { get; set; } = string.Empty;

        public bool IsSaved { get; set; }
    }
}
