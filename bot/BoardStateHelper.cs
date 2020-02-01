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

            result = result.CleanUp().GetNumbers();

            int.TryParse(result, out int value);

            if (value == 341)
            {
                value = 0;
            }

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

            result = result.CleanUp();

            var index = boardImage.PlayerNumber - 1;

            string colour = _suitFinder.GetBlackOrWhite(path);

            //Set active / inactive 
            if (result.Contains("itting") || colour == "white")
            {
                state.Players[index].Eliminated = true;
            }
            else
            {
                result = result.GetNumbers();

                int.TryParse(result, out int value);

                state.Players[index].Eliminated = false;
                state.Players[index].Stack = value;
            }    
        }

        internal void SetPlayerBet(string boardImagepath, BoardImage boardImage, BoardState state)
        {
            var value = GetNumberFromImage(boardImage.Image, boardImagepath);
            var index = boardImage.PlayerNumber - 1;

            state.Players[index].Bet = value;
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

            var bigBlind = result.Trim().Split(" ")[0];

            return bigBlind switch
            {
                "1020" => 20,
                "1530" => 30,
                "2550" => 50,
                "50100" => 100,
                "75150" => 150,
                "100200" => 200,
                "150300" => 300,
                "200400" => 400,
                "300600" => 600,
                _ => 0
            };

        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
