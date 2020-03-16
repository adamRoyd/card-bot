using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public int Chips
        {
            get { return Stack + Bet; }
            set { }
        }
        public int Stack { get; set; } // How much is left after bets
        public int Bet { get; set; } // Bet on table
        public bool IsDealer { get; set; }
        public bool Eliminated { get; set; }
        public bool IsBlind { get; set; } = false;
        public bool IsAllIn { 
            get { return Chips == Bet; }
            set { }
        }
    }
}
