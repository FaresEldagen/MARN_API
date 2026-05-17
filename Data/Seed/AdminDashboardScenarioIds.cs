using System;

namespace MARN_API.Data.Seed
{
    public static class AdminDashboardScenarioIds
    {
        public static readonly Guid ModeratorRoleId = Guid.Parse("aaaaaaaa-1111-2222-3333-444444444444");

        public static readonly Guid PendingRenterId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        public static readonly Guid BannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");
        public static readonly Guid DeletedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000003");
        public static readonly Guid RecentRenterId = Guid.Parse("10000000-0000-0000-0000-000000000004");
        public static readonly Guid ModeratorUserId = Guid.Parse("10000000-0000-0000-0000-000000000005");
        public static readonly Guid SecondAdminId = Guid.Parse("30000000-0000-0000-0000-000000000001");

        public const long PendingPropertyId = 1201;
        public const long DeclinedPropertyId = 1202;
        public const long DeletedPropertyId = 1203;
        public const long RecentPropertyId = 1204;
        public const long ModeratedInactivePropertyId = 1205;

        public const long PendingContractId = 1000101;
        public const long RevenueContractId = 1000102;

        public const long RevenueScheduleDec2025Id = 20101;
        public const long RevenueScheduleJan2026Id = 20102;
        public const long RevenueScheduleFeb2026Id = 20103;
        public const long RevenueScheduleMar2026Id = 20104;
        public const long RevenueScheduleApr2026Id = 20105;
        public const long RevenueScheduleMay2026Id = 20106;
        public const long RevenueScheduleJun2026Id = 20107;

        public const long RevenuePaymentDec2025Id = 30101;
        public const long RevenuePaymentJan2026Id = 30102;
        public const long RevenuePaymentFeb2026Id = 30103;
        public const long RevenuePaymentMar2026Id = 30104;
        public const long RevenuePaymentApr2026Id = 30105;
        public const long RevenuePaymentMay2026Id = 30106;

        public static readonly Guid ModeratedMessageId = Guid.Parse("00000000-0000-0000-0000-000000000101");
        public const long ModeratedCommentId = 900101;

        public const long InReviewUserReportId = 9101;
        public const long ResolvedPropertyReportId = 9102;
        public const long ResolvedMessageReportId = 9103;
        public const long ResolvedCommentReportId = 9104;
        public const long RejectedUserReportId = 9105;

        public const long PropertyDeactivateActionLogId = 8101;
        public const long MessageHideActionLogId = 8102;
        public const long MessageBanUserActionLogId = 8103;
        public const long CommentHideActionLogId = 8104;
        public const long CommentBanUserActionLogId = 8105;
    }
}
