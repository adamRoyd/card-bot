using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ICM.Tests
{
    public class IcmHelperTests
    {
        private readonly IcmHelper _helper;

        public IcmHelperTests()
        {
            _helper = new IcmHelper();
        }

        [Fact]
        public void GetHandIndex_AA_Returns0()
        {
            const string handCode = "AAo";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetHandIndex_AKs_Returns1()
        {
            const string handCode = "AKs";

            var result = _helper.GetHandIndex(handCode);

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetPlayerData_State1_ReturnsCorrectData()
        {

        }

    }
}
