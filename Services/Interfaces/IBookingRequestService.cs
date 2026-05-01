using MARN_API.DTOs.BookingRequest;
using MARN_API.Models; // For ServiceResult if it is in MARN_API.Models or MARN_API.DTOs.Common
using System;
using System.Threading.Tasks;

namespace MARN_API.Services.Interfaces
{
    public interface IBookingRequestService
    {
        Task<ServiceResult<bool>> AddBookingRequestAsync(Guid userId, AddBookingRequestDto dto);
    }
}
