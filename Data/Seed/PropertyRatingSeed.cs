using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyRatingSeed : IEntityTypeConfiguration<PropertyRating>
    {
        public void Configure(EntityTypeBuilder<PropertyRating> builder)
        {
            builder.HasData(
                new PropertyRating
                {
                    Id = 900001,
                    PropertyId = 1001,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Rating = 5,
                    CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyRating
                {
                    Id = 900002,
                    PropertyId = 1001,
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Rating = 4,
                    CreatedAt = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyRating
                {
                    Id = 900003,
                    PropertyId = 1004,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Rating = 5,
                    CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                });
        }
    }
}
