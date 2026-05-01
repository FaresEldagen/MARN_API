using System;
using System.Collections.Generic;
using MARN_API.Enums.Property;

namespace MARN_API.Models
{
    public class Property
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public string ProofOfOwnership { get; set; } = string.Empty;

        public int MaxOccupants { get; set; }
        public bool IsShared { get; set; }

        public int Bedrooms { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public double SquareMeters { get; set; }
        public int Views { get; set; } = 0;

        public decimal Price { get; set; }
        public RentalUnit RentalUnit { get; set; }

        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsActive { get; set; } = true;
        public PropertyStatus Status { get; set; } = PropertyStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }


        public Guid OwnerId { get; set; }
        public virtual Owner Owner { get; set; } = null!;
        public virtual ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new HashSet<RentalTransaction>();
        public virtual ICollection<BookingRequest> BookingRequests { get; set; } = new HashSet<BookingRequest>();
        public virtual ICollection<PropertyRating> PropertyRatings { get; set; } = new HashSet<PropertyRating>();
        public virtual ICollection<PropertyComment> PropertyComments { get; set; } = new HashSet<PropertyComment>();
        public virtual ICollection<PropertyAmenity> Amenities { get; set; } = new HashSet<PropertyAmenity>();
        public virtual ICollection<PropertyRule> Rules { get; set; } = new HashSet<PropertyRule>();
        public virtual ICollection<PropertyMedia> Media { get; set; } = new HashSet<PropertyMedia>();
        public virtual ICollection<SavedProperty> SavedProperty { get; set; } = new HashSet<SavedProperty>();
    }
}



