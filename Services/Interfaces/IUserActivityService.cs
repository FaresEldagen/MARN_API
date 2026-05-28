namespace MARN_API.Services.Interfaces
{
    public interface IUserActivityService
    {
        Task RecordAsync(Guid userId, string activityType, long? propertyId = null, object? metadata = null);
        Task RemoveAsync(Guid userId, string activityType, long? propertyId = null);
    }
}
