using System;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Payment;

namespace MARN_API.Models
{
    public class Contract
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid RenterId { get; set; }
        public ContractStatus Status { get; set; } = ContractStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.OneTime;
        public decimal TotalContractAmount { get; set; }

        public DateOnly LeaseStartDate { get; set; }
        public DateOnly LeaseEndDate { get; set; }

        public DateTime? SignedByRenterAt { get; set; }


        // Fields for contract anchoring
        public string FileName { get; set; } = string.Empty;
        public byte[]? FileBytes { get; set; }
        public string Hash { get; set; } = string.Empty;
        public byte[]? OtsFileBytes { get; set; }
        public string? TransactionId { get; set; }
        public string? MerkleRoot { get; set; }
        public ContractAnchoringStatus AnchoringStatus { get; set; } = ContractAnchoringStatus.Pending;
        public DateTime? AnchoredAt { get; set; }


        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser Renter { get; set; } = null!;
        public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; } = new HashSet<PaymentSchedule>();
    }
}
