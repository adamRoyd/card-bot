using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Card
    {
        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }

        public CardValue Value { get; set; }
        public CardSuit Suit { get; set; }
        public string SimpleValue
        {
            get { return GetSimpleValue(); }
            set { }
        }

        private string GetSimpleValue()
        {
            var cardNumber = (int)Value;
            var simpleValue = cardNumber < 11 ? cardNumber.ToString() : Value.ToString();
            return simpleValue;
        }
    }
}
