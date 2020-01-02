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

            var imgarray = new Image[9];
            var img = Image.FromFile(path);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var index = i * 3 + j;
                    imgarray[index] = new Bitmap(104, 104);
                    var graphics = Graphics.FromImage(imgarray[index]);
                    graphics.DrawImage(img, new Rectangle(0, 0, 104, 104), new Rectangle(i * 104, j * 104, 104, 104), GraphicsUnit.Pixel);
                    graphics.Dispose();
                }
            }

            return boardImages;
        }
    }
}
