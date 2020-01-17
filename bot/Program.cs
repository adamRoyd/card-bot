using System;
using System.Windows.Input;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;
using Engine.Enums;
using System.Linq;
using bot.Logging;

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

            while (true)
            {
                var dateStamp = DateTime.Now.ToString("hhmmss");

                //dateStamp = "064031";

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    ScreenCaptureService.TakeScreenCapture(path);
                }

                var boardState = boardStateService.GetBoardStateFromImagePath(path);

                if (boardState.ReadyForAction)
                {
                    var predictedAction = boardState.MyStackRatio switch
                    {
                        int n when n > 15 => (PredictedAction) new EarlyGamePredictedAction(boardState),
                        int n when n <= 15 => new PushFoldPredictedAction(boardState),
                        _ => null
                    };

                    WriteStatsToConsole(dateStamp, boardState, predictedAction);

                    DoAction(predictedAction, boardState);
                }
                else
                {
                    DeleteFiles(path);
                }
            }
        }

        public static void DoAction(PredictedAction action, BoardState state)
        {
            if (action == null || !state.FoldButton)
            {
                return;
            }

            var keyPress = action.GetAction() switch
            {
                ActionType.Fold => "f",
                ActionType.Check => "c",
                ActionType.Limp => "a",
                ActionType.Unknown => "z",
                ActionType.AllIn => "i",
                ActionType.AllInSteal => "i",
                ActionType.Bet => "z",
                ActionType.Raise => "z"
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
                         $"Plyrs: {boardState.NumberOfPlayers} " +
                         $"Pos: {boardState.MyPosition} " +
                         $"Hand: {boardState.HandCode} " +
                         $"CallAmount: {boardState.CallAmount} " +
                         $"RaiseAmount: {boardState.RaiseAmount} " +
                         $"SR: {boardState.MyStackRatio} " +
                         predictedActionText;

            //Console.WriteLine(stats);

            LogWriter.WriteLine(stats);

            //Console.WriteLine(
            //    $"{boardState.FoldButton} | {boardState.CallButton} | {boardState.RaiseButton}"
            //);

            //Console.WriteLine(
            //    $"{flop1} | {flop2} | {flop3} | {turn} | {river}");

            //foreach (var p in boardState.Players)
            //{
            //    Console.WriteLine($"{p.Position}: {p.Stack} {p.Eliminated}");
            //}

            Console.WriteLine(" ");
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
