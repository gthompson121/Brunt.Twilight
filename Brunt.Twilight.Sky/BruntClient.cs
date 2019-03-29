using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.Sky
{
    public class BruntClient
    {
        public BruntClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-gb");
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.brunt.v1+json");
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (iPhone; CPU iPhone OS 11_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E216");
        }

        string skyUri = "https://sky.brunt.co";
        string deviceUri = "https://thing.brunt.co:8080";
        string sessionId = "";

        private HttpClient httpClient { get; set; }

        public async Task<BruntLogin> Login(BruntLoginCredz loginCredz)
        {
            var msg = new HttpRequestMessage(HttpMethod.Post, skyUri + "/session");
            msg.Content = new StringContent(JsonConvert.SerializeObject(loginCredz), Encoding.UTF8, "application/x-www-form-urlencoded");

            var responseMsg = await httpClient.SendAsync(msg);
            if (!responseMsg.IsSuccessStatusCode) return null;

            var responseBody = await responseMsg.Content.ReadAsStringAsync();
            var loginInfo = JsonConvert.DeserializeObject<BruntLogin>(responseBody);

            if (loginInfo.status != "activate") return null;

            var sessionId = responseMsg.Headers.SingleOrDefault(h => h.Key == "Set-Cookie");
            if (!sessionId.Value.Any()) return null;
            loginInfo.sessionId = GetSesionId(sessionId.Value.First());

            return loginInfo;
        }

        private string GetSesionId(string setCookieValue)
        {
            var split = setCookieValue.Split(';');
            if (split.Length != 3) return string.Empty;

            var skySSSEIONID = split[0].Split('=');
            if (skySSSEIONID.Length != 2) return string.Empty;

            sessionId = skySSSEIONID[1];

            return skySSSEIONID[1];
        }

        public async Task<BruntDevice[]> GetDevices()
        {
            var msg = new HttpRequestMessage(HttpMethod.Get, skyUri + "/thing");

            if (string.IsNullOrEmpty(sessionId)) return null;
            msg.Headers.Add("Cookie", "skySSEIONID=" + sessionId);

            var responseMsg = await httpClient.SendAsync(msg);
            if (!responseMsg.IsSuccessStatusCode) return null;

            var responseBody = await responseMsg.Content.ReadAsStringAsync();
            var devices = JsonConvert.DeserializeObject<BruntDevice[]>(responseBody);

            return devices;
        }

        public async Task<bool?> SetDevicePosition(BruntDevicePositionChange positionChange)
        {
            var msg = new HttpRequestMessage(HttpMethod.Put, deviceUri + "/thing" + positionChange.DeviceName);
            if (string.IsNullOrEmpty(sessionId)) return null;
            msg.Headers.Add("Cookie", "skySSEIONID=" + sessionId);

            msg.Content = new StringContent(JsonConvert.SerializeObject(positionChange), Encoding.UTF8, "application/x-www-form-urlencoded");

            var responseMsg = await httpClient.SendAsync(msg);
            if (!responseMsg.IsSuccessStatusCode) return null;

            return true;
        }
    }
}
