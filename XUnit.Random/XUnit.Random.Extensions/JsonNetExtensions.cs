using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace XUnit.Random.Extensions
{
    public static class JsonNetExtensions
    {
        /// <summary>
        /// Takes the first character of the string, and lowers the case of the first position. 
        /// </summary>
        /// <remarks>
        /// This could also be ToLowerCamelCase, however this does not automatically convert the entire word or words
        /// to lower camel case syntax, so the term here is to indicate usage, and its for parameters in our code,
        /// where we know the usage and know the method is safe.
        /// </remarks>
        /// <param name="value">The value to make the first initial lowercase.</param>
        /// <returns>A first letter lowercased version of the string value parameter value.</returns>
        public static string ToParameterCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || !char.IsLetter(value, 0) || char.IsLower(value, 0)) return value;
            if (value.Length == 1) return value.ToLower(CultureInfo.InvariantCulture);
            return value.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + value.Substring(1);
        }

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
            return string.IsNullOrWhiteSpace(propertyName) ? propertyName : name.ToParameterCase();
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
            var value = propertyValue == null ? null : Convert.ToString(propertyValue);
            var propertyName = type.ToJsonPropertyAttributeName(name);
            var indentPadding = settings.Formatting == Formatting.Indented ? "    " : string.Empty;
            var serializedValue = value == null ? "null" : Convert.ToString(value, CultureInfo.CurrentCulture);
            return $@"{indentPadding}""{propertyName}"":""{serializedValue}""";
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
            var separator = settings.Formatting == Formatting.Indented ? ",\n" : "";
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