using Engine.Models;
using OCR;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace bot
{
    public class BoardStateServiceTests
    {
        private readonly ImageProcessor _imageProcessor;
        private readonly BoardStateHelper _boardStateHelper;
        private readonly SuitFinder _suitFinder;
        private readonly BoardStateService _boardStateService;

        public BoardStateServiceTests()
        {
            _imageProcessor = new ImageProcessor();
            _boardStateHelper = new BoardStateHelper();
            _suitFinder = new SuitFinder();

            _boardStateService = new BoardStateService(_imageProcessor, _boardStateHelper, _suitFinder);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board1_ReturnsCorrectStateCards()
        {
            var path = "..\\..\\..\\images\\board1.png";
            var expectedState = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.J, Engine.Enums.CardSuit.Spades),
                StartingCard2 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Spades),
                Flop1 = new Card(Engine.Enums.CardValue.seven, Engine.Enums.CardSuit.Diamonds),
                Flop2 = new Card(Engine.Enums.CardValue.A, Engine.Enums.CardSuit.Diamonds),
                Flop3 = new Card(Engine.Enums.CardValue.six, Engine.Enums.CardSuit.Spades),
                Turn = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Diamonds),
                River = new Card(Engine.Enums.CardValue.five, Engine.Enums.CardSuit.Spades),
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.StartingCard1.Value, state.StartingCard1.Value);
            Assert.Equal(expectedState.StartingCard2.Value, state.StartingCard2.Value);
            Assert.Equal(expectedState.Flop1.Value, state.Flop1.Value);
            Assert.Equal(expectedState.Flop2.Value, state.Flop2.Value);
            Assert.Equal(expectedState.Flop3.Value, state.Flop3.Value);
            Assert.Equal(expectedState.Turn.Value, state.Turn.Value);
            Assert.Equal(expectedState.River.Value, state.River.Value);

            Assert.Equal(expectedState.StartingCard1.Suit, state.StartingCard1.Suit);
            Assert.Equal(expectedState.StartingCard2.Suit, state.StartingCard2.Suit);
            Assert.Equal(expectedState.Flop1.Suit, state.Flop1.Suit);
            Assert.Equal(expectedState.Flop2.Suit, state.Flop2.Suit);
            Assert.Equal(expectedState.Flop3.Suit, state.Flop3.Suit);
            Assert.Equal(expectedState.Turn.Suit, state.Turn.Suit);
            Assert.Equal(expectedState.River.Suit, state.River.Suit);
        }

    }
}
