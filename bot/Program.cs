using System;
using System.Windows.Input;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;

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

                //dateStamp = "081723";

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);

                    ScreenCaptureService.TakeScreenCapture(path);
                }

                await Task.Delay(1000);

                var boardState = boardStateService.GetBoardStateFromImagePath(path);

                foreach(var p in boardState.Players)
                {
                    Console.WriteLine($"{p.Position}: {p.Stack}");
                }

                var predictedAction = new PredictedAction(boardState);

                //WriteStatsToConsole(dateStamp, boardState, predictedAction);

                DoAction(predictedAction, boardState);

                await Task.Delay(1000);
            }
        }

        public static void DoAction(PredictedAction action, BoardState state)
        {
            if (!state.ReadyForAction)
            {
                return;
            }

            if(state.CallAmount == 0)
            {
                Console.WriteLine("Checking...");
                System.Windows.Forms.SendKeys.SendWait("c");
                return;
            }

            if (action.ActionType == ActionType.Fold)
            {
                Console.WriteLine("Folding...");
                System.Windows.Forms.SendKeys.SendWait("f");
                return;
            }

            if (action.ActionType == ActionType.Limp)
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
            var rank = boardState.StartingHand == null ? -1 : boardState.StartingHand.Rank;
            var flop1 = boardState.Flop1 == null ? "1" : $"{boardState.Flop1.Value}{boardState.Flop1.Suit}";
            var flop2 = boardState.Flop2 == null ? "2" : $"{boardState.Flop2.Value}{boardState.Flop2.Suit}";
            var flop3 = boardState.Flop3 == null ? "3" : $"{boardState.Flop3.Value}{boardState.Flop3.Suit}";
            var turn = boardState.Turn == null ? "" : $"{boardState.Turn.Value}{boardState.Turn.Suit}";
            var river = boardState.River == null ? "" : $"{boardState.River.Value}{boardState.River.Suit}";

            Console.WriteLine(
                $"Id: {dateStamp} " +
                $"HandCode: {boardState.HandCode} " +
                $"Rank: {rank} " +
                $"My Position: {boardState.MyPosition} " +
                $"BB: {boardState.BigBlind} " +
                $"Action: {predictedAction.ActionType}");

            Console.WriteLine(
                $"{flop1} {flop2} {flop3} {turn} {river}");
        }
    }
}
