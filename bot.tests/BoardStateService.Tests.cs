using Engine.Models;
using OCR;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;

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
            _suitFinder = new SuitFinder();

            _boardStateHelper = new BoardStateHelper(_imageProcessor, _suitFinder);
            _boardStateService = new BoardStateService(_imageProcessor, _boardStateHelper, _suitFinder);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board1_ReturnsCorrectState()
        {
            var path = "..\\..\\..\\images\\board1.png";
            var expectedState = new BoardState
            {
                CallAmount = 536,
                Stack = 1650,
                Position1 = true,
                Pot = 661,
                FoldButton = "fold"
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            var serialized = JsonConvert.SerializeObject(state);

            Assert.Equal(expectedState.CallAmount, state.CallAmount);
            Assert.Equal(expectedState.Stack, state.Stack);
            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
            Assert.Equal(expectedState.Pot, state.Pot);
            Assert.Equal(expectedState.FoldButton, state.FoldButton);
        }

        [Fact]
        public void GetBoardStateFromImagePath_FoldAction_ReturnsCorrectState()
        {
            var path = "..\\..\\..\\images\\foldAction.png";
            var expectedState = new BoardState
            {
                PredictedAction = new PredictedAction
                {
                    ActionType = ActionType.Fold
                }
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            
            Assert.Equal(expectedState.PredictedAction.ActionType, state.PredictedAction.ActionType);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board4_ReturnsCorrectStateCards()
        {
            var path = "..\\..\\..\\images\\board4.png";
            var expectedState = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Hearts),
                Flop1 = new Card(Engine.Enums.CardValue.J, Engine.Enums.CardSuit.Clubs),
                Flop2 = new Card(Engine.Enums.CardValue.four, Engine.Enums.CardSuit.Clubs),
                Flop3 = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Diamonds),
                Turn = new Card(Engine.Enums.CardValue.eight, Engine.Enums.CardSuit.Diamonds),
                River = null,
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.StartingCard1.Value, state.StartingCard1.Value);
            Assert.Equal(expectedState.StartingCard2.Value, state.StartingCard2.Value);
            Assert.Equal(expectedState.Flop1.Value, state.Flop1.Value);
            Assert.Equal(expectedState.Flop2.Value, state.Flop2.Value);
            Assert.Equal(expectedState.Flop3.Value, state.Flop3.Value);
            Assert.Equal(expectedState.Turn.Value, state.Turn.Value);

            Assert.Equal(expectedState.StartingCard1.Suit, state.StartingCard1.Suit);
            Assert.Equal(expectedState.StartingCard2.Suit, state.StartingCard2.Suit);
            Assert.Equal(expectedState.Flop1.Suit, state.Flop1.Suit);
            Assert.Equal(expectedState.Flop2.Suit, state.Flop2.Suit);
            Assert.Equal(expectedState.Flop3.Suit, state.Flop3.Suit);
            Assert.Equal(expectedState.Turn.Suit, state.Turn.Suit);

            Assert.Equal(expectedState.River, state.River);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board5_ReturnsCorrectStateCards()
        {
            var path = "..\\..\\..\\images\\board5.png";
            var expectedState = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Spades),
                StartingCard2 = new Card(Engine.Enums.CardValue.six, Engine.Enums.CardSuit.Clubs),
                Flop1 = new Card(Engine.Enums.CardValue.three, Engine.Enums.CardSuit.Spades),
                Flop2 = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Clubs),
                Flop3 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Hearts),
                Turn = null,
                River = null,
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.StartingCard1.Value, state.StartingCard1.Value);
            Assert.Equal(expectedState.StartingCard2.Value, state.StartingCard2.Value);
            Assert.Equal(expectedState.Flop1.Value, state.Flop1.Value);
            Assert.Equal(expectedState.Flop2.Value, state.Flop2.Value);
            Assert.Equal(expectedState.Flop3.Value, state.Flop3.Value);

            Assert.Equal(expectedState.StartingCard1.Suit, state.StartingCard1.Suit);
            Assert.Equal(expectedState.StartingCard2.Suit, state.StartingCard2.Suit);
            Assert.Equal(expectedState.Flop1.Suit, state.Flop1.Suit);
            Assert.Equal(expectedState.Flop2.Suit, state.Flop2.Suit);
            Assert.Equal(expectedState.Flop3.Suit, state.Flop3.Suit);

            Assert.Equal(expectedState.Turn, state.Turn);
            Assert.Equal(expectedState.River, state.River);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board9_ReturnsCorrectStateCards()
        {
            var path = "..\\..\\..\\images\\board9.png";
            var expectedState = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.ten, Engine.Enums.CardSuit.Spades),
                StartingCard2 = new Card(Engine.Enums.CardValue.J, Engine.Enums.CardSuit.Spades),
                Flop1 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Hearts),
                Flop2 = new Card(Engine.Enums.CardValue.five, Engine.Enums.CardSuit.Spades),
                Flop3 = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Hearts),
                Turn = null,
                River = null,
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.StartingCard1.Value, state.StartingCard1.Value);
            Assert.Equal(expectedState.StartingCard2.Value, state.StartingCard2.Value);
            Assert.Equal(expectedState.Flop1.Value, state.Flop1.Value);
            Assert.Equal(expectedState.Flop2.Value, state.Flop2.Value);
            Assert.Equal(expectedState.Flop3.Value, state.Flop3.Value);

            Assert.Equal(expectedState.StartingCard1.Suit, state.StartingCard1.Suit);
            Assert.Equal(expectedState.StartingCard2.Suit, state.StartingCard2.Suit);
            Assert.Equal(expectedState.Flop1.Suit, state.Flop1.Suit);
            Assert.Equal(expectedState.Flop2.Suit, state.Flop2.Suit);
            Assert.Equal(expectedState.Flop3.Suit, state.Flop3.Suit);

            Assert.Equal(expectedState.Turn, state.Turn);
            Assert.Equal(expectedState.River, state.River);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board2_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board2.png";
            var expectedState = new BoardState
            {
                Position2 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board3_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board3.png";
            var expectedState = new BoardState
            {
                Position3 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board4_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board4.png";
            var expectedState = new BoardState
            {
                Position4 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board5_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board5.png";
            var expectedState = new BoardState
            {
                Position5 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board6_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board6.png";
            var expectedState = new BoardState
            {
                Position6 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board7_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board7.png";
            var expectedState = new BoardState
            {
                Position7 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board8_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board8.png";
            var expectedState = new BoardState
            {
                Position8 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board9_ReturnsCorrectDealerPosition()
        {
            var path = "..\\..\\..\\images\\board9.png";
            var expectedState = new BoardState
            {
                Position9 = true
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Position1, state.Position1);
            Assert.Equal(expectedState.Position2, state.Position2);
            Assert.Equal(expectedState.Position3, state.Position3);
            Assert.Equal(expectedState.Position4, state.Position4);
            Assert.Equal(expectedState.Position5, state.Position5);
            Assert.Equal(expectedState.Position6, state.Position6);
            Assert.Equal(expectedState.Position7, state.Position7);
            Assert.Equal(expectedState.Position8, state.Position8);
            Assert.Equal(expectedState.Position9, state.Position9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board2_ReturnsCorrectPot()
        {
            var path = "..\\..\\..\\images\\board2.png";
            var expectedState = new BoardState
            {
                Pot = 120
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Pot, state.Pot);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board3_ReturnsCorrectPot()
        {
            var path = "..\\..\\..\\images\\board3.png";
            var expectedState = new BoardState
            {
                Pot = 150
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Pot, state.Pot);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board9_ReturnsCorrectPot()
        {
            var path = "..\\..\\..\\images\\board9.png";
            var expectedState = new BoardState
            {
                Pot = 1600
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.Pot, state.Pot);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board3_ReturnsCorrectBets()
        {
            var path = "..\\..\\..\\images\\board3.png";
            var expectedState = new BoardState
            {
                //Bet1 = 20,
                //Bet2 = 0,
                //Bet3 = 20,
                //Bet4 = 10,
                //Bet5 = 20,
                //Bet6 = 20,
                //Bet7 = 20,
                //Bet8 = 20,
                //Bet9 = 20
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            //Assert.Equal(expectedState.Bet1, state.Bet1);
            //Assert.Equal(expectedState.Bet2, state.Bet2);
            //Assert.Equal(expectedState.Bet3, state.Bet3);
            //Assert.Equal(expectedState.Bet4, state.Bet4);
            //Assert.Equal(expectedState.Bet5, state.Bet5);
            //Assert.Equal(expectedState.Bet6, state.Bet6);
            //Assert.Equal(expectedState.Bet7, state.Bet7);
            //Assert.Equal(expectedState.Bet8, state.Bet8);
            //Assert.Equal(expectedState.Bet9, state.Bet9);
        }

        [Fact]
        public void GetBoardStateFromImagePath_Board6_ReturnsCorrectCallAmount()
        {
            var path = "..\\..\\..\\images\\board6.png";
            var expectedState = new BoardState
            {
                CallAmount = 30
            };

            var state = _boardStateService.GetBoardStateFromImagePath(path);

            Assert.Equal(expectedState.CallAmount, state.CallAmount);
        }
    }
}
