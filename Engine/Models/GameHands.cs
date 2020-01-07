using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{


    public static class GameHands
    {
        public static List<Hand> EarlyGameHands;
        static GameHands()
        {
            // Ranking...
            EarlyGameHands = new List<Hand>
            {
                // 1 is raise
                new Hand("AAo",1),
                new Hand("KKo",1),
                new Hand("QQo",1),
                new Hand("AKs",1),

                // 2 is bet
                new Hand("JJo",2),
                new Hand("1010o",2),
                new Hand("AQs",2),

                // 3 is limp
                new Hand("99o",3),
                new Hand("88o",3),
                new Hand("77o",3),
                new Hand("66o",3),
                new Hand("55o",3),
                new Hand("44o",3),
                new Hand("33o",3),
                new Hand("22o",3),
                new Hand("QKs",3),
                new Hand("JQs",3),
                new Hand("10Js",3),
                new Hand("910s",3),
                new Hand("89s",3),
                new Hand("78s",3),
                new Hand("67s",3),
                new Hand("56s",3),
                new Hand("45s",3),
                new Hand("34s",3)

            };
        }
    }
    
    public class Hand
    {
        public Hand(string handCode, int rank)
        {
            HandCode = handCode;
            Rank = rank;
        }

        public string HandCode { get; set; }
        public int Rank { get; set; }
    }
}
