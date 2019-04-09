using Brunt.Twilight.Service;
using Brunt.Twilight.Sky;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Brunt.Twilight.Tests
{
    public class BruntSkyTest
    {
        BruntClient bruntClient;
        Config config;

        public BruntSkyTest()
        {
            bruntClient = new BruntClient();
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AssemblyDirectory + "\\appsettings.json"));
        }

        [Fact]
        public async Task Login()
        {
            BruntLoginCredz loginCredz = new BruntLoginCredz()
            {
                ID = config.ID,
                PASS = config.PASS
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

        [Fact(Skip = "Don't want to change blind setting.")]
        public async Task SetDevicePosition()
        {
            await Login();

            var devices = await bruntClient.GetDevices();
            var livingRoom = devices.ToList().SingleOrDefault(d => d.NAME == "Livingroom");

            BruntDevicePositionChange bdpc = new BruntDevicePositionChange()
            {
                DeviceName = livingRoom.thingUri,
                requestPosition = int.Parse(livingRoom.requestPosition) > 100 ? config.SunrisePosition : config.SunsetPosition
            };

            var devicePositionChange = await bruntClient.SetDevicePosition(bdpc);

            Assert.NotNull(devicePositionChange);
            Assert.True(devicePositionChange);

            await Task.Delay(new TimeSpan(0, 0, 30));
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
