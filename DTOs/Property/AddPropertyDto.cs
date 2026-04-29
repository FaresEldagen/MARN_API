using MARN_API.Enums.Property;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Property
{
    public class AddPropertyDto
    {
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000, MinimumLength = 20)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public PropertyType Type { get; set; }

        [Required]
        public bool IsShared { get; set; }

        [Required]
        [Range(1, 50)]
        public int MaxOccupants { get; set; }

        [Required]
        [Range(0, 50)]
        public int Bedrooms { get; set; }

        [Required]
        [Range(0, 50)]
        public int Beds { get; set; }

        [Required]
        [Range(0, 50)]
        public int Bathrooms { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public decimal Price { get; set; }

        [Required]
        public RentalUnit RentalUnit { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public double SquareMeters { get; set; }

        [Required]
        public Microsoft.AspNetCore.Http.IFormFile ProofOfOwnership { get; set; } = null!;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public Microsoft.AspNetCore.Http.IFormFile PrimaryImage { get; set; } = null!;
        public List<Microsoft.AspNetCore.Http.IFormFile> MediaFiles { get; set; } = new List<Microsoft.AspNetCore.Http.IFormFile>();
        public List<AmenityType> Amenities { get; set; } = new List<AmenityType>();
        public List<string> Rules { get; set; } = new List<string>();
    }
}
