using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace WikiDataExtractor.Helpers
{
    public class StringHelper
    {
        public static string SanitizeString(string s)
        {
            s = s.Replace("\n", string.Empty);
            s = s.Replace("\r", string.Empty);
            s = s.Replace("\t", string.Empty);
            s = s.Trim();
            Regex regex = new Regex(@"[^\p{L}\s-_()']");
            return regex.Replace(s, string.Empty);
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
