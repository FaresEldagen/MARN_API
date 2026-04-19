using MARN_API.Data;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class ReportRepo : IReportRepo
    {
        private readonly AppDbContext Context;
        public ReportRepo(AppDbContext context)
        {
            Context = context;
        }


        public async Task DeleteByReporterIdAsync(Guid userId)
        {
            await Context.Reports
                .Where(r => r.ReporterId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
