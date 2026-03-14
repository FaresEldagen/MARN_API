using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IBookingRequestRepo
    {
        public Task<List<RenterPendingBookingRequestDto>> GetRenterPendingRequests(Guid userId);
        public Task<List<BookingRequest>> GetOwnerPendingRequests(Guid userId);
    }
}
