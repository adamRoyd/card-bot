using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models
{
    public class PushFoldPredictedAction : PredictedAction
    {
        public PushFoldPredictedAction(BoardState state, double ev)
        {
            _state = state;
            _ev = ev;
            Hands = GameHands.PushOrFoldHands;
            HandRank = Hands.FirstOrDefault(h => h.HandCode == _state.HandCode)?.Rank;
        }

        public override ActionType GetAction()
        {
            if (_state.HandStage != HandStage.PreFlop)
            {
                return base.GetCheckOrFold();
            }

            if (_ev >= 0.3)
            {
                return ActionType.AllIn;
            }
            else
            {
                return base.GetCheckOrFold();
            }

            // Factors
            // My stack DONE
            // Card sage rank DONE
            // Position DONE
            // Call amount 
            // No. players
            var minPushRank = GetMinimumSageRankForPosition();

            minPushRank = FactorInStackSize(minPushRank);

            minPushRank = FactorInLimps(minPushRank);

            minPushRank = FactorInNumberOfPlayers(minPushRank);

            // Reraising override
            //if (_state.CallAmount > _state.BigBlind || !_state.CallButton)
            //{
            //    minPushRank = 55;
            //}

            MinSagePush = minPushRank;

            if (_state.SageRank >= minPushRank)
            {
                return ActionType.AllIn;
            }
            else
            {
                return base.GetCheckOrFold();
            }
        }

        private int FactorInNumberOfPlayers(int minPushRank)
        {
            var looseness = (9 - _state.NumberOfPlayers) * 2;
            return minPushRank - looseness;
        }

        private int FactorInLimps(int minPushRank)
        {
            var numberOfLimpers = _state.Players.Where(
                                    p => p.Bet == _state.BigBlind)
                                                .Count() - 1;
            //Console.WriteLine($"Number of limpers: {numberOfLimpers}");

            var result = minPushRank + (numberOfLimpers * 3);

            var numberOfBetters = _state.Players.Where(
                                    p => p.Bet > _state.BigBlind)
                                                .Count();

            result += (numberOfBetters * 10);

            return result;
        }

        private int FactorInStackSize(int positionSageRank)
        {
            var difference = 15 - _state.MyStackRatio;

            var factored = positionSageRank - difference;

            return factored;
        }

        public int GetMinimumSageRankForPosition()
        {
            double ratio = (double)_state.MyPosition / (double)_state.NumberOfPlayers;

            // 1 will always be SB
            // 2 always BB
            // ratio of 1 is always dealer
            // the higher the ratio, the better the position
            if (_state.MyPosition == 1)
            {
                return 35;
            }

            if (_state.MyPosition == 2)
            {
                return 45;
            }

            switch (ratio)
            {
                case double n when (n == 1):
                    return 38;
                case double n when (n >= 0.75):
                    return 41;
                case double n when (n >= 0.66):
                    return 44;
                case double n when (n >= 0.5):
                    return 47;
                case double n when (n >= 0.2):
                    return 50;
                default:
                    return 50;
            }
        }
    }
}
