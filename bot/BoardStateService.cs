using Engine.Models;
using OCR;
using System;

namespace bot
{
    public class BoardStateService : IBoardStateService
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IBoardStateHelper _boardStateHelper;

        public BoardStateService(
            IImageProcessor imageProcessor,
            IBoardStateHelper boardStateHelper        
        )
        {
            _imageProcessor = imageProcessor;
            _boardStateHelper = boardStateHelper;
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
                            OCR.Objects.ImageType.Ante => _boardStateHelper.GetAnteFromImage(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.ReadyForAction => _boardStateHelper.GetReadyForAction(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.GameIsFinished => _boardStateHelper.GetGameIsFinished(boardImage.Image, boardImagepath),
                            OCR.Objects.ImageType.IsInPlay => _boardStateHelper.GetIsInPlay(boardImage.Image, boardImagepath),
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
