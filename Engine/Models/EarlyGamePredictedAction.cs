﻿using Engine.Enums;
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
                return base.GetCheckOrFold();
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
                    return ActionType.Fold;
            }
        }

        private ActionType GetLimpHandAction()
        {
            if (_state.CallButton && _state.CallAmount == 0)
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

            double positionRatio = (double)_state.MyPosition / (double)_state.NumberOfPlayers;

            Console.WriteLine($"GetLimpHandAction positionRatio: {positionRatio}");

            if (positionRatio <= 0.60)
            {
                Console.WriteLine("No limp from early position");
                return ActionType.Fold;
            }

            return ActionType.Limp;
        }
    }
}
