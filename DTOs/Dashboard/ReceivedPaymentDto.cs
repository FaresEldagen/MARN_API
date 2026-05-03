using System;

namespace MARN_API.DTOs.Dashboard
{
    public class ReceivedPaymentDto
    {
        public decimal AmountReceived { get; set; }
        public long ContractId { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime AvailableAt { get; set; }
    }
}
