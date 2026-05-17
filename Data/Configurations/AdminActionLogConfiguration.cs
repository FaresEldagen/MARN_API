using MARN_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARN_API.Data.Configurations
{
    public class AdminActionLogConfiguration : IEntityTypeConfiguration<AdminActionLog>
    {
        public void Configure(EntityTypeBuilder<AdminActionLog> builder)
        {
            builder.Property(x => x.ActionType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TargetType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Reason)
                .HasMaxLength(2000);

            builder.Property(x => x.MetadataJson)
                .HasMaxLength(4000);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Report)
                .WithMany(r => r.ActionLogs)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

