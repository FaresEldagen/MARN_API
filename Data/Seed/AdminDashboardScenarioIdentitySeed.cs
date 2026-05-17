using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Account;
using MARN_API.Models;
using System;

namespace MARN_API.Data.Seed
{
    public class AdminDashboardScenarioRoleSeed : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityRole<Guid>
                {
                    Id = AdminDashboardScenarioIds.ModeratorRoleId,
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                });
        }
    }

    public class AdminDashboardScenarioUserSeed : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var demoPasswordHash = "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==";

            builder.HasData(
                new ApplicationUser
                {
                    Id = AdminDashboardScenarioIds.PendingRenterId,
                    UserName = "pending.renter@example.com",
                    NormalizedUserName = "PENDING.RENTER@EXAMPLE.COM",
                    Email = "pending.renter@example.com",
                    NormalizedEmail = "PENDING.RENTER@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SCENARIO-PENDING-RENTER-SECURITY-STAMP",
                    ConcurrencyStamp = "SCENARIO-PENDING-RENTER-CONCURRENCY-STAMP",
                    FirstName = "Pending",
                    LastName = "Renter",
                    ArabicFullName = "مستخدم قيد التحقق",
                    ArabicAddress = "15 شارع التسعين، القاهرة الجديدة",
                    NationalIDNumber = "34567890123456",
                    FrontIdPhoto = "/images/idCards/pending-renter-front.jpg",
                    BackIdPhoto = "/images/idCards/pending-renter-back.jpg",
                    AccountStatus = AccountStatus.Pending,
                    Country = Country.Egypt,
                    Gender = Gender.Female,
                    Language = Language.Arabic,
                    CreatedAt = new DateTime(2026, 5, 2, 9, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = AdminDashboardScenarioIds.BannedRenterId,
                    UserName = "banned.renter@example.com",
                    NormalizedUserName = "BANNED.RENTER@EXAMPLE.COM",
                    Email = "banned.renter@example.com",
                    NormalizedEmail = "BANNED.RENTER@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SCENARIO-BANNED-RENTER-SECURITY-STAMP",
                    ConcurrencyStamp = "SCENARIO-BANNED-RENTER-CONCURRENCY-STAMP",
                    FirstName = "Banned",
                    LastName = "Renter",
                    ArabicFullName = "مستخدم موقوف",
                    ArabicAddress = "22 شارع النصر، مدينة نصر",
                    NationalIDNumber = "45678901234567",
                    FrontIdPhoto = "/images/idCards/banned-renter-front.jpg",
                    BackIdPhoto = "/images/idCards/banned-renter-back.jpg",
                    AccountStatus = AccountStatus.Banned,
                    StatusBeforeBan = AccountStatus.Pending,
                    Country = Country.Egypt,
                    Gender = Gender.Male,
                    Language = Language.English,
                    CreatedAt = new DateTime(2026, 2, 14, 10, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = AdminDashboardScenarioIds.DeletedRenterId,
                    UserName = "deleted.renter@example.com",
                    NormalizedUserName = "DELETED.RENTER@EXAMPLE.COM",
                    Email = "deleted.renter@example.com",
                    NormalizedEmail = "DELETED.RENTER@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SCENARIO-DELETED-RENTER-SECURITY-STAMP",
                    ConcurrencyStamp = "SCENARIO-DELETED-RENTER-CONCURRENCY-STAMP",
                    FirstName = "Deleted",
                    LastName = "Renter",
                    AccountStatus = AccountStatus.Verified,
                    Country = Country.Egypt,
                    Gender = Gender.Male,
                    Language = Language.English,
                    CreatedAt = new DateTime(2026, 3, 3, 8, 0, 0, DateTimeKind.Utc),
                    DeletedAt = new DateTime(2026, 4, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = AdminDashboardScenarioIds.RecentRenterId,
                    UserName = "recent.renter@example.com",
                    NormalizedUserName = "RECENT.RENTER@EXAMPLE.COM",
                    Email = "recent.renter@example.com",
                    NormalizedEmail = "RECENT.RENTER@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SCENARIO-RECENT-RENTER-SECURITY-STAMP",
                    ConcurrencyStamp = "SCENARIO-RECENT-RENTER-CONCURRENCY-STAMP",
                    FirstName = "Recent",
                    LastName = "Renter",
                    Bio = "Fresh account created to validate the dashboard new-user metrics.",
                    AccountStatus = AccountStatus.Verified,
                    Country = Country.Egypt,
                    Gender = Gender.Female,
                    Language = Language.English,
                    CreatedAt = new DateTime(2026, 5, 10, 14, 30, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = AdminDashboardScenarioIds.ModeratorUserId,
                    UserName = "moderator.user@example.com",
                    NormalizedUserName = "MODERATOR.USER@EXAMPLE.COM",
                    Email = "moderator.user@example.com",
                    NormalizedEmail = "MODERATOR.USER@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SCENARIO-MODERATOR-USER-SECURITY-STAMP",
                    ConcurrencyStamp = "SCENARIO-MODERATOR-USER-CONCURRENCY-STAMP",
                    FirstName = "Mona",
                    LastName = "Moderator",
                    Bio = "Seeded moderator candidate for role-management testing.",
                    AccountStatus = AccountStatus.Verified,
                    Country = Country.Egypt,
                    Gender = Gender.Female,
                    Language = Language.English,
                    CreatedAt = new DateTime(2026, 4, 20, 11, 0, 0, DateTimeKind.Utc)
                },
                // Second Admin (previously seeded via AdminDashboardScenarioAdminSeed)
                new ApplicationUser
                {
                    Id = AdminDashboardScenarioIds.SecondAdminId,
                    UserName = "assistant.admin@marn.com",
                    NormalizedUserName = "ASSISTANT.ADMIN@MARN.COM",
                    Email = "assistant.admin@marn.com",
                    NormalizedEmail = "ASSISTANT.ADMIN@MARN.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SCENARIO-SECOND-ADMIN-SECURITY-STAMP",
                    ConcurrencyStamp = "SCENARIO-SECOND-ADMIN-CONCURRENCY-STAMP",
                    FirstName = "Assistant",
                    LastName = "Admin",
                    AccountStatus = AccountStatus.Verified,
                    Country = Country.Egypt,
                    Gender = Gender.Unknown,
                    Language = Language.English,
                    CreatedAt = new DateTime(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc)
                });
        }
    }

    public class AdminDashboardScenarioUserRoleSeed : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            var renterRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var adminRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            builder.HasData(
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.PendingRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.BannedRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.DeletedRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.RecentRenterId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.ModeratorUserId, RoleId = renterRoleId },
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.ModeratorUserId, RoleId = AdminDashboardScenarioIds.ModeratorRoleId },
                new IdentityUserRole<Guid> { UserId = AdminDashboardScenarioIds.SecondAdminId, RoleId = adminRoleId });
        }
    }
}

