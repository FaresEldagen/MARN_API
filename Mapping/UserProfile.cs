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
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<UpdateProfileDto, ApplicationUser>();
        }
    }
}
