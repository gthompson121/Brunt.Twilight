using Brunt.Twilight.API;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Brunt.Twilight.Tests
{
    public class SunriseSunsetAPITests
    {
        private SunsetSunriseClient client;

        double lat = 55.9533;
        double lng = 3.1883;
        DateTime date = DateTime.Parse("2020-01-01");
        string uri = "https://api.sunrise-sunset.org/json?lat={0}&lng={1}&date={2}";

        public SunriseSunsetAPITests()
        {
            client = new SunsetSunriseClient(uri, lng, lat, date);
        }

        [Fact]
        public async Task GetSunRiseSunSet()
        {
            var twilight = await client.GetSunriseSunsetForDate();

            Assert.NotNull(twilight);
            Assert.True(twilight.sunrise == DateTime.Parse("3/29/2019 8:17:53 AM"));
            Assert.True(twilight.sunset == DateTime.Parse("3/29/2019 3:23:43 PM"));
        }

        public static object[][] dataList =
        {
            new object[] { new DateTime(2019,03,29,1,0,0), new DateTime(2019, 03, 29, 6, 0, 0), new DateTime(2019,03,29,19,0,0), false },
            new object[] { new DateTime(2019,03,29,4,0,0), new DateTime(2019, 03, 29, 6, 0, 0), new DateTime(2019,03,29,19,0,0), false },
            new object[] { new DateTime(2019,03,29,11,0,0), new DateTime(2019, 03, 29, 6, 0, 0), new DateTime(2019,03,29,19,0,0), true },
            new object[] { new DateTime(2019,03,29,14,0,0), new DateTime(2019, 03, 29, 6, 0, 0), new DateTime(2019,03,29,19,0,0), true },
            new object[] { new DateTime(2019,03,29,19,0,0), new DateTime(2019, 03, 30, 6, 0, 0), new DateTime(2019,03,30,19,0,0), false },
            new object[] { new DateTime(2019,03,29,22,0,0), new DateTime(2019, 03, 30, 6, 0, 0), new DateTime(2019,03,30,19,0,0), false },
        };

        [Theory, MemberData(nameof(dataList))]
        public void GetNextTwilight(DateTime now, DateTime sunrise, DateTime sunset, bool isSunset)
        {
            SunriseSunset ss = new SunriseSunset()
            {
                sunrise = sunrise,
                sunset = sunset
            };

            var result = client.GetIntervalTillNextTwilight(ss, now);

            if(isSunset)
            {
                Assert.True(result.Item1 == (sunset - now).TotalMilliseconds);
            }
            else
            {
                Assert.True(result.Item1 == (sunrise - now).TotalMilliseconds);
            }
        }
    }
}
