using System;
using Tesseract;
using System.Diagnostics;
using System.Collections.Generic;
using OCR;

namespace bot
{
    class Program
    {
        public static void Main(string[] args)
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\2.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
