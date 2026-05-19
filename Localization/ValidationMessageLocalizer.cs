using System.Text.RegularExpressions;
using MARN_API.Services.Interfaces;

namespace MARN_API.Localization
{
    public static partial class ValidationMessageLocalizer
    {
        public static string Localize(string? fieldName, string? message, IAppTextLocalizer localizer)
        {
            var fallbackMessage = message ?? "The provided value is invalid.";
            var literal = localizer.LocalizeMessage(null, fallbackMessage);
            if (!string.Equals(literal, fallbackMessage, StringComparison.Ordinal))
            {
                return literal;
            }

            if (string.Equals(fieldName, "$", StringComparison.Ordinal) ||
                fallbackMessage.StartsWith("The JSON ", StringComparison.OrdinalIgnoreCase) ||
                fallbackMessage.Contains("BytePositionInLine", StringComparison.OrdinalIgnoreCase))
            {
                return localizer.Get("VALIDATION_INVALID_JSON");
            }

            var normalizedFieldName = ResolveFieldLabel(fieldName, localizer);

            if (TryMatch(RequiredPattern(), fallbackMessage, out var requiredMatch))
            {
                var resolvedField = ResolveFieldLabel(requiredMatch.Groups["field"].Value, localizer, normalizedFieldName);
                return localizer.Get("VALIDATION_FIELD_REQUIRED", null, resolvedField);
            }

            if (TryMatch(EmailPattern(), fallbackMessage, out var emailMatch))
            {
                var resolvedField = ResolveFieldLabel(emailMatch.Groups["field"].Value, localizer, normalizedFieldName);
                return localizer.Get("VALIDATION_EMAIL_ADDRESS", null, resolvedField);
            }

            if (TryMatch(PhonePattern(), fallbackMessage, out var phoneMatch))
            {
                var resolvedField = ResolveFieldLabel(phoneMatch.Groups["field"].Value, localizer, normalizedFieldName);
                return localizer.Get("VALIDATION_PHONE_NUMBER", null, resolvedField);
            }

            if (TryMatch(StringLengthRangePattern(), fallbackMessage, out var stringRangeMatch))
            {
                var resolvedField = ResolveFieldLabel(stringRangeMatch.Groups["field"].Value, localizer, normalizedFieldName);
                return localizer.Get(
                    "VALIDATION_STRING_LENGTH_RANGE",
                    null,
                    resolvedField,
                    stringRangeMatch.Groups["min"].Value,
                    stringRangeMatch.Groups["max"].Value);
            }

            if (TryMatch(StringLengthMaxPattern(), fallbackMessage, out var stringMaxMatch))
            {
                var resolvedField = ResolveFieldLabel(stringMaxMatch.Groups["field"].Value, localizer, normalizedFieldName);
                return localizer.Get(
                    "VALIDATION_STRING_LENGTH_MAX",
                    null,
                    resolvedField,
                    stringMaxMatch.Groups["max"].Value);
            }

            if (TryMatch(RangePattern(), fallbackMessage, out var rangeMatch))
            {
                var resolvedField = ResolveFieldLabel(rangeMatch.Groups["field"].Value, localizer, normalizedFieldName);
                return localizer.Get(
                    "VALIDATION_RANGE_BETWEEN",
                    null,
                    resolvedField,
                    rangeMatch.Groups["min"].Value,
                    rangeMatch.Groups["max"].Value);
            }

            return localizer.LocalizeMessage("VALIDATION_VALUE_INVALID", "{0} is invalid.", arguments: [normalizedFieldName]);
        }

        private static bool TryMatch(Regex regex, string message, out Match match)
        {
            match = regex.Match(message);
            return match.Success;
        }

        private static string ResolveFieldLabel(string? rawFieldName, IAppTextLocalizer localizer, string? fallback = null)
        {
            var fieldName = string.IsNullOrWhiteSpace(rawFieldName)
                ? fallback ?? string.Empty
                : rawFieldName.Trim();

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return string.Empty;
            }

            var fieldKey = $"FIELD_{LocalizationKeyBuilder.BuildLiteralKey(fieldName)}";
            return localizer.GetOrFallback(fieldKey, LocalizationKeyBuilder.HumanizeMemberName(fieldName));
        }

        [GeneratedRegex(@"^The (?<field>.+?) field is required\.$", RegexOptions.Compiled)]
        private static partial Regex RequiredPattern();

        [GeneratedRegex(@"^The (?<field>.+?) field is not a valid e-mail address\.$", RegexOptions.Compiled)]
        private static partial Regex EmailPattern();

        [GeneratedRegex(@"^The (?<field>.+?) field is not a valid phone number\.$", RegexOptions.Compiled)]
        private static partial Regex PhonePattern();

        [GeneratedRegex(@"^The field (?<field>.+?) must be a string with a minimum length of (?<min>\d+) and a maximum length of (?<max>\d+)\.$", RegexOptions.Compiled)]
        private static partial Regex StringLengthRangePattern();

        [GeneratedRegex(@"^The field (?<field>.+?) must be a string with a maximum length of (?<max>\d+)\.$", RegexOptions.Compiled)]
        private static partial Regex StringLengthMaxPattern();

        [GeneratedRegex(@"^The field (?<field>.+?) must be between (?<min>.+?) and (?<max>.+?)\.$", RegexOptions.Compiled)]
        private static partial Regex RangePattern();
    }
}
