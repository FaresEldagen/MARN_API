using AutoMapper;
using MARN_API.DTOs.Property;
using MARN_API.Models;

namespace MARN_API.Mapping
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<AddPropertyDto, Property>()
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.Rules, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore());
        }
    }
}
