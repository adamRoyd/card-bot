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
                return ActionType.Unknown;
            }

            switch (HandRank)
            {
                case 1:
                case 2:
                    return ActionType.AllIn;
                case 3:
                    return GetAllInStealAction();
                case 4:
                    return base.GetCheckOrFold();
                default:
                    return ActionType.Fold;
            }
        }

        private ActionType GetAllInStealAction()
        {
            if (_state.MyPosition == 1 && _state.CallAmount <= _state.BigBlind)
            {
                // SB Action
                return ActionType.AllInSteal;
            }

            double positionRatio = (double)_state.MyPosition / (double)_state.NumberOfPlayers;

            Console.WriteLine($"GetAllInStealAction positionRatio: {positionRatio}");

            if (positionRatio >= 0.75)
            {
                return ActionType.AllInSteal;
            }
            else
            {
                return ActionType.Fold;
            }
        }
    }
}
