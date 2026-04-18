using AutoMapper;
using MARN_API.DTOs.Auth;
using MARN_API.DTOs.Profile;
using MARN_API.Models;
using Microsoft.Extensions.Configuration;

namespace MARN_API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region To Register
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            #endregion


            #region Get Profile Data
            CreateMap<ApplicationUser, ProfileDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.MemberSince,
                    opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ProfileImage,
                    opt => opt.MapFrom<ProfileImageResolver>());

            CreateMap<RoommatePreference, ProfileDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion


            #region Get Profile Settings Data
            CreateMap<ApplicationUser, ProfileSettingsDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom<ProfileSettingsImageResolver>())
                .ForMember(dest => dest.FrontIdPhoto, opt => opt.MapFrom<FrontIdPhotoResolver>())
                .ForMember(dest => dest.BackIdPhoto, opt => opt.MapFrom<BackIdPhotoResolver>());

            CreateMap<RoommatePreference, ProfileSettingsDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion


            #region Update Profile Settings
            CreateMap<UpdateProfileDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            CreateMap<UpdateLegalDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FrontIdPhoto, opt => opt.Ignore())
                .ForMember(dest => dest.BackIdPhoto, opt => opt.Ignore());

            CreateMap<UpdateRoommatePreferencesDto, RoommatePreference>();
            #endregion
        }
    }

    #region Resolvers
    public class ProfileImageResolver : IValueResolver<ApplicationUser, ProfileDto, string?>
    {
        private readonly IConfiguration _configuration;
        public ProfileImageResolver(IConfiguration configuration) => _configuration = configuration;
        public string? Resolve(ApplicationUser source, ProfileDto destination, string? destMember, ResolutionContext context) 
            => ResolverHelper.ResolveImageUrl(_configuration, source.ProfileImage);
    }

    public class ProfileSettingsImageResolver : IValueResolver<ApplicationUser, ProfileSettingsDto, string?>
    {
        private readonly IConfiguration _configuration;
        public ProfileSettingsImageResolver(IConfiguration configuration) => _configuration = configuration;
        public string? Resolve(ApplicationUser source, ProfileSettingsDto destination, string? destMember, ResolutionContext context) 
            => ResolverHelper.ResolveImageUrl(_configuration, source.ProfileImage);
    }

    public class FrontIdPhotoResolver : IValueResolver<ApplicationUser, ProfileSettingsDto, string?>
    {
        private readonly IConfiguration _configuration;
        public FrontIdPhotoResolver(IConfiguration configuration) => _configuration = configuration;
        public string? Resolve(ApplicationUser source, ProfileSettingsDto destination, string? destMember, ResolutionContext context) 
            => ResolverHelper.ResolveImageUrl(_configuration, source.FrontIdPhoto);
    }

    public class BackIdPhotoResolver : IValueResolver<ApplicationUser, ProfileSettingsDto, string?>
    {
        private readonly IConfiguration _configuration;
        public BackIdPhotoResolver(IConfiguration configuration) => _configuration = configuration;
        public string? Resolve(ApplicationUser source, ProfileSettingsDto destination, string? destMember, ResolutionContext context) 
            => ResolverHelper.ResolveImageUrl(_configuration, source.BackIdPhoto);
    }

    public static class ResolverHelper
    {
        public static string? ResolveImageUrl(IConfiguration configuration, string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;
            if (imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return imagePath;

            var baseUrl = configuration["AppSettings:BaseUrl"];
            return $"{baseUrl}{imagePath}";
        }
    }
    #endregion
}
