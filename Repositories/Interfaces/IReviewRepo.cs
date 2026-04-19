namespace MARN_API.Repositories.Interfaces
{
    public interface IReviewRepo
    {
        Task DeleteByUserIdAsync(Guid userId);
    }
}
