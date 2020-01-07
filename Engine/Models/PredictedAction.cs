using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class PredictedAction
    {
        public ActionType ActionType { get; set; }
        public int BetAmount { get; set; }
    }

    public enum ActionType
    {
        Fold = 1,
        Check = 2,
        Bet = 3,
        Raise = 4,
        Unknown = 5
    }
}
