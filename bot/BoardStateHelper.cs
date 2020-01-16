using Engine.Enums;
using Engine.Models;
using OCR;
using OCR.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Tesseract;

namespace bot
{
    public class BoardStateHelper
    {
        private readonly ImageProcessor _imageProcessor;
        private readonly SuitFinder _suitFinder;

        public BoardStateHelper(
            ImageProcessor imageProcessor,
            SuitFinder suitFinder
        )
        {
            _imageProcessor = imageProcessor;
            _suitFinder = suitFinder;
        }

        public void SaveBoardImages(List<BoardImage> boardImages, string path)
        {            
            foreach (var boardImage in boardImages)
            {
                var boardImagePath = $"{path}\\spliced\\{boardImage.Name}.png";

                if (File.Exists(boardImagePath))
                {
                    return;
                }

                boardImage.Image.Save(boardImagePath);
            }
        }

        internal Card GetCardFromImage(Image img, string path)
        {
            var value = _imageProcessor.GetCardValueFromImage(img);
            var suit = _suitFinder.GetSuitFromImage(path);

            if (value == 0) // No card found
            {
                return null;
            }

            return new Card(value, suit);
        }

        internal int GetNumberFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);
            result = result.ToLower().Replace("pot", "").Replace(":", "").Replace(",", "").Trim();
            int.TryParse(result, out int value);
            return value;
        }

        internal string GetWordFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);
            result = result.ToLower().Trim();
            return result;
        }

        internal bool GetReadyForAction(Image image, string path)
        {
            string result = _suitFinder.GetTopRGBColor(path);
            return result == "red";
        }

        internal void SetPlayerStack(string path, BoardImage boardImage, BoardState state)
        {
            string result = _imageProcessor.GetImageCharacters(boardImage.Image, PageSegMode.Auto);

            int.TryParse(result, out int value);

            var index = boardImage.PlayerNumber - 1;

            state.Players[index].Stack = value;

            //Set active / inactive 
            string colour = _suitFinder.GetBlackOrWhite(path);
            state.Players[index].Eliminated = colour == "white";
        }

        internal void SetPlayerIsDealer(string path, BoardImage boardImage, BoardState state)
        {
            var isDealer = _suitFinder.IsDealerButton(path);
            var index = boardImage.PlayerNumber - 1;
            state.Players[index].IsDealer = isDealer;

        }

        internal int GetBigBlindFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);

            var arry = result.Trim().Replace(" ", "").ToCharArray();
            string bigBlind = GetNumbersAtEndOfString(arry);
            bigBlind = Reverse(bigBlind);

            return bigBlind switch
            {
                "1530" => 20,
                "2550" => 30,
                "50100" => 50,
                "75150" => 100,
                "100200" => 150,
                "150300" => 200,
                "200400" => 300,
                _ => 0
            };

        }

        private static string GetNumbersAtEndOfString(char[] arry)
        {
            string bigBlind = "";

            for (var i = arry.Length - 1; i >= 0; i--)
            {
                var test = arry[i].ToString();
                if (int.TryParse(test, out int number))
                {
                    bigBlind = bigBlind + test;
                }
                else
                {
                    break;
                }
            }

            return bigBlind;
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
