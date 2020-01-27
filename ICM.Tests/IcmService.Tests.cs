using Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ICM.Tests
{
    public class IcmServiceTests
    {
        private readonly IcmService icmIntegrator;

        public IcmServiceTests()
        {
            icmIntegrator = new IcmService();
        }

        [Fact]
        public void GetExpectedValue_ThreePlayerAKs_CorrectEV()
        {
            BoardState state = GetThreePlayerBoardState("AKs");

            var result = icmIntegrator.GetExpectedValue(state);

            Assert.Equal(1.4, result);
        }

        [Fact]
        public void GetExpectedValue_ThreePlayerQ8s_CorrectEV()
        {
            BoardState state = GetThreePlayerBoardState("Q8s");
            var result = icmIntegrator.GetExpectedValue(state);

            //Player data is wrong.
            //should be:
            //[0] 2000,200,11
            //[1] 2000,100,10
            //[2] 2000,0,29

            // DO separate calculate ranges test in helper
            // with the same state


            Assert.Equal(0.2, result);
        }

        private static BoardState GetThreePlayerBoardState(string handCode)
        {
            return new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.eight, Engine.Enums.CardSuit.Clubs),
                NumberOfPlayers = 3,
                HandCode = handCode,
                BigBlind = 200,
                MyPosition = 1,
                Players = new Player[]
                {
                    new Player
                    {
                        IsDealer = true,
                        Position = 1,
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = true
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 2, // BB
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 3, // SB
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };
        }
    }
}
