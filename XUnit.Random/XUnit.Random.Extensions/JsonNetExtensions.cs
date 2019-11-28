using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XUnit.Random.Extensions
{
    public static class JsonNetExtensions
    {
        public static string ToJsonPropertyAttributeName<T>(this T type, string name)
            where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));

            // Get custom attributes
            var propertyInfo = type.GetType().GetProperty(name);
            var customAttributes = propertyInfo?.CustomAttributes.ToList();

            // Check for JsonProperty Attribute
            var jsonPropertyAttribute = customAttributes?.FirstOrDefault(p => p.AttributeType == typeof(JsonPropertyAttribute));

            // Check for JsonProperty Attribute PropertyName Argument
            var jsonPropertyName = jsonPropertyAttribute?.NamedArguments?.FirstOrDefault(p => p.MemberName == "PropertyName");
            var jsonTypedValue = jsonPropertyName?.TypedValue;
            var propertyName = Convert.ToString(jsonTypedValue?.Value ?? string.Empty);

            // Return the JsonProperty Attribute PropertyName Value, otherwise return the provided name parameter value.
            return string.IsNullOrWhiteSpace(propertyName) ? name.ToCamel() : propertyName;
        }

        public static bool IsEmittedProperty<T>(this T type, string name, JsonSerializerSettings settings = null)
            where T : class
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
            var jsonIgnoreAttribute = customAttributes?.FirstOrDefault(p => p.AttributeType == typeof(JsonIgnoreAttribute));
            if (jsonIgnoreAttribute != null) return false;

            // Check for Required Attribute
            var jsonRequiredAttribute = customAttributes?.FirstOrDefault(p => p.AttributeType == typeof(JsonRequiredAttribute));
            if (jsonRequiredAttribute != null) return true;

            // Check for JsonProperty Attribute
            var jsonPropertyAttribute = customAttributes?.FirstOrDefault(p => p.AttributeType == typeof(JsonPropertyAttribute));

            // Check for JsonProperty Attribute Required Argument
            const string requiredName = "Required";
            var requiredArgument = jsonPropertyAttribute?.NamedArguments?.FirstOrDefault(p => StringComparer.Ordinal.Equals(p.MemberName, requiredName));
            if (requiredArgument.HasValue)
            {
                // Verify property.
                var requiredArgumentValue = requiredArgument.GetValueOrDefault();
                if (requiredArgumentValue.MemberInfo != null)
                {
                    var requiredArgumentMemberName = requiredArgumentValue.MemberName;
                    if (!string.IsNullOrEmpty(requiredArgumentMemberName) && StringComparer.Ordinal.Equals(requiredArgumentMemberName, requiredName))
                        return true;
                }
            }

            // Check for JsonProperty Attribute NullValueHandling Argument
            const string nullValueHandlingName = "NullValueHandling";
            var nullValueHandlingArgument = jsonPropertyAttribute?.NamedArguments?.FirstOrDefault(p => StringComparer.Ordinal.Equals(p.MemberName, nullValueHandlingName));
            if (nullValueHandlingArgument.HasValue)
            {
                // Verify property.
                var nullValueHandlingArgumentValue = nullValueHandlingArgument.GetValueOrDefault();
                if (nullValueHandlingArgumentValue.MemberInfo != null)
                {
                    var nullValueHandlingArgumentMemberName = nullValueHandlingArgumentValue.MemberName;
                    if (!string.IsNullOrEmpty(nullValueHandlingArgumentMemberName) && StringComparer.Ordinal.Equals(nullValueHandlingArgumentMemberName, nullValueHandlingName))
                    {

                        var nullValueHandlingArgumentTypedValue = (NullValueHandling)nullValueHandlingArgumentValue.TypedValue.Value;
                        if (nullValueHandlingArgumentTypedValue == NullValueHandling.Ignore) return false;
                    }
                }
            }

            // Check JsonSerializerSettings
            return !isNullPropertyValue || settings.NullValueHandling != NullValueHandling.Ignore;
        }

        public static string ToJsonPropertyDefinition<T>(this T type, string name, JsonSerializerSettings settings = null)
            where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (settings == null) settings = new JsonSerializerSettings();

            // Check if property can be emitted.
            var isEmittedProperty = type.IsEmittedProperty(name, settings);
            if (!isEmittedProperty) return null;

            // Create output definition.
            var propertyInfo = type.GetType().GetProperty(name);
            var propertyValue = propertyInfo?.GetValue(type);
            var value = propertyValue.ToJsonPropertyDefinitionValueString(settings);
            var propertyName = type.ToJsonPropertyAttributeName(name);
            var indentPadding = settings.Formatting == Formatting.Indented ? "    " : string.Empty;
            return $@"{indentPadding}""{propertyName}"":{value}";
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

        public static string ToJsonDefinition<T>(this T type, JsonSerializerSettings settings = null)
            where T : class
        {
            // Basic validation.
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (settings == null) settings = new JsonSerializerSettings();

            // Get Property Definitions.
            var propertyDefinitions = type.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanRead)
                .Select(property => type.ToJsonPropertyDefinition(property.Name))
                .Where(propertyDefinition => !string.IsNullOrWhiteSpace(propertyDefinition))
                .ToList();

            // Join Property Definitions together.
            var separator = settings.Formatting == Formatting.Indented ? ",\n" : ",";
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