using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            builder.Property(a => a.Type).HasConversion<int>();
            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(a => new { a.UserId, a.Type, a.CreatedAt });
        }
    }
}



