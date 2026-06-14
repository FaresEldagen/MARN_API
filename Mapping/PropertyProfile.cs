using AutoMapper;
using MARN_API.DTOs.Property;
using MARN_API.Enums.Property;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Mapping
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<AddPropertyDto, Property>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Governorate.ToString()))
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.Rules, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.ProofOfOwnership, opt => opt.Ignore());

            CreateMap<EditPropertyDto, Property>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Governorate.ToString()))
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.Rules, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.ProofOfOwnership, opt => opt.Ignore());

            CreateMap<Property, PropertyEditDataDto>()
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.Rules, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ProofOfOwnershipUrl, opt => opt.Ignore());

            CreateMap<PropertyAmenity, PropertyAmenityDto>();
            CreateMap<PropertyRule, PropertyRuleDto>();
            CreateMap<PropertyMedia, PropertyMediaDto>();

            CreateMap<PropertyFeedback, PropertyFeedbackDto>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}".Trim()))
                .ForMember(dest => dest.UserProfileImage, opt => opt.MapFrom(src => src.User.ProfileImage));
        }
    }
}
