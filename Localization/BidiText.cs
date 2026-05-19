using System.Globalization;
using System.Text;

namespace MARN_API.Localization
{
    public static class BidiText
    {
        private const char LeftToRightIsolate = '\u2066';
        private const char RightToLeftIsolate = '\u2067';
        private const char FirstStrongIsolate = '\u2068';
        private const char PopDirectionalIsolate = '\u2069';

        public static string Format(string? text, CultureInfo? culture = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var baseIsRtl = (culture ?? CultureInfo.CurrentUICulture).TextInfo.IsRightToLeft;
            var builder = new StringBuilder(text.Length + 16);
            var isolateDepth = 0;
            var index = 0;

            while (index < text.Length)
            {
                var current = text[index];

                if (IsIsolateOpener(current))
                {
                    isolateDepth++;
                    builder.Append(current);
                    index++;
                    continue;
                }

                if (current == PopDirectionalIsolate)
                {
                    isolateDepth = Math.Max(0, isolateDepth - 1);
                    builder.Append(current);
                    index++;
                    continue;
                }

                if (isolateDepth > 0)
                {
                    builder.Append(current);
                    index++;
                    continue;
                }

                if (!TryGetIsolatedRunLength(text, index, baseIsRtl, out var runLength))
                {
                    builder.Append(current);
                    index++;
                    continue;
                }

                builder.Append(FirstStrongIsolate);
                builder.Append(text, index, runLength);
                builder.Append(PopDirectionalIsolate);
                index += runLength;
            }

            return builder.ToString();
        }

        public static string Isolate(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text[0] == FirstStrongIsolate && text[^1] == PopDirectionalIsolate)
            {
                return text;
            }

            return $"{FirstStrongIsolate}{text}{PopDirectionalIsolate}";
        }

        private static bool TryGetIsolatedRunLength(string text, int start, bool baseIsRtl, out int runLength)
        {
            runLength = 0;

            if (!ShouldStartIsolatedRun(text, start, baseIsRtl))
            {
                return false;
            }

            var index = start;
            var sawOppositeStrong = false;

            while (index < text.Length)
            {
                var current = text[index];

                if (IsIsolateMarker(current))
                {
                    break;
                }

                if (IsOppositeStrongOrDigit(current, baseIsRtl))
                {
                    sawOppositeStrong = true;
                    index++;
                    continue;
                }

                if (!sawOppositeStrong && IsLeadingWrapper(current))
                {
                    index++;
                    continue;
                }

                if (sawOppositeStrong && IsNeutralInsideRun(current))
                {
                    index++;
                    continue;
                }

                break;
            }

            if (!sawOppositeStrong)
            {
                return false;
            }

            while (index > start && IsTrimmedTrailingCharacter(text[index - 1]))
            {
                index--;
            }

            runLength = index - start;
            return runLength > 0;
        }

        private static bool ShouldStartIsolatedRun(string text, int index, bool baseIsRtl)
        {
            var current = text[index];
            if (IsOppositeStrongOrDigit(current, baseIsRtl))
            {
                return true;
            }

            if (!IsLeadingWrapper(current))
            {
                return false;
            }

            var probe = index;
            while (probe < text.Length && IsLeadingWrapper(text[probe]))
            {
                probe++;
            }

            return probe < text.Length && IsOppositeStrongOrDigit(text[probe], baseIsRtl);
        }

        private static bool IsOppositeStrongOrDigit(char character, bool baseIsRtl)
        {
            if (baseIsRtl)
            {
                return IsLatinLetter(character) || char.IsDigit(character);
            }

            return IsRightToLeftCharacter(character);
        }

        private static bool IsLatinLetter(char character)
        {
            return (character is >= 'A' and <= 'Z') ||
                   (character is >= 'a' and <= 'z') ||
                   (character <= '\u024F' && char.IsLetter(character));
        }

        private static bool IsRightToLeftCharacter(char character)
        {
            return (character >= '\u0590' && character <= '\u08FF') ||
                   (character >= '\uFB1D' && character <= '\uFDFF') ||
                   (character >= '\uFE70' && character <= '\uFEFC');
        }

        private static bool IsNeutralInsideRun(char character)
        {
            return char.IsWhiteSpace(character) ||
                   character is '"' or '\'' or '(' or ')' or '[' or ']' or '{' or '}' or '<' or '>' or
                   '/' or '\\' or '-' or '_' or '+' or '=' or '@' or '#' or '&' or '%' or '.' or ',' or ':' or '|';
        }

        private static bool IsLeadingWrapper(char character)
        {
            return character is '"' or '\'' or '(' or '[' or '{' or '<';
        }

        private static bool IsTrimmedTrailingCharacter(char character)
        {
            return char.IsWhiteSpace(character) ||
                   character is '.' or ',' or ';' or ':' or '!' or '?' or '،' or '؛';
        }

        private static bool IsIsolateOpener(char character)
        {
            return character is LeftToRightIsolate or RightToLeftIsolate or FirstStrongIsolate;
        }

        private static bool IsIsolateMarker(char character)
        {
            return IsIsolateOpener(character) || character == PopDirectionalIsolate;
        }
    }
}
