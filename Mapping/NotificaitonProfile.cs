using AutoMapper;
using MARN_API.DTOs.Notification;
using MARN_API.Models;
using System.Text.Json;

namespace MARN_API.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationCardDto>()
                .ForMember(dest => dest.IsRead,
                    opt => opt.MapFrom(src => src.ReadAt.HasValue))
                .ForMember(dest => dest.Data,
                    opt => opt.ConvertUsing(new JsonToDictionaryConverter(), src => src.Data));

            CreateMap<NotificationRequestDto, Notification>()
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => Guid.Parse(src.UserId)))
                .ForMember(dest => dest.Data,
                    opt => opt.MapFrom<JsonSerializerResolver>());
        }
    }

    #region Resolvers
    public class JsonToDictionaryConverter : IValueConverter<string?, Dictionary<string, string>?>
    {
        public Dictionary<string, string>? Convert(string? sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(sourceMember))
                return null;

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, string>>(sourceMember);
            }
            catch
            {
                return null;
            }
        }
    }

    public class JsonSerializerResolver : IValueResolver<NotificationRequestDto, Notification, string?>
    {
        public string? Resolve(NotificationRequestDto src, Notification dest, string? destMember, ResolutionContext context)
        {
            if (src.Data == null)
                return null;

            return JsonSerializer.Serialize(src.Data);
        }
    }
    #endregion
}
