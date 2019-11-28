using System;
using System.Globalization;
using XUnit.Random.Extensions.Models.Abstractions;

namespace XUnit.Random.Extensions.Models
{
    /// <summary>
    /// <see cref="LowerConvention"/> is responsible for converting a string value to its Lower case form.
    /// </summary>
    public class LowerConvention : IConvention
    {
        /// <summary>
        /// Default comparer.
        /// </summary>
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with lowercase letters. e.g. "The quick brown fox" is
        /// converted into "the quick brown fox". For case-insensitive comparisons or sorting UI components
        /// where a high degree of predictability is a requirement, then do no use this method, but see instead:
        /// <seealso cref="UpperConvention.Convert"/>. See also <a href="https://en.wikipedia.org/wiki/Letter_case#Title_Case">wikipedia#title</a>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>Returns a copy of the <paramref name="value"/> string with lower case letters.</returns>
        public string Convert(string value, CultureInfo cultureInfo = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Comparer.Equals(value, string.Empty)) return string.Empty;
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            return value.ToLower(cultureInfo);
        }
    }
}