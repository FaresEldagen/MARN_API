using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
    {
        public void Configure(EntityTypeBuilder<BookingRequest> builder)
        {
            builder.Property(br => br.StartDate).IsRequired();
            builder.Property(br => br.EndDate).IsRequired();
            builder.Property(br => br.PropertyId).IsRequired();
            builder.Property(br => br.OwnerId).IsRequired();
            builder.Property(br => br.RenterId).IsRequired();

            builder.Property(br => br.Status).HasConversion<int>();
            builder.Property(br => br.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.ToTable(t => t.HasCheckConstraint("CK_BookingRequest_Dates", "[EndDate] > [StartDate]"));

            builder.HasOne(br => br.Property)
                   .WithMany(p => p.BookingRequests)
                   .HasForeignKey(br => br.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(br => br.Renter)
                   .WithMany(u => u.BookingRequestsAsRenter)
                   .HasForeignKey(br => br.RenterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(br => new { br.PropertyId, br.OwnerId, br.RenterId });
        }
    }
}



