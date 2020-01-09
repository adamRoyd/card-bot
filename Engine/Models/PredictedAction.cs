using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class PredictedAction
    {
        public ActionType ActionType 
        {
            get { return GetActionType(); }
            set { }
        }
        public int BetAmount
        {
            get { return GetBetAmount(); }
            set { }
        }

        private readonly BoardState _state;

        public PredictedAction(BoardState state)
        {
            _state = state;
        }

        public ActionType GetActionType()
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
                    return ActionType.Limp;
                default:
                    return ActionType.Unknown;
            }
        }

        private int GetBetAmount()
        {
            return 0;
        }
    }

    public enum ActionType
    {
        Fold = 1,
        Check = 2,
        Bet = 3,
        Raise = 4,
        Limp = 5,
        Unknown = 6
    }
}
