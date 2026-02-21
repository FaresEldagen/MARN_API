using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Title).IsRequired();
            builder.Property(n => n.Body).IsRequired();
            builder.Property(n => n.UserId).IsRequired();
            builder.Property(n => n.Type).IsRequired();

            builder.Property(n => n.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(n => n.Type).HasConversion<int>();

            builder.HasIndex(n => new { n.UserId, n.IsRead });
        }
    }
}



