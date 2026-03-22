using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasMany(o => o.Properties)
                   .WithOne(p => p.Owner)
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}



