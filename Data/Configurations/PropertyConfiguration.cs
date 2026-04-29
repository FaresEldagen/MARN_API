using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            // Global Query Filter: exclude soft-deleted properties from all queries
            builder.HasQueryFilter(p => p.DeletedAt == null);

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
            builder.Property(p => p.Address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.OwnerId).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.MaxOccupants).IsRequired();
            builder.Property(p => p.Latitude).IsRequired();
            builder.Property(p => p.Longitude).IsRequired();
            builder.Property(p => p.IsShared).IsRequired();

            builder.Property(p => p.Type).HasConversion<int>();
            builder.Property(p => p.Status).HasConversion<int>();

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(p => p.OwnerId);
            builder.HasIndex(p => p.Status);
        }
    }
}



