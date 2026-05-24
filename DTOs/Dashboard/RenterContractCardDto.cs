using MARN_API.Enums.Contract;

namespace MARN_API.DTOs.Dashboard
{
    public class RenterContractCardDto
    {
        public long ContractId { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public string ContractStatusDisplayName { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string? MerkleRoot { get; set; }
        public ContractAnchoringStatus AnchoringStatus { get; set; }
        public string AnchoringStatusDisplayName { get; set; } = string.Empty;
        public bool IsAnchoredToBlockChain { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;

        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
    }
}
