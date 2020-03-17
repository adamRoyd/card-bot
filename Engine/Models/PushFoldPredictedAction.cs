using Engine.Enums;
using System.Linq;

namespace Engine.Models
{
    public class PushFoldPredictedAction : PredictedAction
    {
        public PushFoldPredictedAction(BoardState state, double ev)
        {
            _state = state;
            _ev = ev;
        }

        public override ActionType GetAction()
        {
            if (_state.HandStage != HandStage.PreFlop)
            {
                return base.GetCheckOrFold();
            }

            if (_ev >= 0.2)
            {
                return ActionType.AllIn;
            }
            else
            {
                return base.GetCheckOrFold();
            }
        }
    }
}
