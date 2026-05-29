using MARN_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARN_API.Data.Configurations
{
    public class AssistantMessageConfiguration : IEntityTypeConfiguration<AssistantMessage>
    {
        public void Configure(EntityTypeBuilder<AssistantMessage> builder)
        {
            builder.ToTable("assistantMessages", table =>
            {
                table.HasCheckConstraint(
                    "CK_assistantMessages_Role",
                    "[Role] IN ('user', 'assistant', 'tool')");
            });

            builder.HasKey(m => m.MessageId);

            builder.Property(m => m.Role)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.ToolOnly)
                .HasDefaultValue(false);

            builder.Property(m => m.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(m => m.ImagePathsJson)
                .HasColumnType("nvarchar(max)");

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => new { m.SessionId, m.CreatedAt });
            builder.HasIndex(m => new { m.SessionId, m.ToolOnly, m.CreatedAt });
            builder.HasIndex(m => m.UserId);
        }
    }
}
