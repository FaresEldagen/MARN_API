using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Links seeded users to Identity roles (must match <see cref="RoleSeed"/> role IDs).
    /// </summary>
    public class UserRoleSeed : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            var renterRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var ownerRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                new IdentityUserRole<Guid> { UserId = renterAId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = renterBId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = renterCId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = ownerXId, RoleId = ownerRoleId },
                new IdentityUserRole<Guid> { UserId = ownerYId, RoleId = ownerRoleId },
                // Owner Z gets both roles for dual-dashboard testing
                new IdentityUserRole<Guid> { UserId = ownerZId, RoleId = ownerRoleId },
                new IdentityUserRole<Guid> { UserId = ownerZId, RoleId = renterRoleId }
            );
        }
    }
}
