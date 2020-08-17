using bot.Extensions;
using Engine.Models;
using OCR;
using OCR.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public void SetPlayerStack(string path, BoardImage boardImage, BoardState state, Player[] playersFromPreviousHand)
        {
            string result = _imageProcessor.GetImageCharacters(boardImage.Image, PageSegMode.Auto);

            result = result.CleanUp();

            int index = boardImage.PlayerNumber - 1;

            string colour = _suitFinder.GetBlackOrWhite(path);

            if (colour == "white")
            {
                state.Players[index].Eliminated = true;
            }
            else if (result.Contains("itting"))
            {
                state.Players[index].Stack = playersFromPreviousHand[index].Stack;
                state.Players[index].SittingOut = true;
            }
            else
            {
                result = result.GetNumbers();

                int.TryParse(result, out int value);

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

        public bool GetHeroSittingOut(Image image, string path)
        {
            string value = GetWordFromImage(image, path);

            return value.ToLower().Contains("back");
        }

        public void SetPlayerBets(BoardState state)
        {
            for (var i = 0; i < state.Players.Length; i++)
            {
                state.Players[i].Bet = state.PlayersFromPreviousHand[i].Stack - state.Players[i].Stack - state.Ante;
            }

            // If no big blind bet found, it should be on a player sitting out
            if (state.Players.All(p => p.Bet != state.BigBlind) && state.HandStage == Engine.Enums.HandStage.PreFlop && state.ReadyForAction)
            {
                Console.WriteLine("No big blind found! Attempting set...");
                var filteredPlayers = state.Players.Where(p => !p.Eliminated).ToArray();
                var smallBlind = filteredPlayers.FirstOrDefault(p => p.Bet == (state.BigBlind / 2));

                if (smallBlind != null)
                {
                    var bigBlindIndex = Array.IndexOf(filteredPlayers, smallBlind) - 1;
                    var bigBlindPlayer = filteredPlayers[bigBlindIndex];
                    state.Players.FirstOrDefault(p => p.Position == bigBlindPlayer.Position).Bet = state.BigBlind;
                    state.Players.FirstOrDefault(p => p.Position == bigBlindPlayer.Position).Stack -= state.BigBlind;
                }
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
                "4080" => 80,
                "50100" => 100,
                "60120" => 120,
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
