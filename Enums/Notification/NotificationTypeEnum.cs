namespace MARN_API.Enums.Notification
{
    public enum NotificationType
    {
        None = 0,

        NewMessage,
        NewBookingRequest,
        NewReview,

        ContractStarted,
        ContractCanceled,
        ContractCompleted,

        PaymentArrived, 
        UpcomingPayment,
        DelaiedPayment,
        PaymentSucces,
        PaymentFailed,

        PaymentReceived,
        WithdrawSuccess,
        WithdrawFailed
    }
}
