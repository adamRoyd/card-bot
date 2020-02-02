using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Player
    {
        public int Position { get; set; }
        public int Stack { get; set; }
        public int Bet { get; set; }
        public bool IsDealer { get; set; }
        public bool Eliminated { get; set; }
        public bool IsAllIn { 
            get { return Stack == 0; }
            set { }
        }
    }
}
