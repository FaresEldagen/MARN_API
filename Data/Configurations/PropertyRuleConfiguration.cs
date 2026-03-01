using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PropertyRuleConfiguration : IEntityTypeConfiguration<PropertyRule>
    {
        public void Configure(EntityTypeBuilder<PropertyRule> builder)
        {
            builder.Property(pr => pr.Rule).IsRequired();
            builder.Property(pr => pr.PropertyId).IsRequired();

            builder.HasOne(pr => pr.Property)
                   .WithMany(p => p.Rules)
                   .HasForeignKey(pr => pr.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}



