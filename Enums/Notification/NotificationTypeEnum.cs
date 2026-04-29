namespace MARN_API.Enums.Notification
{
    public enum NotificationType
    {
        General = 0,

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
        WithdrawFailed,

        PropertyAdded,
        PropertyEdited,
        PropertyDeleted
    }
}
