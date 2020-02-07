using System;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;
using Engine.Enums;
using bot.Logging;
using System.Linq;
using ICM;
using System.Drawing;
using System.Windows.Forms;

namespace bot
{
    class Program
    {
        private static readonly ILog LogWriter = new FileLogWriter("C:/Temp");

        public static async Task Main(string[] args)
        {
            await BeTheBot();
        }

        public static async Task BeTheBot()
        {
            var imageProcessor = new ImageProcessor();
            var suitFinder = new SuitFinder();
            var boardStateHelper = new BoardStateHelper(imageProcessor, suitFinder);
            var boardStateService = new BoardStateService(imageProcessor, boardStateHelper, suitFinder);
            var icmService = new IcmService();

            while (true)
            {
                var dateStamp = DateTime.Now.ToString("hhmmss");

                //dateStamp = "085848";

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    ScreenCaptureService.TakeScreenCapture(path);
                }

                var boardState = boardStateService.GetBoardStateFromImagePath(path);

                if (boardState.GameIsFinished)
                {
                    await RegisterForNewGame();
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
                    
                    if(boardState.HandStage == HandStage.PreFlop)
                    {
                        ev = icmService.GetExpectedValue(boardState);
                    }
                
                    predictedAction = new PushFoldPredictedAction(boardState, ev);
                }

                WriteStatsToConsole(dateStamp, boardState, predictedAction);

                //DoAction(predictedAction, boardState);
                //break;

                await Task.Delay(2000);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        //TODO make all points slightly random
        public static async Task RegisterForNewGame()
        {
            Random rnd = new Random();
            Console.WriteLine("REGISTERING");
            //Register for next game
            await Task.Delay(rnd.Next(1000, 3000));
            System.Windows.Forms.SendKeys.SendWait("{LEFT}");
            await Task.Delay(rnd.Next(1000, 3000));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            await Task.Delay(rnd.Next(1000, 3000));
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");

            //Wait for game to fill
            await Task.Delay(rnd.Next(60000, 60100));

            //Maximise Window
            var windowMaxPosition = new Point(1843, 14);
            await LinearSmoothMove(windowMaxPosition, 60);
            await Task.Delay(rnd.Next(1000, 3000));
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);

            await Task.Delay(rnd.Next(1000, 2000));

            //Uncheck sitting out
            var sittingOutButton = new Point(1642, 606);
            await LinearSmoothMove(sittingOutButton, 60);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);

            await Task.Delay(rnd.Next(500, 1000));

            //Select info tab
            var infoPosition = new Point(759, 848);
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
                System.Windows.Forms.Cursor.Position = Point.Round(iterPoint);
                await Task.Delay(1);
            }

            // Move the mouse to the final destination.
            System.Windows.Forms.Cursor.Position = newPosition;
        }

        public static void DoAction(PredictedAction action, BoardState state)
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

            System.Windows.Forms.SendKeys.SendWait(keyPress);
        }

        public static void WriteStatsToConsole(
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
                         //$"Ps: {boardState.NumberOfPlayers} " +
                         //$"Pos: {boardState.MyPosition} " +
                         $"Hand: {boardState.HandCode} " +
                         $"Ev: {predictedAction._ev} " +
                         $"Ante: {boardState.Ante} " +
                         $"Finished: {boardState.GameIsFinished} " +
                         predictedActionText;

            LogWriter.WriteLine(stats);


            //Console.WriteLine(
            //    $"{flop1} | {flop2} | {flop3} | {turn} | {river}");

            foreach (var p in boardState.Players.Where(p => !p.Eliminated))
            {
                LogWriter.WriteLine($"{p.Position}: Stack: {p.Stack} Bet: {p.Bet}");
            }
        }

        public static void DeleteFiles(string path)
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
