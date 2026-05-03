using MARN_API.Enums;

namespace MARN_API.DTOs.Contracts
{
    public class ContractDetailsDto
    {
        public ContractStatus ContractStatus { get; set; }
        public long ContractId { get; set; }
        public string Duration { get; set; } = string.Empty;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal TotalContractValue { get; set; }

        public ContractPropertyInfoDto PropertyInfo { get; set; } = new();
        public ContractUserInfo OwnerInfo { get; set; } = new();
        public ContractUserInfo RenterInfo { get; set; } = new();
    }

    public class ContractPropertyInfoDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string RentalDuration { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class ContractUserInfo
    {
        public Guid Id { get; set; }
        public string? ProfileImage { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
