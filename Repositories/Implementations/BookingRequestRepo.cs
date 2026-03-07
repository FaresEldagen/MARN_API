using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class BookingRequestRepo : IBookingRequestRepo
    {
        private readonly AppDbContext Context;
        public BookingRequestRepo(AppDbContext context)
        {
            Context = context;
        }


        public Task<List<BookingRequest>> GetRenterPendingRequests(Guid userId)
        {
            return Context.BookingRequests
                .Where(r => r.RenterId == userId && r.Status == Enums.BookingRequestStatus.Pending)
                .ToListAsync();
        }

        public Task<List<BookingRequest>> GetOwnerPendingRequests(Guid userId)
        {
            return Context.BookingRequests
                .Where(r => r.OwnerId == userId && r.Status == Enums.BookingRequestStatus.Pending)
                .ToListAsync();
        }
    }
}
