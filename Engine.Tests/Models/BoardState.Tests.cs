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
        public void HandCode_ReversedCards_ReturnsCorrectHandCode()
        {
            var state = new BoardState
            {
                StartingCard1 = new Card(
                    Enums.CardValue.two,
                    Enums.CardSuit.Spades
                ),
                StartingCard2 = new Card(
                    Enums.CardValue.Q,
                    Enums.CardSuit.Clubs
                )
            };

            string expectedHandcode = "Q2o"; 

            Assert.Equal(state.HandCode, expectedHandcode);
        }

    }
}
