using Brunt.Twilight.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Brunt.Twilight.Tests
{
    public class ServiceTests
    {
        [Fact(Skip = "Just for debugging")]
        public void OnStartTest()
        {
            var service = new BruntTwilight();
            service.Start();
        }
    }
}
