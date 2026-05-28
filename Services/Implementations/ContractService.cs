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
using System.IO;
using System.Globalization;


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MARN_API.Utilities;

namespace MARN_API.Services.Implementations
{
    public class ContractService : IContractService
    {
        private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en");
        private static readonly CultureInfo ArabicCulture = CultureInfo.GetCultureInfo("ar");
        private readonly IContractRepo _contractRepo;
        private readonly IHashingService _hashingService;
        private readonly IOpenTimestampsService _openTimestampsService;
        private readonly IOpenTimestampsProofReader _proofReader;
        private readonly IContractDocumentStorage _contractDocumentStorage;
        private readonly IContractPdfGenerator _contractPdfGenerator;
        private readonly IBookingRequestRepo _bookingRequestRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IAppTextLocalizer _localizer;
        private readonly ILogger<ContractService> _logger;
        private readonly MARN_API.Data.AppDbContext _context;
        private readonly IUserActivityService _userActivityService;

        public ContractService(
            IContractRepo contractRepo,
            IHashingService hashingService,
            IOpenTimestampsService openTimestampsService,
            IOpenTimestampsProofReader proofReader,
            IContractDocumentStorage contractDocumentStorage,
            IContractPdfGenerator contractPdfGenerator,
            IBookingRequestRepo bookingRequestRepo,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            INotificationService notificationService,
            IAppTextLocalizer localizer,
            ILogger<ContractService> logger,
            MARN_API.Data.AppDbContext context,
            IUserActivityService userActivityService)
        {
            _contractRepo = contractRepo;
            _hashingService = hashingService;
            _openTimestampsService = openTimestampsService;
            _proofReader = proofReader;
            _contractDocumentStorage = contractDocumentStorage;
            _contractPdfGenerator = contractPdfGenerator;
            _bookingRequestRepo = bookingRequestRepo;
            _userManager = userManager;
            _mapper = mapper;
            _notificationService = notificationService;
            _localizer = localizer;
            _logger = logger;
            _context = context;
            _userActivityService = userActivityService;
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

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                await _contractRepo.AddAsync(contract);
                await _bookingRequestRepo.DeleteAsync(booking);
                await transaction.CommitAsync();
                
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            try
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.RenterId.ToString(),
                UserType = NotificationUserType.Renter,
                Type = NotificationType.ContractStarted,
                TitleKey = "NOTIFICATION_CONTRACT_READY_TITLE",
                BodyKey = "NOTIFICATION_CONTRACT_READY_BODY",
                LocalizationArguments = new() { property.Title },
                Title = "Contract Ready for Signature",
                Body = $"The owner of \"{property.Title}\" has generated a contract for you. Please review and sign it."
            });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification for contractId: {contractId}", contract.Id);
            }


            _logger.LogInformation("Create Contract successful for contractId: {contractId}", contract.Id);
            return ServiceResult<long>.Ok(
                contract.Id,
                "Contract created successfully.",
                ServiceResultType.Created,
                code: "ZZ_CONTRACT_CREATED_SUCCESSFULLY");
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
                return ServiceResult<long>.Fail(
                    "Contract is in {0} status. Only Pending contracts can be signed.",
                    resultType: ServiceResultType.BadRequest,
                    code: "CONTRACT_SIGN_STATUS_INVALID",
                    messageArguments: [_localizer.GetEnumDisplayName(contract.Status)]);
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
                    City = GetLocationBilingualDisplayName<City>(property.City),
                    Country = BilingualValue("Egypt", "مصر"),
                    Description = property.Description,
                    Type = GetEnumBilingualDisplayName(property.Type),
                    Governorate = GetLocationBilingualDisplayName<Governorate>(property.State),
                    ZipCode = property.ZipCode,
                    Latitude = property.Latitude,
                    Longitude = property.Longitude,
                    Bedrooms = property.Bedrooms,
                    Beds = property.Beds,
                    Bathrooms = property.Bathrooms,
                    SquareMeters = property.SquareMeters,
                    MaxOccupants = property.MaxOccupants,
                    IsShared = property.IsShared,
                    Amenities = string.Join(", ", property.Amenities.Select(a => GetEnumBilingualDisplayName(a.Amenity))),
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
                return ServiceResult<long>.Fail(
                    "Contract generation failed: Missing required data ({0}).",
                    resultType: ServiceResultType.BadRequest,
                    code: "CONTRACT_PDF_REQUIRED_DATA_MISSING",
                    messageArguments: [ex.ParamName ?? "unknown"]);
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

            string? contractFilePath = null;
            string? otsFilePath = null;

            contract.SignedByRenterAt = DateTime.UtcNow;
            contract.Status = ContractStatus.Active;
            contract.FileName = pdfResult.FileName;
            contract.Hash = hash;
            contract.TransactionId = proofData.TransactionIds.FirstOrDefault();
            contract.MerkleRoot = proofData.MerkleRoots.FirstOrDefault();
            contract.AnchoringStatus = ContractAnchoringStatus.Pending;

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                contractFilePath = await _contractDocumentStorage.SaveContractPdfAsync(contract.Id, pdfResult.Content);
                otsFilePath = await _contractDocumentStorage.SaveOtsProofAsync(contract.Id, otsFileBytes);

                contract.FilePath = contractFilePath;
                contract.OtsFilePath = otsFilePath;
                await _contractRepo.SignContractAsync(contract);
                await CleanupBookingRequestsAfterSigningAsync(contract);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await _contractDocumentStorage.DeleteAsync(contractFilePath);
                await _contractDocumentStorage.DeleteAsync(otsFilePath);
                _logger.LogError(ex, "Sign Contract failed: Could not persist contract or generate payment schedules for contractId: {contractId}", contractId);
                return ServiceResult<long>.Fail(
                    "An error occurred while saving the contract and generating payment schedules. Please try again.",
                    resultType: ServiceResultType.InternalError);
            }

            await TryRecordActivityAsync(userId, UserActivityTypes.Rent, contract.PropertyId);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.Property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.ContractSigned,
                TitleKey = "NOTIFICATION_CONTRACT_SIGNED_TITLE",
                BodyKey = "NOTIFICATION_CONTRACT_SIGNED_BODY",
                LocalizationArguments = new() { $"{renter!.FirstName} {renter.LastName}", property.Title },
                Title = "Contract Signed",
                Body = $"The renter {renter!.FirstName} {renter.LastName} has signed the contract for \"{property.Title}\".",

                ActionType = NotificationActionType.Contract,
                ActionId = contract.Id.ToString()
            });

            _logger.LogInformation("Sign Contract successful for contractId: {contractId}", contractId);
            return ServiceResult<long>.Ok(contract.Id, "Contract signed successfully.", code: "ZZ_CONTRACT_SIGNED_SUCCESSFULLY");
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
                ContractStatusDisplayName = _localizer.GetEnumDisplayName(contract.Status),
                TransactionId = contract.TransactionId,
                MerkleRoot = contract.MerkleRoot,
                AnchoringStatus = contract.AnchoringStatus,
                AnchoringStatusDisplayName = _localizer.GetEnumDisplayName(contract.AnchoringStatus),
                IsAnchoredToBlockChain = contract.AnchoringStatus == ContractAnchoringStatus.Anchored,
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
                    City = property.City,
                    CityDisplayName = GetLocationDisplayName<City>(property.City),
                    Governorate = property.State,
                    GovernorateDisplayName = GetLocationDisplayName<Governorate>(property.State),
                    RentalDuration = property.RentalUnit.ToString(),
                    RentalDurationDisplayName = _localizer.GetEnumDisplayName(property.RentalUnit),
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
            if (contract is null || string.IsNullOrWhiteSpace(contract.FilePath))
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

            var fileBytes = await _contractDocumentStorage.ReadAsync(contract.FilePath);
            if (fileBytes is null)
            {
                _logger.LogWarning("Download Contract PDF failed: Stored file missing for contractId: {contractId}, path: {path}", contractId, contract.FilePath);
                return ServiceResult<ContractFileDto>.Fail("Contract file not found.", resultType: ServiceResultType.NotFound);
            }

            var fileDto = new ContractFileDto
            {
                FileBytes = fileBytes,
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
            if (contract is null || string.IsNullOrWhiteSpace(contract.OtsFilePath))
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

            var fileBytes = await _contractDocumentStorage.ReadAsync(contract.OtsFilePath);
            if (fileBytes is null)
            {
                _logger.LogWarning("Download OTS Proof failed: Stored proof missing for contractId: {contractId}, path: {path}", contractId, contract.OtsFilePath);
                return ServiceResult<ContractFileDto>.Fail("Proof not found.", resultType: ServiceResultType.NotFound);
            }

            var fileDto = new ContractFileDto
            {
                FileBytes = fileBytes,
                ContentType = "application/octet-stream",
                FileName = $"{Path.GetFileNameWithoutExtension(contract.FileName)}.ots"
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
                },
                code: "ZZ_CONTRACT_VERIFICATION_TAMPERING_DETECTED");
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
            },
            code: record.AnchoringStatus == ContractAnchoringStatus.Pending
                ? "ZZ_CONTRACT_VERIFIED_PENDING_BLOCKCHAIN_ANCHORING"
                : "ZZ_CONTRACT_VERIFIED_AND_ANCHORED");
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
                return ServiceResult<bool>.Fail("Contract is already signed.", resultType: ServiceResultType.Forbidden);
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
                    TitleKey = "NOTIFICATION_CONTRACT_CANCELLED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_CANCELLED_BY_RENTER_BODY",
                    LocalizationArguments = new() { contract.Property.Title },
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
                    TitleKey = "NOTIFICATION_CONTRACT_CANCELLED_TITLE",
                    BodyKey = "NOTIFICATION_CONTRACT_CANCELLED_BY_OWNER_BODY",
                    LocalizationArguments = new() { contract.Property.Title },
                    Title = "Contract Cancelled",
                    Body = $"The owner has cancelled the pending contract for \"{contract.Property.Title}\"."
                });
            }

            _logger.LogInformation("Cancel Contract successful for contractId: {contractId}", contractId);
            return ServiceResult<bool>.Ok(true, "Contract cancelled successfully.", code: "ZZ_CONTRACT_CANCELLED_SUCCESSFULLY");
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

        private string GetLocationDisplayName<TEnum>(string? rawValue) where TEnum : struct, Enum
        {
            if (!string.IsNullOrWhiteSpace(rawValue) && Enum.TryParse<TEnum>(rawValue, true, out var parsed))
            {
                return _localizer.GetEnumDisplayName(parsed);
            }

            return rawValue ?? string.Empty;
        }

        private string GetLocationBilingualDisplayName<TEnum>(string? rawValue) where TEnum : struct, Enum
        {
            if (!string.IsNullOrWhiteSpace(rawValue) && Enum.TryParse<TEnum>(rawValue, true, out var parsed))
            {
                return GetEnumBilingualDisplayName(parsed);
            }

            return rawValue ?? string.Empty;
        }

        private string GetEnumBilingualDisplayName<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            return BilingualValue(
                _localizer.GetEnumDisplayName(value, EnglishCulture),
                _localizer.GetEnumDisplayName(value, ArabicCulture));
        }

        private static string BilingualValue(string english, string arabic)
        {
            return $"{english} / {arabic}";
        }

        private async Task CleanupBookingRequestsAfterSigningAsync(Contract contract)
        {
            await _bookingRequestRepo.DeleteByPropertyIdAndRenterIdAsync(contract.PropertyId, contract.RenterId);

            if (!await ShouldDeleteOtherPropertyBookingRequestsAsync(contract))
            {
                return;
            }

            await _bookingRequestRepo.DeleteByPropertyIdExceptRenterIdAsync(contract.PropertyId, contract.RenterId);
        }

        private async Task<bool> ShouldDeleteOtherPropertyBookingRequestsAsync(Contract contract)
        {
            if (!contract.Property.IsShared)
            {
                return true;
            }

            var activeContractsCount = await _context.Contracts.CountAsync(c =>
                c.PropertyId == contract.PropertyId &&
                c.Status == ContractStatus.Active);

            return activeContractsCount >= contract.Property.MaxOccupants;
        }

        private async Task TryRecordActivityAsync(Guid userId, string activityType, long? propertyId = null)
        {
            try
            {
                await _userActivityService.RecordAsync(userId, activityType, propertyId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to record user activity {ActivityType} for user {UserId} and property {PropertyId}",
                    activityType,
                    userId,
                    propertyId);
            }
        }
        #endregion
    }
}
