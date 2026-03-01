namespace MARN_API.Enums
{
    public enum UserActivityType
    {
        Login = 0,
        Logout = 1,
        CreatedProperty = 2,
        UpdatedProperty = 3,
        DeletedProperty = 4,
        CreatedBookingRequest = 5,
        UpdatedBookingRequest = 6,
        CreatedContract = 7,
        PaymentMade = 8,
        ViewedProperty = 9,
        Other = 99
    }
}
