using System;
using Tesseract;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

                var path = $"..\\..\\..\\images\\{dateStamp}";
                var splicedPath = $"..\\..\\..\\images\\{dateStamp}\\spliced";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(splicedPath);
                }

                ScreenCaptureService.TakeScreenCapture(path);

                await Task.Delay(1000);

                var boardState = boardStateService.GetBoardStateFromImagePath(path);
                var predictedAction = new PredictedAction(boardState);

                WriteStatsToConsole(dateStamp, boardState, predictedAction);


                await Task.Delay(2000);
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
                $"Action: {predictedAction.ActionType}");

            Console.WriteLine(
                $"{flop1} {flop2} {flop3} {turn} {river}");
        }
    }
}
