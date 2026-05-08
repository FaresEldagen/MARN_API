namespace MARN_API.Enums.Payment
{
    public enum PaymentScheduleStatus
    {
        NotAvailableYet = 0,
        Available,
        DueToday,

        PaidEarly,
        PaidOnTime,
        PaidLate,

        Overdue
    }
}
