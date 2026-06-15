using System.Globalization;
using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Enums.Contract;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MARN_API.Models;
using MARN_API.Enums.Report;

namespace MARN_API.Repositories.Implementations
{
    public class AdminDetailedStatsRepo : IAdminDetailedStatsRepo
    {
        private readonly AppDbContext _context;

        public AdminDetailedStatsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDetailedUsersResponseDto> GetUsersAsync(AdminDetailedUsersQueryDto query, DateTime? fromUtc, DateTime? toUtc, bool groupByDay)
        {
            var summaryQuery = ApplyUserPeriodFilter(NonAdminUsers(), fromUtc, toUtc);
            if (!query.IncludeDeleted)
            {
                summaryQuery = summaryQuery.Where(u => u.DeletedAt == null);
            }

            var totalUsers = await summaryQuery.LongCountAsync();
            var deletedUsers = await summaryQuery.LongCountAsync(u => u.DeletedAt != null);

            var statusBreakdown = await summaryQuery
                .GroupBy(u => u.AccountStatus)
                .Select(g => new AdminAccountStatusCountDto
                {
                    Status = g.Key,
                    Count = g.LongCount()
                })
                .OrderBy(x => x.Status)
                .ToListAsync();

            var summaryUserIds = await summaryQuery.Select(u => u.Id).ToListAsync();
            var roleBreakdown = await GetRoleBreakdownAsync(summaryUserIds);
            var newUsersOverTime = await GetUserTimePointsAsync(summaryQuery, groupByDay);

            var listQuery = summaryQuery;
            if (query.AccountStatus.HasValue)
            {
                listQuery = listQuery.Where(u => u.AccountStatus == query.AccountStatus.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                var normalizedRole = query.Role.Trim().ToUpperInvariant();
                var roleIds = _context.Roles
                    .Where(r => r.NormalizedName == normalizedRole)
                    .Select(r => r.Id);

                listQuery = listQuery.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && roleIds.Contains(ur.RoleId)));
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                listQuery = listQuery.Where(u =>
                    (u.FirstName + " " + u.LastName).ToLower().Contains(search) ||
                    (u.Email != null && u.Email.ToLower().Contains(search)));
            }

            var totalListCount = await listQuery.CountAsync();
            var users = await listQuery
                .OrderByDescending(u => u.CreatedAt)
                .ThenBy(u => u.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new AdminDetailedUserListItemDto
                {
                    UserId = u.Id,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    ProfileImage = u.ProfileImage,
                    AccountStatus = u.AccountStatus,
                    IsDeleted = u.DeletedAt != null,
                    CreatedAt = u.CreatedAt,
                    OwnedPropertiesCount = _context.Properties.IgnoreQueryFilters().LongCount(p => p.OwnerId == u.Id),
                    ActivePropertiesCount = _context.Properties.IgnoreQueryFilters().LongCount(p => p.OwnerId == u.Id && p.IsActive && p.DeletedAt == null),
                    RenterContractsCount = _context.Contracts.LongCount(c => c.RenterId == u.Id),
                    OwnerContractsCount = _context.Contracts.LongCount(c => c.Property.OwnerId == u.Id),
                    ActiveContractsCount = _context.Contracts.LongCount(c =>
                        c.Status == ContractStatus.Active &&
                        (c.RenterId == u.Id || c.Property.OwnerId == u.Id)),
                    CancelledContractsCount = _context.Contracts.LongCount(c =>
                        c.Status == ContractStatus.Cancelled &&
                        (c.RenterId == u.Id || c.Property.OwnerId == u.Id)),
                    PaymentsMadeCount = _context.Payments.LongCount(p => p.PaymentSchedule.Contract.RenterId == u.Id),
                    PaymentsReceivedCount = _context.Payments.LongCount(p => p.PaymentSchedule.Contract.Property.OwnerId == u.Id),
                    TotalPaidAmount = _context.Payments
                        .Where(p => p.PaymentSchedule.Contract.RenterId == u.Id)
                        .Sum(p => (decimal?)p.AmountTotal) ?? 0m,
                    TotalReceivedAmount = _context.Payments
                        .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == u.Id)
                        .Sum(p => (decimal?)p.OwnerAmount) ?? 0m,
                    ReportsSubmittedCount = _context.Reports.LongCount(r => r.ReporterId == u.Id),
                    ReportsAgainstUserCount = _context.Reports.LongCount(r =>
                        r.ReportableType == ReportableType.User &&
                        r.ReportableGuidId == u.Id)
                })
                .ToListAsync();

            var roleLookup = await GetRolesLookupAsync(users.Select(u => u.UserId).ToList());
            foreach (var user in users)
            {
                user.Roles = roleLookup.TryGetValue(user.UserId, out var roles)
                    ? roles
                    : [];
            }

            return new AdminDetailedUsersResponseDto
            {
                TotalUsers = totalUsers,
                DeletedUsers = deletedUsers,
                StatusBreakdown = statusBreakdown,
                RoleBreakdown = roleBreakdown,
                NewUsersOverTime = newUsersOverTime,
                Users = CreatePagedResult(users, query.PageNumber, query.PageSize, totalListCount)
            };
        }

        public async Task<AdminDetailedPropertiesResponseDto> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query, DateTime? fromUtc, DateTime? toUtc)
        {
            var summaryQuery = ApplyPropertyPeriodFilter(AllProperties(), fromUtc, toUtc);
            if (!query.IncludeDeleted)
            {
                summaryQuery = summaryQuery.Where(p => p.DeletedAt == null);
            }

            var totalProperties = await summaryQuery.LongCountAsync();
            var deletedProperties = await summaryQuery.LongCountAsync(p => p.DeletedAt != null);
            var activeProperties = await summaryQuery.LongCountAsync(p => p.IsActive && p.DeletedAt == null);
            var inactiveProperties = await summaryQuery.LongCountAsync(p => !p.IsActive && p.DeletedAt == null);

            var statusBreakdown = await summaryQuery
                .GroupBy(p => p.Status)
                .Select(g => new AdminPropertyStatusCountDto
                {
                    Status = g.Key,
                    Count = g.LongCount()
                })
                .OrderBy(x => x.Status)
                .ToListAsync();

            var typeBreakdown = await summaryQuery
                .GroupBy(p => p.Type)
                .Select(g => new AdminPropertyTypeCountDto
                {
                    Type = g.Key,
                    Count = g.LongCount()
                })
                .OrderBy(x => x.Type)
                .ToListAsync();

            var governorateBreakdown = await summaryQuery
                .GroupBy(p => p.State)
                .Select(g => new AdminGovernorateCountDto
                {
                    Governorate = g.Key,
                    Count = g.LongCount()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Governorate)
                .ToListAsync();

            var listQuery = summaryQuery;
            if (query.Status.HasValue)
            {
                listQuery = listQuery.Where(p => p.Status == query.Status.Value);
            }

            if (query.Type.HasValue)
            {
                listQuery = listQuery.Where(p => p.Type == query.Type.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Governorate))
            {
                var governorate = query.Governorate.Trim().ToLower();
                listQuery = listQuery.Where(p => p.State.ToLower() == governorate);
            }

            if (query.IsActive.HasValue)
            {
                listQuery = listQuery.Where(p => p.IsActive == query.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                listQuery = listQuery.Where(p =>
                    p.Title.ToLower().Contains(search) ||
                    p.Address.ToLower().Contains(search) ||
                    (p.Owner.FirstName + " " + p.Owner.LastName).ToLower().Contains(search));
            }

            var totalListCount = await listQuery.CountAsync();
            var properties = await listQuery
                .OrderByDescending(p => p.CreatedAt)
                .ThenBy(p => p.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new AdminDetailedPropertyListItemDto
                {
                    PropertyId = p.Id,
                    Title = p.Title,
                    OwnerId = p.OwnerId,
                    OwnerName = (p.Owner.FirstName + " " + p.Owner.LastName).Trim(),
                    Status = p.Status,
                    Type = p.Type,
                    City = p.City,
                    Governorate = p.State,
                    Price = p.Price,
                    AverageRating = p.PropertyFeedbacks.Any()
                        ? (float)Math.Round((double)(p.PropertyFeedbacks.Average(f => (float?)f.Rating) ?? 0f), 1)
                        : 0f,
                    CommentsCount = p.PropertyFeedbacks.Count(f => !f.IsHiddenByModeration && f.Content != null && f.Content != ""),
                    IsActive = p.IsActive,
                    CanDeactivate = p.IsActive && p.DeletedAt == null,
                    CanRestore = !p.IsActive && p.DeletedAt == null,
                    IsDeleted = p.DeletedAt != null,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return new AdminDetailedPropertiesResponseDto
            {
                TotalProperties = totalProperties,
                DeletedProperties = deletedProperties,
                ActiveProperties = activeProperties,
                InactiveProperties = inactiveProperties,
                StatusBreakdown = statusBreakdown,
                TypeBreakdown = typeBreakdown,
                GovernorateBreakdown = governorateBreakdown,
                Properties = CreatePagedResult(properties, query.PageNumber, query.PageSize, totalListCount)
            };
        }

        public Task<AdminPropertyDetailsDto?> GetPropertyDetailsAsync(long propertyId)
        {
            return _context.Properties
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => new AdminPropertyDetailsDto
                {
                    PropertyId = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    OwnerName = (p.Owner.FirstName + " " + p.Owner.LastName).Trim(),
                    OwnerEmail = p.Owner.Email,
                    OwnerPhoneNumber = p.Owner.PhoneNumber,
                    OwnerProfileImage = p.Owner.ProfileImage,
                    Status = p.Status,
                    Type = p.Type,
                    RentalUnit = p.RentalUnit,
                    Address = p.Address,
                    City = p.City,
                    Governorate = p.State,
                    ZipCode = p.ZipCode,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    MaxOccupants = p.MaxOccupants,
                    IsShared = p.IsShared,
                    Bedrooms = p.Bedrooms,
                    Beds = p.Beds,
                    Bathrooms = p.Bathrooms,
                    SquareMeters = p.SquareMeters,
                    ViewsCount = p.Views,
                    Price = p.Price,
                    IsActive = p.IsActive,
                    IsDeleted = p.DeletedAt != null,
                    CreatedAt = p.CreatedAt,
                    ProofOfOwnership = p.ProofOfOwnership,
                    AverageRating = p.PropertyFeedbacks.Any()
                        ? (float)Math.Round((double)(p.PropertyFeedbacks.Average(f => (float?)f.Rating) ?? 0f), 1)
                        : 0f,
                    RatingsCount = p.PropertyFeedbacks.Count,
                    CommentsCount = p.PropertyFeedbacks.Count(f => !f.IsHiddenByModeration && f.Content != null && f.Content != ""),
                    SavedByUsersCount = p.SavedProperty.Count,
                    BookingRequestsCount = p.BookingRequests.Count,
                    Amenities = p.Amenities
                        .OrderBy(a => a.Id)
                        .Select(a => new AdminPropertyAmenityDto
                        {
                            AmenityId = a.Id,
                            Amenity = a.Amenity
                        })
                        .ToList(),
                    Rules = p.Rules
                        .OrderBy(r => r.Id)
                        .Select(r => new AdminPropertyRuleDto
                        {
                            RuleId = r.Id,
                            Text = r.Rule
                        })
                        .ToList(),
                    Media = p.Media
                        .OrderByDescending(m => m.IsPrimary)
                        .ThenBy(m => m.Id)
                        .Select(m => new AdminPropertyMediaDto
                        {
                            MediaId = m.Id,
                            Path = m.Path,
                            IsPrimary = m.IsPrimary
                        })
                        .ToList(),
                    Comments = p.PropertyFeedbacks
                        .Where(f => f.Content != null && f.Content != "")
                        .OrderByDescending(f => f.CreatedAt)
                        .Select(f => new AdminPropertyCommentDto
                        {
                            CommentId = f.Id,
                            UserId = f.UserId,
                            UserName = (f.User.FirstName + " " + f.User.LastName).Trim(),
                            UserProfileImage = f.User.ProfileImage,
                            Content = f.Content!,
                            Rating = f.Rating,
                            CreatedAt = f.CreatedAt,
                            UpdatedAt = f.UpdatedAt,
                            IsHiddenByModeration = f.IsHiddenByModeration,
                            HiddenAt = f.HiddenAt,
                            HiddenByAdminId = f.HiddenByAdminId,
                            HiddenReason = f.HiddenReason
                        })
                        .ToList(),
                    Ratings = p.PropertyFeedbacks
                        .OrderByDescending(f => f.UpdatedAt ?? f.CreatedAt)
                        .Select(f => new AdminPropertyRatingDto
                        {
                            RatingId = f.Id,
                            UserId = f.UserId,
                            UserName = (f.User.FirstName + " " + f.User.LastName).Trim(),
                            UserProfileImage = f.User.ProfileImage,
                            Rating = f.Rating,
                            CreatedAt = f.CreatedAt,
                            UpdatedAt = f.UpdatedAt
                        })
                        .ToList(),
                    Contracts = p.Contracts
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => new AdminPropertyContractDto
                        {
                            ContractId = c.Id,
                            Status = c.Status,
                            AnchoringStatus = c.AnchoringStatus,
                            RenterId = c.RenterId,
                            RenterName = (c.Renter.FirstName + " " + c.Renter.LastName).Trim(),
                            RenterProfileImage = c.Renter.ProfileImage,
                            CreatedAt = c.CreatedAt,
                            LeaseStartDate = c.LeaseStartDate,
                            LeaseEndDate = c.LeaseEndDate,
                            PaymentFrequency = c.PaymentFrequency,
                            TotalContractAmount = c.TotalContractAmount,
                            SignedByRenterAt = c.SignedByRenterAt
                        })
                        .ToList(),
                    BookingRequests = p.BookingRequests
                        .OrderByDescending(b => b.CreatedAt)
                        .Select(b => new AdminPropertyBookingRequestDto
                        {
                            BookingRequestId = b.Id,
                            RenterId = b.RenterId,
                            RenterName = (b.Renter.FirstName + " " + b.Renter.LastName).Trim(),
                            RenterProfileImage = b.Renter.ProfileImage,
                            StartDate = b.StartDate,
                            EndDate = b.EndDate,
                            PaymentFrequency = b.PaymentFrequency,
                            CreatedAt = b.CreatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AdminDetailedContractsResponseDto> GetContractsAsync(AdminDetailedContractsQueryDto query, DateTime? fromUtc, DateTime? toUtc)
        {
            var summaryQuery = ApplyContractPeriodFilter(_context.Contracts.AsNoTracking(), fromUtc, toUtc);

            var totalContracts = await summaryQuery.LongCountAsync();
            var totalContractValue = await summaryQuery.SumAsync(c => (decimal?)c.TotalContractAmount) ?? 0m;

            var statusBreakdown = await summaryQuery
                .GroupBy(c => c.Status)
                .Select(g => new AdminContractStatusCountDto
                {
                    Status = g.Key,
                    Count = g.LongCount()
                })
                .OrderBy(x => x.Status)
                .ToListAsync();

            var listQuery = summaryQuery;
            if (query.Status.HasValue)
            {
                listQuery = listQuery.Where(c => c.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                var parsedContractId = long.TryParse(query.Search.Trim(), out var contractIdValue)
                    ? contractIdValue
                    : (long?)null;

                listQuery = listQuery.Where(c =>
                    (parsedContractId.HasValue && c.Id == parsedContractId.Value) ||
                    c.Property.Title.ToLower().Contains(search) ||
                    (c.Property.Owner.FirstName + " " + c.Property.Owner.LastName).ToLower().Contains(search) ||
                    (c.Renter.FirstName + " " + c.Renter.LastName).ToLower().Contains(search));
            }

            var totalListCount = await listQuery.CountAsync();
            var contractRows = await listQuery
                .OrderByDescending(c => c.CreatedAt)
                .ThenByDescending(c => c.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Status,
                    c.TransactionId,
                    c.MerkleRoot,
                    c.AnchoringStatus,
                    c.CreatedAt,
                    c.LeaseStartDate,
                    c.LeaseEndDate,
                    c.TotalContractAmount,
                    c.PaymentFrequency,
                    c.PropertyId,
                    PropertyTitle = c.Property.Title,
                    OwnerId = c.Property.OwnerId,
                    OwnerName = (c.Property.Owner.FirstName + " " + c.Property.Owner.LastName).Trim(),
                    c.RenterId,
                    RenterName = (c.Renter.FirstName + " " + c.Renter.LastName).Trim()
                })
                .ToListAsync();

            var contracts = contractRows
                .Select(c => new AdminDetailedContractListItemDto
                {
                    ContractId = c.Id,
                    Status = c.Status,
                    TransactionId = c.TransactionId,
                    MerkleRoot = c.MerkleRoot,
                    AnchoringStatus = c.AnchoringStatus,
                    IsAnchoredToBlockChain = c.AnchoringStatus == ContractAnchoringStatus.Anchored,
                    CanCancel = c.Status == ContractStatus.Pending || c.Status == ContractStatus.Active,
                    CreatedAt = c.CreatedAt,
                    LeaseStartDate = c.LeaseStartDate,
                    LeaseEndDate = c.LeaseEndDate,
                    TotalContractAmount = c.TotalContractAmount,
                    PaymentFrequency = c.PaymentFrequency.ToString(),
                    PropertyId = c.PropertyId,
                    PropertyTitle = c.PropertyTitle,
                    OwnerId = c.OwnerId,
                    OwnerName = c.OwnerName,
                    RenterId = c.RenterId,
                    RenterName = c.RenterName
                })
                .ToList();

            return new AdminDetailedContractsResponseDto
            {
                TotalContracts = totalContracts,
                TotalContractValue = totalContractValue,
                StatusBreakdown = statusBreakdown,
                Contracts = CreatePagedResult(contracts, query.PageNumber, query.PageSize, totalListCount)
            };
        }

        public async Task<AdminDetailedRevenueResponseDto> GetRevenueAsync(AdminDetailedRevenueQueryDto query, DateTime? fromUtc, DateTime? toUtc, bool groupByDay)
        {
            var summaryQuery = ApplyPaymentPeriodFilter(_context.Payments.AsNoTracking(), fromUtc, toUtc);

            var totalPayments = await summaryQuery.LongCountAsync();
            var totalSales = await summaryQuery.SumAsync(p => (decimal?)p.AmountTotal) ?? 0m;
            var totalRevenue = await summaryQuery.SumAsync(p => (decimal?)p.PlatformFee) ?? 0m;
            var totalOwnerPayouts = await summaryQuery.SumAsync(p => (decimal?)p.OwnerAmount) ?? 0m;

            var statusBreakdown = await summaryQuery
                .GroupBy(p => p.Status)
                .Select(g => new AdminPaymentStatusSummaryDto
                {
                    Status = g.Key,
                    Count = g.LongCount(),
                    Sales = g.Sum(p => p.AmountTotal),
                    Revenue = g.Sum(p => p.PlatformFee),
                    OwnerPayouts = g.Sum(p => p.OwnerAmount)
                })
                .OrderBy(x => x.Status)
                .ToListAsync();

            var revenueOverTime = await GetRevenueTimePointsAsync(summaryQuery, groupByDay);

            var listQuery = summaryQuery;
            if (query.Status.HasValue)
            {
                listQuery = listQuery.Where(p => p.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                var parsedPaymentId = long.TryParse(query.Search.Trim(), out var paymentIdValue)
                    ? paymentIdValue
                    : (long?)null;
                var parsedContractId = long.TryParse(query.Search.Trim(), out var contractIdValue)
                    ? contractIdValue
                    : (long?)null;

                listQuery = listQuery.Where(p =>
                    (parsedPaymentId.HasValue && p.Id == parsedPaymentId.Value) ||
                    (parsedContractId.HasValue && p.PaymentSchedule.ContractId == parsedContractId.Value) ||
                    p.PaymentSchedule.Contract.Property.Title.ToLower().Contains(search) ||
                    (p.PaymentSchedule.Contract.Property.Owner.FirstName + " " + p.PaymentSchedule.Contract.Property.Owner.LastName).ToLower().Contains(search) ||
                    (p.PaymentSchedule.Contract.Renter.FirstName + " " + p.PaymentSchedule.Contract.Renter.LastName).ToLower().Contains(search));
            }

            var totalListCount = await listQuery.CountAsync();
            var payments = await listQuery
                .OrderByDescending(p => p.PaidAt)
                .ThenByDescending(p => p.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new AdminDetailedPaymentListItemDto
                {
                    PaymentId = p.Id,
                    ContractId = p.PaymentSchedule.ContractId,
                    PaymentScheduleId = p.PaymentScheduleId,
                    Status = p.Status,
                    AmountTotal = p.AmountTotal,
                    PlatformFee = p.PlatformFee,
                    OwnerAmount = p.OwnerAmount,
                    PaidAt = p.PaidAt,
                    AvailableAt = p.AvailableAt,
                    Currency = p.Currency,
                    PropertyId = p.PaymentSchedule.Contract.PropertyId,
                    PropertyTitle = p.PaymentSchedule.Contract.Property.Title,
                    OwnerId = p.PaymentSchedule.Contract.Property.OwnerId,
                    OwnerName = (p.PaymentSchedule.Contract.Property.Owner.FirstName + " " + p.PaymentSchedule.Contract.Property.Owner.LastName).Trim(),
                    RenterId = p.PaymentSchedule.Contract.RenterId,
                    RenterName = (p.PaymentSchedule.Contract.Renter.FirstName + " " + p.PaymentSchedule.Contract.Renter.LastName).Trim()
                })
                .ToListAsync();

            return new AdminDetailedRevenueResponseDto
            {
                TotalPayments = totalPayments,
                TotalSales = totalSales,
                TotalRevenue = totalRevenue,
                TotalOwnerPayouts = totalOwnerPayouts,
                StatusBreakdown = statusBreakdown,
                RevenueOverTime = revenueOverTime,
                Payments = CreatePagedResult(payments, query.PageNumber, query.PageSize, totalListCount)
            };
        }

        public Task<Property?> GetPropertyForAdminActionAsync(long propertyId)
        {
            return _context.Properties
                .IgnoreQueryFilters()
                .Include(p => p.Owner)
                .Include(p => p.PropertyFeedbacks)
                .FirstOrDefaultAsync(p => p.Id == propertyId);
        }

        public Task<Contract?> GetContractForAdminActionAsync(long contractId)
        {
            return _context.Contracts
                .Include(c => c.Property)
                    .ThenInclude(p => p.Owner)
                .Include(c => c.Renter)
                .Include(c => c.PaymentSchedules)
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public Task SaveAdminContractChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        private IQueryable<Models.ApplicationUser> NonAdminUsers()
        {
            var adminRoleIds = _context.Roles
                .Where(r => r.NormalizedName == "ADMIN")
                .Select(r => r.Id);

            return _context.Users
                .IgnoreQueryFilters()
                .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id && adminRoleIds.Contains(ur.RoleId)));
        }

        private IQueryable<Models.Property> AllProperties()
        {
            return _context.Properties
                .IgnoreQueryFilters()
                .AsNoTracking();
        }

        private static IQueryable<Models.ApplicationUser> ApplyUserPeriodFilter(IQueryable<Models.ApplicationUser> query, DateTime? fromUtc, DateTime? toUtc)
        {
            if (fromUtc.HasValue)
            {
                query = query.Where(u => u.CreatedAt >= fromUtc.Value);
            }

            if (toUtc.HasValue)
            {
                query = query.Where(u => u.CreatedAt < toUtc.Value);
            }

            return query;
        }

        private static IQueryable<Models.Property> ApplyPropertyPeriodFilter(IQueryable<Models.Property> query, DateTime? fromUtc, DateTime? toUtc)
        {
            if (fromUtc.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= fromUtc.Value);
            }

            if (toUtc.HasValue)
            {
                query = query.Where(p => p.CreatedAt < toUtc.Value);
            }

            return query;
        }

        private static IQueryable<Models.Contract> ApplyContractPeriodFilter(IQueryable<Models.Contract> query, DateTime? fromUtc, DateTime? toUtc)
        {
            if (fromUtc.HasValue)
            {
                query = query.Where(c => c.CreatedAt >= fromUtc.Value);
            }

            if (toUtc.HasValue)
            {
                query = query.Where(c => c.CreatedAt < toUtc.Value);
            }

            return query;
        }

        private static IQueryable<Models.Payment> ApplyPaymentPeriodFilter(IQueryable<Models.Payment> query, DateTime? fromUtc, DateTime? toUtc)
        {
            if (fromUtc.HasValue)
            {
                query = query.Where(p => p.PaidAt >= fromUtc.Value);
            }

            if (toUtc.HasValue)
            {
                query = query.Where(p => p.PaidAt < toUtc.Value);
            }

            return query;
        }

        private async Task<List<AdminRoleCountDto>> GetRoleBreakdownAsync(List<Guid> userIds)
        {
            if (userIds.Count == 0)
                return [];

            return await _context.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (_, r) => r.Name!)
                .GroupBy(roleName => roleName)
                .Select(g => new AdminRoleCountDto
                {
                    RoleName = g.Key,
                    Count = g.LongCount()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.RoleName)
                .ToListAsync();
        }

        private async Task<Dictionary<Guid, List<string>>> GetRolesLookupAsync(List<Guid> userIds)
        {
            if (userIds.Count == 0)
                return [];

            var roles = await _context.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { ur.UserId, RoleName = r.Name! })
                .ToListAsync();

            return roles
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.RoleName).OrderBy(name => name).ToList());
        }

        private async Task<List<AdminCountTimePointDto>> GetUserTimePointsAsync(IQueryable<Models.ApplicationUser> summaryQuery, bool groupByDay)
        {
            if (groupByDay)
            {
                var groupedRows = await summaryQuery
                    .GroupBy(u => u.CreatedAt.Date)
                    .Select(g => new
                    {
                        PeriodStartUtc = g.Key,
                        Count = g.LongCount()
                    })
                    .ToListAsync();

                return groupedRows
                    .Select(x => new AdminCountTimePointDto
                    {
                        PeriodStartUtc = DateTime.SpecifyKind(x.PeriodStartUtc, DateTimeKind.Utc),
                        Label = x.PeriodStartUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Count = x.Count
                    })
                    .OrderBy(x => x.PeriodStartUtc)
                    .ToList();
            }

            var monthlyRows = await summaryQuery
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.LongCount()
                })
                .ToListAsync();

            return monthlyRows
                .Select(x =>
                {
                    var periodStartUtc = new DateTime(x.Year, x.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    return new AdminCountTimePointDto
                    {
                        PeriodStartUtc = periodStartUtc,
                        Label = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)} {x.Year}",
                        Count = x.Count
                    };
                })
                .OrderBy(x => x.PeriodStartUtc)
                .ToList();
        }

        private async Task<List<AdminRevenueTimePointDto>> GetRevenueTimePointsAsync(IQueryable<Models.Payment> summaryQuery, bool groupByDay)
        {
            if (groupByDay)
            {
                var groupedRows = await summaryQuery
                    .GroupBy(p => p.PaidAt.Date)
                    .Select(g => new
                    {
                        PeriodStartUtc = g.Key,
                        Revenue = g.Sum(p => p.PlatformFee),
                        Sales = g.Sum(p => p.AmountTotal),
                        OwnerPayouts = g.Sum(p => p.OwnerAmount),
                        PaymentsCount = g.LongCount()
                    })
                    .ToListAsync();

                return groupedRows
                    .Select(x => new AdminRevenueTimePointDto
                    {
                        PeriodStartUtc = DateTime.SpecifyKind(x.PeriodStartUtc, DateTimeKind.Utc),
                        Label = x.PeriodStartUtc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Revenue = x.Revenue,
                        Sales = x.Sales,
                        OwnerPayouts = x.OwnerPayouts,
                        PaymentsCount = x.PaymentsCount
                    })
                    .OrderBy(x => x.PeriodStartUtc)
                    .ToList();
            }

            var monthlyRows = await summaryQuery
                .GroupBy(p => new { p.PaidAt.Year, p.PaidAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Revenue = g.Sum(p => p.PlatformFee),
                    Sales = g.Sum(p => p.AmountTotal),
                    OwnerPayouts = g.Sum(p => p.OwnerAmount),
                    PaymentsCount = g.LongCount()
                })
                .ToListAsync();

            return monthlyRows
                .Select(x =>
                {
                    var periodStartUtc = new DateTime(x.Year, x.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    return new AdminRevenueTimePointDto
                    {
                        PeriodStartUtc = periodStartUtc,
                        Label = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)} {x.Year}",
                        Revenue = x.Revenue,
                        Sales = x.Sales,
                        OwnerPayouts = x.OwnerPayouts,
                        PaymentsCount = x.PaymentsCount
                    };
                })
                .OrderBy(x => x.PeriodStartUtc)
                .ToList();
        }

        private static PagedResult<T> CreatePagedResult<T>(List<T> items, int pageNumber, int pageSize, int totalCount)
        {
            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}
