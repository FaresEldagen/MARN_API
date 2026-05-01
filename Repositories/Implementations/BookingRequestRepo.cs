using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums;
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



        #region Dashboards
        public Task<List<RenterPendingBookingRequestDto>> GetRenterPendingRequests(Guid userId)
        {
            return Context.BookingRequests
                .AsNoTracking()
                .Where(r => r.RenterId == userId && r.Status == BookingRequestStatus.Pending)
                .Select(r => new RenterPendingBookingRequestDto
                {
                    BookingRequestId = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,

                    PropertyId = r.PropertyId,
                    PropertyTitle = r.Property.Title,

                    OwnerId = r.Property.Owner.Id,
                    OwnerName = $"{r.Property.Owner.FirstName} {r.Property.Owner.LastName}",
                    OwnerProfileImage = r.Property.Owner.ProfileImage
                })
                .ToListAsync();
        }

        public Task<List<OwnerPendingBookingRequestDto>> GetOwnerPendingRequests(Guid userId)
        {
            return Context.BookingRequests
                .Where(r => r.Property.Owner.Id == userId && r.Status == Enums.BookingRequestStatus.Pending)
                .Select(r => new OwnerPendingBookingRequestDto
                {
                    BookingRequestId = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,

                    PropertyId = r.PropertyId,
                    PropertyTitle = r.Property.Title,

                    RenterId = r.RenterId,
                    RenterName = $"{r.Renter.FirstName} {r.Renter.LastName}",
                    RenterProfileImage = r.Renter.ProfileImage
                })
                .ToListAsync();
        }

        public Task<List<OwnerPendingBookingRequestDto>> GetOwnerPendingRequestsByProperty(Guid userId, long propertyId)
        {
            return Context.BookingRequests
                .AsNoTracking()
                .Where(r => r.Property.Owner.Id == userId
                    && r.PropertyId == propertyId
                    && r.Status == Enums.BookingRequestStatus.Pending)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new OwnerPendingBookingRequestDto
                {
                    BookingRequestId = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    PropertyId = r.PropertyId,
                    PropertyTitle = r.Property.Title,
                    RenterId = r.RenterId,
                    RenterName = $"{r.Renter.FirstName} {r.Renter.LastName}",
                    RenterProfileImage = r.Renter.ProfileImage
                })
                .ToListAsync();
        }
        #endregion


        #region User and Property Deletion
        public async Task DeleteByUserIdAsync(Guid userId)
        {
            await Context.BookingRequests
                .Where(b => b.RenterId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteByPropertyIdAsync(long propertyId)
        {
            await Context.BookingRequests
                .Where(b => b.PropertyId == propertyId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteByPropertyIdsAsync(List<long> propertyIds)
        {
            await Context.BookingRequests
                .Where(b => propertyIds.Contains(b.PropertyId))
                .ExecuteDeleteAsync();
        }
        #endregion


        #region Booking Request Operations
        public async Task AddBookingRequestAsync(BookingRequest bookingRequest)
        {
            await Context.BookingRequests.AddAsync(bookingRequest);
            await Context.SaveChangesAsync();
        }

        public async Task<BookingRequest?> GetByIdAsync(long id)
        {
            return await Context.BookingRequests
                .Include(b => b.Property)
                .Include(b => b.Property.Owner)
                .Include(b => b.Renter)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task DeleteAsync(BookingRequest bookingRequest)
        {
            Context.BookingRequests.Remove(bookingRequest);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
