using AutoMapper;
using MARN_API.DTOs;
using MARN_API.Models;

namespace MARN_API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<UpdateUserDto, ApplicationUser>();
        }
    }
}
