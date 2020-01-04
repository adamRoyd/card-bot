using Engine.Enums;
using Xunit;

namespace OCR.Tests
{
    public class SuitFinderTests
    {
		[Fact]
		public void GetSuitFromColor_Black_ReturnsSpades()
		{
			var path = "..\\..\\..\\images\\A.png";
			var suitfinder = new SuitFinder();

			var suit = suitfinder.GetSuitFromColor(path);

			Assert.Equal(CardSuit.Spades, suit);
		}

		[Fact]
		public void GetSuitFromColor_Red_ReturnsHearts()
		{
			var path = "..\\..\\..\\images\\Q.png";
			var suitfinder = new SuitFinder();

			var suit = suitfinder.GetSuitFromColor(path);

			Assert.Equal(CardSuit.Hearts, suit);
		}

		[Fact]
		public void GetSuitFromColor_Green_ReturnsClubs()
		{
			var path = "..\\..\\..\\images\\2.png";
			var suitfinder = new SuitFinder();

			var suit = suitfinder.GetSuitFromColor(path);

			Assert.Equal(CardSuit.Clubs, suit);
		}

		[Fact]
		public void GetSuitFromColor_Blue_ReturnsDiamonds()
		{
			var path = "..\\..\\..\\images\\K.png";
			var suitfinder = new SuitFinder();

			var suit = suitfinder.GetSuitFromColor(path);

			Assert.Equal(CardSuit.Diamonds, suit);
		}
	}
}
