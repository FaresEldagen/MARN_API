using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IBookingRequestRepo
    {
        public Task<List<RenterPendingBookingRequestDto>> GetRenterPendingRequests(Guid userId);
        public Task<List<OwnerPendingBookingRequestDto>> GetOwnerPendingRequests(Guid userId);
        public Task<List<OwnerPendingBookingRequestDto>> GetOwnerPendingRequestsByProperty(Guid userId, long propertyId);

        public Task DeleteByUserIdAsync(Guid userId);
        public Task DeleteByPropertyIdAsync(long propertyId);
        public Task DeleteByPropertyIdsAsync(List<long> propertyIds);
        public Task AddBookingRequestAsync(BookingRequest bookingRequest);
    }
}
