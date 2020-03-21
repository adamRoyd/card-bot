using bot.Helpers;
using Engine.Models;
using OCR;
using OCR.Objects;
using System;
using System.Collections.Generic;

namespace bot.Services
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

        public BoardState SetGameStatus(string path, BoardState boardState)
        {
            try
            {
                BoardImages boardImages = new BoardImages();

                List<BoardImage> gameStatusImages = _imageProcessor.SliceBoardScreenShot(path, boardImages.GameStatusImages);
                _boardStateHelper.SaveBoardImages(gameStatusImages, path);

                foreach (var boardImage in gameStatusImages)
                {
                    var boardImagepath = $"{path}\\spliced\\{boardImage.Name}.png";

                    boardState[boardImage.Name.ToString()] = boardImage.Type switch
                    {
                        ImageType.ReadyForAction => _boardStateHelper.GetReadyForAction(boardImage.Image, boardImagepath),
                        ImageType.GameIsFinished => _boardStateHelper.GetGameIsFinished(boardImage.Image, boardImagepath),
                        ImageType.IsInPlay => _boardStateHelper.GetIsInPlay(boardImage.Image, boardImagepath),
                        ImageType.SittingOut => _boardStateHelper.GetHeroSittingOut(boardImage.Image, boardImagepath),
                        _ => null
                    };

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

        public BoardState SetLiveHand(string path, BoardState boardState, Player[] playersFromPreviousHand)
        {
            try
            {
                BoardImages boardImages = new BoardImages();

                List<BoardImage> liveHandImages = _imageProcessor.SliceBoardScreenShot(path, boardImages.LiveHandList);
                _boardStateHelper.SaveBoardImages(liveHandImages, path);

                foreach (var boardImage in liveHandImages)
                {
                    var boardImagepath = $"{path}\\spliced\\{boardImage.Name}.png";

                    if (boardImage.Type == OCR.Objects.ImageType.PlayerStack)
                    {
                        _boardStateHelper.SetPlayerStack(boardImagepath, boardImage, boardState, playersFromPreviousHand);
                    }
                    else if (boardImage.Type == OCR.Objects.ImageType.PlayerDealerButton)
                    {
                        _boardStateHelper.SetPlayerIsDealer(boardImagepath, boardImage, boardState);
                    }
                    else
                    {
                        boardState[boardImage.Name.ToString()] = boardImage.Type switch
                        {
                            ImageType.Number => _boardStateHelper.GetNumberFromImage(boardImage.Image, boardImagepath),
                            ImageType.Card => _boardStateHelper.GetCardFromImage(boardImage.Image, boardImagepath),
                            ImageType.Pot => _boardStateHelper.GetNumberFromImage(boardImage.Image, boardImagepath),
                            ImageType.Word => _boardStateHelper.GetWordFromImage(boardImage.Image, boardImagepath),
                            ImageType.BigBlind => _boardStateHelper.GetBigBlindFromImage(boardImage.Image, boardImagepath),
                            ImageType.Ante => _boardStateHelper.GetAnteFromImage(boardImage.Image, boardImagepath),
                            _ => null
                        };
                    }

                    boardImage.Image.Dispose();
                }

                _boardStateHelper.SetPlayerBets(boardState);

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
