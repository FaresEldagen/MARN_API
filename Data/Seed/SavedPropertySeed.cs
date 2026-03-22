using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class SavedPropertySeed : IEntityTypeConfiguration<SavedProperty>
    {
        public void Configure(EntityTypeBuilder<SavedProperty> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                // Renter A saves Property 1001 and 1003
                new SavedProperty
                {
                    PropertyId = 1001,
                    UserId = renterAId
                },
                new SavedProperty
                {
                    PropertyId = 1003,
                    UserId = renterAId
                },

                // Renter B saves Property 1002
                new SavedProperty
                {
                    PropertyId = 1002,
                    UserId = renterBId
                },

                // Owner Z (dual-role) saves Property 1001 and 1002 for renter dashboard
                new SavedProperty
                {
                    PropertyId = 1001,
                    UserId = ownerZId
                },
                new SavedProperty
                {
                    PropertyId = 1002,
                    UserId = ownerZId
                }
            );
        }
    }
}

