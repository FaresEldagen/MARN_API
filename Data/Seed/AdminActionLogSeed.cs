using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Report;

namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Seed admin action logs (merged from AdminDashboardScenarioAdminActionLogSeed).
    /// </summary>
    public class AdminActionLogSeed : IEntityTypeConfiguration<AdminActionLog>
    {
        public void Configure(EntityTypeBuilder<AdminActionLog> builder)
        {
            var primaryAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");
            var moderatedMessageId = Guid.Parse("00000000-0000-0000-0000-000000000101");

            builder.HasData(
                new AdminActionLog
                {
                    Id = 8101,
                    AdminId = primaryAdminId,
                    ReportId = 9102,
                    ActionType = ReportModerationActionType.DeactivateProperty.ToString(),
                    TargetType = ReportableType.Property.ToString(),
                    TargetLongId = 1205,
                    Reason = "Property deactivated until listing details are corrected.",
                    CreatedAt = new DateTime(2026, 5, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = 8102,
                    AdminId = primaryAdminId,
                    ReportId = 9103,
                    ActionType = ReportModerationActionType.HideMessage.ToString(),
                    TargetType = ReportableType.Message.ToString(),
                    TargetGuidId = moderatedMessageId,
                    Reason = "Hidden abusive message.",
                    CreatedAt = new DateTime(2026, 4, 13, 9, 0, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = 8103,
                    AdminId = primaryAdminId,
                    ReportId = 9103,
                    ActionType = ReportModerationActionType.BanUser.ToString(),
                    TargetType = ReportableType.Message.ToString(),
                    TargetGuidId = bannedRenterId,
                    Reason = "Banned sender after abusive chat message.",
                    CreatedAt = new DateTime(2026, 4, 13, 9, 1, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = 8104,
                    AdminId = primaryAdminId,
                    ReportId = 9104,
                    ActionType = ReportModerationActionType.HidePropertyComment.ToString(),
                    TargetType = ReportableType.PropertyComment.ToString(),
                    TargetLongId = 900101,
                    Reason = "Hidden harassing property comment.",
                    CreatedAt = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc)
                },
                new AdminActionLog
                {
                    Id = 8105,
                    AdminId = primaryAdminId,
                    ReportId = 9104,
                    ActionType = ReportModerationActionType.BanUser.ToString(),
                    TargetType = ReportableType.PropertyComment.ToString(),
                    TargetGuidId = bannedRenterId,
                    Reason = "Banned commenter after repeated harassment.",
                    CreatedAt = new DateTime(2026, 4, 14, 12, 1, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
