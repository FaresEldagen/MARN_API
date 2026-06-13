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
            var adminRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var renterDId = Guid.Parse("77777777-7777-7777-7777-777777777777");
            var renterEId = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var adminId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            // Scenario user IDs (merged from AdminDashboardScenarioUserRoleSeed)
            var pendingRenterId = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");
            var deletedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000003");
            var recentRenterId = Guid.Parse("10000000-0000-0000-0000-000000000004");
            var secondAdminId = Guid.Parse("30000000-0000-0000-0000-000000000001");



            builder.HasData(
                // Core renters
                new IdentityUserRole<Guid> { UserId = renterAId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = renterBId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = renterCId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = renterDId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = renterEId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = ownerXId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = ownerYId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = ownerZId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = secondAdminId, RoleId = renterRoleId },

                // Core owners
                new IdentityUserRole<Guid> { UserId = ownerXId, RoleId = ownerRoleId },
                new IdentityUserRole<Guid> { UserId = ownerYId, RoleId = ownerRoleId },
                new IdentityUserRole<Guid> { UserId = ownerZId, RoleId = ownerRoleId },

                // Admin role assignment
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId },
                new IdentityUserRole<Guid> { UserId = secondAdminId, RoleId = adminRoleId },

                // Scenario users (merged from AdminDashboardScenarioUserRoleSeed)
                new IdentityUserRole<Guid> { UserId = pendingRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = bannedRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = deletedRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = recentRenterId, RoleId = renterRoleId }
            );
        }
    }
}
