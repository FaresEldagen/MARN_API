using Google.Apis.Auth;
using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AccountRepo : IAccountRepo
    {
        private readonly AppDbContext Context;
        public AccountRepo(AppDbContext context)
        {
            Context = context;
        }

        public async Task<ApplicationUser?> GetGoogleUser(string key)
        {
            return await(
                from login in Context.UserLogins
                join u in Context.Users.IgnoreQueryFilters()
                    on login.UserId equals u.Id
                where login.LoginProvider == "Google"
                      && login.ProviderKey == key
                select u
            ).FirstOrDefaultAsync();
        }
    }
}
