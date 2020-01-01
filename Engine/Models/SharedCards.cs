using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class SharedCards
    {
        public List<Card> Flop { get; set; }

        public Card Turn { get; set; }

        public Card River { get; set; }

    }
}
