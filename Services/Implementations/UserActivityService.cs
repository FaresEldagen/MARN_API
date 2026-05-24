using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class UserActivityService : IUserActivityService
    {
        private static readonly JsonSerializerOptions MetadataJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly AppDbContext _context;
        private readonly ILogger<UserActivityService> _logger;

        public UserActivityService(AppDbContext context, ILogger<UserActivityService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RecordAsync(Guid userId, string activityType, long? propertyId = null, object? metadata = null)
        {
            var activity = new UserActivity
            {
                UserId = userId,
                PropertyId = propertyId,
                UserActivityType = activityType,
                Metadata = metadata == null ? null : JsonSerializer.Serialize(metadata, MetadataJsonOptions),
                CreatedAt = DateTime.UtcNow
            };

            await _context.UserActivities.AddAsync(activity);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Recorded user activity {ActivityType} for user {UserId} and property {PropertyId}",
                activityType,
                userId,
                propertyId);
        }

        public async Task RemoveAsync(Guid userId, string activityType, long? propertyId = null)
        {
            var query = _context.UserActivities.Where(activity =>
                activity.UserId == userId &&
                activity.UserActivityType == activityType);

            if (propertyId.HasValue)
            {
                query = query.Where(activity => activity.PropertyId == propertyId.Value);
            }
            else
            {
                query = query.Where(activity => activity.PropertyId == null);
            }

            var deletedCount = await query.ExecuteDeleteAsync();

            _logger.LogInformation(
                "Removed {DeletedCount} user activity records of type {ActivityType} for user {UserId} and property {PropertyId}",
                deletedCount,
                activityType,
                userId,
                propertyId);
        }
    }
}
