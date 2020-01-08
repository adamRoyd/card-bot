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

                Console.WriteLine($"Action: {boardState.PredictedAction.ActionType}");

                await Task.Delay(3000);
            }
        }
    }
}
