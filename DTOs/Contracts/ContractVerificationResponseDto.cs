using MARN_API.Enums.Contract;

namespace MARN_API.DTOs.Contracts
{
    public class ContractVerificationResponseDto
    {
        public bool Match { get; set; }
        public string Message { get; set; } = string.Empty;
        public ContractStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public ContractAnchoringStatus AnchoringStatus { get; set; }
        public string AnchoringStatusDisplayName { get; set; } = string.Empty;
        public DateTime? AnchoredAt { get; set; }
    }
}
