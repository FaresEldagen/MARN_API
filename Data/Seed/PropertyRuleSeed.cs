using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyRuleSeed : IEntityTypeConfiguration<PropertyRule>
    {
        public void Configure(EntityTypeBuilder<PropertyRule> builder)
        {
            builder.HasData(
                new PropertyRule { Id = 1, PropertyId = 1001, Rule = "No Smoking inside the apartment." },
                new PropertyRule { Id = 2, PropertyId = 1001, Rule = "No parties or loud music after 11 PM." },
                
                new PropertyRule { Id = 3, PropertyId = 1002, Rule = "Pets are not allowed." },
                
                new PropertyRule { Id = 4, PropertyId = 1003, Rule = "Single occupancy only." },
                
                new PropertyRule { Id = 5, PropertyId = 1004, Rule = "Respect the neighbors." },
                new PropertyRule { Id = 6, PropertyId = 1004, Rule = "Smoking allowed only in the balcony." }
            );
        }
    }
}
