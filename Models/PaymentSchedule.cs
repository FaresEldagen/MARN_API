using MARN_API.Enums.Payment;

namespace MARN_API.Models
{
    public class PaymentSchedule
    {
        public long Id { get; set; }
        public long ContractId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "egp";
        public DateTime DueDate { get; set; }

        public PaymentScheduleStatus Status { get; set; } = PaymentScheduleStatus.Pending;

        public virtual Contract Contract { get; set; } = null!;
        public virtual Payment? Payment { get; set; }
    }
}
