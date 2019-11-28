using System;
using System.Globalization;
using System.Text;
using XUnit.Random.Extensions.Types;

namespace XUnit.Random.Extensions
{
    public static class NamingExtensions
    {
        internal static string ToNone(this string value, CultureInfo cultureInfo = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            value = value.ToRegexReplaceString("[^a-zA-Z0-9_ ]");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"Value has no substance for naming conventions.", nameof(value));
            return value;
        }

        public static string ToCamel(this string value, CultureInfo cultureInfo = null) 
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            value = value.ToRegexReplaceString("[^a-zA-Z0-9_ ]");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"Could not convert to Camel Case.", nameof(value));
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentCulture;

            if (!char.IsLetter(value, 0) || char.IsLower(value, 0)) return value;
            if (value.Length == 1) return value.ToLower(cultureInfo);
            return value.Substring(0, 1).ToLower(cultureInfo) + value.Substring(1);
        }

        public static string ToPascal(this string value, CultureInfo cultureInfo = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            value = value.ToRegexReplaceString("[^a-zA-Z0-9_ ]");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"Could not convert to Pascal Case.", nameof(value));
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentCulture;

            if (!char.IsLetter(value, 0) || char.IsLower(value, 0)) return value;
            if (value.Length == 1) return value.ToLower(cultureInfo);
            return value.Substring(0, 1).ToLower(cultureInfo) + value.Substring(1);
        }

        public static string ToSnake(this string value, CultureInfo cultureInfo = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            value = value.ToRegexReplaceString("[^a-zA-Z0-9_ ]");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"Could not convert to Snake Case.", nameof(value));
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(value) || !char.IsLetter(value, 0) || char.IsLower(value, 0)) return value;
            if (value.Length == 1) return value.ToLower(cultureInfo);
            var words = value.Split(new[] { '_', ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var wordBuilder = new StringBuilder(value.Length, value.Length * 2);
            var isStillUpper = false;
            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                var subWordBuilder = new StringBuilder(word.Length * 2);
                for (var j = 0; j < word.Length; j++)
                {
                    var wordCharacter = word[j];
                    var isUpper = char.IsUpper(wordCharacter);
                    if (j == 0 && isUpper)
                        isStillUpper = true;

                    if (isUpper && j > 0 && subWordBuilder.Length > 0)
                    {
                        if (!isStillUpper)
                        {
                            subWordBuilder.Append('_');
                            isStillUpper = true;
                        }

                        subWordBuilder.Append(char.ToLower(wordCharacter, cultureInfo));
                    }
                    else if (isUpper)
                    {
                        subWordBuilder.Append(char.ToLower(wordCharacter, cultureInfo));
                    }
                    else if (char.IsLetterOrDigit(wordCharacter))
                    {
                        isStillUpper = false;
                        subWordBuilder.Append(wordCharacter);
                    }
                    else
                    {
                        isStillUpper = false;
                    }
                }

                if (subWordBuilder.Length == 0) continue;
                if (i > 0) wordBuilder.Append('_');
                wordBuilder.Append(subWordBuilder);
            }
            if (wordBuilder.Length == 0)
                throw new ArgumentException($"Could not convert to Snake Case.", nameof(value));
            var snakeCaseWord = wordBuilder.ToString();
            return snakeCaseWord;
        }

        /// <summary>
        /// This method rips out punctuation except for underscores.
        /// </summary>
        /// <remarks>
        /// Found a nice article that explained a handy feature: https://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
        /// </remarks>
        public static string ToTitle(this string value, CultureInfo cultureInfo = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            value = value.ToRegexReplaceString("[^a-zA-Z0-9_ ]");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"Could not convert to Title Case.", nameof(value));
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentCulture;
            
            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Takes the first character of the string, and corrects the case of the first position. For LowerCase, UpperCase,
        /// or SnakeCase conventions it puts the whole word in lower or upper case based on convention. SnakeCase creates a
        /// lower case value, but adds in underscore separators when encountering a capitalized letter.
        /// </summary>
        /// <remarks>
        /// This could also be ToLowerCamelCase, however this does not automatically convert the entire word or words
        /// to lower camel case syntax, so the term here is to indicate usage, and its for parameters in our code,
        /// where we know the usage and know the method is safe.
        /// </remarks>
        /// <param name="value">The value to make the first initial lowercase.</param>
        /// <param name="namingConvention">The naming convention for the parameter.</param>
        /// <param name="cultureInfo">The culture to use for text changes for <paramref name="value"/>.</param>
        /// <returns>A first letter lowercased version of the string value parameter value.</returns>
        public static string ToConvention(this string value, NamingConvention namingConvention, CultureInfo cultureInfo = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"Could not convert to {namingConvention} Case.", nameof(value));
            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentCulture;

            switch (namingConvention)
            {
                case NamingConvention.Camel:
                    return value.ToCamel(cultureInfo);
                case NamingConvention.Lower:
                    return value.ToLower(cultureInfo);
                case NamingConvention.Pascal:
                    return value.ToPascal(cultureInfo);
                case NamingConvention.Snake:
                    return value.ToSnake(cultureInfo);
                case NamingConvention.Title:
                    return value.ToTitle(cultureInfo);
                case NamingConvention.Upper:
                    return value.ToUpper(cultureInfo);
                default:
                    return value.ToNone(cultureInfo);
            }
        }
    }
}
