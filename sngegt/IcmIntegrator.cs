using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNGEGT
{
    public class IcmIntegrator
    {
        private static readonly short STACK = 0;
        private static readonly short BETS = 1;
        private static readonly short CALLRANGE = 2;
        //private static readonly short HOLDPERC = 3;
        private static readonly short CALLPERC = 4;
        private static readonly short WINPERC = 5;
        private static readonly short EVWIN = 6;
        private static readonly short EVLOSE = 7;
        private static readonly short PREPOST = 8;
        private ICM icm;
        private BlindInfo blindInfo;
        private double[,] playersData;
        private Player[] players;

        public IcmIntegrator()
        {
            playersData = new double[10, 9];
        }

        public double GetExpectedValue()
        {
            icm = new ICM();
            var numberOfPlayers = 4;

            players = new Player[4]; //TODO replace with engine player
            var myHandIndex = indexFromHand(new int[] { 1, 2 }); // is this correct? might be easier to compare
            // hand codes instead
            int myIndex = 1; // Is this my position?
            blindInfo = new BlindInfo(100, 200, 0);

            double[,] playerData = GetPlayerData(numberOfPlayers);

            var results = new double[] { 0, 0 };

            var moneyPayouts = new double[] { 0.5, 0.3, 0.2 };

            icm.calcPush(
                numberOfPlayers, 
                myHandIndex, 
                myIndex, 
                playerData, 
                results, 
                moneyPayouts, 
                moneyPayouts.Length
            );

            return results[0];
        }

        private double[,] GetPlayerData(int playersCount)
        {

            for (int i = 0; i < playersCount; i++)
            {
                if (players[i].chips.Text != "")
                    playersData[i, STACK] = Convert.ToDouble(players[i].chips.Text);

                if (players[i].bets.Text != "")
                    playersData[i, BETS] = Convert.ToDouble(players[i].bets.Text);
            }

            CalculateRanges(blindInfo, true);


            return playersData;
        }

        public void CalculateRanges(BlindInfo blinds, bool isPush)
        {
            var nosb = false;
            var award = new Award
            {
                name = "9 man",
                playercount = 9,
                wins = { 50, 30, 20 }
            };

            calcRanges ranges = new calcRanges();
            int PLAYERCOUNT = 10;
            int[] playerrange = new int[PLAYERCOUNT];
            int[] stacks = new int[PLAYERCOUNT];
            int allin = -1;

            for (int i = 0; i < PLAYERCOUNT; i++)
                playerrange[i] = 1;

            int found = 0;
            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                if (playersData[i, STACK] != 0)
                    stacks[found++] = (int)playersData[i, STACK];
            }

            // jos ollaan Push -moodissa, niin etsitään oma indeksi pääohjelman vasemmanpuolisimmista radio buttoneista
            if (isPush)
            {
                allin = -1;
                for (int i = 0; i < 10; i++)
                {
                    if (players[i].position.Checked == true)
                    {
                        allin = i;
                        break;
                    }
                }
            }
            // call -moodissa korottaja löytyy all-in radio -buttoneiden avulla
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (players[i].allin.Checked == true)
                    {
                        allin = i;
                        break;
                    }
                }
            }

            ranges.calc(found, stacks, allin, blinds.Bigblind, blinds.Ante, nosb, 0.1, award.wins.ToArray(), playerrange);

            Console.Write("playerrange: ");
            for (int i = 0; i < 10; i++)
                Console.Write(playerrange[i] + ", ");
            Console.Write("\n");

            for (int i = 0; i < found; i++)
                setRange(i, playerrange[i], true);
        }

        public void setRange(int player, int range, bool updateui)
        {
            playersData[player, CALLRANGE] = range;
        }

        public int indexFromHand(int[] iCards)
        {
            // Failure return AA, THIS ISN'T CORRECT!!!!!!
            if (iCards[0] < 0 || iCards[1] < 0)
                return 0;

            int[] card = { 0, 0 };

            // suited
            if (iCards[0] / 13 == iCards[1] / 13)
            {
                if (iCards[0] > iCards[1])
                    return indexArray[iCards[0] % 13, iCards[1] % 13];
                else
                    return indexArray[iCards[1] % 13, iCards[0] % 13];
            }
            // unsuited
            else
            {
                if (iCards[0] % 13 > iCards[1] % 13)
                    return indexArray[iCards[1] % 13, iCards[0] % 13];
                else
                    return indexArray[iCards[0] % 13, iCards[1] % 13];
            }
        }


        private static readonly short[,] indexArray = {
        {168, 167, 164, 159, 152, 143, 132, 119, 104, 87, 68, 47, 24},
        {166, 165, 162, 157, 150, 141, 130, 117, 102, 85, 66, 45, 22},
        {163, 161, 160, 155, 148, 139, 128, 115, 100, 83, 64, 43, 20},
        {158, 156, 154, 153, 146, 137, 126, 113,  98, 81, 62, 41, 18},
        {151, 149, 147, 145, 144, 135, 124, 111,  96, 79, 60, 39, 16},
        {142, 140, 138, 136, 134, 133, 122, 109,  94, 77, 58, 37, 14},
        {131, 129, 127, 125, 123, 121, 120, 107,  92, 75, 56, 35, 12},
        {118, 116, 114, 112, 110, 108, 106, 105,  90, 73, 54, 33, 10},
        {103, 101,  99,  97,  95,  93,  91,  89,  88, 71, 52, 31,  8},
        {86,   84,  82,  80,  78,  76,  74,  72,  70, 69, 50, 29,  6},
        {67,   65,  63,  61,  59,  57,  55,  53,  51, 49, 48, 27,  4},
        {46,   44,  42,  40,  38,  36,  34,  32,  30, 28, 26, 25,  2},
        {23,   21,  19,  17,  15,  13,  11,   9,   7,  5,  3,  1,  0}
        };
    }
}
