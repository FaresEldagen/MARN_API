using System.Globalization;
using MARN_API;
using MARN_API.Localization;
using MARN_API.Services.Interfaces;
using Microsoft.Extensions.Localization;

namespace MARN_API.Services.Implementations
{
    public class AppTextLocalizer : IAppTextLocalizer
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AppTextLocalizer(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public bool HasTranslation(string key, CultureInfo? culture = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            using var scope = CreateCultureScope(culture);
            return !_localizer[key].ResourceNotFound;
        }

        public string Get(string key, CultureInfo? culture = null, params object?[] arguments)
        {
            using var scope = CreateCultureScope(culture);
            var nonNullableArguments = arguments.Select(argument => argument ?? string.Empty).ToArray();
            var localized = arguments.Length == 0 ? _localizer[key] : _localizer[key, nonNullableArguments];
            return localized.ResourceNotFound
                ? FormatForCulture(key, culture)
                : FormatForCulture(localized.Value, culture);
        }

        public string GetOrFallback(string key, string fallback, CultureInfo? culture = null, params object?[] arguments)
        {
            if (HasTranslation(key, culture))
            {
                return Get(key, culture, arguments);
            }

            return FormatForCulture(FormatFallback(fallback, culture, arguments), culture);
        }

        public string LocalizeLiteral(string? message, CultureInfo? culture = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return string.Empty;
            }

            var key = $"TEXT_{LocalizationKeyBuilder.BuildLiteralKey(message)}";
            return HasTranslation(key, culture) ? Get(key, culture) : FormatForCulture(message, culture);
        }

        public string LocalizeMessage(string? code, string? fallbackMessage, CultureInfo? culture = null, object?[]? arguments = null)
        {
            arguments ??= Array.Empty<object?>();

            if (!string.IsNullOrWhiteSpace(code) && HasTranslation(code, culture))
            {
                return Get(code, culture, arguments);
            }

            if (!string.IsNullOrWhiteSpace(fallbackMessage))
            {
                var literalTranslation = LocalizeLiteral(fallbackMessage, culture);
                if (!string.Equals(literalTranslation, fallbackMessage, StringComparison.Ordinal))
                {
                    return FormatForCulture(FormatFallback(literalTranslation, culture, arguments), culture);
                }

                return FormatForCulture(FormatFallback(fallbackMessage, culture, arguments), culture);
            }

            if (!string.IsNullOrWhiteSpace(code))
            {
                return FormatForCulture(code, culture);
            }

            return string.Empty;
        }

        public string GetEnumDisplayName<TEnum>(TEnum value, CultureInfo? culture = null) where TEnum : struct, Enum
        {
            var key = $"ENUM_{typeof(TEnum).Name}_{value}";
            return HasTranslation(key, culture) ? Get(key, culture) : FormatForCulture(value.ToString(), culture);
        }

        public string GetEnumDisplayName(Type enumType, object value, CultureInfo? culture = null)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be an enum.", nameof(enumType));
            }

            var key = $"ENUM_{enumType.Name}_{value}";
            return HasTranslation(key, culture)
                ? Get(key, culture)
                : FormatForCulture(value.ToString() ?? string.Empty, culture);
        }

        private static string FormatFallback(string fallback, CultureInfo? culture, object?[] arguments)
        {
            if (arguments.Length == 0)
            {
                return fallback;
            }

            return string.Format(culture ?? CultureInfo.CurrentUICulture, fallback, arguments);
        }

        private static string FormatForCulture(string text, CultureInfo? culture)
        {
            return BidiText.Format(text, culture);
        }

        private static CultureScope? CreateCultureScope(CultureInfo? culture)
        {
            return culture == null ? null : new CultureScope(culture);
        }
    }
}
