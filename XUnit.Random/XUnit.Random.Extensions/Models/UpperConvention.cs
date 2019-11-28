using System;
using System.Globalization;
using XUnit.Random.Extensions.Models.Abstractions;

namespace XUnit.Random.Extensions.Models
{
    /// <summary>
    /// <see cref="UpperConvention"/> is responsible for converting a string value to its Upper case form.
    /// </summary>
    public class UpperConvention : IConvention
    {
        /// <summary>
        /// Default comparer.
        /// </summary>
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

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
        public string Convert(string value, CultureInfo cultureInfo = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Comparer.Equals(value, string.Empty)) return string.Empty;
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            return value.ToUpper(cultureInfo);
        }
    }
}