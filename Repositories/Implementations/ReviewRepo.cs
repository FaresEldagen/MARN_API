using MARN_API.Data;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext Context;
        public ReviewRepo(AppDbContext context)
        {
            Context = context;
        }


        public async Task DeleteByUserIdAsync(Guid userId)
        {
            await Context.Reviews
                .Where(r => r.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
