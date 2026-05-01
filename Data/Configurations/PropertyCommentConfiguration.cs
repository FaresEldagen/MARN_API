using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyCommentConfiguration : IEntityTypeConfiguration<PropertyComment>
    {
        public void Configure(EntityTypeBuilder<PropertyComment> builder)
        {
            builder.Property(c => c.PropertyId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(c => c.PropertyId);
            builder.HasIndex(c => c.UserId);

            builder.HasOne(c => c.Property)
                .WithMany(p => p.PropertyComments)
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
                .WithMany(u => u.PropertyComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
