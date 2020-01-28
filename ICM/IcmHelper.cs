using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICM
{
    public class IcmHelper
    {
        private static readonly short STACK = 0;
        private static readonly short BETS = 1;
        private static readonly short CALLRANGE = 2;

        public int GetHandIndex(string handCode)
        {
            var hands = GetHandArray();

            var result = Array.FindIndex(hands, h => h == handCode);

            return result;
        }

        // Engine Position starts at SB (1)
        // ICM Position starts at BB (0)
        public int GetIndexFromBigBlind(int myPosition, int numberOfPlayers)
        {
            int diff = numberOfPlayers - myPosition;

            if (diff == 1) // BB
            {
                return 0;
            }
            if (diff == 0) // SB
            {
                return 1;
            }
            return myPosition + 1; // All other positions
        }

        public double[,] GetPlayerData(BoardState _state, double[,] playersData)
        {
            _state.Players.Each((player, n) =>
            {
                //var position = GetIcmPosition(_state.Players, player);

                var truePosition = Array.IndexOf(_state.Players, player) + 1;

                var enginePosition = GetEnginePosition(_state.Players, player);

                var index = GetIndexFromBigBlind(truePosition, _state.NumberOfPlayers);

                playersData[index, STACK] = Convert.ToDouble(player.Stack);
                playersData[index, BETS] = Convert.ToDouble(player.Bet);
            });

            CalculateRanges(_state, playersData, true);

            return playersData;
        }

        private int GetEnginePosition(Player[] players, Player player)
        {
            var playersInGame = players.Where(p => !p.Eliminated).OrderBy(p => p.Position);

            var truePosition = Array.IndexOf(players, player) + 1;

            var dealer = players.FirstOrDefault(p => p.IsDealer);

            if (dealer == null)
            {
                Console.WriteLine("WARNING Dealer is null");
                return -1;
            }

            var dealerPosition = 1;

            foreach (var p in playersInGame)
            {
                if (p.IsDealer)
                {
                    break;
                }
                else
                {
                    dealerPosition++;
                }
            }

            // Rewrite the formula as this is not working and is very confusing!!
            // We know where the dealer is in the list
            // Keep the counting clockwise and find the position relative to the 
            // BB, who will always be two positions behind the dealer.
            var myPosition = playersInGame.Count() + truePosition - dealerPosition;
            return myPosition;
        }

        public void CalculateRanges(BoardState _state, double[,] playersData, bool isPush)
        {

            BlindInfo blindInfo = GetBlindInfo(_state.BigBlind);

            var nosb = false;
            var award = new Award
            {
                name = "9 max",
                playercount = 9,
                wins = { 50, 30, 20 }
            };

            calcRanges ranges = new calcRanges();
            int PLAYERCOUNT = 10;
            int[] playerrange = new int[PLAYERCOUNT];
            int[] stacks = new int[PLAYERCOUNT];
            int indexFromBigBlind = -1;

            for (int i = 0; i < PLAYERCOUNT; i++)
                playerrange[i] = 1;

            int found = 0;
            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                if (playersData[i, STACK] != 0)
                    stacks[found++] = (int)playersData[i, STACK];
            }

            // Get index number of player that is all in (which is you!)
            // Index is the position from the BB. SB is 1, D is 2 etc.
            if (isPush)
            {
                indexFromBigBlind = -1;
                var me = _state.Players.First(p => p.Position == 1);

                var truePosition = Array.IndexOf(_state.Players, me) + 1;
                var myPosition = _state.MyPosition;

                indexFromBigBlind = GetIndexFromBigBlind(_state.MyPosition, _state.NumberOfPlayers);
            }
            else
            {
                var allInPlayer = _state.Players.FirstOrDefault(p => p.IsAllIn);

                if (allInPlayer != null)
                {
                    indexFromBigBlind = -1; // TODO convert position to PositionFromBigBlind

                }
            }

            ranges.calc(
                found, 
                stacks,
                indexFromBigBlind, 
                blindInfo.Bigblind, 
                blindInfo.Ante, 
                nosb, 
                0.1, 
                award.wins.ToArray(), 
                playerrange
            );

            Console.Write("playerrange: ");
            for (int i = 0; i < 10; i++)
                Console.Write(playerrange[i] + ", ");
            Console.Write("\n");

            for (int i = 0; i < found; i++)
            {
                playersData[i, CALLRANGE] = playerrange[i];
            }
        }

        // TODO get antes
        private BlindInfo GetBlindInfo(int bigBlind)
        {
            var smallBlind = bigBlind / 2;
            return new BlindInfo(smallBlind, bigBlind, 0);
        }

        private static string[] GetHandArray()
        {
            var hands = new string[] {
            "AAo",
            "AKs",
            "AKo",
            "AQs",
            "AQo",
            "AJs",
            "AJo",
            "ATs",
            "ATo",
            "A9s",
            "A9o",
            "A8s",
            "A8o",
            "A7s",
            "A7o",
            "A6s",
            "A6o",
            "A5s",
            "A5o",
            "A4s",
            "A4o",
            "A3s",
            "A3o",
            "A2s",
            "A2o",
            "KKo",
            "KQs",
            "KQo",
            "KJs",
            "KJo",
            "KTs",
            "KTo",
            "K9s",
            "K9o",
            "K8s",
            "K8o",
            "K7s",
            "K7o",
            "K6s",
            "K6o",
            "K5s",
            "K5o",
            "K4s",
            "K4o",
            "K3s",
            "K3o",
            "K2s",
            "K2o",
            "QQo",
            "QJs",
            "QJo",
            "QTs",
            "QTo",
            "Q9s",
            "Q9o",
            "Q8s",
            "Q8o",
            "Q7s",
            "Q7o",
            "Q6s",
            "Q6o",
            "Q5s",
            "Q5o",
            "Q4s",
            "Q4o",
            "Q3s",
            "Q3o",
            "Q2s",
            "Q2o",
            "JJo",
            "JTs",
            "JTo",
            "J9s",
            "J9o",
            "J8s",
            "J8o",
            "J7s",
            "J7o",
            "J6s",
            "J6o",
            "J5s",
            "J5o",
            "J4s",
            "J4o",
            "J3s",
            "J3o",
            "J2s",
            "J2o",
            "TTo",
            "T9s",
            "T9o",
            "T8s",
            "T8o",
            "T7s",
            "T7o",
            "T6s",
            "T6o",
            "T5s",
            "T5o",
            "T4s",
            "T4o",
            "T3s",
            "T3o",
            "T2s",
            "T2o",
            "99o",
            "98s",
            "98o",
            "97s",
            "97o",
            "96s",
            "96o",
            "95s",
            "95o",
            "94s",
            "94o",
            "93s",
            "93o",
            "92s",
            "92o",
            "88o",
            "87s",
            "87o",
            "86s",
            "86o",
            "85s",
            "85o",
            "84s",
            "84o",
            "83s",
            "83o",
            "82s",
            "82o",
            "77o",
            "76s",
            "76o",
            "75s",
            "75o",
            "74s",
            "74o",
            "73s",
            "73o",
            "72s",
            "72o",
            "66o",
            "65s",
            "65o",
            "64s",
            "64o",
            "63s",
            "63o",
            "62s",
            "62o",
            "55o",
            "54s",
            "54o",
            "53s",
            "53o",
            "52s",
            "52o",
            "44o",
            "43s",
            "43o",
            "42s",
            "42o",
            "33o",
            "32s",
            "32o",
            "22o"
            };

            return hands;
        }
    }
}

