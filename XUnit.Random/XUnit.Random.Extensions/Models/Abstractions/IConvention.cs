using System.Globalization;

namespace XUnit.Random.Extensions.Models.Abstractions
{
    /// <summary>
    /// <see cref="IConvention"/> is responsible for string values to other capitalization conventions.
    /// </summary>
    public interface IConvention
    {
        /// <summary>
        /// Returns a copy of the <paramref name="value"/> string with a converted value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="cultureInfo">The culture information settings.</param>
        /// <returns>Returns a copy of the <paramref name="value"/> string with converted letter case.</returns>
        string Convert(string value, CultureInfo cultureInfo = null);
    }
}