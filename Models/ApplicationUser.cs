using Microsoft.AspNetCore.Identity;
using MARN_API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARN_API.Models
{
    // Base user for Identity (Renter by default)
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public Language Language { get; set; } = Language.English;
        public string? ProfileImage { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public Country Country { get; set; } = Country.Unknown;

        // KYC / Identity
        public string? FrontIdPhoto { get; set; }
        public string? BackIdPhoto { get; set; }
        public string? ArabicAddress { get; set; }
        public string? ArabicFullName { get; set; }
        public string? NationalIDNumber { get; set; }

        public AccountStatus AccountStatus { get; set; } = AccountStatus.Unverified;

        // Soft delete and audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual RoommatePreference? RoommatePreference { get; set; }
        public virtual ICollection<BookingRequest> BookingRequestsAsRenter { get; set; } = new HashSet<BookingRequest>();
        public virtual ICollection<Contract> ContractsAsRenter { get; set; } = new HashSet<Contract>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
        public virtual ICollection<ChatRoomParticipant> ChatRoomParticipants { get; set; } = new HashSet<ChatRoomParticipant>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<Report> ReportsFiled { get; set; } = new HashSet<Report>();
        public virtual ICollection<UserActivity> Activities { get; set; } = new HashSet<UserActivity>();
        public virtual ICollection<SavedProperty> SavedProperty { get; set; } = new HashSet<SavedProperty>();

        [InverseProperty(nameof(Message.Sender))]
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();

        [InverseProperty(nameof(Message.Receiver))]
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();

    }
}
