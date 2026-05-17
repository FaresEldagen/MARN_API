using AutoMapper;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.DTOs.Notification;
using MARN_API.Enums;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Notification;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Implementations
{
    public class ContractService : IContractService
    {
        private readonly IContractRepo _contractRepo;
        private readonly HashingService _hashingService;
        private readonly OpenTimestampsService _openTimestampsService;
        private readonly OpenTimestampsProofReader _proofReader;
        private readonly ContractPdfGenerator _contractPdfGenerator;
        private readonly IBookingRequestRepo _bookingRequestRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ContractService> _logger;

        public ContractService(
            IContractRepo contractRepo,
            HashingService hashingService,
            OpenTimestampsService openTimestampsService,
            OpenTimestampsProofReader proofReader,
            ContractPdfGenerator contractPdfGenerator,
            IBookingRequestRepo bookingRequestRepo,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            INotificationService notificationService,
            ILogger<ContractService> logger)
        {
            _contractRepo = contractRepo;
            _hashingService = hashingService;
            _openTimestampsService = openTimestampsService;
            _proofReader = proofReader;
            _contractPdfGenerator = contractPdfGenerator;
            _bookingRequestRepo = bookingRequestRepo;
            _userManager = userManager;
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
        }


        public async Task<ServiceResult<long>> CreateContractFromBookingAsync(Guid userId, long bookingRequestId)
        {
            _logger.LogInformation("Create Contract from Booking attempt for userId: {userId}, bookingRequestId: {bookingRequestId}", userId, bookingRequestId);

            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                return ServiceResult<long>.Fail("User not found.", resultType: ServiceResultType.Unauthorized);
            }

            if (currentUser.AccountStatus == Enums.Account.AccountStatus.Banned)
            {
                return ServiceResult<long>.Fail("Banned accounts cannot create contracts.", resultType: ServiceResultType.Forbidden);
            }

            var booking = await _bookingRequestRepo.GetByIdAsync(bookingRequestId);
            if (booking is null)
            {
                _logger.LogWarning("Create Contract failed: Booking request not found for bookingRequestId: {bookingRequestId}", bookingRequestId);
                return ServiceResult<long>.Fail("Booking request not found.", resultType: ServiceResultType.NotFound);
            }

            if (booking.Property.OwnerId != userId)
            {
                _logger.LogWarning("Create Contract failed: User {userId} is not the owner of property {propertyId}", userId, booking.PropertyId);
                return ServiceResult<long>.Fail("You are not the owner of this property.", resultType: ServiceResultType.Forbidden);
            }

            bool hasActiveContract = await _contractRepo.HasActiveContractsForPropertyAsync(booking.PropertyId);
            if (hasActiveContract)
            {
                _logger.LogWarning("Create Contract failed: Property {propertyId} already has an active or pending contract", booking.PropertyId);
                return ServiceResult<long>.Fail("This property already has an active or pending contract.", resultType: ServiceResultType.Conflict);
            }

            var property = booking.Property;
            var leaseStart = DateOnly.FromDateTime(booking.StartDate);
            var leaseEnd = DateOnly.FromDateTime(booking.EndDate);
            var totalContractAmount = CalculateTotalAmount(property.Price, property.RentalUnit, leaseStart, leaseEnd);

            var contract = new Contract
            {
                RenterId = booking.RenterId,
                PropertyId = property.Id,
                LeaseStartDate = leaseStart,
                LeaseEndDate = leaseEnd,
                Status = ContractStatus.Pending,
                AnchoringStatus = ContractAnchoringStatus.Pending,
                PaymentFrequency = booking.PaymentFrequency,
                TotalContractAmount = totalContractAmount
            };

            await _contractRepo.AddAsync(contract);
            await _bookingRequestRepo.DeleteAsync(booking);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.RenterId.ToString(),
                UserType = NotificationUserType.Renter,
                Type = NotificationType.ContractStarted,
                Title = "Contract Ready for Signature",
                Body = $"The owner of \"{property.Title}\" has generated a contract for you. Please review and sign it."
            });

            _logger.LogInformation("Create Contract successful for contractId: {contractId}", contract.Id);
            return ServiceResult<long>.Ok(contract.Id, "Contract created successfully.", ServiceResultType.Created);
        }

        public async Task<ServiceResult<long>> SignContractAsync(Guid userId, long contractId)
        {
            _logger.LogInformation("Sign Contract attempt for userId: {userId}, contractId: {contractId}", userId, contractId);
            var contract = await _contractRepo.GetByIdAsync(contractId);
            if (contract is null)
                return ServiceResult<long>.Fail("Contract not found.", resultType: ServiceResultType.NotFound);

            if (contract.RenterId != userId)
            {
                _logger.LogWarning("Sign Contract failed: User {userId} is not the designated renter for contractId: {contractId}", userId, contractId);
                return ServiceResult<long>.Fail("You are not the designated renter for this contract.", resultType: ServiceResultType.Forbidden);
            }

            var renterUser = await _userManager.FindByIdAsync(userId.ToString());
            if (renterUser == null)
            {
                return ServiceResult<long>.Fail("User not found.", resultType: ServiceResultType.Unauthorized);
            }

            if (renterUser.AccountStatus == Enums.Account.AccountStatus.Banned)
            {
                return ServiceResult<long>.Fail("Banned accounts cannot sign new contracts.", resultType: ServiceResultType.Forbidden);
            }

            if (contract.Status != ContractStatus.Pending)
            {
                _logger.LogWarning("Sign Contract failed: Contract {contractId} is in {status} status", contractId, contract.Status);
                return ServiceResult<long>.Fail($"Contract is in {contract.Status} status. Only Pending contracts can be signed.", resultType: ServiceResultType.BadRequest);
            }

            var property = contract.Property;
            var owner = await _userManager.FindByIdAsync(contract.Property.OwnerId.ToString());
            var renter = await _userManager.FindByIdAsync(contract.RenterId.ToString());

            var pdfRequest = new ContractPdfRequest
            {
                ContractNumber = contract.Id.ToString(),
                IssuedAtUtc = DateTime.UtcNow,
                Landlord = new PartyInfo
                {
                    FullName = $"{owner!.FirstName} {owner.LastName}",
                    NationalId = owner.NationalIDNumber,
                    Email = owner.Email,
                    PhoneNumber = owner.PhoneNumber,
                    Address = owner.ArabicAddress,
                },
                Tenant = new PartyInfo
                {
                    FullName = $"{renter!.FirstName} {renter.LastName}",
                    NationalId = renter.NationalIDNumber,
                    Email = renter.Email,
                    PhoneNumber = renter.PhoneNumber,
                    Address = renter.ArabicAddress,
                },
                Property = new PropertyInfo
                {
                    UnitNumber = property.Id.ToString(),
                    ListingTitle = property.Title,
                    AddressLine = property.Address,
                    City = property.City,
                    Country = "Egypt",
                    Description = property.Description,
                    Type = property.Type.ToString(),
                    State = property.State,
                    ZipCode = property.ZipCode,
                    Latitude = property.Latitude,
                    Longitude = property.Longitude,
                    Bedrooms = property.Bedrooms,
                    Beds = property.Beds,
                    Bathrooms = property.Bathrooms,
                    SquareMeters = property.SquareMeters,
                    MaxOccupants = property.MaxOccupants,
                    IsShared = property.IsShared,
                    Amenities = string.Join(", ", property.Amenities.Select(a => a.Amenity.ToString())),
                    Rules = string.Join("; ", property.Rules.Select(r => r.Rule)),
                    MediaPaths = property.Media.Select(m => m.Path).ToList(),
                },
                RentalTerms = new RentalTermsInfo
                {
                    RentAmount = property.Price,
                    TotalContractAmount = contract.TotalContractAmount,
                    PaymentFrequency = contract.PaymentFrequency,
                    Currency = "EGP",
                    LeaseStartDate = contract.LeaseStartDate,
                    LeaseEndDate = contract.LeaseEndDate,
                },
                ElectronicSignature = new ElectronicSignatureInfo
                {
                    SignerName = $"{renter.FirstName} {renter.LastName}",
                    SignerNationalId = renter.NationalIDNumber,
                    SignedAtUtc = DateTime.UtcNow,
                }
            };

            GeneratedContractPdfResult pdfResult;
            try
            {
                pdfResult = _contractPdfGenerator.Generate(pdfRequest);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning("Sign Contract failed: Missing data for PDF generation: {param}", ex.ParamName);
                return ServiceResult<long>.Fail($"Contract generation failed: Missing required data ({ex.ParamName}).", resultType: ServiceResultType.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sign Contract failed: Unexpected error during PDF generation for contractId: {contractId}", contractId);
                return ServiceResult<long>.Fail("An unexpected error occurred while generating the contract document.", resultType: ServiceResultType.InternalError);
            }

            await using var stream = new MemoryStream(pdfResult.Content);
            var hash = await _hashingService.ComputeSha256HashAsync(stream);

            var otsFileBytes = await _openTimestampsService.SubmitHashAsync(hash);
            var proofData = _proofReader.Extract(otsFileBytes);

            contract.SignedByRenterAt = DateTime.UtcNow;
            contract.Status = ContractStatus.Active;
            contract.FileName = pdfResult.FileName;
            contract.FileBytes = pdfResult.Content;
            contract.Hash = hash;
            contract.OtsFileBytes = otsFileBytes;
            contract.TransactionId = proofData.TransactionIds.FirstOrDefault();
            contract.MerkleRoot = proofData.MerkleRoots.FirstOrDefault();
            contract.AnchoringStatus = ContractAnchoringStatus.Pending;

            try
            {
                await _contractRepo.SignContractAsync(contract);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sign Contract failed: Could not persist contract or generate payment schedules for contractId: {contractId}", contractId);
                return ServiceResult<long>.Fail(
                    "An error occurred while saving the contract and generating payment schedules. Please try again.",
                    resultType: ServiceResultType.InternalError);
            }

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.Property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.ContractSigned,
                Title = "Contract Signed",
                Body = $"The renter {renter!.FirstName} {renter.LastName} has signed the contract for \"{property.Title}\"."
            });

            _logger.LogInformation("Sign Contract successful for contractId: {contractId}", contractId);
            return ServiceResult<long>.Ok(contract.Id, "Contract signed successfully.");
        }


        public async Task<ServiceResult<ContractDetailsDto>> GetContractByIdAsync(Guid userId, long contractId)
        {
            _logger.LogInformation("Get Contract Details attempt for userId: {userId}, contractId: {contractId}", userId, contractId);

            var contract = await _contractRepo.GetByIdAsync(contractId);
            if (contract is null)
            {
                _logger.LogWarning("Get Contract Details failed: Contract not found for contractId: {contractId}", contractId);
                return ServiceResult<ContractDetailsDto>.Fail("Contract not found.", resultType: ServiceResultType.NotFound);
            }

            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            bool isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin && contract.Property.OwnerId != userId && contract.RenterId != userId)
            {
                _logger.LogWarning("Get Contract Details failed: Access denied for userId: {userId}, contractId: {contractId}", userId, contractId);
                return ServiceResult<ContractDetailsDto>.Fail("You do not have access to view this contract.", resultType: ServiceResultType.Forbidden);
            }

            var property = contract.Property;
            var owner = await _userManager.FindByIdAsync(property.OwnerId.ToString());
            var renter = await _userManager.FindByIdAsync(contract.RenterId.ToString());

            var dto = new ContractDetailsDto
            {
                ContractStatus = contract.Status,
                ContractId = contract.Id,
                Duration = FormatDuration(contract.LeaseStartDate, contract.LeaseEndDate, property.RentalUnit),
                StartDate = contract.LeaseStartDate,
                EndDate = contract.LeaseEndDate,
                TotalContractValue = contract.TotalContractAmount,
                PropertyInfo = new ContractPropertyInfoDto
                {
                    Id = property.Id,
                    Name = property.Title,
                    StreetAddress = property.Address,
                    City = property.City.ToString(),
                    State = property.State.ToString(),
                    RentalDuration = property.RentalUnit.ToString(),
                    Price = property.Price
                },
                OwnerInfo = new ContractUserInfo
                {
                    Id = owner!.Id,
                    ProfileImage = owner.ProfileImage,
                    FullName = $"{owner.FirstName} {owner.LastName}",
                    Email = owner.Email!
                },
                RenterInfo = new ContractUserInfo
                {
                    Id = renter!.Id,
                    ProfileImage = renter.ProfileImage,
                    FullName = $"{renter.FirstName} {renter.LastName}",
                    Email = renter.Email!
                }
            };

            _logger.LogInformation("Get Contract Details successful for contractId: {contractId}", contractId);
            return ServiceResult<ContractDetailsDto>.Ok(dto);
        }

        public async Task<ServiceResult<ContractFileDto>> DownloadContractAsync(Guid userId, long contractId)
        {
            _logger.LogInformation("Download Contract PDF attempt for userId: {userId}, contractId: {contractId}", userId, contractId);

            var contract = await _contractRepo.GetByIdAsync(contractId);
            if (contract is null || contract.FileBytes is null)
            {
                _logger.LogWarning("Download Contract PDF failed: Contract or file not found for contractId: {contractId}", contractId);
                return ServiceResult<ContractFileDto>.Fail("Contract file not found.", resultType: ServiceResultType.NotFound);
            }

            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            bool isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin && contract.Property.OwnerId != userId && contract.RenterId != userId)
            {
                _logger.LogWarning("Download Contract PDF failed: Access denied for userId: {userId}, contractId: {contractId}", userId, contractId);
                return ServiceResult<ContractFileDto>.Fail("You do not have access to this contract.", resultType: ServiceResultType.Forbidden);
            }

            var fileDto = new ContractFileDto
            {
                FileBytes = contract.FileBytes,
                ContentType = "application/pdf",
                FileName = contract.FileName
            };

            _logger.LogInformation("Download Contract PDF successful for contractId: {contractId}", contractId);
            return ServiceResult<ContractFileDto>.Ok(fileDto);
        }

        public async Task<ServiceResult<ContractFileDto>> DownloadOtsProofAsync(Guid userId, long contractId)
        {
            _logger.LogInformation("Download OTS Proof attempt for userId: {userId}, contractId: {contractId}", userId, contractId);

            var contract = await _contractRepo.GetByIdAsync(contractId);
            if (contract is null || contract.OtsFileBytes is null)
            {
                _logger.LogWarning("Download OTS Proof failed: Proof not found for contractId: {contractId}", contractId);
                return ServiceResult<ContractFileDto>.Fail("Proof not found.", resultType: ServiceResultType.NotFound);
            }

            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            bool isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin && contract.Property.OwnerId != userId && contract.RenterId != userId)
            {
                _logger.LogWarning("Download OTS Proof failed: Access denied for userId: {userId}, contractId: {contractId}", userId, contractId);
                return ServiceResult<ContractFileDto>.Fail("You do not have access to this contract.", resultType: ServiceResultType.Forbidden);
            }

            var fileDto = new ContractFileDto
            {
                FileBytes = contract.OtsFileBytes,
                ContentType = "application/octet-stream",
                FileName = $"{contract.FileName}.ots"
            };

            _logger.LogInformation("Download OTS Proof successful for contractId: {contractId}", contractId);
            return ServiceResult<ContractFileDto>.Ok(fileDto);
        }

        public async Task<ServiceResult<ContractVerificationResponseDto>> VerifyContractAsync(Guid userId, IFormFile file, long contractId)
        {
            _logger.LogInformation("Verify Contract attempt for userId: {userId}, contractId: {contractId}", userId, contractId);

            var record = await _contractRepo.GetByIdAsync(contractId);
            if (record is null)
            {
                _logger.LogWarning("Verify Contract failed: Contract not found for contractId: {contractId}", contractId);
                return ServiceResult<ContractVerificationResponseDto>.Fail("Contract not found.", resultType: ServiceResultType.NotFound);
            }

            var currentUser = await _userManager.FindByIdAsync(userId.ToString());
            bool isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin && record.Property.OwnerId != userId && record.RenterId != userId)
            {
                _logger.LogWarning("Verify Contract failed: Access denied for userId: {userId}, contractId: {contractId}", userId, contractId);
                return ServiceResult<ContractVerificationResponseDto>.Fail("You do not have access to this contract.", resultType: ServiceResultType.Forbidden);
            }

            await using var stream = file.OpenReadStream();
            var hash = await _hashingService.ComputeSha256HashAsync(stream);

            bool isValid = string.Equals(record.Hash, hash, StringComparison.OrdinalIgnoreCase);

            if (!isValid)
            {
                _logger.LogInformation("Verify Contract successful: Hash mismatch for contractId: {contractId}", contractId);
                return ServiceResult<ContractVerificationResponseDto>.Ok(new ContractVerificationResponseDto
                {
                    Match = false,
                    Message = "Contract has been tampered with.",
                    Status = record.Status,
                    AnchoringStatus = record.AnchoringStatus,
                    AnchoredAt = record.AnchoredAt
                });
            }

            var message = record.AnchoringStatus == ContractAnchoringStatus.Pending
                ? "Original contract verified. Blockchain anchoring is still in progress."
                : "Original contract verified and anchored on Bitcoin.";

            _logger.LogInformation("Verify Contract successful: Hash match for contractId: {contractId}", contractId);
            return ServiceResult<ContractVerificationResponseDto>.Ok(new ContractVerificationResponseDto
            {
                Match = true,
                Message = message,
                Status = record.Status,
                AnchoringStatus = record.AnchoringStatus,
                AnchoredAt = record.AnchoredAt 
            });
        }


        public async Task<ServiceResult<bool>> CancelContractAsync(Guid userId, long contractId)
        {
            _logger.LogInformation("Cancel Contract attempt for userId: {userId}, contractId: {contractId}", userId, contractId);

            var contract = await _contractRepo.GetByIdAsync(contractId);
            if (contract is null)
            {
                _logger.LogWarning("Cancel Contract failed: Contract not found for contractId: {contractId}", contractId);
                return ServiceResult<bool>.Fail("Contract not found.", resultType: ServiceResultType.NotFound);
            }

            if (contract.Status != ContractStatus.Pending)
            {
                _logger.LogWarning("Cancel Contract failed: Contract {contractId} is already in state {status}", contractId, contract.Status);
                return ServiceResult<bool>.Fail("Contract is allready signed.", resultType: ServiceResultType.Forbidden);
            }

            bool isRenter = contract.RenterId == userId;
            bool isOwner = contract.Property.OwnerId == userId;

            if (!isOwner && !isRenter)
            {
                _logger.LogWarning("Cancel Contract failed: Access denied for userId: {userId}, contractId: {contractId}", userId, contractId);
                return ServiceResult<bool>.Fail("You do not have access to cancel this contract.", resultType: ServiceResultType.Forbidden);
            }

            await _contractRepo.DeleteAsync(contract);

            if (isRenter)
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = contract.Property.OwnerId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ContractCanceled,
                    Title = "Contract Cancelled",
                    Body = $"The renter has cancelled the pending contract for \"{contract.Property.Title}\"."
                });
            }
            else if (isOwner)
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = contract.RenterId.ToString(),
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.ContractCanceled,
                    Title = "Contract Cancelled",
                    Body = $"The owner has cancelled the pending contract for \"{contract.Property.Title}\"."
                });
            }

            _logger.LogInformation("Cancel Contract successful for contractId: {contractId}", contractId);
            return ServiceResult<bool>.Ok(true,"Contract cancelled successfully.");
        }


        #region Helpers
        private static string FormatDuration(DateOnly? start, DateOnly? end, RentalUnit unit)
        {
            if (!start.HasValue || !end.HasValue) return "Unknown";
            int count = unit switch
            {
                RentalUnit.Daily => end.Value.DayNumber - start.Value.DayNumber,
                RentalUnit.Monthly => MonthsBetween(start.Value, end.Value),
                RentalUnit.Yearly => YearsBetween(start.Value, end.Value),
                _ => 0
            };
            return $"{count} {unit.ToString().ToLower()}";
        }

        private static decimal CalculateTotalAmount(decimal propertyPrice, RentalUnit rentalUnit, DateOnly leaseStart, DateOnly leaseEnd)
        {
            return rentalUnit switch
            {
                RentalUnit.Daily => propertyPrice * (leaseEnd.DayNumber - leaseStart.DayNumber),
                RentalUnit.Monthly => propertyPrice * MonthsBetween(leaseStart, leaseEnd),
                RentalUnit.Yearly => propertyPrice * YearsBetween(leaseStart, leaseEnd),
                _ => propertyPrice
            };
        }

        private static int MonthsBetween(DateOnly start, DateOnly end)
        {
            var months = (end.Year - start.Year) * 12 + (end.Month - start.Month);
            return months < 1 ? 1 : months;
        }

        private static int YearsBetween(DateOnly start, DateOnly end)
        {
            var years = end.Year - start.Year;
            return years < 1 ? 1 : years;
        }
        #endregion
    }
}
