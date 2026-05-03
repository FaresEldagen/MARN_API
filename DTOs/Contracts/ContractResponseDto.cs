using MARN_API.Enums;
using MARN_API.Enums.Payment;

namespace MARN_API.DTOs.Contracts
{
    public class ContractResponseDto
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid RenterId { get; set; }
        public Guid OwnerId { get; set; }
        public DateOnly? LeaseStartDate { get; set; }
        public DateOnly? LeaseEndDate { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? AnchoredAt { get; set; }
        public string? TransactionId { get; set; }
        public string? MerkleRoot { get; set; }
        public ContractStatus Status { get; set; }
        public ContractAnchoringStatus AnchoringStatus { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }
        public decimal TotalContractAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
