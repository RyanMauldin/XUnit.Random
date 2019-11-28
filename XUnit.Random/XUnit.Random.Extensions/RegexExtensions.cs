using System.Text.RegularExpressions;

namespace XUnit.Random.Extensions
{
    public static class RegexExtensions
    {
        public static string ToRegexReplaceString(this string value, string pattern, string replacement = null,
            RegexOptions options = RegexOptions.None)
        {
            if (replacement == null) replacement = string.Empty;
            var regex = new Regex(pattern, options);
            return regex.Replace(value, replacement);
        }
    }
}