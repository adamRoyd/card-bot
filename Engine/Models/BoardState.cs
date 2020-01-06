using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Engine.Models
{
    public class BoardState
    {
        public Card StartingCard1 { get; set; }
        public Card StartingCard2 { get; set; }
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }
        public HandType Hand { get; set; }
        public bool Position1 { get; set; } = false;
        public bool Position2 { get; set; } = false;
        public bool Position3 { get; set; } = false;
        public bool Position4 { get; set; } = false;
        public bool Position5 { get; set; } = false;
        public bool Position6 { get; set; } = false;
        public bool Position7 { get; set; } = false;
        public bool Position8 { get; set; } = false;
        public bool Position9 { get; set; } = false;
        public int? Bet1 { get; set; }
        public int? Bet2 { get; set; }
        public int? Bet3 { get; set; }
        public int? Bet4 { get; set; }
        public int? Bet5 { get; set; }
        public int? Bet6 { get; set; }
        public int? Bet7 { get; set; }
        public int? Bet8 { get; set; }
        public int? Bet9 { get; set; }
        public int CallAmount { get; set; }
        public int Pot { get; set; }
        public int Stack { get; set; }
        public Stage Stage {
            get { return GetStage(); }
        }
        public PredictedAction PredictedAction {
            get { return GetPredictedAction(); }
        }

        private PredictedAction GetPredictedAction()
        {
            var result = new PredictedAction();

            // The magic starts here

            return result;
        }

        private string GetHandCode()
        {
            string suited = StartingCard1.Suit == StartingCard2.Suit ? "s" : "o";

            string handCode = $"{StartingCard1.Value.ToString()}{StartingCard2.Value.ToString()}{suited}";

            return handCode;
        }

        private Stage GetStage()
        {
            if(Flop1 == null && Flop2 == null && Flop3 == null)
            {
                return Stage.PreFlop;
            }
            else if(Turn == null)
            {
                return Stage.Flop;
            }
            else if(River == null)
            {
                return Stage.Turn;
            }
            else
            {
                return Stage.River;
            }
        }

        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(BoardState);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(BoardState);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }
        }
    }
}
