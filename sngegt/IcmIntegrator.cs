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

        public IcmIntegrator()
        {
            playersData = new double[10, 9];
        }

        public double GetExpectedValue()
        {
            icm = new ICM();
            var players = 4;
            var myHand = new int[] { 1,1 };
            int myIndex = 1;
            blindInfo = new BlindInfo(100, 200, 0);

            double[,] playerData = GetPlayerData(players);

            var results = new double[] { 0, 0 };

            var moneyPayouts = new double[] { 0.5, 0.3, 0.2 };

            int icmc = 1;

            icm.calcPush(players, myHand, myIndex, playerData, moneyPayouts, icmc);

            return results[0];
        }

        private double[,] GetPlayerData(int playersCount)
        {

            var players = new Player[4];

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
    }
}
