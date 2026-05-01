using AutoMapper;
using MARN_API.DTOs.BookingRequest;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MARN_API.Enums;
using MARN_API.DTOs.Common;

namespace MARN_API.Services.Implementations
{
    public class BookingRequestService : IBookingRequestService
    {
        private readonly IBookingRequestRepo _bookingRequestRepo;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IContractRepo _contractRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingRequestService> _logger;

        public BookingRequestService(
            IBookingRequestRepo bookingRequestRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<BookingRequestService> logger)
        {
            _bookingRequestRepo = bookingRequestRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<bool>> AddBookingRequestAsync(Guid userId, AddBookingRequestDto dto)
        {
            _logger.LogInformation("Add Booking Request attempt for userId: {userId}, propertyId: {propertyId}", userId, dto.PropertyId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.AccountStatus != AccountStatus.Verified)
            {
                _logger.LogWarning("Add Booking Request failed: User not verified for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("User account must be verified to add a booking request.", resultType: ServiceResultType.Unauthorized);
            }

            var property = await _propertyRepo.GetByIdAsync(dto.PropertyId);
            if (property == null || !property.IsActive)
            {
                _logger.LogWarning("Add Booking Request failed: Property not active or not found for propertyId: {propertyId}", dto.PropertyId);
                return ServiceResult<bool>.Fail("Property is not active or does not exist.", resultType: ServiceResultType.NotFound);
            }

            bool hasActiveContracts = await _contractRepo.HasActiveContractsForPropertyAsync(dto.PropertyId);
            if (hasActiveContracts)
            {
                _logger.LogWarning("Add Booking Request failed: Property has active contracts for propertyId: {propertyId}", dto.PropertyId);
                return ServiceResult<bool>.Fail("Property is not available as it has active contracts.", resultType: ServiceResultType.Conflict);
            }

            if (!IsDurationDivisible(dto.StartDate, dto.EndDate, property.RentalUnit))
            {
                _logger.LogWarning("Add Booking Request failed: Duration not divisible for propertyId: {propertyId}, RentalUnit: {RentalUnit}", dto.PropertyId, property.RentalUnit);
                return ServiceResult<bool>.Fail("Start and end date must align with the rental duration unit (e.g. complete months/years).", resultType: ServiceResultType.BadRequest);
            }

            var bookingRequest = _mapper.Map<Models.BookingRequest>(dto);
            bookingRequest.RenterId = userId;
            bookingRequest.Status = Enums.BookingRequestStatus.Pending;
            bookingRequest.CreatedAt = DateTime.UtcNow;

            await _bookingRequestRepo.AddBookingRequestAsync(bookingRequest);

            _logger.LogInformation("Add Booking Request successful for userId: {userId}, propertyId: {propertyId}", userId, dto.PropertyId);
            return ServiceResult<bool>.Ok(true, "Booking request added successfully.");
        }

        private bool IsDurationDivisible(DateTime start, DateTime end, RentalUnit rentalUnit)
        {
            if (end <= start) return false;

            switch (rentalUnit)
            {
                case RentalUnit.Daily:
                    return start.TimeOfDay == end.TimeOfDay;
                case RentalUnit.Monthly:
                    int months = (end.Year - start.Year) * 12 + end.Month - start.Month;
                    return start.AddMonths(months) == end;
                case RentalUnit.Yearly:
                    int years = end.Year - start.Year;
                    return start.AddYears(years) == end;
                default:
                    return false;
            }
        }
    }
}
