using Engine.Enums;
using System.Collections.Generic;

namespace Engine.Models
{
    public abstract class PredictedAction
    {
        public List<Hand> Hands { get; set; }
        public int? HandRank { get; set; }
        public BoardState _state { get; set; }
        public double _ev { get; set; }
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
