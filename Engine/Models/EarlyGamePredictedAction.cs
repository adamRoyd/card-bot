using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class EarlyGamePredictedAction : PredictedAction
    {
        public EarlyGamePredictedAction()
        {

        }

        public override ActionType GetAction()
        {
            if (_state.StartingHand == null)
            {
                return ActionType.Fold;
            }

            switch (_state.StartingHand.Rank)
            {
                case 1:
                    return ActionType.Raise;
                case 2:
                    return ActionType.Bet;
                case 3:
                    return GetLimpHandAction();
                case 4:
                case 5:
                    return ActionType.Fold;
                default:
                    return ActionType.Unknown;
            }
        }

        private ActionType GetLimpHandAction()
        {
            // TODO special consideration for small blind
            if (_state.MyPosition > 5)
            {
                Console.WriteLine("No limp from early position");
                return ActionType.Fold;
            }

            if (_state.CallAmount != _state.BigBlind)
            {
                Console.WriteLine("Call Amount does not match big blind");
                return ActionType.Fold;
            }

            if (_state.BigBlind > 50)
            {
                Console.WriteLine("Big blind too high");
                return ActionType.Fold;
            }

            return ActionType.Limp;
        }
    }
}
