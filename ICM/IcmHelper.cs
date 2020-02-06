using Engine.Models;
using ICM.Objects;
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

            if (result == -1)
            {
                Console.WriteLine("WARNING GetHandIndex is -1");
            }

            return result;
        }

        public double[,] GetPlaterDataForPush(BoardState _state)
        {
            var playersData = new double[_state.NumberOfPlayers, 9];
            var filteredPlayers = _state.Players.Where(p => !p.Eliminated).ToArray();

            filteredPlayers.Where(p => !p.Eliminated).Each((player, n) =>
            {
                var index = GetPlayerIndex(_state, player);

                playersData[index, STACK] = Convert.ToDouble(player.Stack);
                playersData[index, BETS] = Convert.ToDouble(player.Bet);
            });

            var me = filteredPlayers.First(p => p.Position == 1);
            var myIndex = GetPlayerIndex(_state, me);

            playersData = RearrangeFoldedPlayers(playersData, _state, myIndex);

            CalculateRanges(_state, playersData);

            // We need to remove any bets from ICM calc as bets massively skew the EV calculation
            playersData = RemoveBets(playersData, _state);

            return playersData;
        }

        public double[,] GetPlayerDataForFold(BoardState _state)
        {
            var playersData = new double[_state.NumberOfPlayers, 9];
            var filteredPlayers = _state.Players.Where(p => !p.Eliminated).ToArray();

            filteredPlayers.Where(p => !p.Eliminated).Each((player, n) =>
            {
                var index = GetPlayerIndex(_state, player);

                playersData[index, STACK] = Convert.ToDouble(player.Stack);
                playersData[index, BETS] = Convert.ToDouble(player.Bet);
            });

            var me = filteredPlayers.First(p => p.Position == 1);
            var myIndex = GetPlayerIndex(_state, me);

            //playersData = RearrangeFoldedPlayers(playersData, _state, myIndex);

            CalculateRanges(_state, playersData);

            // We need to remove any bets from ICM calc as bets massively skew the EV calculation
            //playersData = RemoveBets(playersData, _state);

            return playersData;
        }


        private double[,] RearrangeFoldedPlayers(double[,] playersData, BoardState state, int myIndex)
        {
            List<PlayerData> playersDataList = new List<PlayerData>();
            var smallBlind = GetBlindInfo(state.BigBlind).Smallblind;

            // convert to list!
            for (var i = 0; i < state.NumberOfPlayers; i++)
            {
                playersDataList.Add(new PlayerData
                {
                    Stack = playersData[i, STACK],
                    Bet = playersData[i, BETS],
                    CallRange = playersData[i, CALLRANGE]
                });
            }

            // If the hero is the big blind, there are no non-betters ahead of hero
            // send hero to the back of the list and then
            // send non-betting players to back
            if (myIndex == 0)
            {
                playersDataList.Add(playersDataList.ElementAt(0));
                playersDataList.RemoveAt(0);


                var betterList = playersDataList.Where(p => p.Bet > smallBlind).ToList();
                var nonBetterList = playersDataList.Where(p => p.Bet <= smallBlind).ToList();

                foreach(var nonBetter in nonBetterList)
                {
                    betterList.Add(nonBetter);
                }

                playersDataList = betterList;
            }
            else
            {
                // search for non-betters ahead of hero
                // if found, send to back and re-scan the same index (since everyone shifted right)
                // stop at SB position or at hero position
                for (var i = 0; i < state.NumberOfPlayers; i++)
                {
                    var player = playersDataList.ElementAt(i);

                    if (i == myIndex || player.Bet == smallBlind)
                    {
                        break;
                    }

                    if (player.Bet == 0)
                    {
                        // send to back
                        playersDataList.Add(playersDataList.ElementAt(i));
                        playersDataList.RemoveAt(i);
                        //rescan
                        i -= 1;
                        myIndex -= 1;
                    }
                }
            }

            //convert back to 2d array (ugh)
            var newArray = new double[playersDataList.Count, 9];
            for (var i = 0; i < playersDataList.Count; i++)
            {
                var player = playersDataList.ElementAt(i);

                newArray[i, BETS] = player.Bet;
                newArray[i, STACK] = player.Stack;
                newArray[i, CALLRANGE] = player.CallRange;

            }

            return newArray;
        }

        private double[,] RemoveBets(double[,] playersData, BoardState _state)
        {
            var smallBlind = GetBlindInfo(_state.BigBlind).Smallblind;

            for (var i = 0; i < _state.NumberOfPlayers; i++)
            {
                if (playersData[i, BETS] >= _state.BigBlind)
                {
                    playersData[i, BETS] = 0;
                }
                else if (playersData[i, BETS] == smallBlind)
                {
                    // Reinstate true big blind
                    playersData[i - 1, BETS] = _state.BigBlind;
                }
            }

            return playersData;

        }

        public int GetPlayerIndex(BoardState _state, Player player)
        {
            try
            {
                var filteredPlayers = _state.Players.Where(p => !p.Eliminated).ToArray();

                var numberOfPlayers = filteredPlayers.Where(p => !p.Eliminated).Count();

                var playerPosition = Array.IndexOf(filteredPlayers, player) + 1;

                var dealerPosition = Array.IndexOf(filteredPlayers,
                                        filteredPlayers.FirstOrDefault(p => p.IsDealer)) + 1;

                var filteredList = filteredPlayers.Where(p => p.Bet >= _state.BigBlind).OrderBy(p => p.Position).ToList();
                var earliestBetter = filteredList.FirstOrDefault();

                if (earliestBetter == null)
                {
                    throw new Exception("Earliest Better is null");
                }

                var earliestBetterPosition = Array.IndexOf(filteredPlayers, earliestBetter) + 1;

                var index = -1;

                // Count up from big blind position and find a match
                for (var i = 0; i < numberOfPlayers; i++)
                {
                    if (earliestBetterPosition == playerPosition)
                    {
                        index = i;
                        break;
                    }

                    if (earliestBetterPosition == numberOfPlayers)
                    {
                        earliestBetterPosition = 0;
                    }

                    earliestBetterPosition++;
                }

                return index;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CalculateRanges(BoardState _state, double[,] playersData)
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
            int PLAYERCOUNT = _state.NumberOfPlayers;
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

            //TODO implement call properly
            var isPush = true;//_state.Players.Where(p => !p.Eliminated).All(p => !p.IsAllIn);

            // Get index number of player that is all in
            if (isPush)
            {
                indexFromBigBlind = -1;
                var me = _state.Players.First(p => p.Position == 1);

                indexFromBigBlind = GetPlayerIndex(_state, me);
            }
            else
            {
                var allInPlayer = _state.Players.FirstOrDefault(p => p.IsAllIn);

                indexFromBigBlind = GetPlayerIndex(_state, allInPlayer);
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

            //Console.Write("playerrange: ");
            //for (int i = 0; i < 10; i++)
            //    Console.Write(playerrange[i] + ", ");
            //Console.Write("\n");

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
            "A10s",
            "A10o",
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
            "K10s",
            "K10o",
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
            "Q10s",
            "Q10o",
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
            "J10s",
            "J10o",
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
            "1010o",
            "109s",
            "109o",
            "108s",
            "108o",
            "107s",
            "107o",
            "106s",
            "106o",
            "105s",
            "105o",
            "104s",
            "104o",
            "103s",
            "103o",
            "102s",
            "102o",
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

