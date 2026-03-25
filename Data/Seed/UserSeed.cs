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

            // All seeded renters share the same demo password:
            // Password: Password123!
            // Hash generated with ASP.NET Core Identity PasswordHasher
            var demoPasswordHash = "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==";

            builder.HasData(
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
                }
            );
        }
    }
}

