using bot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace bot.tests
{
    public class HandHistoryTests
    {
        private readonly IHandHistoryService _handHistoryService;

        public HandHistoryTests(IHandHistoryService handHistoryService)
        {
            _handHistoryService = handHistoryService;
        }

        [Fact]
        public void GetLatestHandHistory_TestHistory_ReturnsCorrectHistory()
        {
            string path = $"..\\..\\..\\assets\\handHistories\\test.txt";

            _handHistoryService.GetPlayersFromHistory(path);

            Assert.NotNull(path);
        }
    }
}
