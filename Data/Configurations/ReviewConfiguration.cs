using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.PropertyId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Rating).IsRequired();

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.ToTable(r => r
                    .HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5"));

            builder.HasOne(r => r.Property)
                   .WithMany(p => p.Reviews)
                   .HasForeignKey(r => r.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                   .WithMany(u => u.Reviews)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Unique review per property per user
            builder.HasIndex(r => new { r.PropertyId, r.UserId }).IsUnique();
        }
    }
}



