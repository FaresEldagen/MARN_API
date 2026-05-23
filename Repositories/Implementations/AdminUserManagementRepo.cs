using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums.Account;
using MARN_API.Enums.Contract;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AdminUserManagementRepo : IAdminUserManagementRepo
    {
        private readonly AppDbContext _context;

        public AdminUserManagementRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AdminUserListItemDto>> GetUsersAsync(AdminUserManagementQueryDto query)
        {
            var usersQuery = BuildUsersQuery(query);

            var totalCount = await usersQuery.CountAsync();
            var users = await usersQuery
                .OrderByDescending(u => u.CreatedAt)
                .ThenBy(u => u.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new AdminUserListItemDto
                {
                    UserId = u.Id,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImage = u.ProfileImage,
                    AccountStatus = u.AccountStatus,
                    IsDeleted = u.DeletedAt != null,
                    CreatedAt = u.CreatedAt,
                    OwnedPropertiesCount = _context.Properties.IgnoreQueryFilters().Count(p => p.OwnerId == u.Id),
                    ActiveContractsCount = _context.Contracts.Count(c =>
                        c.Status == ContractStatus.Active &&
                        (c.RenterId == u.Id || c.Property.OwnerId == u.Id))
                })
                .ToListAsync();

            var roleLookup = await GetRolesLookupAsync(users.Select(u => u.UserId).ToList());
            foreach (var user in users)
            {
                user.Roles = roleLookup.TryGetValue(user.UserId, out var roles)
                    ? roles
                    : [];
            }

            return CreatePagedResult(users, query.PageNumber, query.PageSize, totalCount);
        }

        public async Task<AdminUserDetailsDto?> GetUserDetailsAsync(Guid userId)
        {
            var user = await NonAdminUsers()
                .Where(u => u.Id == userId)
                .Select(u => new AdminUserDetailsDto
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    ProfileImage = u.ProfileImage,
                    Bio = u.Bio,
                    CreatedAt = u.CreatedAt,
                    DeletedAt = u.DeletedAt,
                    IsDeleted = u.DeletedAt != null,
                    EmailConfirmed = u.EmailConfirmed,
                    TwoFactorEnabled = u.TwoFactorEnabled,
                    DateOfBirth = u.DateOfBirth,
                    Gender = u.Gender,
                    Country = u.Country,
                    Language = u.Language,
                    AccountStatus = u.AccountStatus,
                    FrontIdPhoto = u.FrontIdPhoto,
                    BackIdPhoto = u.BackIdPhoto,
                    ArabicAddress = u.ArabicAddress,
                    ArabicFullName = u.ArabicFullName,
                    NationalIDNumber = u.NationalIDNumber,
                    Summary = new AdminUserActivitySummaryDto
                    {
                        OwnedPropertiesCount = _context.Properties.IgnoreQueryFilters().Count(p => p.OwnerId == u.Id),
                        ActiveOwnedPropertiesCount = _context.Properties.IgnoreQueryFilters().Count(p => p.OwnerId == u.Id && p.IsActive && p.DeletedAt == null),
                        RenterContractsCount = _context.Contracts.Count(c => c.RenterId == u.Id),
                        OwnerContractsCount = _context.Contracts.Count(c => c.Property.OwnerId == u.Id),
                        ActiveContractsAsRenterCount = _context.Contracts.Count(c => c.RenterId == u.Id && c.Status == ContractStatus.Active),
                        ActiveContractsAsOwnerCount = _context.Contracts.Count(c => c.Property.OwnerId == u.Id && c.Status == ContractStatus.Active),
                        PendingBookingRequestsAsRenterCount = _context.BookingRequests.Count(b => b.RenterId == u.Id),
                        PendingBookingRequestsAsOwnerCount = _context.BookingRequests.Count(b => b.Property.OwnerId == u.Id),
                        PaidPaymentsCount = _context.Payments.Count(p => p.PaymentSchedule.Contract.RenterId == u.Id),
                        ReceivedPaymentsCount = _context.Payments.Count(p => p.PaymentSchedule.Contract.Property.OwnerId == u.Id)
                    }
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            user.Roles = await GetUserRolesAsync(userId);
            user.OwnedProperties = await _context.Properties
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new AdminManagedPropertyDto
                {
                    PropertyId = p.Id,
                    Title = p.Title,
                    Status = p.Status,
                    IsActive = p.IsActive,
                    IsDeleted = p.DeletedAt != null,
                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                    Address = p.Address,
                    PrimaryImage = p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault(),
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            user.RenterContracts = await _context.Contracts
                .AsNoTracking()
                .Where(c => c.RenterId == userId)
                .OrderByDescending(c => c.LeaseEndDate)
                .Select(c => new RenterContractCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    TransactionId = c.TransactionId,
                    MerkleRoot = c.MerkleRoot,
                    AnchoringStatus = c.AnchoringStatus,
                    IsAnchoredToBlockChain = c.AnchoringStatus == MARN_API.Enums.Contract.ContractAnchoringStatus.Anchored,
                    ExpiryDate = c.LeaseEndDate.ToDateTime(TimeOnly.MinValue),
                    OwnerId = c.Property.OwnerId,
                    OwnerName = (c.Property.Owner.FirstName + " " + c.Property.Owner.LastName).Trim(),
                    PropertyId = c.PropertyId,
                    PropertyTitle = c.Property.Title
                })
                .ToListAsync();

            user.OwnerContracts = await _context.Contracts
                .AsNoTracking()
                .Where(c => c.Property.OwnerId == userId)
                .OrderByDescending(c => c.LeaseEndDate)
                .Select(c => new OwnerContractCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    TransactionId = c.TransactionId,
                    MerkleRoot = c.MerkleRoot,
                    AnchoringStatus = c.AnchoringStatus,
                    IsAnchoredToBlockChain = c.AnchoringStatus == MARN_API.Enums.Contract.ContractAnchoringStatus.Anchored,
                    ExpiryDate = c.LeaseEndDate.ToDateTime(TimeOnly.MinValue),
                    RenterId = c.RenterId,
                    RenterName = (c.Renter.FirstName + " " + c.Renter.LastName).Trim(),
                    PropertyId = c.PropertyId,
                    PropertyTitle = c.Property.Title
                })
                .ToListAsync();

            user.PaidPayments = await _context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentSchedule.Contract.RenterId == userId)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new PaidPaymentDto
                {
                    AmountPaid = p.AmountTotal,
                    ContractId = p.PaymentSchedule.ContractId,
                    PaidAt = p.PaidAt
                })
                .ToListAsync();

            user.ReceivedPayments = await _context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new ReceivedPaymentDto
                {
                    AmountReceived = p.OwnerAmount,
                    ContractId = p.PaymentSchedule.ContractId,
                    PaidAt = p.PaidAt,
                    AvailableAt = p.AvailableAt,
                    Status = p.Status
                })
                .ToListAsync();

            return user;
        }

        public Task<ApplicationUser?> GetUserByIdAsync(Guid userId)
        {
            return NonAdminUsers().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<bool> IsAdminUserAsync(Guid userId)
        {
            var adminRoleIds = _context.Roles
                .Where(r => r.NormalizedName == "ADMIN")
                .Select(r => r.Id);

            return _context.UserRoles.AnyAsync(ur => ur.UserId == userId && adminRoleIds.Contains(ur.RoleId));
        }

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (_, r) => r.Name!)
                .OrderBy(name => name)
                .ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        private IQueryable<ApplicationUser> BuildUsersQuery(AdminUserManagementQueryDto query)
        {
            var usersQuery = NonAdminUsers();

            if (!query.IncludeDeleted)
            {
                usersQuery = usersQuery.Where(u => u.DeletedAt == null);
            }

            if (query.AccountStatus.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.AccountStatus == query.AccountStatus.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                var normalizedRole = query.Role.Trim().ToUpperInvariant();
                var roleIds = _context.Roles
                    .Where(r => r.NormalizedName == normalizedRole)
                    .Select(r => r.Id);

                usersQuery = usersQuery.Where(u =>
                    _context.UserRoles.Any(ur => ur.UserId == u.Id && roleIds.Contains(ur.RoleId)));
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                usersQuery = usersQuery.Where(u =>
                    (u.FirstName + " " + u.LastName).ToLower().Contains(search) ||
                    (u.Email != null && u.Email.ToLower().Contains(search)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(search)));
            }

            return usersQuery;
        }

        private IQueryable<ApplicationUser> NonAdminUsers()
        {
            var adminRoleIds = _context.Roles
                .Where(r => r.NormalizedName == "ADMIN")
                .Select(r => r.Id);

            return _context.Users
                .IgnoreQueryFilters()
                .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id && adminRoleIds.Contains(ur.RoleId)));
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
