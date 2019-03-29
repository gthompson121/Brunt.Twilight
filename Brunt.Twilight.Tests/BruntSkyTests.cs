using Brunt.Twilight.Sky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Brunt.Twilight.Tests
{
    public class BruntSkyTest
    {
        BruntClient bruntClient;

        public BruntSkyTest()
        {
            bruntClient = new BruntClient();
        }

        [Fact]
        public async Task Login()
        {
            BruntLoginCredz loginCredz = new BruntLoginCredz()
            {
                ID = "gthompson121@gmail.com",
                PASS = "Chubby17*"
            };

            var loginInfo = await bruntClient.Login(loginCredz);

            Assert.NotNull(loginInfo);
            Assert.True(loginCredz.ID == loginInfo.ID);
            Assert.False(string.IsNullOrEmpty(loginInfo.sessionId));
        }

        [Fact]
        public async Task GetBruntDevices()
        {
            await Login();

            var devices = await bruntClient.GetDevices();

            Assert.NotNull(devices);
            Assert.True(devices.Length == 2);
        }

        [Fact]
        public async Task SetDevicePosition()
        {
            await Login();

            var devices = await bruntClient.GetDevices();
            var livingRoom = devices.ToList().SingleOrDefault(d => d.NAME == "Livingroom");

            BruntDevicePositionChange bdpc = new BruntDevicePositionChange()
            {
                DeviceName = livingRoom.thingUri,
                requestPosition = int.Parse(livingRoom.requestPosition) > 100 ? "50" : "100"
            };

            var devicePositionChange = await bruntClient.SetDevicePosition(bdpc);

            Assert.NotNull(devicePositionChange);
            Assert.True(devicePositionChange);

            await Task.Delay(new TimeSpan(0, 0, 30));
        }
    }
}
