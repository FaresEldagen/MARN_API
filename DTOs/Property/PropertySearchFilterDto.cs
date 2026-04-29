using MARN_API.Enums.Property;

namespace MARN_API.DTOs.Property
{
    /// <summary>
    /// Search, filter, and ordering parameters for property listings.
    /// All filters are optional — omit any field to skip that filter.
    /// </summary>
    public class PropertySearchFilterDto
    {
        // ── Free-text search ────────────────────────────────────────
        /// <summary>Keyword to match against title, description, address, city, or state.</summary>
        public string? Keyword { get; set; }

        // ── Location ────────────────────────────────────────────────
        /// <summary>User's latitude for distance-based sorting / radius filter.</summary>
        public double? Latitude { get; set; }

        /// <summary>User's longitude for distance-based sorting / radius filter.</summary>
        public double? Longitude { get; set; }

        /// <summary>Maximum distance in kilometres from the supplied lat/lng. Requires Latitude and Longitude.</summary>
        public double? RadiusKm { get; set; }

        /// <summary>Filter by city.</summary>
        public City? City { get; set; }

        /// <summary>Filter by governorate (state).</summary>
        public Governorate? Governorate { get; set; }

        // ── Property characteristics ────────────────────────────────
        /// <summary>Filter by property type (Apartment, House, Room, Villa, Studio, SharedRoom).</summary>
        public PropertyType? Type { get; set; }

        /// <summary>Filter by rental duration unit (Daily, Monthly, Yearly).</summary>
        public RentalUnit? RentalUnit { get; set; }

        /// <summary>Only shared properties (true) or private (false). Null = both.</summary>
        public bool? IsShared { get; set; }

        // ── Price ───────────────────────────────────────────────────
        /// <summary>Minimum price (inclusive).</summary>
        public decimal? MinPrice { get; set; }

        /// <summary>Maximum price (inclusive).</summary>
        public decimal? MaxPrice { get; set; }

        // ── Room / capacity counts ──────────────────────────────────
        /// <summary>Minimum number of bedrooms.</summary>
        public int? MinBedrooms { get; set; }

        /// <summary>Minimum number of beds.</summary>
        public int? MinBeds { get; set; }

        /// <summary>Minimum number of bathrooms.</summary>
        public int? MinBathrooms { get; set; }

        /// <summary>Minimum max-occupant capacity.</summary>
        public int? MinMaxOccupants { get; set; }

        // ── Area ────────────────────────────────────────────────────
        /// <summary>Minimum square metres.</summary>
        public double? MinSquareMeters { get; set; }

        /// <summary>Maximum square metres.</summary>
        public double? MaxSquareMeters { get; set; }

        // ── Rating ──────────────────────────────────────────────────
        /// <summary>Minimum average rating (1-5).</summary>
        public float? MinRating { get; set; }

        // ── Amenities ───────────────────────────────────────────────
        /// <summary>Properties must have ALL of these amenities.</summary>
        public List<AmenityType>? Amenities { get; set; }

        // ── Ordering ────────────────────────────────────────────────
        /// <summary>
        /// Field to sort by: Newest, Price, Rating, Bedrooms, Bathrooms, SquareMeters, Distance.
        /// Default: Newest.
        /// </summary>
        public PropertySortBy? SortBy { get; set; }

        /// <summary>true = ascending, false = descending. Default: depends on SortBy.</summary>
        public bool? SortAscending { get; set; }

        // ── Pagination ──────────────────────────────────────────────
        /// <summary>Page number (1-based). Default: 1.</summary>
        public int Page { get; set; } = 1;

        /// <summary>Items per page (max 50). Default: 20.</summary>
        public int PageSize { get; set; } = 20;
    }
}
