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
        public int Position { get; set; }
        public int Pot { get; set; }
        public Stage Stage { get; set; }
        public Action Action { get; set; }

        public string GetHandCode()
        {
            string suited = StartingCard1.Suit == StartingCard2.Suit ? "s" : "o";

            string handCode = $"{StartingCard1.Value.ToString()}{StartingCard2.Value.ToString()}{suited}";

            return handCode;
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
