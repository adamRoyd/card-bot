using System;
using System.Windows.Input;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;
using Engine.Enums;

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
            var boardStateService = new BoardStateService(imageProcessor,boardStateHelper, suitFinder);

            while (true)
            {
                var dateStamp = DateTime.Now.ToString("hhmmss");

                //dateStamp = "033047";

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

                switch (boardState.GameStage)
                {
                    case GameStage.EarlyGame:
                        predictedAction = new EarlyGamePredictedAction
                        {
                            _state = boardState
                        };
                        break;
                    default:
                        predictedAction = null;
                        break;
                }

                WriteStatsToConsole(dateStamp, boardState, predictedAction);

                DoAction(predictedAction, boardState);

                await Task.Delay(1000);

                break;
            }
        }

        public static void DoAction(PredictedAction action, BoardState state)
        {
            if (!state.ReadyForAction || action == null)
            {
                return;
            }

            if(state.CallAmount == 0)
            {
                Console.WriteLine("Checking...");
                System.Windows.Forms.SendKeys.SendWait("c");
                return;
            }

            if (action.Action == ActionType.Fold)
            {
                Console.WriteLine("Folding...");
                System.Windows.Forms.SendKeys.SendWait("f");
                return;
            }

            if (action.Action == ActionType.Limp)
            {
                Console.WriteLine("Limping...");
                System.Windows.Forms.SendKeys.SendWait("c");
                return;
            }
        }

        public static void WriteStatsToConsole(
            string dateStamp, 
            BoardState boardState, 
            PredictedAction predictedAction
        )
        {
            //foreach (var p in boardState.Players)
            //{
            //    Console.WriteLine($"{p.Position}: {p.Stack}");
            //}

            var rank = boardState.StartingHand == null ? -1 : boardState.StartingHand.Rank;
            var flop1 = boardState.Flop1 == null ? "  " : $"{boardState.Flop1.SimpleValue}{boardState.Flop1.Suit}";
            var flop2 = boardState.Flop2 == null ? "  " : $"{boardState.Flop2.SimpleValue}{boardState.Flop2.Suit}";
            var flop3 = boardState.Flop3 == null ? "  " : $"{boardState.Flop3.SimpleValue}{boardState.Flop3.Suit}";
            var turn = boardState.Turn == null ? "  " : $"{boardState.Turn.SimpleValue}{boardState.Turn.Suit}";
            var river = boardState.River == null ? "  " : $"{boardState.River.SimpleValue}{boardState.River.Suit}";

            Console.WriteLine(
                $"Id: {dateStamp} " +
                $"Players: {boardState.NumberOfPlayers} " +
                $"HandCode: {boardState.HandCode} " +
                $"Rank: {rank} " +
                $"My Position: {boardState.MyPosition} " +
                $"BB: {boardState.BigBlind} " +
                $"MyStack: {boardState.Stack1} " +
                $"Action: {predictedAction?.Action}");

            //Console.WriteLine(
            //    $"{flop1} | {flop2} | {flop3} | {turn} | {river}");
        }
    }
}
