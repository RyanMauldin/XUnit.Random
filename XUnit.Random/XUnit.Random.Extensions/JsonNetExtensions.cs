using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XUnit.Random.Extensions.Types;

namespace XUnit.Random.Extensions
{
    public static class JsonNetExtensions
    {
        private const string PropertyName = "PropertyName";
        private const string RequiredName = "Required";
        private const string NullValueHandlingName = "NullValueHandling";
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        public static string ToJsonPropertyAttributeName<T>(this T type, string name, JsonSerializerSettings settings = null,
        Convention convention = Convention.Camel, CultureInfo cultureInfo = null) where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (settings == null) settings = new JsonSerializerSettings();
            if (cultureInfo == null) cultureInfo = settings.Culture ?? CultureInfo.CurrentCulture;

            // Get custom attributes
            var propertyInfo = type.GetType().GetProperty(name);
            var customAttributes = propertyInfo?.CustomAttributes.ToList();

            // Check for JsonProperty Attribute
            var jsonPropertyAttribute = customAttributes?
                .FirstOrDefault(customAttribute => customAttribute.AttributeType == typeof(JsonPropertyAttribute));

            // Check for JsonProperty Attribute PropertyName Argument
            var jsonPropertyName = jsonPropertyAttribute?.NamedArguments?
                .FirstOrDefault(namedArgument => Comparer.Equals(namedArgument.MemberName, PropertyName));
            var jsonTypedValue = jsonPropertyName?.TypedValue;
            var propertyName = Convert.ToString(jsonTypedValue?.Value ?? string.Empty);

            // Return the JsonProperty Attribute PropertyName Value, otherwise return the provided name parameter value.
            return string.IsNullOrWhiteSpace(propertyName) ? name.ToConvention(convention, cultureInfo) : propertyName;
        }

        public static bool IsEmittedProperty<T>(this T type, string name, JsonSerializerSettings settings = null) where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (settings == null) settings = new JsonSerializerSettings();

            // Get custom attributes
            var propertyInfo = type.GetType().GetProperty(name);
            var propertyValue = propertyInfo?.GetValue(type);
            var isNullPropertyValue = propertyValue == null;
            var customAttributes = propertyInfo?.CustomAttributes.ToList();

            // Check for JsonIgnore Attribute
            var jsonIgnoreAttribute = customAttributes?
                .FirstOrDefault(customAttribute => customAttribute.AttributeType == typeof(JsonIgnoreAttribute));
            if (jsonIgnoreAttribute != null) return false;

            // Check for Required Attribute
            var jsonRequiredAttribute = customAttributes?
                .FirstOrDefault(customAttribute => customAttribute.AttributeType == typeof(JsonRequiredAttribute));
            if (jsonRequiredAttribute != null) return true;

            // Check for JsonProperty Attribute
            var jsonPropertyAttribute = customAttributes?
                .FirstOrDefault(customAttribute => customAttribute.AttributeType == typeof(JsonPropertyAttribute));

            // Check for JsonProperty Attribute Required Argument
            var requiredArgument = jsonPropertyAttribute?.NamedArguments?
                .FirstOrDefault(namedArgument => Comparer.Equals(namedArgument.MemberName, RequiredName));
            if (requiredArgument.HasValue)
            {
                // Verify property.
                var argumentValue = requiredArgument.GetValueOrDefault();
                if (argumentValue.MemberInfo != null)
                {
                    var memberName = argumentValue.MemberName;
                    if (!string.IsNullOrEmpty(memberName) && Comparer.Equals(memberName, RequiredName))
                        return true;
                }
            }

            // Check for JsonProperty Attribute NullValueHandling Argument
            var nullValueHandlingArgument = jsonPropertyAttribute?.NamedArguments?
                .FirstOrDefault(namedArgument => Comparer.Equals(namedArgument.MemberName, NullValueHandlingName));
            if (nullValueHandlingArgument.HasValue)
            {
                // Verify property.
                var argumentValue = nullValueHandlingArgument.GetValueOrDefault();
                if (argumentValue.MemberInfo != null)
                {
                    var memberName = argumentValue.MemberName;
                    if (!string.IsNullOrEmpty(memberName) && Comparer.Equals(memberName, NullValueHandlingName))
                    {
                        var argumentTypedValue = (NullValueHandling)argumentValue.TypedValue.Value;
                        switch (argumentTypedValue)
                        {
                            case NullValueHandling.Include:
                                return true;
                            case NullValueHandling.Ignore when isNullPropertyValue:
                                return false;
                        }
                    }
                }
            }

            // Check JsonSerializerSettings
            return !isNullPropertyValue || settings.NullValueHandling != NullValueHandling.Ignore;
        }

        public static string ToJsonPropertyDefinition<T>(this T type, string name, JsonSerializerSettings settings = null,
            Convention convention = Convention.Camel, CultureInfo cultureInfo = null) where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (settings == null) settings = new JsonSerializerSettings();
            if (cultureInfo == null) cultureInfo = settings.Culture ?? CultureInfo.CurrentCulture;

            // Check if property can be emitted.
            var isEmittedProperty = type.IsEmittedProperty(name, settings);
            if (!isEmittedProperty) return null;

            // Create output definition.
            var propertyInfo = type.GetType().GetProperty(name);
            var propertyValue = propertyInfo?.GetValue(type);
            var value = propertyValue.ToJsonPropertyDefinitionValueString(settings);
            var propertyName = type.ToJsonPropertyAttributeName(name, settings, convention, cultureInfo);
            var indentPadding = settings.Formatting == Formatting.Indented ? "  " : string.Empty;
            var propertyPadding = settings.Formatting == Formatting.Indented ? " " : string.Empty;
            return $@"{indentPadding}""{propertyName}"":{propertyPadding}{value}";
        }

        public static string ToJsonPropertyDefinitionValueString<T>(this T value, JsonSerializerSettings settings = null)
        {
            // Basic validation.
            if (settings == null) settings = new JsonSerializerSettings();
            if (value == null) return "null";
            var valueType = Nullable.GetUnderlyingType(value.GetType()) != null
                ? Nullable.GetUnderlyingType(value.GetType())?.Name
                : value.GetType().Name;
            switch (valueType)
            {
                case nameof(DateTime):
                    var dateValue = Convert.ToDateTime(value);
                    string returnValue;

                    // Check for DateFormatString Json Setting.
                    if (!string.IsNullOrWhiteSpace(settings.DateFormatString))
                    {
                        returnValue = dateValue.ToString(settings.DateFormatString);
                        return $@"""{returnValue}""";
                    }

                    // Check for DateFormatHandling Json Setting.
                    switch (settings.DateFormatHandling)
                    {
                        case DateFormatHandling.IsoDateFormat:
                            returnValue = dateValue.ToString(new IsoDateTimeConverter().DateTimeFormat);
                            return $@"""{returnValue}""";
                        case DateFormatHandling.MicrosoftDateFormat:
                            returnValue = dateValue.ToString(new DateTimeFormatInfo().FullDateTimePattern);
                            return $@"""{returnValue}""";
                        default:
                            returnValue = Convert.ToString(dateValue.ToUnixTimestamp());
                            return $@"""\/Date({returnValue})\/""";
                    }
                case nameof(Byte):
                case nameof(SByte):
                case nameof(UInt16):
                case nameof(UInt32):
                case nameof(UInt64):
                case nameof(Int16):
                case nameof(Int32):
                case nameof(Int64):
                case nameof(Decimal):
                case nameof(Double):
                case nameof(Single):
                    // Handle Numeric Values
                    return Convert.ToString(value);
                default:
                    // Wrap string values or other values.
                    returnValue = Convert.ToString(value);
                    return $@"""{returnValue}""";
            }
        }

        public static string ToJsonDefinition<T>(this T type, JsonSerializerSettings settings = null, 
            Convention convention = Convention.Camel, CultureInfo cultureInfo = null) where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (settings == null) settings = new JsonSerializerSettings();
            if (cultureInfo == null) cultureInfo = settings.Culture ?? CultureInfo.CurrentCulture;

            // Get Property Definitions.
            var propertyDefinitions = type.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanRead)
                .Select(property => type.ToJsonPropertyDefinition(property.Name, settings, convention, cultureInfo))
                .Where(propertyDefinition => !string.IsNullOrWhiteSpace(propertyDefinition))
                .ToList();

            // Join Property Definitions together.
            var separator = settings.Formatting == Formatting.Indented ? ",\r\n" : ",";
            var definition = string.Join(separator, propertyDefinitions);

            // Assemble Serialized Definitions.
            var builder = new StringBuilder(128);
            builder = settings.Formatting == Formatting.Indented ? builder.AppendLine("{") : builder.Append("{");
            builder = settings.Formatting == Formatting.Indented ? builder.AppendLine(definition) : builder.Append(definition);
            builder = builder.Append("}");
            return builder.ToString();
        }
    }
}