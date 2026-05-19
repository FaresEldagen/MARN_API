using MARN_API.DTOs.Dashboard;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;

namespace MARN_API.DTOs.Admin
{
    public class AdminUserManagementQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public string? Role { get; set; }
        public bool IncludeDeleted { get; set; } = true;
    }

    public class AdminUserListItemDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = [];
        public List<string> RolesDisplayNames { get; set; } = [];
        public int OwnedPropertiesCount { get; set; }
        public int ActiveContractsCount { get; set; }
    }

    public class AdminUserDetailsDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string GenderDisplayName { get; set; } = string.Empty;
        public Country Country { get; set; }
        public string CountryDisplayName { get; set; } = string.Empty;
        public Language Language { get; set; }
        public string LanguageDisplayName { get; set; } = string.Empty;
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;

        public string? FrontIdPhoto { get; set; }
        public string? BackIdPhoto { get; set; }
        public string? ArabicAddress { get; set; }
        public string? ArabicFullName { get; set; }
        public string? NationalIDNumber { get; set; }

        public List<string> Roles { get; set; } = [];
        public List<string> RolesDisplayNames { get; set; } = [];
        public AdminUserActivitySummaryDto Summary { get; set; } = new();
        public List<AdminManagedPropertyDto> OwnedProperties { get; set; } = [];
        public List<RenterContractCardDto> RenterContracts { get; set; } = [];
        public List<OwnerContractCardDto> OwnerContracts { get; set; } = [];
        public List<PaidPaymentDto> PaidPayments { get; set; } = [];
        public List<ReceivedPaymentDto> ReceivedPayments { get; set; } = [];
    }

    public class AdminUserActivitySummaryDto
    {
        public int OwnedPropertiesCount { get; set; }
        public int ActiveOwnedPropertiesCount { get; set; }
        public int RenterContractsCount { get; set; }
        public int OwnerContractsCount { get; set; }
        public int ActiveContractsAsRenterCount { get; set; }
        public int ActiveContractsAsOwnerCount { get; set; }
        public int PendingBookingRequestsAsRenterCount { get; set; }
        public int PendingBookingRequestsAsOwnerCount { get; set; }
        public int PaidPaymentsCount { get; set; }
        public int ReceivedPaymentsCount { get; set; }
    }

    public class AdminManagedPropertyDto
    {
        public long PropertyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public PropertyStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }
        public string RentalUnitDisplayName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? PrimaryImage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
