using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Property;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyRepo : IPropertyRepo
    {
        private readonly AppDbContext Context;
        public PropertyRepo(AppDbContext context)
        {
            Context = context;
        }


        #region Owner Dashboard and Profile
        public Task<List<OwnerDashboardPropertyCardDto>> GetOwnerDashboardProperties(Guid userId)
        {
            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new OwnerDashboardPropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty,
                    Title = p.Title,
                    Address = p.Address,
                    Type = p.Type,
                    Views = p.Views,
                    IsSaved = p.SavedProperty.Any(s => s.UserId == userId),

                    OccupiedPlaces = p.Contracts
                        .Where(c => c.Status == ContractStatus.Active)
                        .Select(c => c.Property.IsShared ? 1 : c.Property.MaxOccupants)
                        .Sum(),
                    TotalPlaces = p.MaxOccupants,
                    
                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                    
                    AverageRating = p.PropertyRatings.Any() ? p.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f : 0f,
                    Ratings = p.PropertyRatings.Count,

                    ActiveContracts = p.Contracts
                        .Where(c => c.PropertyId == p.Id && c.Status == ContractStatus.Active)
                        .Select(c => new OwnerPropertyContractDto
                        {
                            ContractId = c.Id,
                            RenterId = c.RenterId,
                            RenterName = $"{c.Renter.FirstName} {c.Renter.LastName}",
                            RenterProfileImage = c.Renter.ProfileImage
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public Task<List<PropertyCardDto>> GetOwnerProfileProperties(Guid userId)
        {
            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new PropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty,
                    Title = p.Title,
                    Address = p.Address,
                    IsSaved = p.SavedProperty.Any(s => s.UserId == userId),

                    MaxOccupants = p.MaxOccupants,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,

                    Type = p.Type,
                    AverageRating = p.PropertyRatings.Any() ? p.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f : 0f,
                    Ratings = p.PropertyRatings.Count,

                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                })
                .ToListAsync();
        }

        public Task<int> GetOwnedPropertiesViewsCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId && p.DeletedAt == null)
                .SumAsync(p => p.Views);
        }

        public Task<int> GetOwnedPropertiesPlacesCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId && p.DeletedAt == null)
                .SumAsync(p => p.MaxOccupants);
        }

        public async Task<float> GetOwnerAverageRating(Guid userid)
        {
            var avg = await Context.Properties
                .Where(p => p.OwnerId == userid)
                .SelectMany(p => p.PropertyRatings)
                .AverageAsync(r => (float?)r.Rating);

            return avg ?? 0f;
        }

        public Task<int> GetOwnerRatingsCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId)
                .SelectMany(p => p.PropertyRatings)
                .CountAsync();
        }

        public Task<bool> ExistsAsync(long propertyId)
        {
            return Context.Properties
                .AsNoTracking()
                .AnyAsync(p => p.Id == propertyId);
        }
        #endregion

        
        #region Property Operation
        public async Task AddPropertyAsync(Property property)
        {
            await Context.Properties.AddAsync(property);
            await Context.SaveChangesAsync();
        }

        public async Task<PropertySearchResultDto> SearchPropertiesAsync(PropertySearchFilterDto filter, Guid? currentUserId)
        {
            var hasUser = currentUserId.HasValue;
            var userId = currentUserId ?? Guid.Empty;

            // Clamp pagination
            if (filter.Page < 1) filter.Page = 1;
            if (filter.PageSize < 1) filter.PageSize = 20;
            if (filter.PageSize > 50) filter.PageSize = 50;

            // Base query: only active, verified, non-deleted properties
            var query = Context.Properties
                .AsNoTracking()
                .Where(p => p.IsActive
                         && p.Status == MARN_API.Enums.Property.PropertyStatus.Verified
                         && p.DeletedAt == null);

            // ── Keyword ─────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim().ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(kw) ||
                    p.Description.ToLower().Contains(kw) ||
                    p.Address.ToLower().Contains(kw) ||
                    p.City.ToLower().Contains(kw) ||
                    p.State.ToLower().Contains(kw));
            }

            // ── Location radius (Haversine approximation) ───────────
            if (filter.Latitude.HasValue && filter.Longitude.HasValue && filter.RadiusKm.HasValue && filter.RadiusKm.Value > 0)
            {
                var lat = filter.Latitude.Value;
                var lng = filter.Longitude.Value;
                var radiusKm = filter.RadiusKm.Value;

                // Pre-filter with a bounding box to reduce calculation load
                var latDelta = radiusKm / 111.0;
                var lngDelta = radiusKm / (111.0 * Math.Cos(lat * Math.PI / 180.0));

                query = query.Where(p =>
                    p.Latitude >= lat - latDelta && p.Latitude <= lat + latDelta &&
                    p.Longitude >= lng - lngDelta && p.Longitude <= lng + lngDelta);

                // Haversine exact filter
                query = query.Where(p =>
                    6371.0 * 2.0 * Math.Atan2(
                        Math.Sqrt(
                            Math.Sin((p.Latitude - lat) * Math.PI / 180.0 / 2.0) *
                            Math.Sin((p.Latitude - lat) * Math.PI / 180.0 / 2.0) +
                            Math.Cos(lat * Math.PI / 180.0) *
                            Math.Cos(p.Latitude * Math.PI / 180.0) *
                            Math.Sin((p.Longitude - lng) * Math.PI / 180.0 / 2.0) *
                            Math.Sin((p.Longitude - lng) * Math.PI / 180.0 / 2.0)
                        ),
                        Math.Sqrt(
                            1.0 - (
                                Math.Sin((p.Latitude - lat) * Math.PI / 180.0 / 2.0) *
                                Math.Sin((p.Latitude - lat) * Math.PI / 180.0 / 2.0) +
                                Math.Cos(lat * Math.PI / 180.0) *
                                Math.Cos(p.Latitude * Math.PI / 180.0) *
                                Math.Sin((p.Longitude - lng) * Math.PI / 180.0 / 2.0) *
                                Math.Sin((p.Longitude - lng) * Math.PI / 180.0 / 2.0)
                            )
                        )
                    ) <= radiusKm
                );
            }

            // ── City / Governorate ──────────────────────────────────
            if (filter.City.HasValue)
            {
                var cityName = filter.City.Value.ToString();
                query = query.Where(p => p.City.Contains(cityName));
            }
            if (filter.Governorate.HasValue)
            {
                var govName = filter.Governorate.Value.ToString();
                query = query.Where(p => p.State.Contains(govName));
            }

            // ── Property type ───────────────────────────────────────
            if (filter.Type.HasValue)
                query = query.Where(p => p.Type == filter.Type.Value);

            // ── Rental unit ─────────────────────────────────────────
            if (filter.RentalUnit.HasValue)
                query = query.Where(p => p.RentalUnit == filter.RentalUnit.Value);

            // ── IsShared ────────────────────────────────────────────
            if (filter.IsShared.HasValue)
                query = query.Where(p => p.IsShared == filter.IsShared.Value);

            // ── Price range ─────────────────────────────────────────
            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            // ── Room counts ─────────────────────────────────────────
            if (filter.MinBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
            if (filter.MinBeds.HasValue)
                query = query.Where(p => p.Beds >= filter.MinBeds.Value);
            if (filter.MinBathrooms.HasValue)
                query = query.Where(p => p.Bathrooms >= filter.MinBathrooms.Value);
            if (filter.MinMaxOccupants.HasValue)
                query = query.Where(p => p.MaxOccupants >= filter.MinMaxOccupants.Value);

            // ── Area ────────────────────────────────────────────────
            if (filter.MinSquareMeters.HasValue)
                query = query.Where(p => p.SquareMeters >= filter.MinSquareMeters.Value);
            if (filter.MaxSquareMeters.HasValue)
                query = query.Where(p => p.SquareMeters <= filter.MaxSquareMeters.Value);

            // ── Rating ──────────────────────────────────────────────
            if (filter.MinRating.HasValue)
            {
                var minRating = filter.MinRating.Value;
                query = query.Where(p =>
                    p.PropertyRatings.Any() &&
                    p.PropertyRatings.Average(r => (float?)r.Rating) >= minRating);
            }

            // ── Amenities (must have ALL) ───────────────────────────
            if (filter.Amenities != null && filter.Amenities.Count > 0)
            {
                foreach (var amenity in filter.Amenities)
                {
                    query = query.Where(p => p.Amenities.Any(a => a.Amenity == amenity));
                }
            }

            // ── Count before paging ─────────────────────────────────
            var totalCount = await query.CountAsync();

            // ── Sorting ─────────────────────────────────────────────
            var sortBy = filter.SortBy ?? PropertySortBy.Newest;

            IOrderedQueryable<Models.Property> orderedQuery = sortBy switch
            {
                PropertySortBy.Price => filter.SortAscending ?? true
                    ? query.OrderBy(p => p.Price)
                    : query.OrderByDescending(p => p.Price),

                PropertySortBy.Rating => filter.SortAscending ?? false
                    ? query.OrderBy(p => p.PropertyRatings.Any() ? p.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f : 0f)
                    : query.OrderByDescending(p => p.PropertyRatings.Any() ? p.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f : 0f),

                PropertySortBy.Bedrooms => filter.SortAscending ?? false
                    ? query.OrderBy(p => p.Bedrooms)
                    : query.OrderByDescending(p => p.Bedrooms),

                PropertySortBy.Bathrooms => filter.SortAscending ?? false
                    ? query.OrderBy(p => p.Bathrooms)
                    : query.OrderByDescending(p => p.Bathrooms),

                PropertySortBy.SquareMeters => filter.SortAscending ?? false
                    ? query.OrderBy(p => p.SquareMeters)
                    : query.OrderByDescending(p => p.SquareMeters),

                PropertySortBy.Distance when filter.Latitude.HasValue && filter.Longitude.HasValue =>
                    (filter.SortAscending ?? true)
                    ? query.OrderBy(p =>
                        6371.0 * 2.0 * Math.Atan2(
                            Math.Sqrt(
                                Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) *
                                Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) +
                                Math.Cos(filter.Latitude.Value * Math.PI / 180.0) *
                                Math.Cos(p.Latitude * Math.PI / 180.0) *
                                Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0) *
                                Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0)
                            ),
                            Math.Sqrt(
                                1.0 - (
                                    Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) *
                                    Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) +
                                    Math.Cos(filter.Latitude.Value * Math.PI / 180.0) *
                                    Math.Cos(p.Latitude * Math.PI / 180.0) *
                                    Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0) *
                                    Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0)
                                )
                            )
                        ))
                    : query.OrderByDescending(p =>
                        6371.0 * 2.0 * Math.Atan2(
                            Math.Sqrt(
                                Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) *
                                Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) +
                                Math.Cos(filter.Latitude.Value * Math.PI / 180.0) *
                                Math.Cos(p.Latitude * Math.PI / 180.0) *
                                Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0) *
                                Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0)
                            ),
                            Math.Sqrt(
                                1.0 - (
                                    Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) *
                                    Math.Sin((p.Latitude - filter.Latitude.Value) * Math.PI / 180.0 / 2.0) +
                                    Math.Cos(filter.Latitude.Value * Math.PI / 180.0) *
                                    Math.Cos(p.Latitude * Math.PI / 180.0) *
                                    Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0) *
                                    Math.Sin((p.Longitude - filter.Longitude.Value) * Math.PI / 180.0 / 2.0)
                                )
                            )
                        )),

                // Default: newest
                _ => filter.SortAscending ?? false
                    ? query.OrderBy(p => p.CreatedAt)
                    : query.OrderByDescending(p => p.CreatedAt),
            };

            // Tiebreaker for stable paging
            orderedQuery = orderedQuery.ThenByDescending(p => p.Id);

            // ── Projection + Pagination ─────────────────────────────
            var items = await orderedQuery
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(p => new PropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty,
                    Title = p.Title,
                    Address = p.Address,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    MaxOccupants = p.MaxOccupants,
                    Type = p.Type,
                    AverageRating = p.PropertyRatings.Any()
                        ? p.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f
                        : 0f,
                    Ratings = p.PropertyRatings.Count,
                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                    IsSaved = hasUser && p.SavedProperty.Any(s => s.UserId == userId),
                })
                .ToListAsync();

            return new PropertySearchResultDto
            {
                Items = items,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<Property?> GetByIdAsync(long id)
        {
            return await Context.Properties.Include(p => p.Contracts).FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<PropertyDetailsDto?> GetPropertyDetailsAsync(long propertyId, Guid? currentUserId)
        {
            var hasCurrentUser = currentUserId.HasValue;
            var userId = currentUserId ?? Guid.Empty;

            return Context.Properties
                .AsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => new PropertyDetailsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Type = p.Type,
                    MaxOccupants = p.MaxOccupants,
                    IsShared = p.IsShared,
                    Bedrooms = p.Bedrooms,
                    Beds = p.Beds,
                    Bathrooms = p.Bathrooms,
                    SquareMeters = p.SquareMeters,
                    ViewsCount = p.Views,
                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                    Address = p.Address,
                    City = p.City,
                    State = p.State,
                    ZipCode = p.ZipCode,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    IsActive = p.IsActive,
                    Availability = p.IsShared 
                        ? p.Contracts.Count(c => c.Status == ContractStatus.Active) < p.MaxOccupants
                        : !p.Contracts.Any(c => c.Status == ContractStatus.Active),
                    CreatedAt = p.CreatedAt,
                    IsSaved = hasCurrentUser && p.SavedProperty.Any(s => s.UserId == userId),
                    AverageRating = p.PropertyRatings.Any() ? p.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f : 0f,
                    RatingsCount = p.PropertyRatings.Count,
                    CommentsCount = p.PropertyComments.Count,
                    CurrentUserRating = hasCurrentUser
                        ? p.PropertyRatings
                            .Where(r => r.UserId == userId)
                            .Select(r => (int?)r.Rating)
                            .FirstOrDefault()
                        : null,

                    Amenities = p.Amenities
                        .Select(a => new PropertyAmenityItemDto
                        {
                            Id = a.Id,
                            Amenity = a.Amenity
                        })
                        .ToList(),
                    Rules = p.Rules
                        .Select(r => new PropertyRuleItemDto
                        {
                            Id = r.Id,
                            Text = r.Rule
                        })
                        .ToList(),
                    Media = p.Media
                        .OrderByDescending(m => m.IsPrimary)
                        .ThenBy(m => m.Id)
                        .Select(m => new PropertyMediaItemDto
                        {
                            Id = m.Id,
                            Path = m.Path,
                            IsPrimary = m.IsPrimary
                        })
                        .ToList(),
                    CurrentUserBookingRequests = hasCurrentUser
                        ? p.BookingRequests
                            .Where(b => b.RenterId == userId)
                            .OrderByDescending(b => b.CreatedAt)
                            .Select(b => new PropertyBookingRequestDto
                            {
                                BookingRequestId = b.Id,
                                StartDate = b.StartDate,
                                EndDate = b.EndDate,
                                PaymentFrequency = b.PaymentFrequency
                            })
                            .ToList()
                        : new List<PropertyBookingRequestDto>(),
                    ActiveRenters = p.Contracts
                        .Where(c => c.Status == ContractStatus.Active)
                        .Select(c => new ActiveRenterDto
                        {
                            Id = c.RenterId,
                            Name = $"{c.Renter.FirstName} {c.Renter.LastName}",
                            ProfilePhoto = c.Renter.ProfileImage,
                            MatchingPercentage = p.IsShared ? 85.0 : null // Dummy value for now
                        })
                        .ToList(),
                    Comments = p.PropertyComments
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => new PropertyCommentDetailsDto
                        {
                            CommentId = c.Id,
                            CommenterId = c.UserId,
                            CommenterFullName = $"{c.User.FirstName} {c.User.LastName}",
                            CommenterProfileImage = string.IsNullOrEmpty(c.User.ProfileImage) ? null : c.User.ProfileImage,
                            CreatedAt = c.CreatedAt,
                            Rating = p.PropertyRatings
                                .Where(pr => pr.UserId == c.UserId)
                                .Select(pr => (int?)pr.Rating)
                                .FirstOrDefault(),
                            Content = c.Content,
                            StayInfo = new PropertyCommentStayInfoDto
                            {
                                CheckIn = p.Contracts
                                    .Where(contract => contract.RenterId == c.UserId)
                                    .OrderByDescending(contract => contract.LeaseEndDate)
                                    .Select(contract => contract.LeaseStartDate)
                                    .FirstOrDefault(),
                                CheckOut = p.Contracts
                                    .Where(contract => contract.RenterId == c.UserId)
                                    .OrderByDescending(contract => contract.LeaseEndDate)
                                    .Select(contract => contract.LeaseEndDate)
                                    .FirstOrDefault(),
                                IsContractActive = p.Contracts
                                    .Where(contract => contract.RenterId == c.UserId)
                                    .OrderByDescending(contract => contract.LeaseEndDate)
                                    .Select(contract => contract.Status == ContractStatus.Active)
                                    .FirstOrDefault()
                            }
                        })
                        .ToList(),
                    HostedBy = new PropertyHostedByDto
                    {
                        Id = p.OwnerId,
                        FullName = $"{p.Owner.FirstName} {p.Owner.LastName}",
                        ProfileImage = string.IsNullOrEmpty(p.Owner.ProfileImage) ? null : p.Owner.ProfileImage,
                        AverageRating = Context.Properties
                            .Where(op => op.OwnerId == p.OwnerId)
                            .SelectMany(op => op.PropertyRatings)
                            .Average(r => (float?)r.Rating) ?? 0f,
                        PropertiesCount = Context.Properties.Count(op => op.OwnerId == p.OwnerId),
                        Bio = p.Owner.Bio
                    },
                    OwnerExtras = new OwnerPropertyExtrasDto()
                })
                .FirstOrDefaultAsync();
        }

        public async Task IncrementViewsAsync(long propertyId)
        {
            await Context.Properties
                .Where(p => p.Id == propertyId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Views, p => p.Views + 1));
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            Context.Properties.Update(property);
            await Context.SaveChangesAsync();
        }

        #region Deletion
        public async Task<List<long>> GetPropertyIdsByOwnerAsync(Guid ownerId)
        {
            return await Context.Properties
                .IgnoreQueryFilters()
                .Where(p => p.OwnerId == ownerId)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<List<string>> GetMediaPathsByPropertyIdsAsync(List<long> propertyIds)
        {
            return await Context.PropertyMedia
                .Where(m => propertyIds.Contains(m.PropertyId))
                .Select(m => m.Path)
                .ToListAsync();
        }

        public async Task DeleteMediaByPropertyIdsAsync(List<long> propertyIds)
        {
            await Context.PropertyMedia
                .Where(m => propertyIds.Contains(m.PropertyId))
                .ExecuteDeleteAsync();
        }
        #endregion

        #endregion
    }
}
