using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums;

namespace MARN_API.Data.Seed
{
    public class ReportSeed : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasData(
                new Report
                {
                    Id = 1,
                    ReporterId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ReviewerId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    ReportableType = ReportableType.Property,
                    ReportableId = 1001,
                    Reason = "Misleading information about the property.",
                    Status = ReportStatus.InReview,
                    CreatedAt = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
