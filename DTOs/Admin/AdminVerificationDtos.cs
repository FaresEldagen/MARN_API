using System.ComponentModel.DataAnnotations;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;

namespace MARN_API.DTOs.Admin
{
    public class AdminVerificationQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class AdminVerificationDecisionDto
    {
        [StringLength(1000)]
        public string? Reason { get; set; }
    }

    public class AdminUserVerificationDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;

        public string? FrontIdPhoto { get; set; }
        public string? BackIdPhoto { get; set; }
        public string? ArabicFullName { get; set; }
        public string? ArabicAddress { get; set; }
        public string? NationalIDNumber { get; set; }
    }

    public class AdminPropertyVerificationDto
    {
        public long PropertyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public PropertyStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid OwnerId { get; set; }
        public string OwnerFullName { get; set; } = string.Empty;
        public string? OwnerEmail { get; set; }
        public AccountStatus OwnerAccountStatus { get; set; }
        public string OwnerAccountStatusDisplayName { get; set; } = string.Empty;

        public string ProofOfOwnership { get; set; } = string.Empty;
        public string? PrimaryImage { get; set; }
        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }
        public string RentalUnitDisplayName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string CityDisplayName { get; set; } = string.Empty;
        public string Governorate { get; set; } = string.Empty;
        public string GovernorateDisplayName { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
