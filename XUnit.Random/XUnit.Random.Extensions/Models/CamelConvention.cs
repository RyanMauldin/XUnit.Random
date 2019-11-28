using System;
using System.Globalization;
using System.Text;
using XUnit.Random.Extensions.Models.Abstractions;

namespace XUnit.Random.Extensions.Models
{
    /// <summary>
    /// <see cref="CamelConvention"/> is responsible for converting a string value to its Camel case form.
    /// </summary>
    public class CamelConvention : IConvention
    {
        /// <summary>
        /// Default comparer.
        /// </summary>
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        /// <summary>
        /// Default RegEx replacement value. Leaves underscore and space for natural word boundaries to split on.
        /// </summary>
        private const string Replace = "[^a-zA-Z0-9_ ]";

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
        public string Convert(string value, CultureInfo cultureInfo = null)
        {
            // Validate.
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Comparer.Equals(value, string.Empty)) return string.Empty;
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;

            // Remove all the does not match pattern, punctuation, etc.
            value = value.ToRegexReplaceString(Replace);

            // Validate after mutation.
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length == 1) return value.ToLower(cultureInfo);

            // Process words by splitting on underscores or whitespace.
            var words = value.Split(new[] { '_', ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // Speed performance consideration if it ends up that there is an underscore every other character.
            var wordBuilder = new StringBuilder(value.Length, value.Length * 2);
            // The isFirstUpperSet variable is a global word short circuit so we can let the second letter being
            // processed know, if the first letter encountered got capitalized. This value is reset per every new
            // word in the phrase. However, the first letter encountered overall should be lowercase, if not
            // proceeded by a number.
            var isFirstUpperSet = false;
            // For each split word.
            foreach (var word in words)
            {
                // Speed performance consideration if it ends up that there is an underscore every other character.
                var subWordBuilder = new StringBuilder(word.Length * 2);
                // For each word character.
                foreach (var wordCharacter in word)
                {
                    // Determine if it is upper case.
                    var isUpper = char.IsUpper(wordCharacter);
                    if (isUpper)
                    {
                        // Check our global lookahead flag.
                        if (isFirstUpperSet)
                        {
                            // Append lowercase letter and move on.
                            subWordBuilder.Append(wordCharacter);
                            continue;
                        }

                        // Set our global lookahead flag to true.
                        isFirstUpperSet = true;

                        // If it is the first character for any word, and no existing numbers have been placed,
                        // it should be a lowercase letter.
                        if (wordBuilder.Length == 0 && subWordBuilder.Length == 0)
                        {
                            // Append lowercase letter and move on.
                            subWordBuilder.Append(char.ToLower(wordCharacter, cultureInfo));
                        }
                        else
                        {
                            // Otherwise, append uppercase letter and move on.
                            subWordBuilder.Append(wordCharacter);
                        }
                        continue;
                    }

                    // Determine if it is lower case.
                    var isLower = char.IsLower(wordCharacter);
                    if (isLower)
                    {
                        // Check our global lookahead flag.
                        if (isFirstUpperSet)
                        {
                            // Set our global lookahead flag to false.
                            isFirstUpperSet = false;
                            // This is an existing lowercase letter or digit, just append it and move on,
                            // and set our global lookahead flag to false.
                            subWordBuilder.Append(wordCharacter);
                            continue;
                        }

                        // If it is the first character for any word, and no existing numbers have been placed,
                        // it should be a lowercase letter.
                        if (wordBuilder.Length == 0 && subWordBuilder.Length == 0)
                        {
                            // Set our global lookahead flag to true.
                            isFirstUpperSet = true;
                        }

                        // Append lowercase letter and move on.
                        subWordBuilder.Append(wordCharacter);
                        continue;
                    }

                    var isNumber = char.IsNumber(wordCharacter);
                    if (isNumber)
                    {
                        // This is an existing digit, just append it and move on, and set our global lookahead flag to false.
                        if (isFirstUpperSet) isFirstUpperSet = false;
                        subWordBuilder.Append(wordCharacter);
                        continue;
                    }
                    
                    // We hit an invalid character that does not fit in this scheme, and if they are the first characters
                    // encountered just skip them, otherwise we will need to set our global lookahead flag to false,
                    // as we will need to capitalize the next lowercase value.
                    if (subWordBuilder.Length > 0 && isFirstUpperSet) isFirstUpperSet = false;
                }

                // If no sub-word could be built, move on.
                if (subWordBuilder.Length == 0) continue;
                // A word could be built, append the sub-word.
                wordBuilder.Append(subWordBuilder);
            }

            // Return built word.
            var fullWord = wordBuilder.ToString();
            return fullWord;
        }
    }
}