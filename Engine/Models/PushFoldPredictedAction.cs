using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models
{
    public class PushFoldPredictedAction : PredictedAction
    {
        public PushFoldPredictedAction(BoardState state)
        {
            _state = state;
            Hands = GameHands.PushOrFoldHands;
            HandRank = Hands.FirstOrDefault(h => h.HandCode == _state.HandCode)?.Rank;
        }

        public override ActionType GetAction()
        {
            if (_state.HandStage != HandStage.PreFlop)
            {
                return base.GetCheckOrFold();
            }

            // Factors
            // My stack DONE
            // Card sage rank DONE
            // Position DONE
            // Call amount 
            // No. players
            var positionSageRank = GetMinimumSageRankForPosition();

            var stackFactored = FactorInStackSize(positionSageRank);

            // Reraising override
            if (_state.CallAmount > _state.BigBlind)
            {
                stackFactored = 55;
            }

            MinSagePush = stackFactored;

            if (_state.SageRank >= stackFactored)
            {
                return ActionType.AllIn;
            }
            else
            {
                return base.GetCheckOrFold();
            }
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
