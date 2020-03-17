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
        public HandType HandType => GetHandType();

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

            foreach (Card card in cards)
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
            get => GetHandCode();
            set { }
        }
        public Player[] Players { get; set; }
        public int NumberOfPlayers
        {
            get => Players.Where(p => !p.Eliminated).Count();
            set { }
        }
        public int MyPosition
        {
            get => GetMyPosition();
            set { }
        }
        public int MyStackRatio
        {
            get => GetMyStack();
            set { }
        }
        public int Pot { get; set; }
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        public int CallAmount { get; set; }
        public int RaiseAmount { get; set; }
        public bool FoldButton { get; set; }
        public bool CallButton { get; set; }
        public bool RaiseButton { get; set; }
        public bool ReadyForAction
        {
            get => (FoldButton || CallButton || RaiseButton) && HandCode != null;
            set { }
        }
        public bool GameIsFinished { get; set; }
        public bool IsInPlay { get; set; }
        public bool SittingOut { get; set; }
        public HandStage HandStage => GetHandStage();
        public GameStage GameStage => GetGameStage();

        public BoardState(Player[] playersFromPreviousHand)
        {
            // Position goes ANTI CLOCKWISE starting at hero
            Players = GetPlayers();

            PlayersFromPreviousHand = playersFromPreviousHand;
        }

        public Player[] PlayersFromPreviousHand { get; set; }

        private int GetMyStack()
        {
            Player me = Players.First(p => p.Position == 1);

            if (me.Stack == 0 || BigBlind == 0)
            {
                return 999999;
            }
            int stackRatio = me.Stack / BigBlind;
            return stackRatio;
        }

        // My position - calculates position clockwise from SB.
        // If 7 players, dealer would be position 7.
        private int GetMyPosition()
        {
            IOrderedEnumerable<Player> playersInGame = Players.Where(p => !p.Eliminated).OrderBy(p => p.Position);

            Player dealer = Players.FirstOrDefault(p => p.IsDealer);

            if (dealer == null)
            {
                Console.WriteLine("WARNING Dealer is null");
                return -1;
            }

            int dealerPosition = 1;

            foreach (Player player in playersInGame)
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
            int myPosition = playersInGame.Count() + 1 - dealerPosition;
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

            int card1Number = (int)StartingCard1.Value;
            int card2Number = (int)StartingCard2.Value;

            string handCode = card1Number >= card2Number ?
                $"{StartingCard1.SimpleValue}{StartingCard2.SimpleValue}{suited}" :
                $"{StartingCard2.SimpleValue}{StartingCard1.SimpleValue}{suited}";

            return handCode;
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

        public Player[] GetPlayers()
        {
            return new Player[]
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
    }
}
