using AltradyNotifier.Logic;
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

        private CultureInfo CultureInfoLcl => new CultureInfo(_config.CultureInfo);

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

                            pushoverMessage.title += $"{(item.FatFinger ? "Fat Finger" : "Quick Scan")} {newItems.Key}'";
                            pushoverMessage.title += $" @ {(item.MarketPrices?.Max(x => x.Time) ?? DateTime.UtcNow).ToLocalTime().ToLongTimePattern(CultureInfoLcl)}";

                            pushoverMessage.message += $"{item.BaseCurrency.ToUpperInvariant()}/{item.QuoteCurrency.ToUpperInvariant()} @ {item.ExchangeName}";

                            if (item.Rise.HasValue && item.Rise > 0)
                                pushoverMessage.message += $"\r\n⇗ {item.Rise.Value.Format(CultureInfoLcl, 1)}%";
                            if (item.Drop.HasValue && item.Drop < 0)
                                pushoverMessage.message += $"\r\n⇘ {item.Drop.Value.Format(CultureInfoLcl, 1)}%";

                            pushoverMessage.message += $"\r\nLast price: {item.QuoteCurrency.ToUnicodeSymbol()} {item.LastPrice.Format(CultureInfoLcl, CalculatePrecision(item.LastPrice))}";
                            pushoverMessage.message += $"\r\nVolume: {"USD".ToUnicodeSymbol()} {item.UsdVolume.Format(CultureInfoLcl, 0)} | {"BTC".ToUnicodeSymbol()} {item.BtcVolume.Format(CultureInfoLcl, 2)}";

                            Log.Debug($"Sending notification | Title: {pushoverMessage.title} | Message: {pushoverMessage.message}");
                            await _pushover.SendMessageAsync(pushoverMessage);
                        }
                    }

                    // Populate history
                    previousQuickScan = PopulateQuickScan(previousQuickScan, currentQuickScan);
                }
                catch (TaskCanceledException) 
                { 
                    throw; 
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }
}
