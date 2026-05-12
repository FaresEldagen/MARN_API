using System;

namespace MARN_API.DTOs.Dashboard
{
    public class PaidPaymentDto
    {
        public decimal AmountPaid { get; set; }
        public long ContractId { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
