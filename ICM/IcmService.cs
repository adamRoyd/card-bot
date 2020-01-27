using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICM
{
    public class IcmService
    {

        private ICM icm;
        private BlindInfo blindInfo;
        private double[,] playersData;
        private BoardState _state;
        private IcmHelper _helper;


        public IcmService()
        {
            playersData = new double[10, 9];
            _helper = new IcmHelper();
        }

        public double GetExpectedValue(BoardState state)
        {
            _state = state;

            icm = new ICM();
            var numberOfPlayers = _state.NumberOfPlayers;

            var myHandIndex = _helper.GetHandIndex(_state.HandCode);
            int indexFromBigBlind = _helper.GetIndexFromBigBlind(_state.MyPosition, _state.NumberOfPlayers);

            double[,] playerData = _helper.GetPlayerData(_state, playersData);

            var results = new double[] { 0, 0 };

            var moneyPayouts = new double[] { 0.5, 0.3, 0.2 };

            icm.calcPush(
                numberOfPlayers,
                myHandIndex,
                indexFromBigBlind,
                playerData,
                results,
                moneyPayouts,
                moneyPayouts.Length
            );

            var result = results[1] - results[0];

            var rounded = Math.Round(result, 3);

            return rounded * 100;
        }
    }
}
