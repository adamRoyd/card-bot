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
                case 2: // TODO take into position...
                    return ActionType.AllIn;
                case 3:
                    return ActionType.AllInSteal;
                case 4:
                    return base.GetCheckOrFold();
                default:
                    return ActionType.Unknown;
            }
        }
    }
}
