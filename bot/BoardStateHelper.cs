using Engine.Enums;
using Engine.Models;
using OCR;
using OCR.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void SaveBoardImages(List<BoardImage> boardImages)
        {
            foreach (var boardImage in boardImages)
            {
                var boardImagePath = $"..\\..\\..\\images\\spliced\\{boardImage.Name}.png";
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

        internal int GetBetFromImage(string path)
        {
            throw new NotImplementedException();
        }

        internal int GetPotFromImage(string path)
        {
            throw new NotImplementedException();
        }

        internal bool GetIsDealerButtonFromImage(string path)
        {
            return _suitFinder.IsDealerButton(path);
        }
    }
}
