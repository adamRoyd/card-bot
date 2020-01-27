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
            return 0;
        }

        // MyPosition starts at BB (1) and counts clockwise
        // ICM Position starts at BB (0) and counts anti clockwise
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
                playersData[n, STACK] = Convert.ToDouble(player.Stack);
                playersData[n, BETS] = Convert.ToDouble(player.Bet);
            });

            CalculateRanges(_state, playersData, true);

            return playersData;
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

            // Get index number of player that is all in.
            // Index is the position from the BB. SB is 1, D is 2 etc.
            if (isPush)
            {
                indexFromBigBlind = -1;
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

            ranges.calc(found, stacks, indexFromBigBlind, blindInfo.Bigblind, blindInfo.Ante, nosb, 0.1, award.wins.ToArray(), playerrange);

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
    }
}
