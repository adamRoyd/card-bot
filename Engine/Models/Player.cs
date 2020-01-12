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
    }
}
