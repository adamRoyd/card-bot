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
        public void GetAction_EGPreFlopBadHand_Fold()
        {
            var state = new BoardState
            {
                Position6 = true,
                Pot = 45,
                CallAmount = 60,
                StartingCard1 = new Card(
                    Enums.CardValue.Q,
                    Enums.CardSuit.Spades
                ),
                StartingCard2 = new Card(
                    Enums.CardValue.two,
                    Enums.CardSuit.Clubs
                ),
                Stack = 1530,
                Players = 8,
                BigBlind = 30
            };

            Assert.Null(state.StartingHand);
            Assert.Equal(ActionType.Fold, state.PredictedAction.ActionType);
        }
    }
}
