using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Engine.Models;

namespace Engine.Tests.Models
{
    public class StartingHand
    {
        [Fact]
        public void StartingHand_AKo_ReturnsCorrectHandCode()
        {

            var card1 = new Card(Enums.CardValue.ace, Enums.CardSuit.Clubs);

            var startingHand = new Engine.Models.StartingHand(); 
        }
    }
}
