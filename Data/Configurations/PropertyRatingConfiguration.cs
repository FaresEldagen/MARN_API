using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyRatingConfiguration : IEntityTypeConfiguration<PropertyRating>
    {
        public void Configure(EntityTypeBuilder<PropertyRating> builder)
        {
            builder.Property(r => r.PropertyId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Rating).IsRequired();

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.ToTable(t => t.HasCheckConstraint("CK_PropertyRating_Rating", "[Rating] >= 1 AND [Rating] <= 5"));

            builder.HasIndex(r => new { r.PropertyId, r.UserId }).IsUnique();

            builder.HasOne(r => r.Property)
                .WithMany(p => p.PropertyRatings)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(u => u.PropertyRatings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
