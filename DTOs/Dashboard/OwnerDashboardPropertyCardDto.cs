using MARN_API.Enums;

namespace MARN_API.DTOs.Dashboard
{
    public class OwnerDashboardPropertyCardDto
    {
        public long Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public long Views { get; set; }

        public int OccupiedPlaces { get; set; }
        public int TotalPlaces { get; set; }

        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }

        public float AverageRating { get; set; }
        public int Ratings { get; set; }

        public ICollection<OwnerPropertyContractDto>? ActiveContracts { get; set; }
    }
}
