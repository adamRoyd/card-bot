using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class PushFoldPredictedAction : PredictedAction
    {
        public override ActionType GetAction()
        {
            if (_state.StartingHand == null)
            {
                return ActionType.Fold;
            }

            switch (_state.StartingHand.Rank)
            {
                case 1:
                case 2: // TODO take into position...
                    return ActionType.AllIn;
                case 3:
                    return ActionType.AllInSteal;
                case 4:
                    return ActionType.Fold;
                default:
                    return ActionType.Unknown;
            }
        }
    }
}
