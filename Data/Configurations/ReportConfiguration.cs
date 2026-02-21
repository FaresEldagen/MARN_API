using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.Property(r => r.Reason).IsRequired();
            builder.Property(r => r.ReporterId).IsRequired();
            builder.Property(r => r.ReportableType).IsRequired();
            builder.Property(r => r.ReportableId).IsRequired();

            builder.Property(r => r.Status).HasConversion<int>();
            builder.Property(r => r.ReportableType).HasConversion<int>();

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}



