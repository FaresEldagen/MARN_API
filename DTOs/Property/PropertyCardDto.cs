using MARN_API.Enums;

namespace MARN_API.DTOs.Property
{
    public class PropertyCardDto
    {
        public long Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int MaxOccupants { get; set; }

        public PropertyType Type { get; set; }
        public float AverageRating { get; set; }
        public int Ratings { get; set; }

        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }
    }
}
