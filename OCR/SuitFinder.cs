using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OCR
{
    public class SuitFinder
    {
        public static List<Color> TenMostUsedColors { get; private set; }
        public static List<int> TenMostUsedColorIncidences { get; private set; }

        public static Color MostUsedColor { get; private set; }
        public static int MostUsedColorIncidence { get; private set; }

        private static int pixelColor;

        private static Dictionary<int, int> dctColorIncidence;

        public bool IsDealerButton(string path)
        {
            var topColor = GetTopRGBColor(path);

            return topColor != "green";
        }


        public CardSuit GetSuitFromImage(string path)
        {
            var topColor = GetTopRGBColor(path);

            if (topColor == "red")
            {
                return CardSuit.Hearts;
            }

            if (topColor == "blue")
            {
                return CardSuit.Diamonds;
            }

            if (topColor == "green")
            {
                return CardSuit.Clubs;
            }

            return CardSuit.Spades;
        }

        private string GetTopRGBColor(string path)
        {
            Bitmap theBitMap = Bitmap.FromFile(path) as Bitmap;

            TenMostUsedColors = new List<Color>();

            MostUsedColor = Color.Empty;
            MostUsedColorIncidence = 0;

            dctColorIncidence = new Dictionary<int, int>();

            for (int row = 0; row < theBitMap.Size.Width; row++)
            {
                for (int col = 0; col < theBitMap.Size.Height; col++)
                {
                    pixelColor = theBitMap.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }

            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow.Take(10))
            {
                TenMostUsedColors.Add(Color.FromArgb(kvp.Key));
            }

            var redCount = 0;
            var blueCount = 0;
            var greenCount = 0;

            foreach (var topcolor in TenMostUsedColors)
            {
                if (topcolor.G > topcolor.B && topcolor.G > topcolor.R)
                {
                    greenCount++;
                }

                if (topcolor.B > topcolor.G && topcolor.B > topcolor.R)
                {
                    blueCount++;
                }

                if (topcolor.R > topcolor.G && topcolor.R > topcolor.B)
                {
                    redCount++;
                }
            }

            if (redCount > blueCount && redCount > greenCount)
            {
                return "red";
            }

            if (blueCount > redCount && blueCount > greenCount)
            {
                return "blue";
            }

            if (greenCount > blueCount && greenCount > redCount)
            {
                return "green";
            }

            return "black";
        }

    }
}
