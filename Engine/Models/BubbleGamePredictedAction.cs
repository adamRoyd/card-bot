using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class BubbleGamePredictedAction : PredictedAction
    {
        public override ActionType GetAction()
        {
            if (_state.HandStage != HandStage.PreFlop)
            {
                return ActionType.Unknown;
            }

            switch (_state.MyPosition) {
                case 1: return GetSBAction();
                case 2: return GetBBAction();
                case 3: return GetUTCAction();
                case 4: return GetDealerAction();
                default:
                    return ActionType.Fold;
            }
        }

        private ActionType GetDealerAction()
        {
            throw new NotImplementedException();
        }

        private ActionType GetUTCAction()
        {
            throw new NotImplementedException();
        }

        private ActionType GetBBAction()
        {
            throw new NotImplementedException();
        }

        private ActionType GetSBAction()
        {
            throw new NotImplementedException();
        }
    }
}
