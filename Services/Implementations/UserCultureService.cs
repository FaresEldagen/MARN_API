using System.Globalization;
using MARN_API.Data;
using MARN_API.Enums.Account;
using MARN_API.Localization;
using MARN_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Services.Implementations
{
    public class UserCultureService : IUserCultureService
    {
        private readonly AppDbContext _dbContext;

        public UserCultureService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CultureInfo> ResolveUserCultureAsync(string? userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return LocalizationConstants.GetCulture(LocalizationConstants.DefaultCulture);
            }

            var language = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.Id == parsedUserId)
                .Select(user => user.Language)
                .FirstOrDefaultAsync();

            var cultureCode = language == Language.Arabic
                ? LocalizationConstants.ArabicCulture
                : LocalizationConstants.DefaultCulture;

            return LocalizationConstants.GetCulture(cultureCode);
        }
    }
}
