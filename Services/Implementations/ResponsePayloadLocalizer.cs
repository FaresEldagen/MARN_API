using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Property;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class ResponsePayloadLocalizer : IResponsePayloadLocalizer
    {
        private readonly IAppTextLocalizer _localizer;

        public ResponsePayloadLocalizer(IAppTextLocalizer localizer)
        {
            _localizer = localizer;
        }

        public void Localize(object? payload)
        {
            if (payload is null)
            {
                return;
            }

            var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
            LocalizeValue(payload, visited);
        }

        private void LocalizeValue(object? value, HashSet<object> visited)
        {
            if (value is null)
            {
                return;
            }

            var type = value.GetType();
            if (IsSimple(type))
            {
                return;
            }

            if (value is IEnumerable enumerable and not string)
            {
                foreach (var item in enumerable)
                {
                    LocalizeValue(item, visited);
                }

                return;
            }

            if (!type.IsValueType && !visited.Add(value))
            {
                return;
            }

            PopulateDisplayProperties(value, type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                var propertyValue = property.GetValue(value);
                if (propertyValue is null || propertyValue is string || IsSimple(property.PropertyType))
                {
                    continue;
                }

                LocalizeValue(propertyValue, visited);
            }
        }

        private void PopulateDisplayProperties(object instance, Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var displayProperty in properties.Where(p => p.CanWrite && p.PropertyType == typeof(string) && p.Name.EndsWith("DisplayName", StringComparison.Ordinal)))
            {
                var sourceName = displayProperty.Name[..^"DisplayName".Length];
                var sourceProperty = properties.FirstOrDefault(p => p.Name == sourceName && p.CanRead);
                if (sourceProperty == null)
                {
                    continue;
                }

                var localizedValue = ResolveDisplayName(instance, sourceProperty);
                if (!string.IsNullOrWhiteSpace(localizedValue))
                {
                    displayProperty.SetValue(instance, localizedValue);
                }
            }

            foreach (var displayProperty in properties.Where(p => p.CanWrite && typeof(ICollection<string>).IsAssignableFrom(p.PropertyType) && p.Name.EndsWith("DisplayNames", StringComparison.Ordinal)))
            {
                var sourceName = displayProperty.Name[..^"DisplayNames".Length];
                var sourceProperty = properties.FirstOrDefault(p => p.Name == sourceName && p.CanRead);
                if (sourceProperty == null)
                {
                    continue;
                }

                var localizedValues = ResolveDisplayNames(instance, sourceProperty);
                if (localizedValues.Count > 0)
                {
                    displayProperty.SetValue(instance, localizedValues);
                }
            }
        }

        private string? ResolveDisplayName(object instance, PropertyInfo sourceProperty)
        {
            var rawValue = sourceProperty.GetValue(instance);
            if (rawValue is null)
            {
                return null;
            }

            var sourceType = Nullable.GetUnderlyingType(sourceProperty.PropertyType) ?? sourceProperty.PropertyType;
            if (sourceType.IsEnum)
            {
                return _localizer.GetEnumDisplayName(sourceType, rawValue);
            }

            if (rawValue is string stringValue)
            {
                return ResolveStringDisplayName(sourceProperty.Name, stringValue);
            }

            return null;
        }

        private List<string> ResolveDisplayNames(object instance, PropertyInfo sourceProperty)
        {
            var rawValue = sourceProperty.GetValue(instance);
            if (rawValue is null || rawValue is string || rawValue is not IEnumerable enumerable)
            {
                return [];
            }

            var results = new List<string>();
            var elementType = GetEnumerableElementType(sourceProperty.PropertyType);
            var normalizedElementType = elementType == null ? null : Nullable.GetUnderlyingType(elementType) ?? elementType;

            foreach (var item in enumerable)
            {
                if (item is null)
                {
                    continue;
                }

                if (normalizedElementType?.IsEnum == true)
                {
                    results.Add(_localizer.GetEnumDisplayName(normalizedElementType, item));
                    continue;
                }

                if (item is string stringItem)
                {
                    results.Add(ResolveStringDisplayName(sourceProperty.Name, stringItem) ?? stringItem);
                }
            }

            return results;
        }

        private string? ResolveStringDisplayName(string sourceName, string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return rawValue;
            }

            if (sourceName.Equals("Role", StringComparison.OrdinalIgnoreCase) ||
                sourceName.Equals("RoleName", StringComparison.OrdinalIgnoreCase) ||
                sourceName.Equals("Roles", StringComparison.OrdinalIgnoreCase))
            {
                return LocalizeRole(rawValue);
            }

            if (sourceName.Equals("City", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<City>(rawValue);
            }

            if (sourceName.Equals("State", StringComparison.OrdinalIgnoreCase) ||
                sourceName.Equals("Governorate", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<Governorate>(rawValue);
            }

            if (sourceName.Equals("PaymentFrequency", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<PaymentFrequency>(rawValue);
            }

            if (sourceName.Equals("RentalUnit", StringComparison.OrdinalIgnoreCase) ||
                sourceName.Equals("RentalDuration", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<RentalUnit>(rawValue);
            }

            if (sourceName.Equals("ContractStatus", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<ContractStatus>(rawValue);
            }

            if (sourceName.Equals("AnchoringStatus", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<ContractAnchoringStatus>(rawValue);
            }

            if (sourceName.Equals("SleepSchedule", StringComparison.OrdinalIgnoreCase))
            {
                return TryLocalizeParsedEnum<SleepSchedule>(rawValue);
            }

            return rawValue;
        }

        private string LocalizeRole(string rawValue)
        {
            var trimmedValue = rawValue.Trim();
            var roleKey = $"ROLE_{trimmedValue.Replace(' ', '_')}";
            return _localizer.GetOrFallback(roleKey, trimmedValue);
        }

        private string TryLocalizeParsedEnum<TEnum>(string rawValue) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(rawValue, true, out var parsed)
                ? _localizer.GetEnumDisplayName(parsed)
                : rawValue;
        }

        private static Type? GetEnumerableElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            var genericEnumerable = type
                .GetInterfaces()
                .Append(type)
                .FirstOrDefault(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return genericEnumerable?.GetGenericArguments()[0];
        }

        private static bool IsSimple(Type type)
        {
            var normalizedType = Nullable.GetUnderlyingType(type) ?? type;
            return normalizedType.IsPrimitive ||
                   normalizedType.IsEnum ||
                   normalizedType == typeof(string) ||
                   normalizedType == typeof(decimal) ||
                   normalizedType == typeof(DateTime) ||
                   normalizedType == typeof(DateOnly) ||
                   normalizedType == typeof(TimeOnly) ||
                   normalizedType == typeof(Guid);
        }

        private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public static readonly ReferenceEqualityComparer Instance = new();

            public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }
    }
}
