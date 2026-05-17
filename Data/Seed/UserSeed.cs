using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Account;

namespace MARN_API.Data.Seed
{
    public class UserSeed : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var renterDId = Guid.Parse("77777777-7777-7777-7777-777777777777");
            var renterEId = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var adminId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            // All seeded users share the same demo password:
            // Password: Password123!
            // Hash generated with ASP.NET Core Identity PasswordHasher
            var demoPasswordHash = "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==";

            builder.HasData(
                // ── Renters ──
                new ApplicationUser
                {
                    Id = renterAId,
                    UserName = "renter.a@example.com",
                    NormalizedUserName = "RENTER.A@EXAMPLE.COM",
                    Email = "renter.a@example.com",
                    NormalizedEmail = "RENTER.A@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-RENTER-A-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-RENTER-A-CONCURRENCY-STAMP",

                    FirstName = "Renter",
                    LastName = "Alpha",
                    ArabicFullName = "رينتر ألفا",
                    ArabicAddress = "123 شارع النيل، القاهرة",
                    NationalIDNumber = "12345678901234",
                    FrontIdPhoto = "/images/idCards/95c1567c-357c-4c0a-b711-e0ba27c1a96f.jpg",
                    BackIdPhoto = "/images/idCards/b8ee0c84-7a46-457d-a6d5-9696166b3c87.jpg",
                    Language = Language.Arabic,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    ProfileImage = "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = renterBId,
                    UserName = "renter.b@example.com",
                    NormalizedUserName = "RENTER.B@EXAMPLE.COM",
                    Email = "renter.b@example.com",
                    NormalizedEmail = "RENTER.B@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-RENTER-B-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-RENTER-B-CONCURRENCY-STAMP",

                    FirstName = "Renter",
                    LastName = "Beta",
                    ArabicFullName = "رينتر بيتا",
                    ArabicAddress = "456 شارع المعادي، القاهرة",
                    NationalIDNumber = "23456789012345",
                    FrontIdPhoto = "/images/idCards/f9797aa8-46ce-4dbb-ad14-2a521ed962fc.jpg",
                    BackIdPhoto = "/images/idCards/0b2b1890-82ff-4459-be9a-6dc65971849a.jpg",
                    Language = Language.English,
                    Gender = Gender.Female,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = renterCId,
                    UserName = "renter.c@example.com",
                    NormalizedUserName = "RENTER.C@EXAMPLE.COM",
                    Email = "renter.c@example.com",
                    NormalizedEmail = "RENTER.C@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-RENTER-C-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-RENTER-C-CONCURRENCY-STAMP",

                    FirstName = "Renter",
                    LastName = "Gamma",
                    Language = Language.English,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = renterDId,
                    UserName = "renter.d@example.com",
                    NormalizedUserName = "RENTER.D@EXAMPLE.COM",
                    Email = "renter.d@example.com",
                    NormalizedEmail = "RENTER.D@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-RENTER-D-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-RENTER-D-CONCURRENCY-STAMP",
                    FirstName = "Renter",
                    LastName = "Delta",
                    Language = Language.English,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = renterEId,
                    UserName = "renter.e@example.com",
                    NormalizedUserName = "RENTER.E@EXAMPLE.COM",
                    Email = "renter.e@example.com",
                    NormalizedEmail = "RENTER.E@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-RENTER-E-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-RENTER-E-CONCURRENCY-STAMP",
                    FirstName = "Renter",
                    LastName = "Epsilon",
                    Language = Language.English,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc)
                },

                // ── Owners (role determined by AspNetUserRoles) ──
                new ApplicationUser
                {
                    Id = ownerXId,
                    UserName = "owner.x@example.com",
                    NormalizedUserName = "OWNER.X@EXAMPLE.COM",
                    Email = "owner.x@example.com",
                    NormalizedEmail = "OWNER.X@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-OWNER-X-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-OWNER-X-CONCURRENCY-STAMP",

                    FirstName = "Owner",
                    LastName = "X",
                    Language = Language.English,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc)
                },
                new ApplicationUser
                {
                    Id = ownerYId,
                    UserName = "owner.y@example.com",
                    NormalizedUserName = "OWNER.Y@EXAMPLE.COM",
                    Email = "owner.y@example.com",
                    NormalizedEmail = "OWNER.Y@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-OWNER-Y-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-OWNER-Y-CONCURRENCY-STAMP",

                    FirstName = "Owner",
                    LastName = "Y",
                    Language = Language.English,
                    Gender = Gender.Female,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                // Owner Z: dual-role account (Owner + Renter) with data for both dashboards
                new ApplicationUser
                {
                    Id = ownerZId,
                    UserName = "owner.z@example.com",
                    NormalizedUserName = "OWNER.Z@EXAMPLE.COM",
                    Email = "owner.z@example.com",
                    NormalizedEmail = "OWNER.Z@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-OWNER-Z-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-OWNER-Z-CONCURRENCY-STAMP",

                    FirstName = "Owner",
                    LastName = "Z",
                    Language = Language.English,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc)
                },

                // ── Admin (role determined by AspNetUserRoles) ──
                new ApplicationUser
                {
                    Id = adminId,
                    UserName = "admin@marn.com",
                    NormalizedUserName = "ADMIN@MARN.COM",
                    Email = "admin@marn.com",
                    NormalizedEmail = "ADMIN@MARN.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-ADMIN-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-ADMIN-CONCURRENCY-STAMP",

                    FirstName = "System",
                    LastName = "Admin",
                    Language = Language.English,
                    Gender = Gender.Unknown,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Verified,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
