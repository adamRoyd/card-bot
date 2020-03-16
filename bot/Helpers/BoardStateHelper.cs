using bot.Extensions;
using Engine.Models;
using OCR;
using OCR.Objects;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Tesseract;

namespace bot.Helpers
{
    public class BoardStateHelper : IBoardStateHelper
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly ISuitFinder _suitFinder;

        public BoardStateHelper(
            IImageProcessor imageProcessor,
            ISuitFinder suitFinder
        )
        {
            _imageProcessor = imageProcessor;
            _suitFinder = suitFinder;
        }

        public void SaveBoardImages(List<BoardImage> boardImages, string path)
        {
            foreach (BoardImage boardImage in boardImages)
            {
                string boardImagePath = $"{path}\\spliced\\{boardImage.Name}.png";

                if (File.Exists(boardImagePath))
                {
                    return;
                }

                boardImage.Image.Save(boardImagePath);
            }
        }

        public Card GetCardFromImage(Image img, string path)
        {
            Engine.Enums.CardValue value = _imageProcessor.GetCardValueFromImage(img);
            Engine.Enums.CardSuit suit = _suitFinder.GetSuitFromImage(path);

            if (value == 0) // No card found
            {
                return null;
            }

            return new Card(value, suit);
        }

        public int GetNumberFromImage(Image image, string path)
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

        public string GetWordFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);
            result = result.ToLower().Trim();
            return result;
        }

        public bool GetReadyForAction(Image image, string path)
        {
            string result = _suitFinder.GetTopRGBColor(path);
            return result == "red";
        }

        public void SetPlayerStack(string path, BoardImage boardImage, BoardState state)
        {
            string result = _imageProcessor.GetImageCharacters(boardImage.Image, PageSegMode.Auto);

            result = result.CleanUp();

            int index = boardImage.PlayerNumber - 1;

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

        public object GetGameIsFinished(Image image, string path)
        {
            string value = GetWordFromImage(image, path);

            if (value.Contains("thank"))
            {
                return true;
            }

            return false;
        }

        public bool GetIsInPlay(Image image, string path)
        {
            string result = _suitFinder.GetTopRGBColor(path);
            return result == "green";
        }

        public bool GetSittingOut(Image image, string path)
        {
            string value = GetWordFromImage(image, path);

            return value.ToLower().Contains("back");
        }

        public void SetPlayerBets(BoardState state)
        {
            for (var i = 0; i < state.Players.Length; i++)
            {
                state.Players[i].Bet = state.PlayersFromPreviousHand[i].Stack - state.Players[i].Stack;
            }
        }

        public void SetPlayerIsDealer(string path, BoardImage boardImage, BoardState state)
        {
            bool isDealer = _suitFinder.IsDealerButton(path);
            int index = boardImage.PlayerNumber - 1;
            state.Players[index].IsDealer = isDealer;

        }

        public int GetBigBlindFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);

            string bigBlind = result.Trim().Split(" ")[0];

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
                "400800" => 800,
                "5001000" => 1000,
                _ => 0
            };
        }

        public int GetAnteFromImage(Image image, string path)
        {
            string result = _imageProcessor.GetImageCharacters(image, PageSegMode.Auto);

            if (result.Trim().Split(" ").Length < 3)
            {
                return 0;
            }

            string ante = result.Trim().Split(" ")[2];

            int.TryParse(ante, out int parsed);

            return parsed;
        }


    }
}
