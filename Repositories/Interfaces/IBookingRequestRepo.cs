using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IBookingRequestRepo
    {
        #region Dashboards
        public Task<List<RenterPendingBookingRequestDto>> GetRenterPendingRequests(Guid userId);
        public Task<List<OwnerPendingBookingRequestDto>> GetOwnerPendingRequests(Guid userId);
        public Task<List<OwnerPendingBookingRequestDto>> GetOwnerPendingRequestsByProperty(Guid userId, long propertyId);
        #endregion


        #region User and Property Deletion
        public Task DeleteByUserIdAsync(Guid userId);
        public Task DeleteByPropertyIdAsync(long propertyId);
        public Task DeleteByPropertyIdsAsync(List<long> propertyIds);
        #endregion


        #region Booking Request Operations
        public Task AddBookingRequestAsync(BookingRequest bookingRequest);
        public Task<BookingRequest?> GetByIdAsync(long id);
        public Task DeleteAsync(BookingRequest bookingRequest);
        #endregion
    }
}
