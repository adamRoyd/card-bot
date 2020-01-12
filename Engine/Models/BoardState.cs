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
        public Player[] Players { get; set; }
        public int NumberOfPlayers
        {
            get { return Players.Where(p => !p.Eliminated).Count(); }
            set { }
        }
        public int MyPosition
        {
            get { return GetMyPosition(); }
            set { }
        }
        public int CallAmount { get; set; }
        public int Pot { get; set; }
        public int Stack1 { get; set; }
        public int BigBlind { get; set; }
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

        public BoardState()
        {
            Players = new Player[]
                {
                    new Player{
                        Position = 1
                    },
                    new Player{
                        Position = 2
                    },
                    new Player{
                        Position = 3
                    },
                    new Player{
                        Position = 4
                    },
                    new Player{
                        Position = 5
                    },
                    new Player{
                        Position = 6
                    },
                    new Player{
                        Position = 7
                    },
                    new Player{
                        Position = 8
                    },
                    new Player{
                        Position = 9
                    }
                };
        }


        private int GetMyPosition()
        {
            var playersInGame = Players.Where(p => !p.Eliminated).OrderBy(p => p.Position);

            var dealer = Players.FirstOrDefault(p => p.IsDealer);

            if (dealer == null)
            {
                Console.WriteLine("WARNING Dealer is null");
            }

            var dealerPosition = 1;

            foreach(var player in playersInGame)
            {
                if (player.IsDealer)
                {
                    break;
                }
                else
                {
                    dealerPosition++;
                }
            }

            var myPosition = playersInGame.Count() + 1 - dealerPosition; 

            return dealer.Position;
        }

        private GameStage GetGameStage()
        {
            if (BigBlind < 100)
            {
                return GameStage.EarlyGame;
            }

            if ((BigBlind >= 100 && BigBlind < 300))
            {
                return GameStage.MiddleGame;
            }

            if (true)
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

            var card1Number = (int)StartingCard1.Value;
            var card2Number = (int)StartingCard2.Value;

            string handCode = card1Number >= card2Number ? 
                $"{StartingCard1.SimpleValue}{StartingCard2.SimpleValue}{suited}" : 
                $"{StartingCard2.SimpleValue}{StartingCard1.SimpleValue}{suited}";

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
