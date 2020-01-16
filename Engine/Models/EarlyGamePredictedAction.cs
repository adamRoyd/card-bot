using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Models
{
    public class EarlyGamePredictedAction : PredictedAction
    {
        public EarlyGamePredictedAction(BoardState state)
        {
            _state = state;
            Hands = GameHands.EarlyGameHands;
            HandRank = Hands.FirstOrDefault(h => h.HandCode == _state.HandCode)?.Rank; 
        }

        public override ActionType GetAction()
        {
            if(_state.HandStage != HandStage.PreFlop)
            {
                return ActionType.Unknown;
            }

            switch (HandRank)
            {
                case 1:
                    return ActionType.Raise;
                case 2:
                    return ActionType.Bet;
                case 3:
                    return GetLimpHandAction();
                case 4:
                case 5:
                    return base.GetCheckOrFold();
                default:
                    return ActionType.Unknown;
            }
        }

        private ActionType GetLimpHandAction()
        {
            if (_state.CallAmount == 0) // TODO this needs to change
            {
                return ActionType.Check;
            }

            if (_state.CallAmount != _state.BigBlind)
            {
                Console.WriteLine("Call Amount does not match big blind");
                return ActionType.Fold;
            }

            if (_state.MyPosition == 1 || _state.MyPosition == 2)
            {
                // BB or SB
                return ActionType.Limp;
            }

            if (_state.BigBlind > 50)
            {
                Console.WriteLine("Big blind too high");
                return ActionType.Fold;
            }
            
            var positionDifference = _state.NumberOfPlayers - _state.MyPosition;

            if (positionDifference > 3) // further than 3 away from dealer button
            {
                Console.WriteLine("No limp from early position");
                return ActionType.Fold;
            }

            return ActionType.Limp;
        }
    }
}
