using MARN_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARN_API.Data.Configurations
{
    public class AdminAnalyticsReportConfiguration : IEntityTypeConfiguration<AdminAnalyticsReport>
    {
        public void Configure(EntityTypeBuilder<AdminAnalyticsReport> builder)
        {
            builder.Property(x => x.Scope).HasConversion<int>();
            builder.Property(x => x.Format).HasConversion<int>();
            builder.Property(x => x.RequestedPeriod).HasConversion<int>();

            builder.Property(x => x.Grouping)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(260);

            builder.Property(x => x.StoredFilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.GeneratedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(x => x.GeneratedAt);
            builder.HasIndex(x => new { x.Scope, x.Format });

            builder.HasOne(x => x.GeneratedByAdmin)
                .WithMany()
                .HasForeignKey(x => x.GeneratedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
