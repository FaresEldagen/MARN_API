using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyAmenityConfiguration : IEntityTypeConfiguration<PropertyAmenity>
    {
        public void Configure(EntityTypeBuilder<PropertyAmenity> builder)
        {
            builder.Property(pa => pa.Amenity).HasConversion<int>();

            builder.Property(pa => pa.PropertyId).IsRequired();
            builder.Property(pa => pa.Amenity).IsRequired();

            builder.HasOne(pa => pa.Property)
                   .WithMany(p => p.Amenities)
                   .HasForeignKey(pa => pa.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}



