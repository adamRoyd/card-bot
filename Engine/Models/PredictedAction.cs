using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public abstract class PredictedAction
    {
        public List<Hand> Hands { get; set; }
        public int? HandRank { get; set; }
        public int MinSagePush { get; set; }
        public BoardState _state { get; set; }

        public abstract ActionType GetAction();

        internal ActionType GetCheckOrFold()
        {
            if (_state.CallButton && _state.CallAmount == 0)
            {
                return ActionType.Check;
            }
            else
            {
                return ActionType.Fold;
            }
        }

    }
}
