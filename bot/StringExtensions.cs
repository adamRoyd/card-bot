using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace bot
{
    public static class StringExtensions
    {
        public static string GetNumbers(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public static string CleanUp(this string input)
        {
            return input.ToLower()
                .Replace("pot", "")
                .Replace(":", "")
                .Replace(",", "")
                .Replace(" ", "").Trim();
        }
    }
}
