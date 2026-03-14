using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyMediaSeed : IEntityTypeConfiguration<PropertyMedia>
    {
        public void Configure(EntityTypeBuilder<PropertyMedia> builder)
        {
            builder.HasData(
                new PropertyMedia
                {
                    Id = 2001,
                    PropertyId = 1001,
                    Path = "/images/seed/property1-main.jpg",
                    IsPrimary = true
                },
                new PropertyMedia
                {
                    Id = 2002,
                    PropertyId = 1002,
                    Path = "/images/seed/property2-main.jpg",
                    IsPrimary = true
                },
                new PropertyMedia
                {
                    Id = 2003,
                    PropertyId = 1003,
                    Path = "/images/seed/property3-main.jpg",
                    IsPrimary = true
                }
            );
        }
    }
}

