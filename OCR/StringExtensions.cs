using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OCR
{
    public static class StringExtensions
    {
        public static string StripPunctuation(this string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveLineBreaks(this string s)
        {
            var result = Regex.Replace(s, @"\r\n?|\n", "");
            return result;
        }
    }
}
