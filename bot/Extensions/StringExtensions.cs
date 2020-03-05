using System.Linq;

namespace bot.Extensions
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
                .Replace("s", "5")
                .Replace(" ", "").Trim();
        }

        public static string RemoveOnes(this string input, int playerNumber)
        {
            var result = input;

            if (playerNumber < 6)
            {
                // remove trailing 1s
                var arry = input.ToCharArray();

                for (var i = arry.Length - 1; i >= 0; i--)
                {
                    if (arry[i] == '1')
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    else
                    {
                        break;
                    }
                }

                //if string is over 4 characters, remove another
                while (result.Length > 4)
                {
                    result = result.Remove(result.Length - 1);
                }
            }
            else
            {
                // remove leading 1s
                var arry = input.ToCharArray();

                for (var i = 0; i <= arry.Length; i++)
                {
                    if (i == arry.Length) break;

                    if (arry[i] == '1' && arry[i + 1] == '1')
                    {
                        result = result.Remove(0, 1);
                    }
                    else
                    {
                        break;
                    }
                }

                //if string is over 4 characters, remove another
                while (result.Length > 4)
                {
                    result = result.Remove(0, 1);
                }
            }

            return result;
        }
    }
}
