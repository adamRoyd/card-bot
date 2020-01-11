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

        internal void SetPlayerStack(BoardImage boardImage, BoardState state)
        {
            string result = _imageProcessor.GetImageCharacters(boardImage.Image, PageSegMode.Auto);

            int.TryParse(result, out int value);

            var index = boardImage.PlayerNumber - 1;

            state.Players[index].Stack = value;
        }

        internal int GetBigBlindFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);

            if (result.Split(" ").Length < 4)
            {
                return 0;
            }

            var bigBlind = result.Split(" ")[3];

            if(bigBlind == "1530")
            {
                return 20;
            }
            if (bigBlind == "2550")
            {
                return 30;
            }
            if(bigBlind == "50100")
            {
                return 50;
            }
            if(bigBlind == "75150")
            {
                return 100;
            }
            if(bigBlind == "100200")
            {
                return 150;
            }
            if(bigBlind == "150300")
            {
                return 200;
            }
            return 0;
        }

        internal bool GetIsDealerButtonFromImage(string path)
        {
            return _suitFinder.IsDealerButton(path);
        }
    }
}
