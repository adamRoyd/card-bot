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
        public HandType HandType {
            get { return GetHandType(); }
        }

        private HandType GetHandType()
        {
            List<Card> cards = new List<Card>
            {
                StartingCard1,
                StartingCard2,
                Flop1,
                Flop2,
                Flop3,
                Turn,
                River
            };

            cards = cards.Where(c => c != null).OrderBy(c => c.Value).ToList();

            var duplicates = cards.GroupBy(x => x.SimpleValue)
              .Where(g => g.Count() > 1)
              .Select(y => new { Value = y.Key, Count = y.Count() })
              .ToList();

            foreach (var card in cards)
            {
                Console.WriteLine($"card! {card.SimpleValue}");
            };

            foreach (var dupilcate in duplicates)
            {
                Console.WriteLine($"duplicate! Value: {dupilcate.Value} Count: {dupilcate.Count}");
            }

            return HandType.HighCard;
        }

        public string HandCode
        {
            get { return GetHandCode(); }
            set { }
        }
        public int SageRank
        {
            get { return GetSageRank(); }
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
        public int MyStackRatio
        {
            get { return GetMyStack(); }
            set { }
        }
        public int Pot { get; set; }
        public int BigBlind { get; set; }
        public int CallAmount { get; set; }
        public int RaiseAmount { get; set; }
        public bool FoldButton { get; set; }
        public bool CallButton { get; set; }
        public bool RaiseButton { get; set; }
        public bool ReadyForAction
        {
            get { return (FoldButton || CallButton || RaiseButton) && HandCode != null; }
            set { }
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

        private int GetMyStack()
        {
            var me = Players.First(p => p.Position == 1);

            if (me.Stack == 0 || BigBlind == 0)
            {
                return 999999;
            }
            var stackRatio = me.Stack / BigBlind;
            return stackRatio;
        }


        private int GetMyPosition()
        {
            var playersInGame = Players.Where(p => !p.Eliminated).OrderBy(p => p.Position);

            var dealer = Players.FirstOrDefault(p => p.IsDealer);

            if (dealer == null)
            {
                Console.WriteLine("WARNING Dealer is null");
                return -1;
            }

            var dealerPosition = 1;

            foreach (var player in playersInGame)
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
            // 1 is SB
            // 2 is BB
            // 3 is cutoff etc
            var myPosition = playersInGame.Count() + 1 - dealerPosition;
            return myPosition;
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

        private int GetSageRank()
        {
            if (StartingCard1 == null || StartingCard2 == null)
            {
                return 0;
            }

            int result = 0;

            if (StartingCard1.Suit == StartingCard2.Suit)
            {
                result += 4;
            }

            var card1Number = (int)StartingCard1.Value;
            var card2Number = (int)StartingCard2.Value;

            if (card1Number == card2Number)
            {
                result += 22;
            }

            var highestNumber = card1Number > card2Number ? card1Number : card2Number;
            var lowestNumber = card1Number > card2Number ? card2Number : card1Number;

            result = result + (highestNumber * 2) + lowestNumber;

            if((card1Number - card2Number == 1) || (card1Number - card2Number == -1))
            {
                result += 2;
            }

            return result;
        }

        private HandStage GetHandStage()
        {
            if (Flop1 == null)
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
