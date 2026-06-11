using MARN_API.DTOs.Contracts;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MARN_API.Data;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Property;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using MARN_API.Services.Implementations;
using MARN_API.Utilities;

namespace MARN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempGenController : ControllerBase
    {
        private static readonly byte[] DummyOtsAttestation = [0x01, 0x02, 0x03];

        private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en");
        private static readonly CultureInfo ArabicCulture = CultureInfo.GetCultureInfo("ar");


        private readonly IContractPdfGenerator _contractPdfGenerator;
        private readonly IHashingService _hashingService;
        private readonly IOpenTimestampsService _openTimestampsService;
        private readonly IOpenTimestampsProofReader _proofReader;
        private readonly IContractDocumentStorage _contractDocumentStorage;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;
        private readonly IAppTextLocalizer _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public TempGenController(
            IContractPdfGenerator contractPdfGenerator,
            IHashingService hashingService,
            IOpenTimestampsService openTimestampsService,
            IOpenTimestampsProofReader proofReader,
            IContractDocumentStorage contractDocumentStorage,
            IWebHostEnvironment env,
            AppDbContext dbContext,
            IAppTextLocalizer localizer,
            UserManager<ApplicationUser> userManager)
        {
            _contractPdfGenerator = contractPdfGenerator;
            _hashingService = hashingService;
            _openTimestampsService = openTimestampsService;
            _proofReader = proofReader;
            _contractDocumentStorage = contractDocumentStorage;
            _env = env;
            _dbContext = dbContext;
            _localizer = localizer;
            _userManager = userManager;
        }

        /// <summary>
        /// [TEST ONLY] Generates sample contracts, creates PDFs, computes hashes, submits to OpenTimestamps, and saves results for seeding/testing purposes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var generatedAtUtc = DateTime.UtcNow;
            var resultsPath = ContractDocumentPathBuilder.BuildContractsRootAbsolutePath(_env.ContentRootPath);
            Directory.CreateDirectory(resultsPath);

            var contracts = new[]
            {
                new { Id = 1000001, PropertyId = 1001L, RenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 60000m, Start = new DateTime(2026, 1, 1), End = new DateTime(2027, 1, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                new { Id = 1000002, PropertyId = 1002L, RenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Amount = 90000m, Start = new DateTime(2026, 1, 1), End = new DateTime(2027, 1, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Quarterly },
                new { Id = 1000003, PropertyId = 1100L, RenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 96000m, Start = new DateTime(2025, 6, 1), End = new DateTime(2027, 6, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.OneTime },
                new { Id = 1000004, PropertyId = 1100L, RenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Amount = 48000m, Start = new DateTime(2026, 2, 1), End = new DateTime(2027, 2, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                new { Id = 1000005, PropertyId = 1002L, RenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 90000m, Start = new DateTime(2024, 1, 1), End = new DateTime(2024, 12, 31), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Quarterly },
                new { Id = 1000006, PropertyId = 1004L, RenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Amount = 180000m, Start = new DateTime(2026, 5, 1), End = new DateTime(2027, 5, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                
                // Dashboard Scenario Contracts
                new { Id = 1000101, PropertyId = 1204L, RenterId = Guid.Parse("10000000-0000-0000-0000-000000000004"), Amount = 15600m, Start = new DateTime(2026, 6, 1), End = new DateTime(2026, 7, 31), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                new { Id = 1000102, PropertyId = 1003L, RenterId = Guid.Parse("88888888-8888-8888-8888-888888888888"), Amount = 42000m, Start = new DateTime(2025, 12, 1), End = new DateTime(2026, 6, 30), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                new { Id = 1000103, PropertyId = 1205L, RenterId = Guid.Parse("10000000-0000-0000-0000-000000000002"), Amount = 30000m, Start = new DateTime(2026, 6, 1), End = new DateTime(2026, 12, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly }
            };

            var results = new List<TempGeneratedContractResult>();

            foreach (var c in contracts)
            {
                var property = await _dbContext.Properties
                    .Include(p => p.Media)
                    .Include(p => p.Amenities)
                    .Include(p => p.Rules)
                    .FirstOrDefaultAsync(p => p.Id == c.PropertyId);

                if (property == null)
                {
                    results.Add(new TempGeneratedContractResult
                    {
                        ContractId = c.Id,
                        PropertyId = c.PropertyId,
                        Success = false,
                        Error = $"Property {c.PropertyId} not found."
                    });
                    continue;
                }

                var owner = await _userManager.FindByIdAsync(property.OwnerId.ToString());
                var renter = await _userManager.FindByIdAsync(c.RenterId.ToString());

                var pdfRequest = new ContractPdfRequest
                {
                    ContractNumber = c.Id.ToString(),
                    IssuedAtUtc = DateTime.UtcNow,
                    Landlord = new PartyInfo
                    {
                        FullName = $"{owner?.FirstName} {owner?.LastName}",
                        NationalId = owner?.NationalIDNumber ?? "N/A",
                        Email = owner?.Email,
                        PhoneNumber = owner?.PhoneNumber,
                        Address = owner?.ArabicAddress
                    },
                    Tenant = new PartyInfo
                    {
                        FullName = $"{renter?.FirstName} {renter?.LastName}",
                        NationalId = renter?.NationalIDNumber ?? "N/A",
                        Email = renter?.Email,
                        PhoneNumber = renter?.PhoneNumber,
                        Address = renter?.ArabicAddress
                    },
                    Property = new PropertyInfo 
                    { 
                        UnitNumber = property.Id.ToString(),
                        ListingTitle = property.Title, 
                        AddressLine = property.Address, 
                        City = GetLocationBilingualDisplayName<City>(property.City), 
                        Country = GetEnumBilingualDisplayName(Country.Egypt), 
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
                        MediaPaths = property.Media.Select(m => m.Path).ToList()
                    },
                    RentalTerms = new RentalTermsInfo 
                    { 
                        RentAmount = property.Price, 
                        TotalContractAmount = c.Amount, 
                        PaymentFrequency = c.Frequency, 
                        Currency = "EGP", 
                        LeaseStartDate = DateOnly.FromDateTime(c.Start), 
                        LeaseEndDate = DateOnly.FromDateTime(c.End) 
                    },
                    ElectronicSignature = new ElectronicSignatureInfo 
                    { 
                        SignerName = $"{renter?.FirstName} {renter?.LastName}", 
                        SignerNationalId = renter?.NationalIDNumber ?? "N/A", 
                        SignedAtUtc = DateTime.UtcNow 
                    }
                };

                var pdfResult = _contractPdfGenerator.Generate(pdfRequest);
                using var stream = new MemoryStream(pdfResult.Content);
                var hash = await _hashingService.ComputeSha256HashAsync(stream);

                byte[] otsFileBytes;
                OpenTimestampsProofReader.OpenTimestampsProofExtractionResult? proofData = null;
                string? otsSubmissionError = null;
                var usedFallbackProof = false;

                try
                {
                    otsFileBytes = await _openTimestampsService.SubmitHashAsync(hash);
                    proofData = _proofReader.Extract(otsFileBytes);
                }
                catch (Exception ex)
                {
                    usedFallbackProof = true;
                    otsSubmissionError = ex.Message;
                    otsFileBytes = _openTimestampsService.BuildDetachedOtsFile(hash, DummyOtsAttestation);
                }

                var pdfRelativePath = await _contractDocumentStorage.SaveContractPdfAsync(c.Id, pdfResult.Content);
                var otsRelativePath = await _contractDocumentStorage.SaveOtsProofAsync(c.Id, otsFileBytes);
                var pdfPath = Path.Combine(_env.ContentRootPath, pdfRelativePath.Replace('/', Path.DirectorySeparatorChar));
                var otsPath = Path.Combine(_env.ContentRootPath, otsRelativePath.Replace('/', Path.DirectorySeparatorChar));

                var txId = proofData?.TransactionIds.FirstOrDefault();
                var merkleRoot = proofData?.MerkleRoots.FirstOrDefault();

                var dbContract = await _dbContext.Contracts.FirstOrDefaultAsync(co => co.Id == c.Id);
                var databaseContractUpdated = dbContract != null;
                if (dbContract != null)
                {
                    dbContract.FileName = pdfResult.FileName;
                    dbContract.Hash = hash;
                    dbContract.FilePath = pdfRelativePath;
                    dbContract.OtsFilePath = otsRelativePath;
                    dbContract.TransactionId = txId;
                    dbContract.MerkleRoot = merkleRoot;
                    _dbContext.Contracts.Update(dbContract);
                }

                results.Add(new TempGeneratedContractResult
                {
                    ContractId = c.Id,
                    PropertyId = c.PropertyId,
                    Success = true,
                    FileName = pdfResult.FileName,
                    Hash = hash,
                    TransactionId = txId,
                    MerkleRoot = merkleRoot,
                    PendingCalendarUrls = proofData?.PendingCalendarUrls ?? [],
                    UsedFallbackProof = usedFallbackProof,
                    OtsSubmissionError = otsSubmissionError,
                    DatabaseContractUpdated = databaseContractUpdated,
                    PdfPath = pdfPath,
                    OtsPath = otsPath
                });
            }

            var outputStr = string.Join(
                Environment.NewLine + Environment.NewLine,
                results.Select(FormatSeedResult));
            await System.IO.File.WriteAllTextAsync(Path.Combine(resultsPath, "results.txt"), outputStr);

            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                generatedAtUtc,
                seedPath = resultsPath,
                totalContracts = results.Count,
                successfulContracts = results.Count(r => r.Success),
                fallbackProofContracts = results.Count(r => r.UsedFallbackProof),
                contracts = results
            });
        }

        /// <summary>
        /// [TEST ONLY] Generates a preview contract PDF using seeded property and user data.
        /// </summary>
        [HttpGet("preview-contract")]
        public async Task<IActionResult> PreviewContract([FromQuery] long? propertyId = null, [FromQuery] Guid? renterId = null)
        {
            IQueryable<Property> propertyQuery = _dbContext.Properties
                .Include(p => p.Media)
                .Include(p => p.Amenities)
                .Include(p => p.Rules)
                .Where(p => p.OwnerId != Guid.Empty);

            if (propertyId.HasValue)
            {
                propertyQuery = propertyQuery.Where(p => p.Id == propertyId.Value);
            }

            var property = await propertyQuery
                .OrderBy(p => p.Id)
                .FirstOrDefaultAsync();

            if (property == null)
            {
                return NotFound("No seeded property was found for contract preview.");
            }

            var owner = await _userManager.FindByIdAsync(property.OwnerId.ToString());
            if (owner == null)
            {
                return NotFound($"Owner not found for property {property.Id}.");
            }

            IQueryable<ApplicationUser> renterQuery = _dbContext.Users
                .IgnoreQueryFilters()
                .Where(u => u.Id != property.OwnerId && u.DeletedAt == null);

            if (renterId.HasValue)
            {
                renterQuery = renterQuery.Where(u => u.Id == renterId.Value);
            }

            var renter = await renterQuery
                .OrderBy(u => u.Id)
                .FirstOrDefaultAsync();

            if (renter == null)
            {
                return NotFound("No seeded renter was found for contract preview.");
            }

            var (leaseStart, leaseEnd, paymentFrequency) = GetPreviewTerms(property.RentalUnit);
            var totalContractAmount = CalculateTotalAmount(property.Price, property.RentalUnit, leaseStart, leaseEnd);

            var request = new ContractPdfRequest
            {
                ContractNumber = $"PREVIEW-{property.Id}-{renter.Id.ToString()[..8]}",
                IssuedAtUtc = DateTime.UtcNow,
                Landlord = new PartyInfo
                {
                    FullName = $"{owner.FirstName} {owner.LastName}".Trim(),
                    NationalId = owner.NationalIDNumber,
                    Email = owner.Email,
                    PhoneNumber = owner.PhoneNumber,
                    Address = owner.ArabicAddress
                },
                Tenant = new PartyInfo
                {
                    FullName = $"{renter.FirstName} {renter.LastName}".Trim(),
                    NationalId = renter.NationalIDNumber,
                    Email = renter.Email,
                    PhoneNumber = renter.PhoneNumber,
                    Address = renter.ArabicAddress
                },
                Property = new PropertyInfo
                {
                    UnitNumber = property.Id.ToString(),
                    ListingTitle = property.Title,
                    AddressLine = property.Address,
                    City = GetLocationBilingualDisplayName<City>(property.City),
                    Country = GetEnumBilingualDisplayName(Country.Egypt),
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
                    MediaPaths = property.Media.Select(m => m.Path).ToList()
                },
                RentalTerms = new RentalTermsInfo
                {
                    RentAmount = property.Price,
                    TotalContractAmount = totalContractAmount,
                    PaymentFrequency = paymentFrequency,
                    Currency = "EGP",
                    LeaseStartDate = leaseStart,
                    LeaseEndDate = leaseEnd
                },
                ElectronicSignature = new ElectronicSignatureInfo
                {
                    SignerName = $"{renter.FirstName} {renter.LastName}".Trim(),
                    SignerNationalId = renter.NationalIDNumber,
                    SignedAtUtc = DateTime.UtcNow
                }
            };

            var pdfResult = _contractPdfGenerator.Generate(request);

            Response.Headers["X-Preview-PropertyId"] = property.Id.ToString();
            Response.Headers["X-Preview-OwnerId"] = owner.Id.ToString();
            Response.Headers["X-Preview-RenterId"] = renter.Id.ToString();

            return File(pdfResult.Content, "application/pdf", pdfResult.FileName);
        }

        private static (DateOnly Start, DateOnly End, PaymentFrequency PaymentFrequency) GetPreviewTerms(Enums.Property.RentalUnit rentalUnit)
        {
            var start = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1));

            return rentalUnit switch
            {
                Enums.Property.RentalUnit.Daily => (start, start.AddDays(7), PaymentFrequency.OneTime),
                Enums.Property.RentalUnit.Monthly => (start, start.AddMonths(6), PaymentFrequency.Monthly),
                Enums.Property.RentalUnit.Yearly => (start, start.AddYears(1), PaymentFrequency.Quarterly),
                _ => (start, start.AddMonths(1), PaymentFrequency.OneTime)
            };
        }

        private static decimal CalculateTotalAmount(decimal propertyPrice, Enums.Property.RentalUnit rentalUnit, DateOnly leaseStart, DateOnly leaseEnd)
        {
            return rentalUnit switch
            {
                Enums.Property.RentalUnit.Daily => propertyPrice * (leaseEnd.DayNumber - leaseStart.DayNumber),
                Enums.Property.RentalUnit.Monthly => propertyPrice * MonthsBetween(leaseStart, leaseEnd),
                Enums.Property.RentalUnit.Yearly => propertyPrice * YearsBetween(leaseStart, leaseEnd),
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
            return $"{_localizer.GetEnumDisplayName(value, EnglishCulture)} / {_localizer.GetEnumDisplayName(value, ArabicCulture)}";
        }

        private static string FormatSeedResult(TempGeneratedContractResult result)
        {
            if (!result.Success)
            {
                return $"// Contract {result.ContractId}: {result.Error}";
            }

            return $@"
// For Contract {result.ContractId}
FileName = ""{result.FileName}"",
Hash = ""{result.Hash}"",
TransactionId = {(result.TransactionId != null ? $"\"{result.TransactionId}\"" : "null")},
MerkleRoot = {(result.MerkleRoot != null ? $"\"{result.MerkleRoot}\"" : "null")},
FilePath = ""{ContractDocumentPathBuilder.BuildPdfRelativePath(result.ContractId)}"",
OtsFilePath = ""{ContractDocumentPathBuilder.BuildOtsRelativePath(result.ContractId)}""".Trim();
        }

        private sealed class TempGeneratedContractResult
        {
            public long ContractId { get; init; }
            public long PropertyId { get; init; }
            public bool Success { get; init; }
            public string? FileName { get; init; }
            public string? Hash { get; init; }
            public string? TransactionId { get; init; }
            public string? MerkleRoot { get; init; }
            public List<string> PendingCalendarUrls { get; init; } = [];
            public bool UsedFallbackProof { get; init; }
            public string? OtsSubmissionError { get; init; }
            public bool DatabaseContractUpdated { get; init; }
            public string? PdfPath { get; init; }
            public string? OtsPath { get; init; }
            public string? Error { get; init; }
        }
    }
}
