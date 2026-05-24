using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AdminVerificationRepo : IAdminVerificationRepo
    {
        private readonly AppDbContext _context;

        public AdminVerificationRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AdminUserVerificationDto>> GetPendingUserVerificationsAsync(int pageNumber, int pageSize)
        {
            var query = NonAdminUsers()
                .AsNoTracking()
                .Where(u => u.AccountStatus == AccountStatus.Pending)
                .OrderBy(u => u.CreatedAt)
                .ThenBy(u => u.Id);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new AdminUserVerificationDto
                {
                    UserId = u.Id,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImage = u.ProfileImage,
                    CreatedAt = u.CreatedAt,
                    AccountStatus = u.AccountStatus,
                    FrontIdPhoto = u.FrontIdPhoto,
                    BackIdPhoto = u.BackIdPhoto,
                    ArabicFullName = u.ArabicFullName,
                    ArabicAddress = u.ArabicAddress,
                    NationalIDNumber = u.NationalIDNumber
                })
                .ToListAsync();

            return CreatePagedResult(items, pageNumber, pageSize, totalCount);
        }

        public Task<AdminUserVerificationDto?> GetUserVerificationDetailsAsync(Guid userId)
        {
            return NonAdminUsers()
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new AdminUserVerificationDto
                {
                    UserId = u.Id,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImage = u.ProfileImage,
                    CreatedAt = u.CreatedAt,
                    AccountStatus = u.AccountStatus,
                    FrontIdPhoto = u.FrontIdPhoto,
                    BackIdPhoto = u.BackIdPhoto,
                    ArabicFullName = u.ArabicFullName,
                    ArabicAddress = u.ArabicAddress,
                    NationalIDNumber = u.NationalIDNumber
                })
                .FirstOrDefaultAsync();
        }

        public Task<ApplicationUser?> GetUserForVerificationAsync(Guid userId)
        {
            return NonAdminUsers().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<PagedResult<AdminPropertyVerificationDto>> GetPendingPropertyVerificationsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Properties
                .AsNoTracking()
                .Where(p => p.Status == PropertyStatus.Pending)
                .OrderBy(p => p.CreatedAt)
                .ThenBy(p => p.Id);

            var totalCount = await query.CountAsync();
            var items = await ProjectPropertyVerification(query)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return CreatePagedResult(items, pageNumber, pageSize, totalCount);
        }

        public Task<AdminPropertyVerificationDto?> GetPropertyVerificationDetailsAsync(long propertyId)
        {
            return ProjectPropertyVerification(_context.Properties.AsNoTracking().Where(p => p.Id == propertyId))
                .FirstOrDefaultAsync();
        }

        public Task<Property?> GetPropertyForVerificationAsync(long propertyId)
        {
            return _context.Properties.FirstOrDefaultAsync(p => p.Id == propertyId);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        private IQueryable<ApplicationUser> NonAdminUsers()
        {
            var adminRoleIds = _context.Roles
                .Where(r => r.NormalizedName == "ADMIN")
                .Select(r => r.Id);

            return _context.Users
                .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id && adminRoleIds.Contains(ur.RoleId)));
        }

        private static IQueryable<AdminPropertyVerificationDto> ProjectPropertyVerification(IQueryable<Property> query)
        {
            return query.Select(p => new AdminPropertyVerificationDto
            {
                PropertyId = p.Id,
                Title = p.Title,
                Description = p.Description,
                Type = p.Type,
                Status = p.Status,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId,
                OwnerFullName = (p.Owner.FirstName + " " + p.Owner.LastName).Trim(),
                OwnerEmail = p.Owner.Email,
                OwnerAccountStatus = p.Owner.AccountStatus,
                ProofOfOwnership = p.ProofOfOwnership,
                PrimaryImage = p.Media
                    .Where(m => m.IsPrimary)
                    .Select(m => m.Path)
                    .FirstOrDefault(),
                Price = p.Price,
                RentalUnit = p.RentalUnit,
                Address = p.Address,
                City = p.City,
                Governorate = p.State,
                ZipCode = p.ZipCode,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            });
        }

        private static PagedResult<T> CreatePagedResult<T>(List<T> items, int pageNumber, int pageSize, int totalCount)
        {
            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}
