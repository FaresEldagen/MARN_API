using System.Globalization;

namespace MARN_API.Localization
{
    public static class LocalizationConstants
    {
        public const string DefaultCulture = "en";
        public const string ArabicCulture = "ar";

        public static readonly IReadOnlyList<string> SupportedCultureCodes = new[]
        {
            DefaultCulture,
            ArabicCulture
        };

        public static readonly IReadOnlyList<CultureInfo> SupportedCultures = SupportedCultureCodes
            .Select(code => new CultureInfo(code))
            .ToList();

        public static string NormalizeCultureName(string? cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return DefaultCulture;
            }

            var normalized = cultureName.Trim().ToLowerInvariant();

            if (normalized.StartsWith("ar"))
            {
                return ArabicCulture;
            }

            return DefaultCulture;
        }

        public static CultureInfo GetCulture(string? cultureName)
        {
            return new CultureInfo(NormalizeCultureName(cultureName));
        }
    }
}
