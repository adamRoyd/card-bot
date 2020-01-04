using OCR.Objects;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using Tesseract;

namespace OCR
{
    public class ImageProcessor
    {
        public string GetTextFromImage(Pix img)
        {
            string result = null;

            try
            {

                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    using (img)
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            Console.WriteLine($"Text (GetText): {text}");

                            text = text.RemoveLineBreaks().StripPunctuation().Trim();

                            result = text;
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
                result = e.ToString();
            }

            return result;
        }

        public BoardImages SliceBoardImage(string path)
        {
            var boardImages = new BoardImages();

            var img = Image.FromFile(path);

            var graphic = Graphics.FromImage(boardImages.StartingCard1.Image);

            graphic.DrawImage(
                img, 
                new Rectangle(
                    0, 
                    0, 
                    boardImages.StartingCard1.Image.Width,
                    boardImages.StartingCard1.Image.Height
                ), 
                new Rectangle(
                    boardImages.StartingCard1.X, 
                    boardImages.StartingCard1.Y,
                    boardImages.StartingCard1.Image.Width,
                    boardImages.StartingCard1.Image.Height
                ), 
                GraphicsUnit.Pixel
                );

            graphic.Dispose();


            return boardImages;
        }
    }
}
