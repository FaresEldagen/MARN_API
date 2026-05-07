namespace MARN_API.Enums.Notification
{
    public enum NotificationType
    {
        General = 0,

        NewMessage,

        NewBookingRequest,
        BookingRequestCanceled,
        BookingRequestRejected,

        NewReview,

        ContractStarted,
        ContractCanceled,
        ContractSigned,
        ContractCompleted,

        UpcomingPayment,
        PaymentArrived,
        DelayedPayment,
        PaymentSuccessful,
        PaymentFailed,

        PaymentReceived,
        AvailableForWithdrawal,
        WithdrawSuccess,
        WithdrawFailed,

        PropertyAdded,
        PropertyEdited,
        PropertyDeleted
    }
}
