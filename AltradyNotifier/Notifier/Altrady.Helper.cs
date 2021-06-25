using AltradyNotifier.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AltradyNotifier.Notifier
{
    public partial class Altrady
    {
        private static List<(string baseCurrency, string quoteCurrency)> ParseMarketString(string market)
        {
            if (string.IsNullOrEmpty(market))
                return new List<(string, string)>();

            return market.Split(',')
                .Select(x => x.Split('/'))
                .Where(x => x.Length == 2)
                .Select(x => (x[0].Trim(), x[1].Trim()))
                .Where(x => !(string.IsNullOrEmpty(x.Item1) && string.IsNullOrEmpty(x.Item2)))
                .ToList();
        }

        private int CalculatePrecision(decimal minTradeSize)
        {
            int precision = 0;

            while ((minTradeSize - (long)minTradeSize) > 0 && precision < _config.MaxPrecision)
            {
                precision++;
                minTradeSize = decimal.Multiply(minTradeSize, 10m);
            }

            return precision;
        }

        private static List<Entities.Altrady.QuickScanEndpoint.Market> GetDistinctQuickScanMarkets(List<Entities.Altrady.QuickScanEndpoint.Market> markets)
        {
            return markets
                .GroupBy(x => x.Id)
                .ToDictionary(k => k.Key, v => v.OrderByDescending(_ => _.MarketPrices.Max(_ => _.Time)).First())
                .Select(x => x.Value)
                .ToList();
        }

        private (string title, string message) CreatePushoverMessage(Entities.Altrady.QuickScanEndpoint.Market marketItem, int timeframe)
        {

            (string title, string message) pushoverMessage = default;

            // Title
            pushoverMessage.title += $"{(marketItem.FatFinger ? "Fat Finger" : "Quick Scan")} {timeframe}'";
            pushoverMessage.title += $" @ {(marketItem.MarketPrices?.Max(x => x.Time) ?? DateTime.UtcNow).ToLocalTime().ToLongTimePattern(CultureInfoLcl)}";

            // Message
            pushoverMessage.message += $"{marketItem.BaseCurrency.ToUpperInvariant()}/{marketItem.QuoteCurrency.ToUpperInvariant()} @ {marketItem.ExchangeName}";

            if (marketItem.Rise.HasValue && marketItem.Rise > 0)
                pushoverMessage.message += $"\r\n⇗ {marketItem.Rise.Value.Format(CultureInfoLcl, 1)}%";

            if (marketItem.Drop.HasValue && marketItem.Drop < 0)
                pushoverMessage.message += $"\r\n⇘ {marketItem.Drop.Value.Format(CultureInfoLcl, 1)}%";

            pushoverMessage.message += $"\r\nLast price: {marketItem.QuoteCurrency.ToUnicodeSymbol()} {marketItem.LastPrice.Format(CultureInfoLcl, CalculatePrecision(marketItem.LastPrice))}";
            pushoverMessage.message += $"\r\nVolume: {"USD".ToUnicodeSymbol()} {marketItem.UsdVolume.Format(CultureInfoLcl, 0)} | {"BTC".ToUnicodeSymbol()} {marketItem.BtcVolume.Format(CultureInfoLcl, 2)}";

            return pushoverMessage;
        }
    }
}
