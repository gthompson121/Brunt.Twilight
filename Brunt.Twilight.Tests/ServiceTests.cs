using Brunt.Twilight.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Brunt.Twilight.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void OnStartTest()
        {
            var service = new BruntTwilight();
            service.Start();
        }

        [Fact]
        public void OnTimerTest()
        {
            var service = new BruntTwilight();
            service.OnTimer(null, null, true);
        }
    }
}
