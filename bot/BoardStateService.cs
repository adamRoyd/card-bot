using Engine.Models;
using OCR;
using System;
using System.Collections.Generic;
using System.IO;
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
                var boardImagepath = $"..\\..\\..\\images\\spliced\\{boardImage.Name}.png";

                boardState[boardImage.Name.ToString()] = boardImage.Type switch
                {
                    OCR.Objects.ImageType.Card => _boardStateHelper.GetCardFromImage(boardImage.Image, boardImagepath),
                    OCR.Objects.ImageType.Bet => _boardStateHelper.GetBetFromImage(boardImagepath),
                    OCR.Objects.ImageType.Pot => _boardStateHelper.GetPotFromImage(boardImage.Image, boardImagepath),
                    OCR.Objects.ImageType.DealerButton => _boardStateHelper.GetIsDealerButtonFromImage(boardImagepath)
                };
            }

            return boardState;
        }
    }
}
