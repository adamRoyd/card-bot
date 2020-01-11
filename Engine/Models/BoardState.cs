using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public HandType HandType { get; set; }
        public string HandCode
        {
            get { return GetHandCode(); }
        }
        public bool Position1 { get; set; } = false;
        public bool Position2 { get; set; } = false;
        public bool Position3 { get; set; } = false;
        public bool Position4 { get; set; } = false;
        public bool Position5 { get; set; } = false;
        public bool Position6 { get; set; } = false;
        public bool Position7 { get; set; } = false;
        public bool Position8 { get; set; } = false;
        public bool Position9 { get; set; } = false;
        public int MyPosition
        {
            get { return GetMyPosition(); }
            set { }
        }
        public int CallAmount { get; set; }
        public int Pot { get; set; }
        public int Stack { get; set; }
        public int BigBlind { get; set; }
        public int Players { get; set; }
        public bool ReadyForAction { get; set; }

        public Hand StartingHand
        {
            get { return GetStartingHand(); }
        }
        public HandStage HandStage
        {
            get { return GetHandStage(); }
        }
        public GameStage GameStage
        {
            get { return GetGameStage(); }
        }

        private int GetMyPosition()
        {
            if (Position1)
            {
                return 1;
            }
            if (Position2)
            {
                return 2;
            }
            if (Position3)
            {
                return 3;
            }
            if (Position4)
            {
                return 4;
            }
            if (Position5)
            {
                return 5;
            }
            if (Position6)
            {
                return 6;
            }
            if (Position7)
            {
                return 7;
            }
            if (Position8)
            {
                return 8;
            }
            if (Position9)
            {
                return 9;
            }
            return 0;
        }

        private GameStage GetGameStage()
        {
            if (BigBlind < 100)
            {
                return GameStage.EarlyGame;
            }

            if ((BigBlind >= 100 && BigBlind < 300) && Players > 4)
            {
                return GameStage.MiddleGame;
            }

            if (Players < 5)
            {
                return GameStage.LateGame;
            }

            return GameStage.EarlyGame;
        }

        private string GetHandCode()
        {
            if (StartingCard1 == null || StartingCard2 == null)
            {
                return "null";
            }

            string suited = StartingCard1.Suit == StartingCard2.Suit ? "s" : "o";

            //TODO this should be in the Card class to get the cleaned up value for Flop turn river
            var card1Number = (int)StartingCard1.Value;
            var card1 = card1Number < 11 ? card1Number.ToString() : StartingCard1.Value.ToString();
            var card2Number = (int)StartingCard2.Value;
            var card2 = card2Number < 11 ? card2Number.ToString() : StartingCard2.Value.ToString();

            string handCode = card1Number >= card2Number ? $"{card1}{card2}{suited}" : $"{card2}{card1}{suited}";

            return handCode;
        }

        private Hand GetStartingHand()
        {

            var hand = GameHands.EarlyGameHands.FirstOrDefault(h => h.HandCode == HandCode);

            if (hand == null)
            {
                hand = new Hand(HandCode, -1);
            }

            return hand;
        }

        private HandStage GetHandStage()
        {
            if (Flop1 == null && Flop2 == null && Flop3 == null)
            {
                return HandStage.PreFlop;
            }
            else if (Turn == null)
            {
                return HandStage.Flop;
            }
            else if (River == null)
            {
                return HandStage.Turn;
            }
            else
            {
                return HandStage.River;
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
