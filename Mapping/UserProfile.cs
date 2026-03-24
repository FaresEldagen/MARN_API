using AutoMapper;
using MARN_API.DTOs.Auth;
using MARN_API.DTOs.Profile;
using MARN_API.Models;

namespace MARN_API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // To Register
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            // Get Profile Data
            CreateMap<RoommatePreference, ProfileDto>();
            CreateMap<ApplicationUser, ProfileDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.MemberSince,
                    opt => opt.MapFrom(src => src.CreatedAt));

            // Get Profile Settings Data
            CreateMap<RoommatePreference, ProfileSettingsDto>();
            CreateMap<ApplicationUser, ProfileSettingsDto>();

            // Update Profile Settings
            CreateMap<UpdateProfileDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());
            CreateMap<UpdateLegalDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FrontIdPhoto, opt => opt.Ignore())
                .ForMember(dest => dest.BackIdPhoto, opt => opt.Ignore());
            CreateMap<UpdateRoommatePreferencesDto, RoommatePreference>();

            // 

        }
    }
}
