﻿using Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ICM.Tests
{
    public class IcmHelperTests
    {
        private readonly IcmHelper _helper;

        public IcmHelperTests()
        {
            _helper = new IcmHelper();
        }

        [Fact]
        public void GetHandIndex_AA()
        {
            const string handCode = "AAo";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetHandIndex_J7s_Returns76()
        {
            const string handCode = "J7s";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(76, result);
        }

        [Fact]
        public void GetHandIndex_55o_Returns153()
        {
            const string handCode = "55o";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(153, result);
        }

        [Fact]
        public void GetHandIndex_27o_Returns143()
        {
            const string handCode = "72o";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(143, result);
        }

        [Fact]
        public void GetHandIndex_22o_Returns168()
        {
            const string handCode = "22o";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(168, result);
        }

        [Fact]
        public void GetIndexFromBigBlind_3playersBB()
        {
            var players = new Player[]
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
                        Position = 8, // BB
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9, // SB
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    }
               };

            var result = _helper.GetPlayerIndex(players, players[0]);

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetIndexFromBigBlind_3playersSB_returns1()
        {
            var players = new Player[]
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
                        Position = 8, // BB
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9, // SB
                        Bet = 100,
                        Stack = 2000,
                        IsAllIn = false
                    }
               };

            var result = _helper.GetPlayerIndex(players, players[1]);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetIndexFromBigBlind_4playersBB()
        {
            var players = new Player[]
               {
                    new Player
                    {
                        IsDealer = false,
                        Position = 1,
                        Bet = 100, // SB
                        Stack = 2000,
                        IsAllIn = true
                    },
                    new Player
                    {
                        IsDealer = true,
                        Position = 2,
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = true
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8,
                        Bet = 0,
                        Stack = 2000,
                        IsAllIn = false
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9, // BB
                        Bet = 200,
                        Stack = 2000,
                        IsAllIn = false
                    }
               };

            var result = _helper.GetPlayerIndex(players, players[0]);

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetIndexFromBigBlind_5playersBB()
        {
            var players = new Player[]
               {
                    new Player
                    {
                        IsDealer = false,
                        Position = 1
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 2
                    },
                    new Player
                    {
                        IsDealer = true,
                        Position = 3
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 8
                    },
                    new Player
                    {
                        IsDealer = false,
                        Position = 9
                    }
               };

            var result = _helper.GetPlayerIndex(players, players[3]);

            Assert.Equal(3, result);
        }

    }
}