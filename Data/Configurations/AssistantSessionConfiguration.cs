using MARN_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARN_API.Data.Configurations
{
    public class AssistantSessionConfiguration : IEntityTypeConfiguration<AssistantSession>
    {
        public void Configure(EntityTypeBuilder<AssistantSession> builder)
        {
            builder.ToTable("assistantSessions");

            builder.HasKey(s => s.SessionId);

            builder.Property(s => s.SessionName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(s => s.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Messages)
                .WithOne(m => m.Session)
                .HasForeignKey(m => m.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new { s.UserId, s.UpdatedAt });
        }
    }
}
