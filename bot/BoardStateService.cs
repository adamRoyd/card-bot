using Engine.Models;
using OCR;
using System;
using System.Collections.Generic;
using System.Text;
using Tesseract;

namespace bot
{
    public class BoardStateService
    {
        private readonly ImageProcessor _imageProcessor;
        private readonly BoardStateHelper _boardStateHelper;
        private readonly SuitFinder _suitFinder;

        public BoardStateService(
            ImageProcessor imageProcessor, 
            BoardStateHelper boardStateHelper, 
            SuitFinder suitFinder
        )
        {
            _imageProcessor = imageProcessor;
            _boardStateHelper = boardStateHelper;
            _suitFinder = suitFinder;
        }

        public BoardState GetBoardStateFromImagePath(string path)
        {
            var boardImages = _imageProcessor.SliceBoardScreenShot(path);

            var boardState = new BoardState();

            _boardStateHelper.SaveBoardImages(boardImages);

            foreach (var boardImage in boardImages)
            {
                var boardImagePath = $"..\\..\\..\\images\\spliced\\{boardImage.Name}.png";

                var image = Pix.LoadFromFile(boardImagePath);
                var value = _imageProcessor.GetCardValueFromImage(image);
                var suit = _suitFinder.GetSuitFromColor(boardImagePath);

                // if type card...
                boardState[boardImage.Name.ToString()] = new Card(value, suit);
            }

            return boardState;
        }
    }
}
