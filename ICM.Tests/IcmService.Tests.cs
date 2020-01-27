﻿using Engine.Models;
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
        public void GetExpectedValue_Runs()
        {
            var state = new BoardState
            {
                NumberOfPlayers = 3,
                HandCode = "AKs",
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
                        Position = 2,
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 3,
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    }
                }
            };

            var result = icmIntegrator.GetExpectedValue(state);

            Assert.Equal(1.4, result);
        }
    }
}
