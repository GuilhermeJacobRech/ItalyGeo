using System.Globalization;
using System.Text.RegularExpressions;

namespace WikiDataExtractor.Helpers
{
    public class StringHelper
    {
        public static string SanitizeString(string s, bool keepNumbers, bool keepChars)
        {
            s = s.Replace("\n", string.Empty);
            s = s.Replace("\r", string.Empty);
            s = s.Replace("\t", string.Empty);
            s = s.Trim();

            string pattern;

            if (keepNumbers && !keepChars)
            {
                // Keep only numbers, commas, and dots if keepNumbers is true and keepChars is false
                pattern = @"[^0-9,.]";
                s = Regex.Replace(s, pattern, "");

                // Remove trailing comma or dot if it exists
                s = Regex.Replace(s, "[.,]+$", "");
            }
            else if (!keepNumbers && keepChars)
            {
                // Keep only letters, spaces, and specific characters if keepNumbers is false and keepChars is true
                pattern = @"[^\p{L}\s-_()']";
                s = Regex.Replace(s, pattern, "");
            }
            else if (keepNumbers && keepChars)
            {
                // Keep letters, numbers, spaces, and specific characters if both keepNumbers and keepChars are true
                pattern = @"[^\p{L}\d\s-_()'+]";
                s = Regex.Replace(s, pattern, "");
            }
            else
            {
                // Remove both numbers and letters, only keeping spaces and specific characters if both are false
                pattern = @"[^\s-_()']";
                s = Regex.Replace(s, pattern, "");
            }

            return s;
        }

        public static int ConvertToInt(string s)
        {
            Regex regex = new Regex(@"\D");
            s = regex.Replace(s, string.Empty);
            if (int.TryParse(s, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static float ConvertToFloat(string s)
        {
            s = Regex.Replace(s, @"[^\d.,]", "");

            if (s.Contains(",") && s.Contains("."))
            {
                if (s.LastIndexOf(',') > s.LastIndexOf('.'))
                {
                    s = s.Replace(".", "");
                    s = s.Replace(",", ".");
                }
                else
                {
                    s = s.Replace(",", "");
                }
            }
            else if (s.Contains(","))
            {
                s = s.Replace(",", ".");
            }

            if (float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
