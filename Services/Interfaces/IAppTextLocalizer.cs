using System.Globalization;

namespace MARN_API.Services.Interfaces
{
    public interface IAppTextLocalizer
    {
        bool HasTranslation(string key, CultureInfo? culture = null);
        string Get(string key, CultureInfo? culture = null, params object?[] arguments);
        string GetOrFallback(string key, string fallback, CultureInfo? culture = null, params object?[] arguments);
        string LocalizeLiteral(string? message, CultureInfo? culture = null);
        string LocalizeMessage(string? code, string? fallbackMessage, CultureInfo? culture = null, object?[]? arguments = null);
        string GetEnumDisplayName<TEnum>(TEnum value, CultureInfo? culture = null) where TEnum : struct, Enum;
        string GetEnumDisplayName(Type enumType, object value, CultureInfo? culture = null);
    }
}
