using Engine.Models;
using System;
using System.Linq;



namespace ICM
{
    public class IcmService
    {
        private ICMCalc icm;
        private BoardState _state;
        private IcmHelper _helper;

        public IcmService()
        {
            _helper = new IcmHelper();
        }

        public double GetExpectedValue(BoardState state)
        {
            try
            {
                _state = state;

                icm = new ICMCalc();
                var numberOfPlayers = _state.NumberOfPlayers;
                var me = _state.Players.First(p => p.Position == 1);

                var myHandIndex = _helper.GetHandIndex(_state.HandCode);
                int indexFromBigBlind = _helper.GetPlayerIndex(_state, me);

                var isPush = _state.Players.Where(p => !p.Eliminated).All(p => !p.IsAllIn);

                var results = new double[] { 0, 0 };

                var moneyPayouts = new double[] { 0.5, 0.3, 0.2 };

                if (isPush)
                {
                    double[,] playerData = _helper.GetPlaterDataForPush(_state);

                    icm.calcPush(
                        numberOfPlayers,
                        myHandIndex,
                        indexFromBigBlind,
                        playerData,
                        results,
                        moneyPayouts,
                        moneyPayouts.Length
                    );

                    var evFold = results[0] * 100;
                    evFold = Math.Round(evFold, 2);

                    var evPush = results[1] * 100;
                    evPush = Math.Round(evPush, 2);

                    Console.WriteLine($"Calc push p: {numberOfPlayers} hand: {myHandIndex} myPos: {indexFromBigBlind} " +
                        $"evFold: {evFold} evPush: {evPush}");

                    for (var i = 0; i < numberOfPlayers; i++)
                    {
                        Console.WriteLine($"Player {i} Stack: {playerData[i, 0]} " +
                            $"Bet: {playerData[i, 1]} Range: {playerData[i, 2]}");
                    }
                }
                else
                {

                    double[,] playerData = _helper.GetPlayerDataForFold(_state);

                    var allInPlayer = _state.Players.FirstOrDefault(p => p.IsAllIn);

                    // TODO get player index needs to be different for calculating a call?
                    var allInIndex = _helper.GetPlayerIndex(_state, allInPlayer);

                    icm.calcCall(
                        numberOfPlayers,
                        myHandIndex,
                        indexFromBigBlind,
                        allInIndex,
                        playerData,
                        results,
                        moneyPayouts,
                        moneyPayouts.Length
                    );

                    var evFold = results[0] * 100;
                    evFold = Math.Round(evFold, 2);

                    var evPush = results[1] * 100;
                    evPush = Math.Round(evPush, 2);

                    Console.WriteLine($"Calc fold p: {numberOfPlayers} hand: {myHandIndex} myPos: {indexFromBigBlind} " +
                        $"evFold: {evFold} evPush: {evPush}");

                    for (var i = 0; i < numberOfPlayers; i++)
                    {
                        Console.WriteLine($"Player {i} Stack: {playerData[i, 0]} " +
                            $"Bet: {playerData[i, 1]} Range: {playerData[i, 2]}");
                    }
                }

                var result = results[1] - results[0];

                result *= 100;

                var rounded = Math.Round(result, 1);

                return rounded;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Get expected value exception: {e}");
                return 0;
            }

        }
    }
}
