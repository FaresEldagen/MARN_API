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
    #endregion
}
