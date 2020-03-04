using bot.Logging;
using Engine.Enums;
using Engine.Models;
using ICM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bot.Services
{
    public class PokerBotService : IPokerBotService
    {
        private static readonly ILog LogWriter = new FileLogWriter("C:/Temp");
        private readonly IBoardStateService _boardStateService;
        private readonly IIcmService _icmService;
        private readonly IScreenCaptureService _screenCaptureService;

        public PokerBotService
        (
            IBoardStateService boardStateService,
            IIcmService icmService,
            IScreenCaptureService screenCaptureService
        )
        {
            _boardStateService = boardStateService;
            _icmService = icmService;
            _screenCaptureService = screenCaptureService;
        }

        public async Task Run()
        {
            while (true)
            {
                var dateStamp = DateTime.Now.ToString("hhmmss");
                dateStamp = "035414";

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    _screenCaptureService.CaptureScreenToFile($"{path}\\board.png", ImageFormat.Png);
                }

                var boardState = _boardStateService.GetBoardStateFromImagePath(path);

                if (boardState.GameIsFinished)
                {
                    //TODO add counter here
                    break;
                    //await RegisterForNewGame(boardStateService);
                    //continue;
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

                WriteStatsToConsole(dateStamp, boardState, predictedAction);

                DoAction(predictedAction, boardState);
                break;

                await Task.Delay(2000);
            }
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public static async Task RegisterForNewGame
        (
            IBoardStateService boardStateService,
            IScreenCaptureService screenCaptureService
        )
        {
            Random rnd = new Random();
            Console.WriteLine("REGISTERING");
            //Register for next game
            await Task.Delay(rnd.Next(1000, 3000));
            System.Windows.Forms.SendKeys.SendWait("{LEFT}");
            await Task.Delay(rnd.Next(1000, 3000));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            await Task.Delay(rnd.Next(2000, 4000));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");


            // TODO avoid code duplication
            while (true)
            {
                Console.WriteLine("Waiting for game...");
                // if isInPlay, carry on
                // otherwise retake screenshots, wait and try again
                var dateStamp = DateTime.Now.ToString("hhmmss");

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    screenCaptureService.CaptureScreenToFile($"{path}\\board.png", ImageFormat.Png);
                }

                var boardState = boardStateService.GetBoardStateFromImagePath(path);

                if (!boardState.IsInPlay)
                {
                    //DeleteFiles(path);
                    continue;
                }

                Console.WriteLine("GAME HAS STARTED. Maximising...");
                System.Windows.Forms.SendKeys.SendWait("m");

                break;
            }

            await Task.Delay(rnd.Next(500, 1000));

            //Select info tab
            var infoPosition = new Point(rnd.Next(759, 764), rnd.Next(845, 849));
            await LinearSmoothMove(infoPosition, 60);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public static async Task LinearSmoothMove(Point newPosition, int steps)
        {
            Point start = System.Windows.Forms.Cursor.Position;
            PointF iterPoint = start;

            // Find the slope of the line segment defined by start and newPosition
            PointF slope = new PointF(newPosition.X - start.X, newPosition.Y - start.Y);

            // Divide by the number of steps
            slope.X = slope.X / steps;
            slope.Y = slope.Y / steps;

            // Move the mouse to each iterative point.
            for (int i = 0; i < steps; i++)
            {
                iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                Cursor.Position = Point.Round(iterPoint);
                await Task.Delay(1);
            }

            // Move the mouse to the final destination.
            Cursor.Position = newPosition;
        }

        private void DoAction(PredictedAction action, BoardState state)
        {
            if (action == null)
            {
                return;
            }

            var keyPress = action.GetAction() switch
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

        private void WriteStatsToConsole(
            string dateStamp,
            BoardState boardState,
            PredictedAction predictedAction
        )
        {
            var flop1 = boardState.Flop1 == null ? "  " : $"{boardState.Flop1.SimpleValue}{boardState.Flop1.Suit}";
            var flop2 = boardState.Flop2 == null ? "  " : $"{boardState.Flop2.SimpleValue}{boardState.Flop2.Suit}";
            var flop3 = boardState.Flop3 == null ? "  " : $"{boardState.Flop3.SimpleValue}{boardState.Flop3.Suit}";
            var turn = boardState.Turn == null ? "  " : $"{boardState.Turn.SimpleValue}{boardState.Turn.Suit}";
            var river = boardState.River == null ? "  " : $"{boardState.River.SimpleValue}{boardState.River.Suit}";

            var predictedActionText = boardState.ReadyForAction ? $"Action: {predictedAction?.GetAction()}" : "";

            var stats = $"Id: {dateStamp} " +
                         $"Ps: {boardState.NumberOfPlayers} " +
                         $"Pos: {boardState.MyPosition} " +
                         $"Hand: {boardState.HandCode} " +
                         $"Ev: {predictedAction._ev} " +
                         $"Ante: {boardState.Ante} " +
                         $"SR: {boardState.MyStackRatio} " +
                         predictedActionText;

            LogWriter.WriteLine(stats);

            //Console.WriteLine(
            //    $"{flop1} | {flop2} | {flop3} | {turn} | {river}");

            foreach (var p in boardState.Players.Where(p => !p.Eliminated))
            {
                LogWriter.WriteLine($"{p.Position}: Stack: {p.Stack} Bet: {p.Bet}");
            }
        }

        private void DeleteFiles(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

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
