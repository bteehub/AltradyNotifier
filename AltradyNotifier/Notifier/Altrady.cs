using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AltradyNotifier.Notifier
{
    public partial class Altrady
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly Entities.Configuration.Global _config;
        private readonly CancellationToken _token;

        private readonly Api.Rest _apiRest;
        private readonly Pushover.Pushover _pushover;

        private CultureInfo _cultureInfo => new CultureInfo(_config.CultureInfo);


        public Altrady(Entities.Configuration.Global config, CancellationToken token)
        {
            _config = config;
            _token = token;

            _apiRest = new Api.Rest(config, token);
            _pushover = new Pushover.Pushover(config.Pushover.UserToken, config.Pushover.ApplicationToken);
        }

        public async Task RunAsync()
        {
            var previousQuickScan = await GetQuickScanAsync();

            while (!_token.IsCancellationRequested)
            {
                try
                {
                    // Get new data
                    var currentQuickScan = await GetQuickScanAsync();

                    // Filter data
                    previousQuickScan = FilterQuickScan(previousQuickScan);
                    currentQuickScan = FilterQuickScan(currentQuickScan);

                    // Get new items for notifications
                    var newMarketsQuickScan = GetNewQuickScan(previousQuickScan, currentQuickScan);

                    foreach (var newItems in newMarketsQuickScan)
                    {
                        foreach (var item in newItems.Value)
                        {
                            (string title, string message) pushoverMessage = default;

                            pushoverMessage.title = $"Quick Scan ({newItems.Key}) @ {(item.marketPrices?.Max(x => x.time) ?? DateTime.Now).ToLocalTime():HH:mm}";

                            pushoverMessage.message = $"Exchange: {item.exchangeName}";
                            pushoverMessage.message += $"\r\nMarket: {item.baseCurrency.ToUpper()}/{item.quoteCurrency.ToUpper()}";
                            if (item.rise != null && item.rise > 0)
                                pushoverMessage.message += $"\r\nRise: {item.rise.Value.Format(_cultureInfo, 2)}%";
                            if (item.drop != null && item.drop < 0)
                                pushoverMessage.message += $"\r\nDrop: {item.drop.Value.Format(_cultureInfo, 2)}%";

                            pushoverMessage.message += $"\r\nLast price: ₿ {item.lastPrice.Format(_cultureInfo, CalculatePrecision(item.lastPrice))}";
                            pushoverMessage.message += $"\r\nVolume: $ {item.usdVolume.Format(_cultureInfo, 0)} | ₿ {item.btcVolume.Format(_cultureInfo, 2)}";

                            Log.Debug($"Sending notification | Title: {pushoverMessage.title} | Message: {pushoverMessage.message}");
                            await _pushover.SendMessageAsync(pushoverMessage);
                        }
                    }

                    // Populate history
                    previousQuickScan = PopulateQuickScan(previousQuickScan, currentQuickScan);
                }
                catch (TaskCanceledException) { throw; }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }
}
