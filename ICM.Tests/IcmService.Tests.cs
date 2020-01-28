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
        public void GetExpectedValue_ThreePlayerQ8s_CorrectEV()
        {
            BoardState state = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.Q, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.eight, Engine.Enums.CardSuit.Clubs),
                NumberOfPlayers = 3,
                BigBlind = 200,
                MyPosition = 1,
                Players = new Player[]
                {
                    new Player
                    {
                        IsDealer = true,
                        Position = 1, // index 2
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8, // BB //index 0
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9, // SB // index 1
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };


            var result = icmIntegrator.GetExpectedValue(state);
            Assert.Equal(0.2, result);
        }

        [Fact]
        public void GetExpectedValue_ThreePlayerT3o_CorrectEV()
        {
            BoardState state = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.ten, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.three, Engine.Enums.CardSuit.Hearts),
                NumberOfPlayers = 5,
                BigBlind = 200,
                MyPosition = 1,
                Players = new Player[]
                {
                    new Player
                    {
                        IsDealer = false,
                        Position = 1, //CO
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 5, //CO+1
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 6, //BB
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8,
                        Bet = 100, //SB
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = true,
                        Position = 9,
                        Bet = 0, //D
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };

            var result = icmIntegrator.GetExpectedValue(state);
            Assert.Equal(-0.3, result);
        }


        [Fact]
        public void GetExpectedValue_ThreePlayerA5s_CorrectEV()
        {
            BoardState state = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.A, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.five, Engine.Enums.CardSuit.Clubs),
                NumberOfPlayers = 5,
                BigBlind = 200,
                MyPosition = 1,
                Players = new Player[]
                {
                    new Player
                    {
                        IsDealer = false,
                        Position = 1, //CO
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 5, //CO+1
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 6, //BB
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8,
                        Bet = 100, //SB
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = true,
                        Position = 9,
                        Bet = 0, //D
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };

            var result = icmIntegrator.GetExpectedValue(state);
            Assert.Equal(0.4, result);
        }

        [Fact]
        public void GetExpectedValue_ThreePlayer42s_CorrectEV()
        {
            BoardState state = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.four, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Clubs),
                NumberOfPlayers = 5,
                BigBlind = 200,
                MyPosition = 1,
                Players = new Player[]
                {
                    new Player
                    {
                        IsDealer = false,
                        Position = 1, //SB
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = true,
                        Position = 5, //D
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 6, //CO
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8,
                        Bet = 0, //CO+1
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9,
                        Bet = 200, //BB
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };

            var result = icmIntegrator.GetExpectedValue(state);
            Assert.Equal(0.1, result);
        }

        [Fact]
        public void GetExpectedValue_AllIn42s_CorrectEV()
        {
            BoardState state = new BoardState
            {
                StartingCard1 = new Card(Engine.Enums.CardValue.four, Engine.Enums.CardSuit.Clubs),
                StartingCard2 = new Card(Engine.Enums.CardValue.two, Engine.Enums.CardSuit.Clubs),
                NumberOfPlayers = 5,
                BigBlind = 200,
                MyPosition = 1,
                Players = new Player[]
                {
                    new Player
                    {
                        IsDealer = false,
                        Position = 1, //SB
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = true,
                        Position = 5, //D
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = true
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 6, //CO
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8,
                        Bet = 0, //CO+1
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9,
                        Bet = 200, //BB
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };

            var result = icmIntegrator.GetExpectedValue(state);
            Assert.Equal(-7.4, result);
        }
    }
}
