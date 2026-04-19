namespace MARN_API.Repositories.Interfaces
{
    public interface IReportRepo
    {
        Task DeleteByReporterIdAsync(Guid userId);
    }
}
