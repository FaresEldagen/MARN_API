using MARN_API.Enums.Contract;

namespace MARN_API.DTOs.Contracts
{
    public class ContractDetailsDto
    {
        public ContractStatus ContractStatus { get; set; }
        public string ContractStatusDisplayName { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string? MerkleRoot { get; set; }
        public ContractAnchoringStatus AnchoringStatus { get; set; }
        public string AnchoringStatusDisplayName { get; set; } = string.Empty;
        public bool IsAnchoredToBlockChain { get; set; }
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
        public string CityDisplayName { get; set; } = string.Empty;
        public string Governorate { get; set; } = string.Empty;
        public string GovernorateDisplayName { get; set; } = string.Empty;
        public string RentalDuration { get; set; } = string.Empty;
        public string RentalDurationDisplayName { get; set; } = string.Empty;
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
