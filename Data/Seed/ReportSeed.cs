using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Report;

namespace MARN_API.Data.Seed
{
    public class ReportSeed : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            var primaryAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var secondAdminId = Guid.Parse("30000000-0000-0000-0000-000000000001");
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");
            var recentRenterId = Guid.Parse("10000000-0000-0000-0000-000000000004");
            var moderatedMessageId = Guid.Parse("00000000-0000-0000-0000-000000000101");

            builder.HasData(
                // Core report
                new Report
                {
                    Id = 1,
                    ReporterId = renterAId,
                    ReportableType = ReportableType.Property,
                    ReportableId = 1001,
                    ReportableGuidId = null,
                    Reason = "Misleading information about the property.",
                    Status = ReportStatus.InReview,
                    ReviewerNote = null,
                    ActionTaken = null,
                    CreatedAt = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc)
                },

                // ── Scenario Reports (merged from AdminDashboardScenarioReportSeed) ──

                // InReview user report
                new Report
                {
                    Id = 9101,
                    ReporterId = renterAId,
                    ReportableType = ReportableType.User,
                    ReportableGuidId = recentRenterId,
                    Reason = "Profile details look inconsistent and need manual review.",
                    Status = ReportStatus.InReview,
                    CreatedAt = new DateTime(2026, 5, 11, 9, 30, 0, DateTimeKind.Utc)
                },
                // Resolved property report
                new Report
                {
                    Id = 9102,
                    ReporterId = renterBId,
                    ReviewerId = primaryAdminId,
                    ReportableType = ReportableType.Property,
                    ReportableId = 1205,
                    Reason = "Listing contains misleading availability details.",
                    Status = ReportStatus.Resolved,
                    ReviewerNote = "Property deactivated until the owner corrects the listing.",
                    ActionTaken = ReportModerationActionType.DeactivateProperty,
                    CreatedAt = new DateTime(2026, 5, 8, 10, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 5, 8, 12, 0, 0, DateTimeKind.Utc)
                },
                // Resolved message report
                new Report
                {
                    Id = 9103,
                    ReporterId = ownerXId,
                    ReviewerId = primaryAdminId,
                    ReportableType = ReportableType.Message,
                    ReportableGuidId = moderatedMessageId,
                    Reason = "Abusive language in chat.",
                    Status = ReportStatus.Resolved,
                    ReviewerNote = "Message hidden and sender banned.",
                    ActionTaken = ReportModerationActionType.HideMessage,
                    CreatedAt = new DateTime(2026, 4, 13, 8, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 4, 13, 9, 0, 0, DateTimeKind.Utc)
                },
                // Resolved comment report
                new Report
                {
                    Id = 9104,
                    ReporterId = renterAId,
                    ReviewerId = primaryAdminId,
                    ReportableType = ReportableType.PropertyComment,
                    ReportableId = 900101,
                    Reason = "Comment includes harassment.",
                    Status = ReportStatus.Resolved,
                    ReviewerNote = "Comment hidden and the commenter was banned.",
                    ActionTaken = ReportModerationActionType.HidePropertyComment,
                    CreatedAt = new DateTime(2026, 4, 14, 10, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc)
                },
                // Rejected user report
                new Report
                {
                    Id = 9105,
                    ReporterId = renterCId,
                    ReviewerId = secondAdminId,
                    ReportableType = ReportableType.User,
                    ReportableGuidId = recentRenterId,
                    Reason = "Suspicious behavior, but without evidence.",
                    Status = ReportStatus.Rejected,
                    ReviewerNote = "Insufficient evidence after review.",
                    CreatedAt = new DateTime(2026, 5, 9, 9, 0, 0, DateTimeKind.Utc),
                    ReviewedAt = new DateTime(2026, 5, 9, 11, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
