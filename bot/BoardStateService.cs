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

                var boardState = new BoardState();

                _boardStateHelper.SaveBoardImages(boardImages, path);

                foreach (var boardImage in boardImages)
                {
                    var boardImagepath = $"{path}\\spliced\\{boardImage.Name}.png";

                    if (boardImage.Type == OCR.Objects.ImageType.PlayerStack)
                    {
                        _boardStateHelper.SetPlayerStack(boardImagepath, boardImage, boardState);
                    }
                    else if (boardImage.Type == OCR.Objects.ImageType.PlayerDealerButton)
                    {
                        _boardStateHelper.SetPlayerIsDealer(boardImagepath, boardImage, boardState);
                    }
                    else if (boardImage.Type == OCR.Objects.ImageType.PlayerBet)
                    {
                        _boardStateHelper.SetPlayerBet(boardImagepath, boardImage, boardState);
                    }
                    else
                    {
                        boardState[boardImage.Name.ToString()] = boardImage.Type switch
                        {
                            OCR.Objects.ImageType.Number => _boardStateHelper.GetNumberFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.Card => _boardStateHelper.GetCardFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.Pot => _boardStateHelper.GetNumberFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.Word => _boardStateHelper.GetWordFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.BigBlind => _boardStateHelper.GetBigBlindFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.ReadyForAction => _boardStateHelper.GetReadyForAction(boardImage.Image, boardImagepath),
                            _ => null
                        };
                    }

                    boardImage.Image.Dispose();
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
