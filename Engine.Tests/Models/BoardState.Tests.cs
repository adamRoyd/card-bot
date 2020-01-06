using Engine.Enums;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Engine.Tests.Models
{
    public class BoardStateTests
    {
        [Fact]
        public void GetAction_Board1State_ReturnsCorrectCalculations()
        {
            var state = new BoardState
            {
                Position1 = true,
                Pot = 661,
                CallAmount = 536,
                StartingCard1 = new Card(
                    Enums.CardValue.seven, 
                    Enums.CardSuit.Spades
                ),
                StartingCard2 = new Card(
                    Enums.CardValue.seven,
                    Enums.CardSuit.Hearts
                )
            };

            Assert.Equal(Stage.PreFlop, state.Stage);
            Assert.Equal(ActionType.Fold, state.PredictedAction.ActionType);
        }
    }
}
