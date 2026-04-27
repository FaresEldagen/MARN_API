using System.Text.Json.Serialization;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class RentalTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RenterId { get; set; }
        public Guid OwnerId { get; set; }
        public long PropertyId { get; set; }
        public string? StripeSessionId { get; set; }
        public long? PaymentId { get; set; }
        public long? ContractId { get; set; }
        public RentalTransactionStatus Status { get; set; } = RentalTransactionStatus.Initiated;

        [JsonPropertyName("payment_status")]
        public RentalPaymentStatus PaymentStatus { get; set; } = RentalPaymentStatus.NotPaid;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}
