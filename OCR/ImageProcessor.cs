using Engine.Enums;
using OCR.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Tesseract;

namespace OCR
{
    public class ImageProcessor
    {
        private readonly TesseractEngine engine;

        public ImageProcessor()
        {
            engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
        }

        public int GetPotValueFromImage(Image img)
        {
            var result = GetCharacters(img, PageSegMode.SingleLine);
            int.TryParse(result, out int value);
            return value;
        }


        public CardValue GetCardValueFromImage(Image img)
        {
            var result = GetCharacters(img, PageSegMode.SingleBlock);

            try
            {
                Enum.TryParse(result, out CardValue cardValue);
                return cardValue;
            }
            catch (Exception)
            {
                Console.WriteLine($"Unable to parse card value from string ${result}");
                throw;
            }
        }

        private string GetCharacters(Image img, PageSegMode mode)
        {
            var byteArray = ImageToByteArray(img);

            var pix = Pix.LoadTiffFromMemory(byteArray);

            string result = null;

            try
            {

                using (engine)
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    using (img)
                    {
                        using (var page = engine.Process(pix))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            Console.WriteLine($"Text (GetText): {text}");

                            text = text.RemoveLineBreaks().StripPunctuation().Trim();

                            result = text;

                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
                result = e.ToString();
                throw;
            }
        }

        public List<BoardImage> SliceBoardScreenShot(string path)
        {
            var boardImages = new BoardImages();

            var img = Image.FromFile(path);

            foreach(var boardImage in boardImages.BoardImageList)
            {
                DrawImage(boardImage, img);
            }    

            return boardImages.BoardImageList;
        }

        private void DrawImage(BoardImage boardImage, Image baseImage)
        {
            var graphic = Graphics.FromImage(boardImage.Image);

            graphic.DrawImage(
                baseImage,
                new Rectangle(
                    0,
                    0,
                    boardImage.Image.Width,
                    boardImage.Image.Height
                ),
                new Rectangle(
                    boardImage.X,
                    boardImage.Y,
                    boardImage.Image.Width,
                    boardImage.Image.Height
                ),
                GraphicsUnit.Pixel
                );

            graphic.Dispose();
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                return ms.ToArray();
            }
        }
    }
}
