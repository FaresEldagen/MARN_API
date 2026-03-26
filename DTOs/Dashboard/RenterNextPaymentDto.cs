namespace MARN_API.DTOs.Dashboard
{
    public class RenterNextPaymentDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
    }
}
