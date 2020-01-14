using System;
using System.Windows.Input;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;
using Engine.Enums;
using System.Linq;

namespace bot
{
    class Program
    {
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

                dateStamp = "071231";

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    ScreenCaptureService.TakeScreenCapture(path);
                }

                await Task.Delay(100);

                var boardState = boardStateService.GetBoardStateFromImagePath(path);

                PredictedAction predictedAction;

                switch (boardState.MyStackRatio)
                {
                    case int n when n > 15:
                        predictedAction = new EarlyGamePredictedAction(boardState);
                        break;
                    case int n when n <= 15:
                        predictedAction = new PushFoldPredictedAction(boardState);
                        break;
                    default:
                        predictedAction = null;
                        break;
                }


                WriteStatsToConsole(dateStamp, boardState, predictedAction);

                DoAction(predictedAction, boardState);

                await Task.Delay(1000);

                //break;
            }
        }

        public static void DoAction(PredictedAction action, BoardState state)
        {
            if (action == null || !state.ReadyForAction)
            {
                return;
            }

            var keyPress = action.GetAction() switch
            {
                ActionType.Fold => "f",
                ActionType.Check => "c",
                ActionType.Limp => "c",
                ActionType.Unknown => "",
                ActionType.AllIn => "",
                ActionType.AllInSteal => "",
                ActionType.Bet => "",
                ActionType.Raise => ""
            };

            System.Windows.Forms.SendKeys.SendWait(keyPress);
        }

        public static void WriteStatsToConsole(
            string dateStamp,
            BoardState boardState,
            PredictedAction predictedAction
        )
        {
            //foreach (var p in boardState.Players)
            //{
            //    Console.WriteLine($"{p.Position}: {p.Stack} {p.Eliminated}");
            //}

            var flop1 = boardState.Flop1 == null ? "  " : $"{boardState.Flop1.SimpleValue}{boardState.Flop1.Suit}";
            var flop2 = boardState.Flop2 == null ? "  " : $"{boardState.Flop2.SimpleValue}{boardState.Flop2.Suit}";
            var flop3 = boardState.Flop3 == null ? "  " : $"{boardState.Flop3.SimpleValue}{boardState.Flop3.Suit}";
            var turn = boardState.Turn == null ? "  " : $"{boardState.Turn.SimpleValue}{boardState.Turn.Suit}";
            var river = boardState.River == null ? "  " : $"{boardState.River.SimpleValue}{boardState.River.Suit}";

            var predictedActionText = boardState.ReadyForAction ? $"Action: {predictedAction?.GetAction()}" : "";

            Console.WriteLine(
                $"Id: {dateStamp} " +
                $"Plyrs: {boardState.NumberOfPlayers} " +
                $"Pos: {boardState.MyPosition} " +
                $"Hand: {boardState.HandCode} " +
                $"Rank: {predictedAction.HandRank} " +
                $"BB: {boardState.BigBlind} " +
                $"SR: {boardState.MyStackRatio} " +
                predictedActionText);

            //Console.WriteLine(
            //    $"{flop1} | {flop2} | {flop3} | {turn} | {river}");

            Console.WriteLine(" ");
        }
    }
}
