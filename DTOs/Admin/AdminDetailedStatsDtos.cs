using MARN_API.DTOs.Common;
using MARN_API.Enums.Account;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Property;

namespace MARN_API.DTOs.Admin
{
    public class AdminDetailedStatsPeriodQueryDto
    {
        public string Period { get; set; } = "allTime";
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class AdminDetailedUsersQueryDto : AdminDetailedStatsPeriodQueryDto
    {
        public string? Search { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public string? Role { get; set; }
        public bool IncludeDeleted { get; set; } = true;
    }

    public class AdminDetailedPropertiesQueryDto : AdminDetailedStatsPeriodQueryDto
    {
        public string? Search { get; set; }
        public PropertyStatus? Status { get; set; }
        public PropertyType? Type { get; set; }
        public string? Governorate { get; set; }
        public bool? IsActive { get; set; }
        public bool IncludeDeleted { get; set; } = true;
    }

    public class AdminDetailedContractsQueryDto : AdminDetailedStatsPeriodQueryDto
    {
        public string? Search { get; set; }
        public ContractStatus? Status { get; set; }
    }

    public class AdminDetailedRevenueQueryDto : AdminDetailedStatsPeriodQueryDto
    {
        public string? Search { get; set; }
        public PaymentStatus? Status { get; set; }
    }

    public class AdminAppliedPeriodDto
    {
        public string Period { get; set; } = string.Empty;
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string Grouping { get; set; } = string.Empty;
    }

    public class AdminCountTimePointDto
    {
        public DateTime PeriodStartUtc { get; set; }
        public string Label { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminRevenueTimePointDto
    {
        public DateTime PeriodStartUtc { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Sales { get; set; }
        public decimal OwnerPayouts { get; set; }
        public long PaymentsCount { get; set; }
    }

    public class AdminRoleCountDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string RoleNameDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminGovernorateCountDto
    {
        public string Governorate { get; set; } = string.Empty;
        public string GovernorateDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminPaymentStatusSummaryDto
    {
        public PaymentStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
        public decimal Sales { get; set; }
        public decimal Revenue { get; set; }
        public decimal OwnerPayouts { get; set; }
    }

    public class AdminDetailedUsersResponseDto
    {
        public AdminAppliedPeriodDto AppliedPeriod { get; set; } = new();
        public long TotalUsers { get; set; }
        public long DeletedUsers { get; set; }
        public List<AdminAccountStatusCountDto> StatusBreakdown { get; set; } = [];
        public List<AdminRoleCountDto> RoleBreakdown { get; set; } = [];
        public List<AdminCountTimePointDto> NewUsersOverTime { get; set; } = [];
        public PagedResult<AdminDetailedUserListItemDto> Users { get; set; } = new();
    }

    public class AdminAccountStatusCountDto
    {
        public AccountStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminDetailedUserListItemDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDisplayName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = [];
        public List<string> RolesDisplayNames { get; set; } = [];
        public long OwnedPropertiesCount { get; set; }
        public long ActivePropertiesCount { get; set; }
        public long RenterContractsCount { get; set; }
        public long OwnerContractsCount { get; set; }
        public long ActiveContractsCount { get; set; }
        public long CancelledContractsCount { get; set; }
        public long PaymentsMadeCount { get; set; }
        public long PaymentsReceivedCount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public long ReportsSubmittedCount { get; set; }
        public long ReportsAgainstUserCount { get; set; }
    }

    public class AdminDetailedPropertiesResponseDto
    {
        public AdminAppliedPeriodDto AppliedPeriod { get; set; } = new();
        public long TotalProperties { get; set; }
        public long DeletedProperties { get; set; }
        public long ActiveProperties { get; set; }
        public long InactiveProperties { get; set; }
        public List<AdminPropertyStatusCountDto> StatusBreakdown { get; set; } = [];
        public List<AdminPropertyTypeCountDto> TypeBreakdown { get; set; } = [];
        public List<AdminGovernorateCountDto> GovernorateBreakdown { get; set; } = [];
        public PagedResult<AdminDetailedPropertyListItemDto> Properties { get; set; } = new();
    }

    public class AdminPropertyStatusCountDto
    {
        public PropertyStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminPropertyTypeCountDto
    {
        public PropertyType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminDetailedPropertyListItemDto
    {
        public long PropertyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public PropertyStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string CityDisplayName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string StateDisplayName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool CanDeactivate { get; set; }
        public bool CanRestore { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AdminDetailedContractsResponseDto
    {
        public AdminAppliedPeriodDto AppliedPeriod { get; set; } = new();
        public long TotalContracts { get; set; }
        public decimal TotalContractValue { get; set; }
        public List<AdminContractStatusCountDto> StatusBreakdown { get; set; } = [];
        public PagedResult<AdminDetailedContractListItemDto> Contracts { get; set; } = new();
    }

    public class AdminDetailedContractListItemDto
    {
        public long ContractId { get; set; }
        public ContractStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public bool CanCancel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateOnly LeaseStartDate { get; set; }
        public DateOnly LeaseEndDate { get; set; }
        public decimal TotalContractAmount { get; set; }
        public string PaymentFrequency { get; set; } = string.Empty;
        public string PaymentFrequencyDisplayName { get; set; } = string.Empty;
        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
    }

    public class AdminDetailedRevenueResponseDto
    {
        public AdminAppliedPeriodDto AppliedPeriod { get; set; } = new();
        public long TotalPayments { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalOwnerPayouts { get; set; }
        public List<AdminPaymentStatusSummaryDto> StatusBreakdown { get; set; } = [];
        public List<AdminRevenueTimePointDto> RevenueOverTime { get; set; } = [];
        public PagedResult<AdminDetailedPaymentListItemDto> Payments { get; set; } = new();
    }

    public class AdminDetailedPaymentListItemDto
    {
        public long PaymentId { get; set; }
        public long ContractId { get; set; }
        public long PaymentScheduleId { get; set; }
        public PaymentStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public decimal AmountTotal { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal OwnerAmount { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime AvailableAt { get; set; }
        public string Currency { get; set; } = string.Empty;
        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
    }
}
