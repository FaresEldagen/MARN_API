using MARN_API.Enums;
using MARN_API.Enums.Property;

namespace MARN_API.DTOs.Property
{
    public class PropertyDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public int MaxOccupants { get; set; }
        public bool IsShared { get; set; }
        public int Bedrooms { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public double SquareMeters { get; set; }
        public int ViewsCount { get; set; }
        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsActive { get; set; }
        public bool Availability { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSaved { get; set; }
        public float AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public int CommentsCount { get; set; }
        public int? CurrentUserRating { get; set; }

        public List<PropertyAmenityItemDto> Amenities { get; set; } = new();
        public List<PropertyRuleItemDto> Rules { get; set; } = new();
        public List<PropertyMediaItemDto> Media { get; set; } = new();
        public List<PropertyCommentDetailsDto> Comments { get; set; } = new();
        public List<PropertyBookingRequestDto> CurrentUserBookingRequests { get; set; } = new();

        public PropertyHostedByDto HostedBy { get; set; } = new();
        public OwnerPropertyExtrasDto OwnerExtras { get; set; } = new();
    }

    public class PropertyAmenityItemDto
    {
        public long Id { get; set; }
        public AmenityType Amenity { get; set; }
    }

    public class PropertyRuleItemDto
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class PropertyMediaItemDto
    {
        public long Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }

    public class PropertyHostedByDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public float AverageRating { get; set; }
        public int PropertiesCount { get; set; }
        public string? Bio { get; set; }
    }

    public class PropertyCommentDetailsDto
    {
        public long CommentId { get; set; }
        public Guid CommenterId { get; set; }
        public string CommenterFullName { get; set; } = string.Empty;
        public string? CommenterProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public PropertyCommentStayInfoDto StayInfo { get; set; } = new();
    }

    public class PropertyCommentStayInfoDto
    {
        public DateOnly? CheckIn { get; set; }
        public DateOnly? CheckOut { get; set; }
        public bool IsContractActive { get; set; }
    }

    public class PropertyBookingRequestDto
    {
        public long BookingRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }
    }

    public class OwnerPropertyExtrasDto
    {
        public PropertyStatus? PropertyStatus { get; set; }
        public List<OwnerPropertyContractHistoryDto> ContractsHistory { get; set; } = new();
        public List<OwnerPropertyPendingBookingRequestDto> PendingBookingRequests { get; set; } = new();
    }

    public class OwnerPropertyContractHistoryDto
    {
        public long ContractId { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
    }

    public class OwnerPropertyPendingBookingRequestDto
    {
        public long BookingRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
        public string? RenterProfileImage { get; set; }
    }
}
