﻿using bot.Logging;
using Engine.Enums;
using Engine.Models;
using ICM;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bot.Services
{
    public class PokerBotService : IPokerBotService
    {
        private readonly Random rnd = new Random();
        private readonly IBoardStateService _boardStateService;
        private readonly IIcmService _icmService;
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly IHandHistoryService _handHistoryService;
        private readonly ILogger _logger;

        public PokerBotService
        (
            IBoardStateService boardStateService,
            IIcmService icmService,
            IScreenCaptureService screenCaptureService,
            IHandHistoryService handHistoryService,
            ILogger<PokerBotService> logger
        )
        {
            _boardStateService = boardStateService;
            _icmService = icmService;
            _screenCaptureService = screenCaptureService;
            _handHistoryService = handHistoryService;
            _logger = logger;
        }

        public async Task Run()
        {
            while (true)
            {
                try
                {
                    string dateStamp = DateTime.Now.ToString("hhmmss");
                    //dateStamp = "021526";

                    string path = $"..\\..\\..\\images\\{dateStamp}";
                    string splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        Directory.CreateDirectory(splicedPath);

                        _screenCaptureService.CaptureScreenToFile($"{path}\\board.png", ImageFormat.Png);
                    }

                    string historyPath = $"C:\\Temp\\handhistories\\CannonballJim";
                    Player[] players = _handHistoryService.GetPlayersFromHistory(historyPath);

                    BoardState boardState = _boardStateService.GetBoardStateFromImagePath(path, players);

                    if (boardState.SittingOut)
                    {
                        Console.WriteLine("Sitting out!");
                        await ClickImBackButton();
                        continue;
                    }

                    if (boardState.GameIsFinished)
                    {
                        break;
                        await RegisterForNewGame();
                        await WaitForGameToStart(_boardStateService, _screenCaptureService);
                        continue;
                    }

                    if (!boardState.ReadyForAction || boardState.HandCode == "null")
                    {
                        DeleteFiles(path);
                        continue;
                    }

                    PredictedAction predictedAction;

                    if (boardState.MyStackRatio > 20 && boardState.NumberOfPlayers > 4)
                    {
                        predictedAction = new EarlyGamePredictedAction(boardState);
                    }
                    else
                    {
                        double ev = 0;

                        if (boardState.HandStage == HandStage.PreFlop)
                        {
                            ev = _icmService.GetExpectedValue(boardState);
                        }

                        predictedAction = new PushFoldPredictedAction(boardState, ev);
                    }

                    LogStats(dateStamp, boardState, predictedAction);
                    //break;
                    DoAction(predictedAction, boardState);

                    await Task.Delay(2000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async Task RegisterForNewGame()
        {
            Console.WriteLine("REGISTERING");
            await Task.Delay(rnd.Next(1000, 3000));
            SendKeys.SendWait("{LEFT}");
            await Task.Delay(rnd.Next(1000, 3000));
            SendKeys.SendWait("{ENTER}");
            await Task.Delay(rnd.Next(2000, 4000));
            SendKeys.SendWait("{ENTER}");
        }

        private async Task WaitForGameToStart(
            IBoardStateService boardStateService,
            IScreenCaptureService screenCaptureService)
        {
            while (true)
            {
                Console.WriteLine("Waiting for game...");
                // if isInPlay, carry on
                // otherwise retake screenshots, wait and try again
                string dateStamp = DateTime.Now.ToString("hhmmss");

                string path = $"..\\..\\..\\images\\{dateStamp}";
                string splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    screenCaptureService.CaptureScreenToFile($"{path}\\board.png", ImageFormat.Png);
                }

                BoardState boardState = boardStateService.GetBoardStateFromImagePath(path, null);

                if (!boardState.IsInPlay)
                {
                    DeleteFiles(path);
                    continue;
                }

                Console.WriteLine("GAME HAS STARTED. Maximising...");
                SendKeys.SendWait("m");

                break;
            }

            await Task.Delay(rnd.Next(500, 1000));

            // Select info tab
            Point infoPosition = new Point(rnd.Next(759, 764), rnd.Next(845, 849));
            await MouseService.LinearSmoothMove(infoPosition, 60);
            MouseService.Click();
        }

        private async Task ClickImBackButton()
        {
            await Task.Delay(rnd.Next(500, 1000));
            Point infoPosition = new Point(rnd.Next(1370, 1390), rnd.Next(960, 980));
            await MouseService.LinearSmoothMove(infoPosition, 60);
            MouseService.Click();
            await Task.Delay(rnd.Next(500, 1000));
            infoPosition = new Point(rnd.Next(760, 764), rnd.Next(940, 944));
            await MouseService.LinearSmoothMove(infoPosition, 60);
            await Task.Delay(rnd.Next(500, 1000));

        }

        private void DoAction(PredictedAction action, BoardState state)
        {
            if (action == null || state.HandStage != HandStage.PreFlop)
            {
                return;
            }

            string keyPress = action.GetAction() switch
            {
                ActionType.Fold => "f",
                ActionType.Check => "c",
                ActionType.Limp => "f",
                ActionType.Unknown => "f",
                ActionType.AllIn => "i",
                ActionType.Bet => "f",
                ActionType.Raise => "i",
                _ => "f"
            };

            SendKeys.SendWait(keyPress);
        }

        private void LogStats(
            string dateStamp,
            BoardState boardState,
            PredictedAction predictedAction
        )
        {
            string stats = $"Id: {dateStamp} " +
                         $"Ps: {boardState.NumberOfPlayers} " +
                         $"Pos: {boardState.MyPosition} " +
                         $"Hand: {boardState.HandCode} " +
                         $"Ev: {predictedAction._ev} " +
                         $"Ante: {boardState.Ante} " +
                         $"SR: {boardState.MyStackRatio} " +
                         $"Action: {predictedAction?.GetAction()}";

            Console.WriteLine(stats);

            foreach (Player p in boardState.Players.Where(p => !p.Eliminated))
            {
                Console.WriteLine($"{p.Position}: Stack: {p.Stack} Bet: {p.Bet}");

            }
        }

        private void DeleteFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            Directory.Delete(path);
        }
    }
}
