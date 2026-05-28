using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            builder.Property(a => a.UserActivityType)
                   .IsRequired()
                   .HasMaxLength(32);

            builder.Property(a => a.Metadata)
                   .HasColumnType("nvarchar(max)");

            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(a => new { a.UserId, a.UserActivityType, a.CreatedAt });
        }
    }
}



