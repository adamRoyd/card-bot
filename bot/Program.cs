using System;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;
using Engine.Enums;
using bot.Logging;
using System.Linq;
using ICM;

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

                dateStamp = "045446";

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    ScreenCaptureService.TakeScreenCapture(path);
                }

                var boardState = boardStateService.GetBoardStateFromImagePath(path);

                if (!boardState.ReadyForAction || boardState.HandCode == "null")
                {
                    DeleteFiles(path);
                    continue;
                }

                PredictedAction predictedAction;

                if (boardState.MyStackRatio > 20)
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
                break;

                await Task.Delay(2000);
            }
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
                         $"Ps: {boardState.NumberOfPlayers} " +
                         $"Pos: {boardState.MyPosition} " +
                         $"Hand: {boardState.HandCode} " +
                         $"Ev: {predictedAction._ev} " +
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
