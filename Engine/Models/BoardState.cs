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
        public int CallAmount { get; set; }
        public int Pot { get; set; }
        public int Stack { get; set; }
        public int BigBlind { get; set; }
        public int Players { get; set; }
        public string FoldButton { get; set; }
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
        public PredictedAction PredictedAction
        {
            get { return GetPredictedAction(); }
            set { }
        }

        private PredictedAction GetPredictedAction()
        {
            var result = new PredictedAction();

            if (FoldButton != "fold")
            {
                // Not ready to make action
                result.ActionType = ActionType.Unknown;
                return result;
            }

            // Use polymorphism for each Card stage action type
            // Pre, post etc.
            // Early, Middle, Late etc.
            switch (HandStage)
            {
                case HandStage.PreFlop:
                    GetPreFlopAction(result);
                    break;
                default:
                    result.ActionType = ActionType.Unknown;
                    break;
            }

            return result;
        }

        private void GetPreFlopAction(PredictedAction action)
        {
            GetEarlyGamePreFlopAction(action);
            //switch (GameStage)
            //{
            //    case GameStage.EarlyGame:
            //        GetEarlyGamePreFlopAction(action);
            //        break;
            //    default:
            //        action.ActionType = ActionType.Unknown;
            //        break;
            //}
        }

        private void GetEarlyGamePreFlopAction(PredictedAction action)
        {
            switch (StartingHand.Rank)
            {
                case 1:
                case 2:
                case 3:
                    action.ActionType = ActionType.Unknown;
                    break;
            }
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
            if(StartingCard1 == null || StartingCard2 == null)
            {
                return "N/A";
            }

            string suited = StartingCard1.Suit == StartingCard2.Suit ? "s" : "o";

            var card1Number = (int)StartingCard1.Value;
            var card1 = card1Number < 11 ? card1Number.ToString() : StartingCard1.Value.ToString();
            var card2Number = (int)StartingCard2.Value;
            var card2 = card2Number < 11 ? card2Number.ToString() : StartingCard2.Value.ToString();

            string handCode = $"{card1}{card2}{suited}";

            return handCode;
        }

        private Hand GetStartingHand()
        {

            var hand = GameHands.EarlyGameHands.FirstOrDefault(h => h.HandCode == HandCode);

            if (hand == null)
            {
                hand = new Hand(HandCode, 0);
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
