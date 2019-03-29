using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.API
{
    public class SunsetSunriseClient
    {
        public SunsetSunriseClient(string uri, double lng, double lat, DateTime date)
        {
            _uri = uri;
            _lat = lat;
            _lng = lng;
            _date = date;
        }

        private string _uri { get; set; }
        private double _lng { get; set; }
        private double _lat { get; set; }
        public DateTime _date { get; set; }

        public async Task<SunriseSunset> GetSunriseSunsetForDate()
        {
            string formatedUri = string.Format(_uri, _lat, _lng, _date.Date.ToString("yyyy-MM-dd"));
            using (var client = new HttpClient())
            {
                var msg = new HttpRequestMessage(HttpMethod.Get, formatedUri);

                var responseMsg = await client.SendAsync(msg);
                if (!responseMsg.IsSuccessStatusCode) return null;

                var json = await responseMsg.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<SunriseSunsetResponse>(json);
                if (response.status != StatusCode.OK) return null;

                return response.results;
            }
        }

        public Tuple<double, bool> GetIntervalTillNextTwilight(SunriseSunset twilightInfo, DateTime utcNow)
        {
            // between sunrise and sunset
            if (twilightInfo.sunrise < utcNow && twilightInfo.sunset > utcNow)
            {
                return new Tuple<double,bool>((twilightInfo.sunset - utcNow).TotalMilliseconds, true);
            }
            // else past sunset but same day
            else if (twilightInfo.sunrise > utcNow && twilightInfo.sunset > utcNow)
            {
                return new Tuple<double, bool>((twilightInfo.sunrise - utcNow).TotalMilliseconds, false);
            }
            // else before sunrise
            else
            {
                return new Tuple<double, bool>((utcNow - twilightInfo.sunrise).TotalMilliseconds, false);
            }
        }
    }
}
