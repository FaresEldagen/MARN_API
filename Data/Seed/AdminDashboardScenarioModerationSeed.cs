using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;
using System;

namespace MARN_API.Data.Seed
{
    public class AdminDashboardScenarioMessageSeed : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            var encryptedSeedMessage = "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=";

            builder.HasData(
                new Message
                {
                    Id = AdminDashboardScenarioIds.ModeratedMessageId,
                    SenderId = AdminDashboardScenarioIds.BannedRenterId,
                    ReceiverId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Content = encryptedSeedMessage,
                    SentAt = new DateTime(2026, 4, 12, 19, 30, 0, DateTimeKind.Utc),
                    IsHiddenByModeration = true,
                    HiddenAt = new DateTime(2026, 4, 13, 9, 0, 0, DateTimeKind.Utc),
                    HiddenByAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    HiddenReason = "Seeded moderation example for admin dashboard testing."
                });
        }
    }

    public class AdminDashboardScenarioPropertyCommentSeed : IEntityTypeConfiguration<PropertyComment>
    {
        public void Configure(EntityTypeBuilder<PropertyComment> builder)
        {
            builder.HasData(
                new PropertyComment
                {
                    Id = AdminDashboardScenarioIds.ModeratedCommentId,
                    PropertyId = 1001,
                    UserId = AdminDashboardScenarioIds.BannedRenterId,
                    Content = "This seeded comment was hidden by moderation for admin review testing.",
                    CreatedAt = new DateTime(2026, 4, 14, 8, 0, 0, DateTimeKind.Utc),
                    IsHiddenByModeration = true,
                    HiddenAt = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc),
                    HiddenByAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    HiddenReason = "Seeded moderation example for admin dashboard testing."
                });
        }
    }

    public class AdminDashboardScenarioReportSeed : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            var primaryAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            builder.HasData(
                new Report
                {
                    Id = AdminDashboardScenarioIds.InReviewUserReportId,
                    ReporterId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ReportableType = ReportableType.User,
                    ReportableGuidId = AdminDashboardScenarioIds.RecentRenterId,
                    Reason = "Profile details look inconsistent and need manual review.",
                    Status = ReportStatus.InReview,
                    CreatedAt = new DateTime(2026, 5, 11, 9, 30, 0, DateTimeKind.Utc)
                },
                new Report
                {
                    Id = AdminDashboardScenarioIds.ResolvedPropertyReportId,
                    ReporterId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ReviewerId = primaryAdminId,
                    ReportableType = ReportableType.Property,
                    ReportableId = AdminDashboardScenarioIds.ModeratedInactivePropertyId,
                    Reason = "Listing contains misleading availability details.",
                    Status = ReportStatus.Resolved,
                    ReviewerNote = "Property deactivated until the owner corrects the listing.",
                    ActionTaken = ReportModerationActionType.DeactivateProperty,
                    CreatedAt = new DateTime(2026, 5, 8, 10, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 5, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new Report
                {
                    Id = AdminDashboardScenarioIds.ResolvedMessageReportId,
                    ReporterId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ReviewerId = primaryAdminId,
                    ReportableType = ReportableType.Message,
                    ReportableGuidId = AdminDashboardScenarioIds.ModeratedMessageId,
                    Reason = "Abusive language in chat.",
                    Status = ReportStatus.Resolved,
                    ReviewerNote = "Message hidden and sender banned.",
                    ActionTaken = ReportModerationActionType.HideMessage,
                    CreatedAt = new DateTime(2026, 4, 13, 8, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 4, 13, 9, 0, 0, DateTimeKind.Utc)
                },
                new Report
                {
                    Id = AdminDashboardScenarioIds.ResolvedCommentReportId,
                    ReporterId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ReviewerId = primaryAdminId,
                    ReportableType = ReportableType.PropertyComment,
                    ReportableId = AdminDashboardScenarioIds.ModeratedCommentId,
                    Reason = "Comment includes harassment.",
                    Status = ReportStatus.Resolved,
                    ReviewerNote = "Comment hidden and the commenter was banned.",
                    ActionTaken = ReportModerationActionType.HidePropertyComment,
                    CreatedAt = new DateTime(2026, 4, 14, 10, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc)
                },
                new Report
                {
                    Id = AdminDashboardScenarioIds.RejectedUserReportId,
                    ReporterId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ReviewerId = AdminDashboardScenarioIds.SecondAdminId,
                    ReportableType = ReportableType.User,
                    ReportableGuidId = AdminDashboardScenarioIds.ModeratorUserId,
                    Reason = "Suspicious behavior, but without evidence.",
                    Status = ReportStatus.Rejected,
                    ReviewerNote = "Insufficient evidence after review.",
                    CreatedAt = new DateTime(2026, 5, 9, 9, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 5, 9, 11, 0, 0, DateTimeKind.Utc)
                });
        }
    }

    public class AdminDashboardScenarioAdminActionLogSeed : IEntityTypeConfiguration<AdminActionLog>
    {
        public void Configure(EntityTypeBuilder<AdminActionLog> builder)
        {
            var primaryAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            builder.HasData(
                new AdminActionLog
                {
                    Id = AdminDashboardScenarioIds.PropertyDeactivateActionLogId,
                    AdminId = primaryAdminId,
                    ReportId = AdminDashboardScenarioIds.ResolvedPropertyReportId,
                    ActionType = ReportModerationActionType.DeactivateProperty.ToString(),
                    TargetType = ReportableType.Property.ToString(),
                    TargetLongId = AdminDashboardScenarioIds.ModeratedInactivePropertyId,
                    Reason = "Property deactivated until listing details are corrected.",
                    CreatedAt = new DateTime(2026, 5, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = AdminDashboardScenarioIds.MessageHideActionLogId,
                    AdminId = primaryAdminId,
                    ReportId = AdminDashboardScenarioIds.ResolvedMessageReportId,
                    ActionType = ReportModerationActionType.HideMessage.ToString(),
                    TargetType = ReportableType.Message.ToString(),
                    TargetGuidId = AdminDashboardScenarioIds.ModeratedMessageId,
                    Reason = "Hidden abusive message.",
                    CreatedAt = new DateTime(2026, 4, 13, 9, 0, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = AdminDashboardScenarioIds.MessageBanUserActionLogId,
                    AdminId = primaryAdminId,
                    ReportId = AdminDashboardScenarioIds.ResolvedMessageReportId,
                    ActionType = ReportModerationActionType.BanUser.ToString(),
                    TargetType = ReportableType.Message.ToString(),
                    TargetGuidId = AdminDashboardScenarioIds.BannedRenterId,
                    Reason = "Banned sender after abusive chat message.",
                    CreatedAt = new DateTime(2026, 4, 13, 9, 1, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = AdminDashboardScenarioIds.CommentHideActionLogId,
                    AdminId = primaryAdminId,
                    ReportId = AdminDashboardScenarioIds.ResolvedCommentReportId,
                    ActionType = ReportModerationActionType.HidePropertyComment.ToString(),
                    TargetType = ReportableType.PropertyComment.ToString(),
                    TargetLongId = AdminDashboardScenarioIds.ModeratedCommentId,
                    Reason = "Hidden harassing property comment.",
                    CreatedAt = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = AdminDashboardScenarioIds.CommentBanUserActionLogId,
                    AdminId = primaryAdminId,
                    ReportId = AdminDashboardScenarioIds.ResolvedCommentReportId,
                    ActionType = ReportModerationActionType.BanUser.ToString(),
                    TargetType = ReportableType.PropertyComment.ToString(),
                    TargetGuidId = AdminDashboardScenarioIds.BannedRenterId,
                    Reason = "Banned commenter after repeated harassment.",
                    CreatedAt = new DateTime(2026, 4, 14, 12, 1, 0, DateTimeKind.Utc)
                });
        }
    }
}
