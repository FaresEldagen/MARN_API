using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IBookingRequestRepo
    {
        public Task<List<BookingRequest>> GetRenterPendingRequests(Guid userId);
        public Task<List<BookingRequest>> GetOwnerPendingRequests(Guid userId);
    }
}
