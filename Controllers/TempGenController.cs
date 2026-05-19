using MARN_API.DTOs.Contracts;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MARN_API.Data;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempGenController : ControllerBase
    {
        private readonly IContractPdfGenerator _contractPdfGenerator;
        private readonly IHashingService _hashingService;
        private readonly IOpenTimestampsService _openTimestampsService;
        private readonly IOpenTimestampsProofReader _proofReader;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TempGenController(
            IContractPdfGenerator contractPdfGenerator,
            IHashingService hashingService,
            IOpenTimestampsService openTimestampsService,
            IOpenTimestampsProofReader proofReader,
            IWebHostEnvironment env,
            AppDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _contractPdfGenerator = contractPdfGenerator;
            _hashingService = hashingService;
            _openTimestampsService = openTimestampsService;
            _proofReader = proofReader;
            _env = env;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// [TEST ONLY] Generates sample contracts, creates PDFs, computes hashes, submits to OpenTimestamps, and saves results for seeding/testing purposes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var seedPath = Path.Combine(_env.ContentRootPath, "Data", "Seed", "Files");
            if (!Directory.Exists(seedPath)) Directory.CreateDirectory(seedPath);

            var contracts = new[]
            {
                new { Id = 1000001, PropertyId = 1001L, RenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 60000m, Start = new DateTime(2025, 1, 1), End = new DateTime(2026, 1, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                new { Id = 1000002, PropertyId = 1002L, RenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Amount = 90000m, Start = new DateTime(2025, 1, 1), End = new DateTime(2026, 1, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Quarterly },
                new { Id = 1000003, PropertyId = 1100L, RenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 96000m, Start = new DateTime(2024, 6, 1), End = new DateTime(2026, 6, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.OneTime },
                new { Id = 1000004, PropertyId = 1100L, RenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Amount = 480000m, Start = new DateTime(2025, 2, 1), End = new DateTime(2026, 2, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly },
                new { Id = 1000005, PropertyId = 1002L, RenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 90000m, Start = new DateTime(2024, 1, 1), End = new DateTime(2024, 12, 31), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Quarterly },
                new { Id = 1000006, PropertyId = 1004L, RenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Amount = 180000m, Start = new DateTime(2025, 5, 1), End = new DateTime(2026, 5, 1), Frequency = MARN_API.Enums.Payment.PaymentFrequency.Monthly }
            };

            var results = new List<string>();

            foreach (var c in contracts)
            {
                var property = await _dbContext.Properties
                    .Include(p => p.Media)
                    .Include(p => p.Amenities)
                    .Include(p => p.Rules)
                    .FirstOrDefaultAsync(p => p.Id == c.PropertyId);

                if (property == null)
                {
                    results.Add($"// Property {c.PropertyId} not found");
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
                try
                {
                    otsFileBytes = await _openTimestampsService.SubmitHashAsync(hash);
                }
                catch (Exception)
                {
                    // Fallback if OTS fails, just create a dummy OTS file
                    otsFileBytes = _openTimestampsService.BuildDetachedOtsFile(hash, new byte[] { 0x01, 0x02, 0x03 });
                }

                var proofData = _proofReader.Extract(otsFileBytes);

                var pdfPath = Path.Combine(seedPath, $"{c.Id}.pdf");
                var otsPath = Path.Combine(seedPath, $"{c.Id}.ots");

                await System.IO.File.WriteAllBytesAsync(pdfPath, pdfResult.Content);
                await System.IO.File.WriteAllBytesAsync(otsPath, otsFileBytes);

                var txId = proofData.TransactionIds.FirstOrDefault();
                var merkleRoot = proofData.MerkleRoots.FirstOrDefault();

                var dbContract = await _dbContext.Contracts.FirstOrDefaultAsync(co => co.Id == c.Id);
                if (dbContract != null)
                {
                    dbContract.FileName = pdfResult.FileName;
                    dbContract.Hash = hash;
                    dbContract.FileBytes = pdfResult.Content;
                    dbContract.OtsFileBytes = otsFileBytes;
                    dbContract.TransactionId = txId;
                    dbContract.MerkleRoot = merkleRoot;
                    _dbContext.Contracts.Update(dbContract);
                }

                results.Add($@"
                // For Contract {c.Id}
                FileName = ""{pdfResult.FileName}"",
                Hash = ""{hash}"",
                TransactionId = {(txId != null ? $"\"{txId}\"" : "null")},
                MerkleRoot = {(merkleRoot != null ? $"\"{merkleRoot}\"" : "null")},
                // Use File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, ""Data"", ""Seed"", ""Files"", ""{c.Id}.pdf""))
                // Use File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, ""Data"", ""Seed"", ""Files"", ""{c.Id}.ots""))
");
            }

            var outputStr = string.Join("\n", results);
            await System.IO.File.WriteAllTextAsync(Path.Combine(seedPath, "results.txt"), outputStr);

            await _dbContext.SaveChangesAsync();

            return Ok(results);
        }
    }
}