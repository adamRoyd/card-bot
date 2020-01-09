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

        [Fact]
        public void StartingHand_AKs_ReturnsCorrectStartingHand()
        {
            var state = new BoardState
            {
                StartingCard1 = new Card(
                    Enums.CardValue.A,
                    Enums.CardSuit.Spades
                ),
                StartingCard2 = new Card(
                    Enums.CardValue.K,
                    Enums.CardSuit.Spades
                )
            };

            Assert.Equal(1, state.StartingHand.Rank);
            Assert.Equal("AKs", state.StartingHand.HandCode);
        }

        [Fact]
        public void StartingHand_BadHand_ReturnsCorrectStartingHand()
        {
            var state = new BoardState
            {
                StartingCard2 = new Card(
                    Enums.CardValue.K,
                    Enums.CardSuit.Spades
                )
            };

            Assert.Equal(-1, state.StartingHand.Rank);
            Assert.Equal("null", state.StartingHand.HandCode);
        }
    }
}
