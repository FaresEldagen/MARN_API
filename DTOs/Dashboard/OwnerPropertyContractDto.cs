using MARN_API.Enums.Contract;

namespace MARN_API.DTOs.Dashboard
{
    public class OwnerPropertyContractDto
    {
        public long ContractId { get; set; }
        public string? TransactionId { get; set; }
        public string? MerkleRoot { get; set; }
        public ContractAnchoringStatus AnchoringStatus { get; set; }
        public string AnchoringStatusDisplayName { get; set; } = string.Empty;
        public bool IsAnchoredToBlockChain { get; set; }
        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
        public string? RenterProfileImage { get; set; }
    }
}
