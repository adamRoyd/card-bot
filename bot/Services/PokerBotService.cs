using bot.Logging;
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
        private readonly ILogger _logger;

        public PokerBotService
        (
            IBoardStateService boardStateService,
            IIcmService icmService,
            IScreenCaptureService screenCaptureService,
            ILogger<PokerBotService> logger
        )
        {
            _boardStateService = boardStateService;
            _icmService = icmService;
            _screenCaptureService = screenCaptureService;
            _logger = logger;
        }

        public async Task Run()
        {
            while (true)
            {
                string dateStamp = DateTime.Now.ToString("hhmmss");
                dateStamp = "035414";

                string path = $"..\\..\\..\\images\\{dateStamp}";
                string splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    _screenCaptureService.CaptureScreenToFile($"{path}\\board.png", ImageFormat.Png);
                }

                BoardState boardState = _boardStateService.GetBoardStateFromImagePath(path);

                if (boardState.GameIsFinished)
                {
                    //TODO add counter here
                    await RegisterForNewGame();
                    await WaitForGameToStart(_boardStateService, _screenCaptureService);
                    //continue;
                }

                if (!boardState.ReadyForAction ||  boardState.HandCode == "null")
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

                DoAction(predictedAction, boardState);
                break;

                await Task.Delay(2000);
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

                BoardState boardState = boardStateService.GetBoardStateFromImagePath(path);

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

        private void DoAction(PredictedAction action, BoardState state)
        {
            if (action == null)
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
            string flop1 = boardState.Flop1 == null ? "  " : $"{boardState.Flop1.SimpleValue}{boardState.Flop1.Suit}";
            string flop2 = boardState.Flop2 == null ? "  " : $"{boardState.Flop2.SimpleValue}{boardState.Flop2.Suit}";
            string flop3 = boardState.Flop3 == null ? "  " : $"{boardState.Flop3.SimpleValue}{boardState.Flop3.Suit}";
            string turn = boardState.Turn == null ? "  " : $"{boardState.Turn.SimpleValue}{boardState.Turn.Suit}";
            string river = boardState.River == null ? "  " : $"{boardState.River.SimpleValue}{boardState.River.Suit}";

            string predictedActionText = boardState.ReadyForAction ? $"Action: {predictedAction?.GetAction()}" : "";

            string stats = $"Id: {dateStamp} " +
                         $"Ps: {boardState.NumberOfPlayers} " +
                         $"Pos: {boardState.MyPosition} " +
                         $"Hand: {boardState.HandCode} " +
                         $"Ev: {predictedAction._ev} " +
                         $"Ante: {boardState.Ante} " +
                         $"SR: {boardState.MyStackRatio} " +
                         predictedActionText;

            _logger.LogInformation(stats);

            //Console.WriteLine(
            //    $"{flop1} | {flop2} | {flop3} | {turn} | {river}");

            foreach (Player p in boardState.Players.Where(p => !p.Eliminated))
            {
                _logger.LogInformation($"{p.Position}: Stack: {p.Stack} Bet: {p.Bet}");
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
