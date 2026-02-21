using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARN_API.Data.Seed
{
    public class RoleSeed : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Renter", NormalizedName = "RENTER" },
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Owner", NormalizedName = "OWNER" },
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }
}




