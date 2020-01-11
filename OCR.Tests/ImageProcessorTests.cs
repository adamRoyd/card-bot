using System;
using System.Linq;
using Tesseract;
using Xunit;

namespace OCR.Tests
{
    public class ImageProcessorTests
    {
        private readonly ImageProcessor imageProcessor;

        public ImageProcessorTests()
        {
            imageProcessor = new ImageProcessor();
        }

        [Fact]
        public void GetCardValueFromImage_2_ReturnsCorrectValue()
        {
            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\2.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.two, text);
        }

        [Fact]
        public void GetCardValueFromImage_3_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\3.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.three, text);
        }


        [Fact]
        public void GetCardValueFromImage_4_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\4.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.four, text);
        }


        [Fact]
        public void GetCardValueFromImage_5_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\5.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.five, text);
        }


        [Fact]
        public void GetCardValueFromImage_6_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\6.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.six, text);
        }


        [Fact]
        public void GetCardValueFromImage_7_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\7.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.seven, text);
        }


        [Fact]
        public void GetCardValueFromImage_8_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\8.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.eight, text);
        }

        [Fact]
        public void GetCardValueFromImage_9_ReturnsCorrectValue()
        {
            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\9.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.nine, text);
        }

        [Fact]
        public void GetCardValueFromImage_10_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\10.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.ten, text);
        }

        [Fact]
        public void GetCardValueFromImage_J_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\J.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.J, text);
        }

        [Fact]
        public void GetCardValueFromImage_Q_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\Q.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.Q, text);
        }

        [Fact]
        public void GetCardValueFromImage_K_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\K.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.K, text);
        }

        [Fact]
        public void GetCardValueFromImage_A_ReturnsCorrectValue()
        {

            var image = System.Drawing.Image.FromFile("C:\\Temp\\images\\A.png");

            var text = imageProcessor.GetCardValueFromImage(image);

            Assert.Equal(Engine.Enums.CardValue.A, text);
        }

        [Fact]
        public void SliceBoardImage_Board4_ReturnsCorrectStartingCard1()
        {
            var path = "..\\..\\..\\images\\board1.png";


            var boardImages = imageProcessor.SliceBoardScreenShot(path);

            var boardImage = boardImages.FirstOrDefault(b => b.Name == Objects.ImageName.StartingCard1);
            var cardValue = imageProcessor.GetCardValueFromImage(boardImage.Image);

            Assert.Equal(Engine.Enums.CardValue.A, cardValue);
        }


        [Fact]
        public void SliceBoardImage_Board1_ReturnsBigBlind()
        {
            var path = "..\\..\\..\\images\\1";


            var boardImages = imageProcessor.SliceBoardScreenShot(path);

            var boardImage = boardImages.FirstOrDefault(b => b.Name == Objects.ImageName.BigBlind);
            var raw = imageProcessor.GetImageCharacters(boardImage.Image,PageSegMode.SingleLine);

            var bigBlind = raw.Split(" ")[3];

            Assert.Equal("2550", bigBlind);
        }
    }
}
