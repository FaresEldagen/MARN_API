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
                    Path = "/images/properties/property1-main.jpg",
                    IsPrimary = true
                },
                new PropertyMedia
                {
                    Id = 2002,
                    PropertyId = 1001,
                    Path = "/images/properties/property1-secondary.jpg",
                    IsPrimary = false
                },
                new PropertyMedia
                {
                    Id = 2003,
                    PropertyId = 1002,
                    Path = "/images/properties/property2-main.jpg",
                    IsPrimary = true
                },
                new PropertyMedia
                {
                    Id = 2004,
                    PropertyId = 1003,
                    Path = "/images/properties/property3-main.jpg",
                    IsPrimary = true
                },
                new PropertyMedia
                {
                    Id = 2005,
                    PropertyId = 1004,
                    Path = "/images/properties/property4-main.jpg",
                    IsPrimary = true
                },
                new PropertyMedia
                {
                    Id = 2006,
                    PropertyId = 1100,
                    Path = "/images/properties/property100-main.jpg",
                    IsPrimary = true
                }
            );
        }
    }
}

