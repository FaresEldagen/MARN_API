using System.Text;

namespace MARN_API.Localization
{
    public static class LocalizationKeyBuilder
    {
        private static readonly string[] GenericErrorFragments =
        [
            "failed",
            "an error occurred",
            "could not be processed",
            "unexpected error",
            "invalid request"
        ];

        private static readonly HashSet<string> GenericSummaries = new(StringComparer.OrdinalIgnoreCase)
        {
            "An error occurred.",
            "The request could not be processed.",
            "You are not authorized to perform this action.",
            "An unexpected error occurred. Please try again later.",
            "Invalid request",
            "Invalid request."
        };

        public static string BuildLiteralKey(string message)
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

        public static string BuildErrorCode(string? explicitCode, string? message, IEnumerable<string>? errors, string fallbackCode)
        {
            if (!string.IsNullOrWhiteSpace(explicitCode))
            {
                return explicitCode.Trim();
            }

            var source = ResolveCodeSourceMessage(message, errors);
            return string.IsNullOrWhiteSpace(source)
                ? fallbackCode
                : BuildLiteralKey(source);
        }

        public static string ResolvePrimaryMessage(string? message, IEnumerable<string>? errors, string fallbackMessage)
        {
            var normalizedErrors = NormalizeErrors(errors);
            if (!string.IsNullOrWhiteSpace(message))
            {
                var trimmedMessage = message.Trim();
                if (IsGenericSummary(trimmedMessage) && normalizedErrors.Count == 1)
                {
                    return normalizedErrors[0];
                }

                return trimmedMessage;
            }

            return normalizedErrors.FirstOrDefault() ?? fallbackMessage;
        }

        public static string HumanizeMemberName(string? memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName))
            {
                return string.Empty;
            }

            var trimmed = memberName.Trim();
            var builder = new StringBuilder(trimmed.Length + 8);

            for (var index = 0; index < trimmed.Length; index++)
            {
                var current = trimmed[index];
                if (index > 0 &&
                    char.IsUpper(current) &&
                    !char.IsUpper(trimmed[index - 1]) &&
                    !char.IsWhiteSpace(trimmed[index - 1]))
                {
                    builder.Append(' ');
                }

                builder.Append(current);
            }

            return builder.ToString();
        }

        private static string? ResolveCodeSourceMessage(string? message, IEnumerable<string>? errors)
        {
            var normalizedErrors = NormalizeErrors(errors);
            if (!string.IsNullOrWhiteSpace(message))
            {
                var trimmedMessage = message.Trim();
                if (IsGenericSummary(trimmedMessage) && normalizedErrors.Count > 0)
                {
                    return normalizedErrors[0];
                }

                return trimmedMessage;
            }

            return normalizedErrors.FirstOrDefault();
        }

        private static List<string> NormalizeErrors(IEnumerable<string>? errors)
        {
            return errors?
                .Where(error => !string.IsNullOrWhiteSpace(error))
                .Select(error => error.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToList()
                ?? [];
        }

        private static bool IsGenericSummary(string message)
        {
            if (GenericSummaries.Contains(message))
            {
                return true;
            }

            return GenericErrorFragments.Any(fragment =>
                message.Contains(fragment, StringComparison.OrdinalIgnoreCase));
        }
    }
}
