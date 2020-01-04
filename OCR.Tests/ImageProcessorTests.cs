using System;
using Tesseract;
using Xunit;

namespace OCR.Tests
{
    public class ImageProcessorTests
    {
        [Fact]
        public void GetTextFromImage_2_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\2.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("2", text);
        }

        [Fact]
        public void GetTextFromImage_3_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\3.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("3", text);
        }


        [Fact]
        public void GetTextFromImage_4_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\4.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("4", text);
        }


        [Fact]
        public void GetTextFromImage_5_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\5.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("5", text);
        }


        [Fact]
        public void GetTextFromImage_6_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\6.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("6", text);
        }


        [Fact]
        public void GetTextFromImage_7_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\7.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("7", text);
        }


        [Fact]
        public void GetTextFromImage_8_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\8.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("8", text);
        }


        [Fact]
        public void GetTextFromImage_9_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\9.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("9", text);
        }

        [Fact]
        public void GetTextFromImage_10_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\10.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("10", text);
        }

        [Fact]
        public void GetTextFromImage_J_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\J.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("J", text);
        }

        [Fact]
        public void GetTextFromImage_Q_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\Q.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("Q", text);
        }

        [Fact]
        public void GetTextFromImage_K_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\K.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("K", text);
        }

        [Fact]
        public void GetTextFromImage_A_ReturnsCorrectValue()
        {
            var image = Pix.LoadFromFile("C:\\Temp\\images\\A.png");

            var imageProcessor = new ImageProcessor();

            var text = imageProcessor.GetTextFromImage(image);

            Assert.Equal("A", text);
        }

        [Fact]
        public void SliceBoardImage_ReturnsCorrectObject()
        {
            var path = "..\\..\\..\\images\\board1.png";
            var imageProcessor = new ImageProcessor();

            var boardImages = imageProcessor.SliceBoardImage(path);

            boardImages.StartingCard1.Image.Save("..\\..\\..\\images\\test.png");
        }
    }
}
