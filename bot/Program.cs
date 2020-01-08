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
        public async static Task Main(string[] args)
        {
            await BeTheBot();
        }


        public static async Task BeTheBot()
        {
            while (true)
            {
                ScreenCaptureService.TakeScreenCapture();

                Console.WriteLine("Scren capture taken");

                await Task.Delay(3000);


            }
        }
    }
}
