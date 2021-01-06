using AltradyNotifier.Entities.Pushover;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AltradyNotifier.Pushover
{
    public class Pushover
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly string _baseUrl;

        private readonly string _user;
        private readonly string _token;
        
        public Pushover(string user, string token)
        {
            _baseUrl = "https://api.pushover.net/1";

            _user = user.Trim();
            _token = token.Trim();
        }

        public async Task<Reponse.RateLimit> SendMessageAsync((string title, string message) titleMessage) 
            => await SendMessageAsync(titleMessage.title, titleMessage.message);

        public async Task<Reponse.RateLimit> SendMessageAsync(string title, string message)
        {
            var requestUrl = $"{_baseUrl}/messages.json";

            title = title.Substring(0, Math.Min(250, title.Length));
            message = message.Substring(0, Math.Min(1024 - title.Length, message.Length));

            object postData = new
            {
                token = _token,
                user = _user,
                title,
                message,
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUrl),
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json")
            };

            using var client = new HttpClient();

            var response = await client.SendAsync(request); 
            
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();

                return ParseSendMessageResponse(response.Headers);
            }
            else
            {
                Log.Debug($"Response successful: {response.IsSuccessStatusCode}, reason: {response.ReasonPhrase}");
            }

            return null;
        }

        public async Task<Reponse.RateLimit> GetRateLimitAsync()
        {
            string requestUrl = $"{_baseUrl}/apps/limits.json?token={_token}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl)
            };

            using var client = new HttpClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Reponse.RateLimit>(responseContent);
            }

            return null;
        }

        private Reponse.RateLimit ParseSendMessageResponse(HttpResponseHeaders headers)
        {
            if (!headers.Any())
                return null;
            
            if (!int.TryParse(headers.FirstOrDefault(x => string.Equals("X-Limit-App-Limit", x.Key)).Value?.FirstOrDefault() ?? string.Empty, out int limit))
                return null;

            if (!int.TryParse(headers.FirstOrDefault(x => string.Equals("X-Limit-App-Remaining", x.Key)).Value?.FirstOrDefault() ?? string.Empty, out int remaining))
                return null;

            if (!int.TryParse(headers.FirstOrDefault(x => string.Equals("X-Limit-App-Reset", x.Key)).Value?.FirstOrDefault() ?? string.Empty, out int reset))
                return null;            

            return new Reponse.RateLimit
            {
                Limit = limit,
                Remaining = remaining,
                Reset = DateTimeOffset.FromUnixTimeSeconds(reset).LocalDateTime,
                Status = string.Empty
            };
        }
    }
}
