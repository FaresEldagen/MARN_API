using System;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class Contract
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid RenterId { get; set; }
        public Guid OwnerId { get; set; }

        public string ContractNumber { get; set; } = string.Empty;
        public DateOnly? LeaseStartDate { get; set; }
        public DateOnly? LeaseEndDate { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[]? FileBytes { get; set; }
        public string Hash { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AnchoredAt { get; set; }
        public byte[]? OtsFileBytes { get; set; }
        public string? TransactionId { get; set; }
        public string? MerkleRoot { get; set; }

        public string? RenterSignature { get; set; }
        public string? OwnerSignature { get; set; }

        public ContractStatus Status { get; set; } = ContractStatus.Pending;
        public ContractAnchoringStatus AnchoringStatus { get; set; } = ContractAnchoringStatus.Pending;
        public DateTime? SignedByRenterAt { get; set; }
        public DateTime? SignedByOwnerAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.OneTime;
        public bool IsLocked { get; set; } = false;
        public int Version { get; set; } = 1;
        public string? IPAddress { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser Renter { get; set; } = null!;
    }
}
