using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AltradyNotifier.Api
{
    public class Rest
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        
        private readonly Entities.Configuration.Global _config;
        private readonly CancellationToken _token;

        private int _fallBackMultiplier;
        
        public Rest(Entities.Configuration.Global config, CancellationToken token)
        {
            _config = config;
            _token = token;

            _fallBackMultiplier = 1;
        }

        public async Task<object> GetBasesAsync(string algorithm = null)
        {
            string endpoint = "/bases";

            var param = new List<(string, string)>();

            if (string.IsNullOrEmpty(algorithm))
                param.Add(("algorithm", algorithm));

            return await GetDataAsync(endpoint, null);
        }

        public async Task<object> GetMarketsAsync(string algorithm, string exchangeCode)
        {
            string endpoint = "/markets";

            var param = new List<(string, string)>
            {
                ("algorithm", algorithm),
                ("exchange_code", exchangeCode)
            };

            return await GetDataAsync(endpoint, param);
        }

        public async Task<object> GetMarketsQuickScanAsync(string timeframe)
        {
            string endpoint = "/markets/quick_scan";

            var param = new List<(string, string)>
            {
                ("timeframe", timeframe)
            };

            return await GetDataAsync(endpoint, param);
        }

        private async Task<object> GetDataAsync(string endpoint, List<(string key, string value)> param = null)
        {
            await DelayApiCall();

            try
            {
                string apiUrl = "https://api.cryptobasescanner.com/v1";

                // Create URL
                string requestUri = $"{apiUrl}{endpoint}?api_key={_config.Altrady.ApiKey}";

                if (param != null)                
                    requestUri += string.Join('&', param.Select(x => $"{x.key}={x.value}"));

                // Create request with headers
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri)
                };

                // Send Request
                using var client = new HttpClient();

                var response = await client.SendAsync(request, _token);
                if (response.IsSuccessStatusCode)
                {
                    _fallBackMultiplier = 1;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Log.Debug($"Response successful: {response.IsSuccessStatusCode}, status code: {response.StatusCode}, reason: {response.ReasonPhrase}");

                    if (response.StatusCode == HttpStatusCode.TooManyRequests || response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.GatewayTimeout)
                        _fallBackMultiplier *= 2;
                }
            }
            catch (TaskCanceledException) { throw; }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        private async Task DelayApiCall()
        {
            double maxApiCallsPerMilliSecond = _config.Altrady.MaxApiCallsPerHour / 60d / 60d / 1000d;

            int apiDelay = (int)(1d / maxApiCallsPerMilliSecond);
            apiDelay += new Random().Next(251, 499); // Add some additional delay

            _fallBackMultiplier = Math.Min(_fallBackMultiplier, int.MaxValue / apiDelay); // prevent int overflow on next line

            await Task.Delay(_fallBackMultiplier * apiDelay, _token);
        }
    }
}
