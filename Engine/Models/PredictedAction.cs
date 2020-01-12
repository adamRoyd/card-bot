using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public abstract class PredictedAction
    {
        public ActionType Action 
        {
            get { return GetAction(); }
            set { }
        }

        public BoardState _state { get; set; }

        public abstract ActionType GetAction();
    }
}
