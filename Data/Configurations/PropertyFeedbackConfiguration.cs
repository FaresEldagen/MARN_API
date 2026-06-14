using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyFeedbackConfiguration : IEntityTypeConfiguration<PropertyFeedback>
    {
        public void Configure(EntityTypeBuilder<PropertyFeedback> builder)
        {
            builder.Property(f => f.PropertyId).IsRequired();
            builder.Property(f => f.UserId).IsRequired();
            builder.Property(f => f.Rating).IsRequired();

            builder.Property(f => f.Content)
                .HasMaxLength(1000);

            builder.Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(f => f.HiddenReason)
                .HasMaxLength(2000);

            builder.ToTable(t => t.HasCheckConstraint("CK_PropertyFeedback_Rating", "[Rating] >= 1 AND [Rating] <= 5"));

            builder.HasIndex(f => new { f.PropertyId, f.UserId }).IsUnique();
            builder.HasIndex(f => f.UserId);

            builder.HasOne(f => f.Property)
                .WithMany(p => p.PropertyFeedbacks)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.User)
                .WithMany(u => u.PropertyFeedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
