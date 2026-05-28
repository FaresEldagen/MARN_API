using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using MARN_API.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MARN_API.Services.Implementations
{
    public class CsvSeedImportService : ICsvSeedImportService
    {
        private static readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null,
            HeaderValidated = null
        };

        private static readonly Dictionary<string, string> ActivityTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [UserActivityTypes.View] = UserActivityTypes.View,
            [UserActivityTypes.Save] = UserActivityTypes.Save,
            [UserActivityTypes.Search] = UserActivityTypes.Search,
            [UserActivityTypes.Booking] = UserActivityTypes.Booking,
            [UserActivityTypes.Rent] = UserActivityTypes.Rent
        };

        private readonly AppDbContext _context;
        private readonly ILogger<CsvSeedImportService> _logger;

        public CsvSeedImportService(AppDbContext context, ILogger<CsvSeedImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<CsvSeedImportResultDto>> ImportPropertiesAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var fileValidation = ValidateFile(file);
            if (fileValidation != null)
            {
                return fileValidation;
            }

            List<PropertyCsvRow> rows;
            try
            {
                rows = await ReadRowsAsync<PropertyCsvRow>(file);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse property seed CSV file {FileName}", file.FileName);
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "The uploaded property CSV file could not be parsed.",
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_PROPERTY_PARSE_FAILED");
            }

            var validationErrors = new List<string>();
            var skippedMessages = new List<string>();
            var entities = new List<Property>();

            var ownerIds = rows
                .Select(row => row.OwnerId)
                .Where(value => Guid.TryParse(value, out _))
                .Select(Guid.Parse)
                .Distinct()
                .ToList();

            var existingOwnerIds = await _context.Users
                .IgnoreQueryFilters()
                .Where(user => ownerIds.Contains(user.Id))
                .Select(user => user.Id)
                .ToListAsync();

            var existingPropertyIds = (await _context.Properties
                .IgnoreQueryFilters()
                .Select(property => property.Id)
                .ToListAsync())
                .ToHashSet();

            var seenIds = new HashSet<long>();
            var ownerIdSet = existingOwnerIds.ToHashSet();

            for (var index = 0; index < rows.Count; index++)
            {
                var rowNumber = index + 2;
                var row = rows[index];

                if (!TryParseLong(row.Id, "Id", rowNumber, validationErrors, out var id) ||
                    !TryParseGuid(row.OwnerId, "OwnerId", rowNumber, validationErrors, out var ownerId) ||
                    !TryParseEnum(row.Type, "Type", rowNumber, validationErrors, out PropertyType propertyType) ||
                    !TryParseBool(row.IsShared, "IsShared", rowNumber, validationErrors, out var isShared) ||
                    !TryParseInt(row.MaxOccupants, "MaxOccupants", rowNumber, validationErrors, out var maxOccupants) ||
                    !TryParseInt(row.Bedrooms, "Bedrooms", rowNumber, validationErrors, out var bedrooms) ||
                    !TryParseInt(row.Beds, "Beds", rowNumber, validationErrors, out var beds) ||
                    !TryParseInt(row.Bathrooms, "Bathrooms", rowNumber, validationErrors, out var bathrooms) ||
                    !TryParseDouble(row.SquareMeters, "SquareMeters", rowNumber, validationErrors, out var squareMeters) ||
                    !TryParseInt(row.Views, "Views", rowNumber, validationErrors, out var views) ||
                    !TryParseDecimal(row.Price, "Price", rowNumber, validationErrors, out var price) ||
                    !TryParseEnum(row.RentalUnit, "RentalUnit", rowNumber, validationErrors, out RentalUnit rentalUnit) ||
                    !TryParseDouble(row.Latitude, "Latitude", rowNumber, validationErrors, out var latitude) ||
                    !TryParseDouble(row.Longitude, "Longitude", rowNumber, validationErrors, out var longitude) ||
                    !TryParseBool(row.IsActive, "IsActive", rowNumber, validationErrors, out var isActive) ||
                    !TryParseEnum(row.Status, "Status", rowNumber, validationErrors, out PropertyStatus status) ||
                    !TryParseDateTime(row.CreatedAt, "CreatedAt", rowNumber, validationErrors, out var createdAt))
                {
                    continue;
                }

                if (!Enum.TryParse<Governorate>(row.State, ignoreCase: true, out _))
                {
                    validationErrors.Add($"Row {rowNumber}: State '{row.State}' is not a valid governorate enum name.");
                    continue;
                }

                if (!ownerIdSet.Contains(ownerId))
                {
                    validationErrors.Add($"Row {rowNumber}: OwnerId '{ownerId}' does not exist.");
                    continue;
                }

                if (!seenIds.Add(id))
                {
                    skippedMessages.Add($"Row {rowNumber}: Property Id '{id}' is duplicated inside the CSV and was skipped.");
                    continue;
                }

                if (existingPropertyIds.Contains(id))
                {
                    skippedMessages.Add($"Row {rowNumber}: Property Id '{id}' already exists and was skipped.");
                    continue;
                }

                entities.Add(new Property
                {
                    Id = id,
                    OwnerId = ownerId,
                    Title = row.Title?.Trim() ?? string.Empty,
                    Description = row.Description?.Trim() ?? string.Empty,
                    Type = propertyType,
                    ProofOfOwnership = row.ProofOfOwnership?.Trim() ?? string.Empty,
                    MaxOccupants = maxOccupants,
                    IsShared = isShared,
                    Bedrooms = bedrooms,
                    Beds = beds,
                    Bathrooms = bathrooms,
                    SquareMeters = squareMeters,
                    Views = views,
                    Price = price,
                    RentalUnit = rentalUnit,
                    Address = row.Address?.Trim() ?? string.Empty,
                    City = row.City?.Trim() ?? string.Empty,
                    State = row.State?.Trim() ?? string.Empty,
                    ZipCode = row.ZipCode?.Trim() ?? string.Empty,
                    Latitude = latitude,
                    Longitude = longitude,
                    IsActive = isActive,
                    Status = status,
                    CreatedAt = createdAt
                });
            }

            if (validationErrors.Count > 0)
            {
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "Property CSV validation failed.",
                    errors: validationErrors,
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_PROPERTY_VALIDATION_FAILED");
            }

            if (entities.Count > 0)
            {
                try
                {
                    await SaveWithIdentityInsertAsync(entities);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save imported property CSV rows.");
                    return ServiceResult<CsvSeedImportResultDto>.Fail(
                        "Property CSV could not be saved to the database.",
                        resultType: ServiceResultType.InternalError,
                        code: "CSV_PROPERTY_SAVE_FAILED");
                }
            }

            return ServiceResult<CsvSeedImportResultDto>.Ok(
                BuildResult("properties", rows.Count, entities.Count, skippedMessages),
                "Property CSV seeded successfully.",
                code: "ZZ_CSV_PROPERTY_IMPORT_SUCCESS");
        }

        public async Task<ServiceResult<CsvSeedImportResultDto>> ImportUsersAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var fileValidation = ValidateFile(file);
            if (fileValidation != null)
            {
                return fileValidation;
            }

            List<UserCsvRow> rows;
            try
            {
                rows = await ReadRowsAsync<UserCsvRow>(file);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse user seed CSV file {FileName}", file.FileName);
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "The uploaded user CSV file could not be parsed.",
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_USER_PARSE_FAILED");
            }

            var validationErrors = new List<string>();
            var skippedMessages = new List<string>();
            var entities = new List<ApplicationUser>();

            var existingUsers = await _context.Users
                .IgnoreQueryFilters()
                .Select(user => new { user.Id, user.NormalizedEmail, user.NormalizedUserName })
                .ToListAsync();

            var existingIds = existingUsers.Select(user => user.Id).ToHashSet();
            var existingEmails = existingUsers
                .Where(user => !string.IsNullOrWhiteSpace(user.NormalizedEmail))
                .Select(user => user.NormalizedEmail!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var existingUserNames = existingUsers
                .Where(user => !string.IsNullOrWhiteSpace(user.NormalizedUserName))
                .Select(user => user.NormalizedUserName!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var seenIds = new HashSet<Guid>();
            var seenEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenUserNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (var index = 0; index < rows.Count; index++)
            {
                var rowNumber = index + 2;
                var row = rows[index];

                if (!TryParseGuid(row.Id, "Id", rowNumber, validationErrors, out var id) ||
                    !TryParseBool(row.EmailConfirmed, "EmailConfirmed", rowNumber, validationErrors, out var emailConfirmed) ||
                    !TryParseBool(row.PhoneNumberConfirmed, "PhoneNumberConfirmed", rowNumber, validationErrors, out var phoneNumberConfirmed) ||
                    !TryParseBool(row.TwoFactorEnabled, "TwoFactorEnabled", rowNumber, validationErrors, out var twoFactorEnabled) ||
                    !TryParseBool(row.LockoutEnabled, "LockoutEnabled", rowNumber, validationErrors, out var lockoutEnabled) ||
                    !TryParseInt(row.AccessFailedCount, "AccessFailedCount", rowNumber, validationErrors, out var accessFailedCount) ||
                    !TryParseEnum(row.Language, "Language", rowNumber, validationErrors, out Language language) ||
                    !TryParseEnum(row.Gender, "Gender", rowNumber, validationErrors, out Gender gender) ||
                    !TryParseEnum(row.Country, "Country", rowNumber, validationErrors, out Country country) ||
                    !TryParseEnum(row.AccountStatus, "AccountStatus", rowNumber, validationErrors, out AccountStatus accountStatus) ||
                    !TryParseDateTime(row.CreatedAt, "CreatedAt", rowNumber, validationErrors, out var createdAt))
                {
                    continue;
                }

                var normalizedEmail = NormalizeRequired(row.NormalizedEmail, "NormalizedEmail", rowNumber, validationErrors);
                var normalizedUserName = NormalizeRequired(row.NormalizedUserName, "NormalizedUserName", rowNumber, validationErrors);
                var email = NormalizeRequired(row.Email, "Email", rowNumber, validationErrors);
                var userName = NormalizeRequired(row.UserName, "UserName", rowNumber, validationErrors);
                var passwordHash = NormalizeRequired(row.PasswordHash, "PasswordHash", rowNumber, validationErrors);
                var securityStamp = NormalizeRequired(row.SecurityStamp, "SecurityStamp", rowNumber, validationErrors);
                var concurrencyStamp = NormalizeRequired(row.ConcurrencyStamp, "ConcurrencyStamp", rowNumber, validationErrors);
                var firstName = NormalizeRequired(row.FirstName, "FirstName", rowNumber, validationErrors);
                var lastName = NormalizeRequired(row.LastName, "LastName", rowNumber, validationErrors);

                if (string.IsNullOrWhiteSpace(normalizedEmail) ||
                    string.IsNullOrWhiteSpace(normalizedUserName) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(userName) ||
                    string.IsNullOrWhiteSpace(passwordHash) ||
                    string.IsNullOrWhiteSpace(securityStamp) ||
                    string.IsNullOrWhiteSpace(concurrencyStamp) ||
                    string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(lastName))
                {
                    continue;
                }

                if (!seenIds.Add(id))
                {
                    skippedMessages.Add($"Row {rowNumber}: User Id '{id}' is duplicated inside the CSV and was skipped.");
                    continue;
                }

                if (!seenEmails.Add(normalizedEmail))
                {
                    skippedMessages.Add($"Row {rowNumber}: NormalizedEmail '{normalizedEmail}' is duplicated inside the CSV and was skipped.");
                    continue;
                }

                if (!seenUserNames.Add(normalizedUserName))
                {
                    skippedMessages.Add($"Row {rowNumber}: NormalizedUserName '{normalizedUserName}' is duplicated inside the CSV and was skipped.");
                    continue;
                }

                if (existingIds.Contains(id))
                {
                    skippedMessages.Add($"Row {rowNumber}: User Id '{id}' already exists and was skipped.");
                    continue;
                }

                if (existingEmails.Contains(normalizedEmail))
                {
                    skippedMessages.Add($"Row {rowNumber}: NormalizedEmail '{normalizedEmail}' already exists and was skipped.");
                    continue;
                }

                if (existingUserNames.Contains(normalizedUserName))
                {
                    skippedMessages.Add($"Row {rowNumber}: NormalizedUserName '{normalizedUserName}' already exists and was skipped.");
                    continue;
                }

                entities.Add(new ApplicationUser
                {
                    Id = id,
                    UserName = userName,
                    NormalizedUserName = normalizedUserName,
                    Email = email,
                    NormalizedEmail = normalizedEmail,
                    PasswordHash = passwordHash,
                    EmailConfirmed = emailConfirmed,
                    PhoneNumberConfirmed = phoneNumberConfirmed,
                    TwoFactorEnabled = twoFactorEnabled,
                    LockoutEnabled = lockoutEnabled,
                    AccessFailedCount = accessFailedCount,
                    SecurityStamp = securityStamp,
                    ConcurrencyStamp = concurrencyStamp,
                    FirstName = firstName,
                    LastName = lastName,
                    ArabicFullName = NullIfWhiteSpace(row.ArabicFullName),
                    ArabicAddress = NullIfWhiteSpace(row.ArabicAddress),
                    NationalIDNumber = NullIfWhiteSpace(row.NationalIDNumber),
                    FrontIdPhoto = NullIfWhiteSpace(row.FrontIdPhoto),
                    BackIdPhoto = NullIfWhiteSpace(row.BackIdPhoto),
                    Language = language,
                    Gender = gender,
                    Country = country,
                    AccountStatus = accountStatus,
                    ProfileImage = NullIfWhiteSpace(row.ProfileImage),
                    CreatedAt = createdAt
                });
            }

            if (validationErrors.Count > 0)
            {
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "User CSV validation failed.",
                    errors: validationErrors,
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_USER_VALIDATION_FAILED");
            }

            if (entities.Count > 0)
            {
                try
                {
                    await _context.Users.AddRangeAsync(entities);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save imported user CSV rows.");
                    return ServiceResult<CsvSeedImportResultDto>.Fail(
                        "User CSV could not be saved to the database.",
                        resultType: ServiceResultType.InternalError,
                        code: "CSV_USER_SAVE_FAILED");
                }
            }

            return ServiceResult<CsvSeedImportResultDto>.Ok(
                BuildResult("users", rows.Count, entities.Count, skippedMessages),
                "User CSV seeded successfully.",
                code: "ZZ_CSV_USER_IMPORT_SUCCESS");
        }

        public async Task<ServiceResult<CsvSeedImportResultDto>> ImportUserActivitiesAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var fileValidation = ValidateFile(file);
            if (fileValidation != null)
            {
                return fileValidation;
            }

            List<UserActivityCsvRow> rows;
            try
            {
                rows = await ReadRowsAsync<UserActivityCsvRow>(file);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse user activity seed CSV file {FileName}", file.FileName);
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "The uploaded user activity CSV file could not be parsed.",
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_USER_ACTIVITY_PARSE_FAILED");
            }

            var validationErrors = new List<string>();
            var skippedMessages = new List<string>();
            var entities = new List<UserActivity>();

            var userIds = rows
                .Select(row => row.UserId)
                .Where(value => Guid.TryParse(value, out _))
                .Select(Guid.Parse)
                .Distinct()
                .ToList();

            var propertyIds = rows
                .Select(row => row.PropertyId)
                .Where(value => long.TryParse(value, out _))
                .Select(long.Parse)
                .Distinct()
                .ToList();

            var existingUserIds = (await _context.Users
                .IgnoreQueryFilters()
                .Where(user => userIds.Contains(user.Id))
                .Select(user => user.Id)
                .ToListAsync())
                .ToHashSet();

            var existingPropertyIds = (await _context.Properties
                .IgnoreQueryFilters()
                .Where(property => propertyIds.Contains(property.Id))
                .Select(property => property.Id)
                .ToListAsync())
                .ToHashSet();

            var existingActivityIds = (await _context.UserActivities
                .Select(activity => activity.Id)
                .ToListAsync())
                .ToHashSet();

            var seenIds = new HashSet<long>();

            for (var index = 0; index < rows.Count; index++)
            {
                var rowNumber = index + 2;
                var row = rows[index];

                if (!TryParseLong(row.Id, "Id", rowNumber, validationErrors, out var id) ||
                    !TryParseGuid(row.UserId, "UserId", rowNumber, validationErrors, out var userId) ||
                    !TryParseDateTime(row.CreatedAt, "CreatedAt", rowNumber, validationErrors, out var createdAt))
                {
                    continue;
                }

                var propertyId = ParseNullableLong(row.PropertyId, "PropertyId", rowNumber, validationErrors);
                if (propertyId == long.MinValue)
                {
                    continue;
                }

                var activityType = NormalizeActivityType(row.UserActivityType, rowNumber, validationErrors);
                if (activityType == null)
                {
                    continue;
                }

                var metadata = NullIfWhiteSpace(row.Metadata);
                if (metadata != null && !IsValidJson(metadata))
                {
                    validationErrors.Add($"Row {rowNumber}: Metadata is not valid JSON.");
                    continue;
                }

                if (!existingUserIds.Contains(userId))
                {
                    validationErrors.Add($"Row {rowNumber}: UserId '{userId}' does not exist.");
                    continue;
                }

                if (propertyId.HasValue && !existingPropertyIds.Contains(propertyId.Value))
                {
                    validationErrors.Add($"Row {rowNumber}: PropertyId '{propertyId.Value}' does not exist.");
                    continue;
                }

                if (activityType == UserActivityTypes.Search)
                {
                    if (propertyId.HasValue)
                    {
                        validationErrors.Add($"Row {rowNumber}: Search activities must not include PropertyId.");
                        continue;
                    }

                    if (metadata == null)
                    {
                        validationErrors.Add($"Row {rowNumber}: Search activities must include Metadata.");
                        continue;
                    }
                }
                else
                {
                    if (!propertyId.HasValue)
                    {
                        validationErrors.Add($"Row {rowNumber}: Activity type '{activityType}' must include PropertyId.");
                        continue;
                    }

                    if (metadata != null)
                    {
                        validationErrors.Add($"Row {rowNumber}: Activity type '{activityType}' must not include Metadata because the current code records it as null.");
                        continue;
                    }
                }

                if (!seenIds.Add(id))
                {
                    skippedMessages.Add($"Row {rowNumber}: UserActivity Id '{id}' is duplicated inside the CSV and was skipped.");
                    continue;
                }

                if (existingActivityIds.Contains(id))
                {
                    skippedMessages.Add($"Row {rowNumber}: UserActivity Id '{id}' already exists and was skipped.");
                    continue;
                }

                entities.Add(new UserActivity
                {
                    Id = id,
                    UserId = userId,
                    PropertyId = propertyId,
                    UserActivityType = activityType,
                    Metadata = metadata,
                    CreatedAt = createdAt
                });
            }

            if (validationErrors.Count > 0)
            {
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "User activity CSV validation failed.",
                    errors: validationErrors,
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_USER_ACTIVITY_VALIDATION_FAILED");
            }

            if (entities.Count > 0)
            {
                try
                {
                    await SaveWithIdentityInsertAsync(entities);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save imported user activity CSV rows.");
                    return ServiceResult<CsvSeedImportResultDto>.Fail(
                        "User activity CSV could not be saved to the database.",
                        resultType: ServiceResultType.InternalError,
                        code: "CSV_USER_ACTIVITY_SAVE_FAILED");
                }
            }

            return ServiceResult<CsvSeedImportResultDto>.Ok(
                BuildResult("user-activities", rows.Count, entities.Count, skippedMessages),
                "User activity CSV seeded successfully.",
                code: "ZZ_CSV_USER_ACTIVITY_IMPORT_SUCCESS");
        }

        private static ServiceResult<CsvSeedImportResultDto>? ValidateFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "A non-empty CSV file is required.",
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_FILE_REQUIRED");
            }

            if (!string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<CsvSeedImportResultDto>.Fail(
                    "Only .csv files are supported.",
                    resultType: ServiceResultType.BadRequest,
                    code: "CSV_FILE_TYPE_INVALID");
            }

            return null;
        }

        private async Task SaveWithIdentityInsertAsync<TEntity>(List<TEntity> entities)
            where TEntity : class
        {
            if (entities.Count == 0)
            {
                return;
            }

            var entityType = _context.Model.FindEntityType(typeof(TEntity))
                ?? throw new InvalidOperationException($"Entity metadata not found for {typeof(TEntity).Name}.");

            var identityProperty = entityType.FindPrimaryKey()?.Properties
                .FirstOrDefault(property => property.ValueGenerated == ValueGenerated.OnAdd);

            if (identityProperty == null)
            {
                await _context.Set<TEntity>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                return;
            }

            var qualifiedTableName = BuildQualifiedTableName(entityType);
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var identityInsertEnabled = false;

                try
                {
                    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {qualifiedTableName} ON");
                    identityInsertEnabled = true;

                    await _context.Set<TEntity>().AddRangeAsync(entities);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {qualifiedTableName} OFF");
                    identityInsertEnabled = false;

                    await transaction.CommitAsync();
                }
                catch
                {
                    _context.ChangeTracker.Clear();

                    if (identityInsertEnabled)
                    {
                        try
                        {
                            await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {qualifiedTableName} OFF");
                        }
                        catch (Exception identityOffException)
                        {
                            _logger.LogWarning(identityOffException, "Failed to disable IDENTITY_INSERT for {TableName}", qualifiedTableName);
                        }
                    }

                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        private static string BuildQualifiedTableName(IEntityType entityType)
        {
            var tableName = entityType.GetTableName()
                ?? throw new InvalidOperationException($"Table name not found for {entityType.Name}.");
            var schema = entityType.GetSchema() ?? "dbo";

            return $"[{schema}].[{tableName}]";
        }

        private static async Task<List<TRow>> ReadRowsAsync<TRow>(Microsoft.AspNetCore.Http.IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CsvConfiguration);

            return csv.GetRecords<TRow>().ToList();
        }

        private static CsvSeedImportResultDto BuildResult(string seedType, int totalRows, int importedRows, List<string> skippedMessages)
        {
            return new CsvSeedImportResultDto
            {
                SeedType = seedType,
                TotalRows = totalRows,
                ImportedRows = importedRows,
                SkippedRows = skippedMessages.Count,
                Messages = skippedMessages
            };
        }

        private static bool TryParseGuid(string? value, string fieldName, int rowNumber, List<string> errors, out Guid parsedValue)
        {
            if (Guid.TryParse(value, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid GUID.");
            return false;
        }

        private static bool TryParseLong(string? value, string fieldName, int rowNumber, List<string> errors, out long parsedValue)
        {
            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid long.");
            return false;
        }

        private static bool TryParseInt(string? value, string fieldName, int rowNumber, List<string> errors, out int parsedValue)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid int.");
            return false;
        }

        private static bool TryParseDouble(string? value, string fieldName, int rowNumber, List<string> errors, out double parsedValue)
        {
            if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid double.");
            return false;
        }

        private static bool TryParseDecimal(string? value, string fieldName, int rowNumber, List<string> errors, out decimal parsedValue)
        {
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid decimal.");
            return false;
        }

        private static bool TryParseBool(string? value, string fieldName, int rowNumber, List<string> errors, out bool parsedValue)
        {
            if (bool.TryParse(value, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid boolean.");
            return false;
        }

        private static bool TryParseDateTime(string? value, string fieldName, int rowNumber, List<string> errors, out DateTime parsedValue)
        {
            if (DateTime.TryParse(
                    value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid UTC date/time.");
            return false;
        }

        private static bool TryParseEnum<TEnum>(string? value, string fieldName, int rowNumber, List<string> errors, out TEnum parsedValue)
            where TEnum : struct, Enum
        {
            if (Enum.TryParse<TEnum>(value, ignoreCase: true, out parsedValue))
            {
                return true;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid {typeof(TEnum).Name}.");
            return false;
        }

        private static long? ParseNullableLong(string? value, string fieldName, int rowNumber, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
            {
                return parsedValue;
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' with value '{value}' is not a valid long.");
            return long.MinValue;
        }

        private static string? NormalizeActivityType(string? value, int rowNumber, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"Row {rowNumber}: UserActivityType is required.");
                return null;
            }

            if (ActivityTypes.TryGetValue(value.Trim(), out var normalized))
            {
                return normalized;
            }

            errors.Add($"Row {rowNumber}: UserActivityType '{value}' is not supported.");
            return null;
        }

        private static string NormalizeRequired(string? value, string fieldName, int rowNumber, List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            errors.Add($"Row {rowNumber}: Field '{fieldName}' is required.");
            return string.Empty;
        }

        private static string? NullIfWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool IsValidJson(string value)
        {
            try
            {
                using var _ = JsonDocument.Parse(value);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private sealed class PropertyCsvRow
        {
            public string Id { get; set; } = string.Empty;
            public string OwnerId { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string IsShared { get; set; } = string.Empty;
            public string MaxOccupants { get; set; } = string.Empty;
            public string Bedrooms { get; set; } = string.Empty;
            public string Beds { get; set; } = string.Empty;
            public string Bathrooms { get; set; } = string.Empty;
            public string SquareMeters { get; set; } = string.Empty;
            public string Views { get; set; } = string.Empty;
            public string Price { get; set; } = string.Empty;
            public string RentalUnit { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string State { get; set; } = string.Empty;
            public string ZipCode { get; set; } = string.Empty;
            public string Latitude { get; set; } = string.Empty;
            public string Longitude { get; set; } = string.Empty;
            public string IsActive { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string CreatedAt { get; set; } = string.Empty;
            public string ProofOfOwnership { get; set; } = string.Empty;
        }

        private sealed class UserCsvRow
        {
            public string Id { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string NormalizedUserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string NormalizedEmail { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public string EmailConfirmed { get; set; } = string.Empty;
            public string PhoneNumberConfirmed { get; set; } = string.Empty;
            public string TwoFactorEnabled { get; set; } = string.Empty;
            public string LockoutEnabled { get; set; } = string.Empty;
            public string AccessFailedCount { get; set; } = string.Empty;
            public string SecurityStamp { get; set; } = string.Empty;
            public string ConcurrencyStamp { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string ArabicFullName { get; set; } = string.Empty;
            public string ArabicAddress { get; set; } = string.Empty;
            public string NationalIDNumber { get; set; } = string.Empty;
            public string FrontIdPhoto { get; set; } = string.Empty;
            public string BackIdPhoto { get; set; } = string.Empty;
            public string Language { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public string AccountStatus { get; set; } = string.Empty;
            public string ProfileImage { get; set; } = string.Empty;
            public string CreatedAt { get; set; } = string.Empty;
        }

        private sealed class UserActivityCsvRow
        {
            public string Id { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string PropertyId { get; set; } = string.Empty;
            public string UserActivityType { get; set; } = string.Empty;
            public string Metadata { get; set; } = string.Empty;
            public string CreatedAt { get; set; } = string.Empty;
        }
    }
}
