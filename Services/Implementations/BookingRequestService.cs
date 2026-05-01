using AutoMapper;
using MARN_API.DTOs.BookingRequest;
using MARN_API.DTOs.Notification;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Notification;
using MARN_API.Enums.Property;
using MARN_API.Hubs;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly INotificationService _notificationService;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public BookingRequestService(
            IBookingRequestRepo bookingRequestRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<BookingRequestService> logger,
            INotificationService notificationService,
            IChatService chatService,
            IHubContext<ChatHub> hubContext)
        {
            _bookingRequestRepo = bookingRequestRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _notificationService = notificationService;
            _chatService = chatService;
            _hubContext = hubContext;
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

            // Validate PaymentFrequency based on RentalUnit
            if (property.RentalUnit == RentalUnit.Daily && dto.PaymentFrequency != PaymentFrequency.OneTime)
            {
                return ServiceResult<bool>.Fail("For daily rentals, only 'OneTime' payment frequency is allowed.", resultType: ServiceResultType.BadRequest);
            }
            if (property.RentalUnit == RentalUnit.Monthly && dto.PaymentFrequency != PaymentFrequency.OneTime && dto.PaymentFrequency != PaymentFrequency.Monthly)
            {
                return ServiceResult<bool>.Fail("For monthly rentals, only 'OneTime' or 'Monthly' payment frequencies are allowed.", resultType: ServiceResultType.BadRequest);
            }
            // For Yearly, any PaymentFrequency (OneTime, Monthly, Quarterly, Yearly) is allowed, so no check is needed.

            var bookingRequest = _mapper.Map<Models.BookingRequest>(dto);
            bookingRequest.RenterId = userId;
            bookingRequest.Status = Enums.BookingRequestStatus.Pending;
            bookingRequest.CreatedAt = DateTime.UtcNow;

            await _bookingRequestRepo.AddBookingRequestAsync(bookingRequest);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.NewBookingRequest,
                Title = "New Booking Request",
                Body = $"You have received a new booking request for \"{property.Title}\" from {user.FirstName} {user.LastName}."
            });

            _logger.LogInformation("Add Booking Request successful for userId: {userId}, propertyId: {propertyId}", userId, dto.PropertyId);
            return ServiceResult<bool>.Ok(true, "Booking request added successfully.");
        }

        public async Task<ServiceResult<bool>> CancelBookingRequestAsync(Guid userId, long bookingRequestId)
        {
            _logger.LogInformation("Cancel Booking Request attempt for userId: {userId}, bookingRequestId: {bookingRequestId}", userId, bookingRequestId);

            var bookingRequest = await _bookingRequestRepo.GetByIdAsync(bookingRequestId);
            if (bookingRequest == null)
            {
                _logger.LogWarning("Cancel Booking Request failed: Not found for bookingRequestId: {bookingRequestId}", bookingRequestId);
                return ServiceResult<bool>.Fail("Booking request not found.", resultType: ServiceResultType.NotFound);
            }

            // Check if the user is the renter or the property owner
            bool isRenter = bookingRequest.RenterId == userId;
            bool isOwner = bookingRequest.Property.OwnerId == userId;

            if (!isRenter && !isOwner)
            {
                _logger.LogWarning("Cancel Booking Request failed: Unauthorized access for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("Unauthorized to cancel or reject this booking request.", resultType: ServiceResultType.Forbidden);
            }

            await _bookingRequestRepo.DeleteAsync(bookingRequest);

            if (isOwner)
            {
                // Send rejection notification to the renter
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = bookingRequest.RenterId.ToString(),
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.BookingRequestRejected,
                    Title = "Booking Request Rejected",
                    Body = $"Your booking request for \"{bookingRequest.Property.Title}\" was rejected by the owner."
                });
            }
            else
            {
                // Send cancellation notification to the owner
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = bookingRequest.Property.OwnerId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.BookingRequestCanceled,
                    Title = "Booking Request Cancelled",
                    Body = $"The booking request for \"{bookingRequest.Property.Title}\" was cancelled by the renter."
                });
            }

            _logger.LogInformation("Cancel Booking Request successful for userId: {userId}, bookingRequestId: {bookingRequestId}", userId, bookingRequestId);
            return ServiceResult<bool>.Ok(true, "Booking request removed successfully.");
        }

        public async Task<ServiceResult<bool>> StartChatAsync(Guid userId, long bookingRequestId)
        {
            _logger.LogInformation("Start Chat attempt for userId: {userId}, bookingRequestId: {bookingRequestId}", userId, bookingRequestId);

            var bookingRequest = await _bookingRequestRepo.GetByIdAsync(bookingRequestId);
            if (bookingRequest == null)
            {
                return ServiceResult<bool>.Fail("Booking request not found.", resultType: ServiceResultType.NotFound);
            }

            bool isRenter = bookingRequest.RenterId == userId;
            bool isOwner = bookingRequest.Property.OwnerId == userId;

            if (!isRenter && !isOwner)
            {
                return ServiceResult<bool>.Fail("Unauthorized to start a chat for this booking request.", resultType: ServiceResultType.Forbidden);
            }

            string senderId = userId.ToString();
            string receiverId = isRenter ? bookingRequest.Property.OwnerId.ToString() : bookingRequest.RenterId.ToString();

            string content = $"Hello! I am contacting you regarding the booking request (ID: {bookingRequestId}) for the property '{bookingRequest.Property.Title}'.";

            var result = await _chatService.SendMessageAsync(senderId, receiverId, content);
            if (!result.Success)
            {
                return ServiceResult<bool>.Fail(result.Message ?? "Failed to send the initial chat message.", resultType: ServiceResultType.InternalError);
            }

            // Broadcast real-time updates via SignalR
            var payload = result.Data;
            await _hubContext.Clients.User(receiverId).SendAsync("ReceiveMessage", payload);
            await _hubContext.Clients.User(senderId).SendAsync("SendMessage", payload);

            _logger.LogInformation("Start Chat successful for userId: {userId}, bookingRequestId: {bookingRequestId}", userId, bookingRequestId);
            return ServiceResult<bool>.Ok(true, "Chat started successfully.");
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
