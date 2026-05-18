using System.Globalization;
using System.Text;
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
            return localized.ResourceNotFound ? key : localized.Value;
        }

        public string GetOrFallback(string key, string fallback, CultureInfo? culture = null, params object?[] arguments)
        {
            if (HasTranslation(key, culture))
            {
                return Get(key, culture, arguments);
            }

            return FormatFallback(fallback, culture, arguments);
        }

        public string LocalizeLiteral(string? message, CultureInfo? culture = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return string.Empty;
            }

            var key = $"TEXT_{BuildLiteralKey(message)}";
            return HasTranslation(key, culture) ? Get(key, culture) : message;
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
                    return FormatFallback(literalTranslation, culture, arguments);
                }

                return FormatFallback(fallbackMessage, culture, arguments);
            }

            if (!string.IsNullOrWhiteSpace(code))
            {
                return code;
            }

            return string.Empty;
        }

        public string GetEnumDisplayName<TEnum>(TEnum value, CultureInfo? culture = null) where TEnum : struct, Enum
        {
            var key = $"ENUM_{typeof(TEnum).Name}_{value}";
            return HasTranslation(key, culture) ? Get(key, culture) : value.ToString();
        }

        private static string BuildLiteralKey(string message)
        {
            var builder = new StringBuilder(message.Length);
            var previousWasSeparator = false;

            foreach (var character in message.ToUpperInvariant())
            {
                if (char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                    previousWasSeparator = false;
                    continue;
                }

                if (previousWasSeparator)
                {
                    continue;
                }

                builder.Append('_');
                previousWasSeparator = true;
            }

            return builder.ToString().Trim('_');
        }

        private static string FormatFallback(string fallback, CultureInfo? culture, object?[] arguments)
        {
            if (arguments.Length == 0)
            {
                return fallback;
            }

            return string.Format(culture ?? CultureInfo.CurrentUICulture, fallback, arguments);
        }

        private static CultureScope? CreateCultureScope(CultureInfo? culture)
        {
            return culture == null ? null : new CultureScope(culture);
        }
    }
}
