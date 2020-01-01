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
	}
}
