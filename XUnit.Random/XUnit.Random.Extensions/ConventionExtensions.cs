using System;
using System.Globalization;
using XUnit.Random.Extensions.Models;
using XUnit.Random.Extensions.Models.Abstractions;
using XUnit.Random.Extensions.Types;

namespace XUnit.Random.Extensions
{
    /// <summary>
    /// Provides Convention extensions that operate on the <seealso cref="IConvention"/> interface
    /// and supports Camel, Lower, Pascal, Snake, Title, and Upper conversion formats.
    /// </summary>
    public static class ConventionExtensions
    {
        private static readonly IConvention Camel;
        private static readonly IConvention Lower;
        private static readonly IConvention Pascal;
        private static readonly IConvention Snake;
        private static readonly IConvention Title;
        private static readonly IConvention Upper;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ConventionExtensions()
        {
            Camel = new CamelConvention();
            Lower = new LowerConvention();
            Pascal = new PascalConvention();
            Snake = new SnakeConvention();
            Title = new TitleConvention();
            Upper = new UpperConvention();
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string in its desired <paramref name="convention"/> capitalization
        /// format. Supports Camel, Lower, Pascal, Snake, Title, and Upper conversion formats.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="convention">The capitalization convention to convert to.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>
        /// Returns a copy of the <paramref name="value"/> string in its desired <paramref name="convention"/> capitalization format.
        /// </returns>
        public static string ToConvention(this string value, Convention convention, CultureInfo cultureInfo = null)
        {
            switch (convention)
            {
                case Convention.Camel:
                    return value.ToCamelConvention(cultureInfo);
                case Convention.Lower:
                    return value.ToLowerConvention(cultureInfo);
                case Convention.Pascal:
                    return value.ToPascalConvention(cultureInfo);
                case Convention.Snake:
                    return value.ToSnakeConvention(cultureInfo);
                case Convention.Title:
                    return value.ToTitleConvention(cultureInfo);
                case Convention.Upper:
                    return value.ToUpperConvention(cultureInfo);
                case Convention.None:
                    return value;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with spaces and punctuation removed. The first letter of
        /// each word is capitalized, however with the exception the starting letter is lower case. e.g. "The quick brown fox"
        /// is converted into "theQuickBrownFox". This is handy for software development for parameter names. See
        /// <a href="https://en.wikipedia.org/wiki/Camel_case">wikipedia#camel</a>. See also
        /// <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>
        /// Returns a copy of the <paramref name="value"/> string with spaces and punctuation removed. The first letter of
        /// each word is capitalized.
        /// </returns>
        public static string ToCamelConvention(this string value, CultureInfo cultureInfo = null) 
        {
            return Camel.Convert(value, cultureInfo);
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with lowercase letters. e.g. "The quick brown fox" is
        /// converted into "the quick brown fox". For case-insensitive comparisons or sorting UI components
        /// where a high degree of predictability is a requirement, then do no use this method, but see instead:
        /// <seealso cref="UpperConvention.Convert"/>. See also <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>Returns a copy of the <paramref name="value"/> string with lower case letters.</returns>
        public static string ToLowerConvention(this string value, CultureInfo cultureInfo = null)
        {
            return Lower.Convert(value, cultureInfo);
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with spaces and punctuation removed. The first letter of
        /// each word is capitalized. e.g. "The quick brown fox" is converted into "TheQuickBrownFox". This is handy for
        /// software development for class method names. See <a href="https://en.wikipedia.org/wiki/Camel_case">wikipedia#camel</a>.
        /// See also <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>
        /// Returns a copy of the <paramref name="value"/> string with spaces and punctuation removed. The first letter of
        /// each word is capitalized.
        /// </returns>
        public static string ToPascalConvention(this string value, CultureInfo cultureInfo = null)
        {
            return Pascal.Convert(value, cultureInfo);
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with snake case format, where punctuation is removed,
        /// and spaces are replaced by single underscores. e.g. "The quick brown fox" is converted into "the_quick_brown_fox".
        /// See <a href="https://en.wikipedia.org/wiki/Snake_case">wikipedia#snake</a>. See also
        /// <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>
        /// Returns a copy of the <paramref name="value"/> string with with snake case format, where punctuation is removed,
        /// and spaces are replaced by single underscores.
        /// </returns>
        public static string ToSnakeConvention(this string value, CultureInfo cultureInfo = null)
        {
            return Snake.Convert(value, cultureInfo);
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with its Title case form. e.g. "The quick brown fox" is turned into "The Quick Brown Fox".
        /// Title case is a mixed-case style with all words capitalized, except for specific articles, short prepositions and conjunctions.
        /// Title case is defined by rules that are not universally standardized. See
        /// <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>. See also
        /// <a href="https://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/">theburningmonk#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <remarks>
        /// As far as code is concerned, the whole <paramref name="value"/> string is converted to lowercase, prior to
        /// being Title cased. It is normal to have space between words with this convention.
        /// </remarks>
        /// <returns>Returns a copy of the <paramref name="value"/> string with its Title case form.</returns>
        public static string ToTitleConvention(this string value, CultureInfo cultureInfo = null)
        {
            return Title.Convert(value, cultureInfo);
        }

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with capitalized letters. e.g. "The quick brown fox" is
        /// converted into "THE QUICK BROWN FOX". This method can be useful for formatting main document headings, or leveraged
        /// in identifying value equality for case-insensitive string comparisons. Microsoft documentation suggests using the
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/system.string.toupperinvariant?view=netframework-4.8">ToUpperInvariant</a>
        /// method for achieving the most predictive case-insensitive UI sorting experience. The <paramref name="cultureInfo"/>
        /// parameter should be passed the <seealso cref="CultureInfo.InvariantCulture"/> value, to facilitate
        /// <seealso cref="string.ToUpperInvariant"/> type functionality. Consideration should be taken to avoid usage of all
        /// capital values, in contexts where all uppercase words can be read by a user, in the same connotation as shouting.
        /// See <a href="https://en.wikipedia.org/wiki/All_caps">wikipedia#caps</a>. See also
        /// <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>Returns a copy of the <paramref name="value"/> string with capitalized letters.</returns>
        public static string ToUpperConvention(this string value, CultureInfo cultureInfo = null)
        {
            return Upper.Convert(value, cultureInfo);
        }
    }
}