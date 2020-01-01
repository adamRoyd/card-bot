using System;

namespace Engine.Models
{
    public class StartingHand
    {
        public StartingHand(Card card1, Card card2)
        {
            Card1 = card1;
            Card2 = card2;

            HandCode = GetHandCode();
        }

        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        public string HandCode { get; set; }

        private string GetHandCode()
        {
            string suited = Card1.Suit == Card2.Suit ? "s" : "o";

            string handCode = $"{Card1.Value.ToString()}{Card2.Value.ToString()}{suited}";

            return handCode;
        }
    }
}
