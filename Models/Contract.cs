using System;
using System.Collections.Generic;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class Contract
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid RenterId { get; set; }
        public Guid OwnerId { get; set; }

        public string DocumentPath { get; set; } = string.Empty;
        public string DocumentHash { get; set; } = string.Empty; // SHA256
        public string? RenterSignature { get; set; }
        public string? OwnerSignature { get; set; }

        public ContractStatus Status { get; set; } = ContractStatus.Pending;
        public DateTime? SignedByRenterAt { get; set; }
        public DateTime? SignedByOwnerAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsLocked { get; set; } = false;
        public int Version { get; set; } = 1;
        public string? IPAddress { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser Renter { get; set; } = null!;
        public virtual Owner Owner { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}



