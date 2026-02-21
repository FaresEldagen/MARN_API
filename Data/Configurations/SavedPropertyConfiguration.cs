using MARN_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Data.Configurations
{
    public class SavedPropertyConfiguration : IEntityTypeConfiguration<SavedProperty>
    {
        public void Configure(EntityTypeBuilder<SavedProperty> builder)
        {
            builder.HasKey(sp => new { sp.PropertyId, sp.UserId });

            builder.Property(sp => sp.PropertyId).IsRequired();
            builder.Property(sp => sp.UserId).IsRequired();

            builder.HasOne(sp => sp.Property)
                   .WithMany(cr => cr.SavedProperty)
                   .HasForeignKey(cp => cp.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sp => sp.User)
                   .WithMany(u => u.SavedProperty)
                   .HasForeignKey(cp => cp.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}