using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MARN_API.Models;
using MARN_API.Data.Configurations;
using MARN_API.Data.Seed;

namespace MARN_API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        // public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        // DbSets for all entities
        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<RoommatePreference> RoommatePreferences => Set<RoommatePreference>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<ChatRoomParticipant> ChatRoomParticipants => Set<ChatRoomParticipant>();
        public DbSet<PropertyAmenity> PropertyAmenities => Set<PropertyAmenity>();
        public DbSet<PropertyRule> PropertyRules => Set<PropertyRule>();
        public DbSet<PropertyMedia> PropertyMedia => Set<PropertyMedia>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<UserActivity> UserActivities => Set<UserActivity>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<SavedProperty> SavedProperties => Set<SavedProperty>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply entity configurations
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new OwnerConfiguration());
            builder.ApplyConfiguration(new PropertyConfiguration());
            builder.ApplyConfiguration(new ContractConfiguration());
            builder.ApplyConfiguration(new BookingRequestConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new RoommatePreferenceConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());
            builder.ApplyConfiguration(new ChatRoomConfiguration());
            builder.ApplyConfiguration(new ChatMessageConfiguration());
            builder.ApplyConfiguration(new ChatRoomParticipantConfiguration());
            builder.ApplyConfiguration(new PropertyAmenityConfiguration());
            builder.ApplyConfiguration(new PropertyRuleConfiguration());
            builder.ApplyConfiguration(new PropertyMediaConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new ReportConfiguration());
            builder.ApplyConfiguration(new UserActivityConfiguration());
            builder.ApplyConfiguration(new AdminConfiguration());
            builder.ApplyConfiguration(new SavedPropertyConfiguration());


            // Seed initial data
            builder.ApplyConfiguration(new RoleSeed());
        }
    }
}
