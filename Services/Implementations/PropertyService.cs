using AutoMapper;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MARN_API.Services.Implementations
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepo _propertyRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PropertyService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IPropertyAmenityRepo _amenityRepo;
        private readonly IPropertyMediaRepo _mediaRepo;
        private readonly IPropertyRuleRepo _ruleRepo;

        public PropertyService(
            IPropertyRepo propertyRepo, 
            UserManager<ApplicationUser> userManager,
            ILogger<PropertyService> logger,
            IMapper mapper,
            IFileService fileService,
            IPropertyAmenityRepo amenityRepo,
            IPropertyMediaRepo mediaRepo,
            IPropertyRuleRepo ruleRepo)
        {
            _propertyRepo = propertyRepo;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
            _amenityRepo = amenityRepo;
            _mediaRepo = mediaRepo;
            _ruleRepo = ruleRepo;
        }

        public async Task<ServiceResult<bool>> AddPropertyAsync(AddPropertyDto dto, Guid userId)
        {
            _logger.LogInformation("AddProperty attempt for userId: {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("AddProperty failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("User not found", resultType: ServiceResultType.Unauthorized);
            }

            if (user.AccountStatus != AccountStatus.Verified)
            {
                _logger.LogWarning("AddProperty failed: Account not verified for user {UserId}", userId);
                return ServiceResult<bool>.Fail("Your account must be verified to add a property.", resultType: ServiceResultType.Unauthorized);
            }

            var property = _mapper.Map<Property>(dto);
            property.OwnerId = userId;

            if (!await _userManager.IsInRoleAsync(user, "Owner"))
            {
                await _userManager.AddToRoleAsync(user, "Owner");
                _logger.LogInformation("Assigned 'Owner' role to user {UserId} smoothly.", userId);
            }

            await _propertyRepo.AddPropertyAsync(property);
            _logger.LogInformation("Added Property {PropertyId} for user {UserId}", property.Id, userId);

            if (dto.Amenities != null)
            {
                foreach (var am in dto.Amenities)
                {
                    await _amenityRepo.AddByPropertyIdAsync(property.Id, new PropertyAmenity { Amenity = am });
                }
            }

            if (dto.Rules != null)
            {
                foreach (var rule in dto.Rules)
                {
                    await _ruleRepo.AddByPropertyIdAsync(property.Id, new PropertyRule { Rule = rule });
                }
            }

            if (dto.MediaFiles != null)
            {
                bool isPrimarySet = false;
                foreach (var file in dto.MediaFiles)
                {
                    var path = await _fileService.SaveImageAsync(file, "properties");
                    if (path != null)
                    {
                        await _mediaRepo.AddByPropertyIdAsync(property.Id, new PropertyMedia { Path = path, IsPrimary = !isPrimarySet });
                        isPrimarySet = true;
                    }
                }
            }

            _logger.LogInformation("Successfully fully mapped and saved property {PropertyId}", property.Id);
            return ServiceResult<bool>.Ok(true, "Property added successfully.");
        }
    }
}
