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
            try
            {
                var boardImages = _imageProcessor.SliceBoardScreenShot(path);

                var boardState = new BoardState
                {
                    Players = new Player[] 
                    {
                        new Player(),
                        new Player(),
                        new Player(),
                        new Player(),
                        new Player(),
                        new Player(),
                        new Player(),
                        new Player(),
                        new Player()
                    }
                };

                _boardStateHelper.SaveBoardImages(boardImages, path);

                foreach (var boardImage in boardImages)
                {
                    var boardImagepath = $"{path}\\spliced\\{boardImage.Name}.png";

                    if (boardImage.Type == OCR.Objects.ImageType.PlayerStack)
                    {
                        _boardStateHelper.SetPlayerStack(boardImage, boardState);
                    }
                    else if (boardImage.Type == OCR.Objects.ImageType.PlayerDealerButton)
                    {
                        // TODO
                        //_boardStateHelper.GetIsDealerButtonFromImage(boardImagepath)
                    }
                    else
                    {
                        boardState[boardImage.Name.ToString()] = boardImage.Type switch
                        {
                            //TODO rename these image types to Number / Word etc
                            OCR.Objects.ImageType.Card => _boardStateHelper.GetCardFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.Bet => _boardStateHelper.GetNumberFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.Pot => _boardStateHelper.GetNumberFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.Word => _boardStateHelper.GetWordFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.BigBlind => _boardStateHelper.GetBigBlindFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.ReadyForAction => _boardStateHelper.GetReadyForAction(boardImage.Image, boardImagepath)
                        };
                    }
                }

                return boardState;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
