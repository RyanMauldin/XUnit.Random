using System;
using System.Globalization;
using XUnit.Random.Extensions.Models.Abstractions;

namespace XUnit.Random.Extensions.Models
{
    /// <summary>
    /// <see cref="TitleConvention"/> is responsible for converting a string value to its Title case form.
    /// </summary>
    public class TitleConvention : IConvention
    {
        /// <summary>
        /// Default comparer.
        /// </summary>
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with its Title case form. e.g. "The quick brown fox"
        /// is turned into "The Quick Brown Fox". Title case is a mixed-case style with all words capitalized, except for
        /// specific articles, short prepositions and conjunctions. Title case is defined by rules that are not
        /// universally standardized. See <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// See also <a href="https://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/">theburningmonk#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <remarks>
        /// As far as code is concerned, the whole <paramref name="value"/> string is converted to lowercase, prior to
        /// being Title cased. It is normal to have space between words with this convention.
        /// </remarks>
        /// <returns>Returns a copy of the <paramref name="value"/> string with its Title case form.</returns>
        public string Convert(string value, CultureInfo cultureInfo = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Comparer.Equals(value, string.Empty)) return string.Empty;
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }
    }
}