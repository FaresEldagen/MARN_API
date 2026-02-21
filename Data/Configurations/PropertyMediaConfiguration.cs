using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyMediaConfiguration : IEntityTypeConfiguration<PropertyMedia>
    {
        public void Configure(EntityTypeBuilder<PropertyMedia> builder)
        {
            builder.Property(pm => pm.Path).IsRequired();
            builder.Property(pm => pm.PropertyId).IsRequired();

            builder.HasOne(pm => pm.Property)
                   .WithMany(p => p.Media)
                   .HasForeignKey(pm => pm.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}



