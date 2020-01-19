﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AltradyNotifier.Notifier
{
    public partial class Altrady
    {
        /// <summary>
        /// Apply filter and clean up history
        /// </summary>
        private Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> FilterQuickScan(Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> quickScan)
        {
            foreach (var timeframe in quickScan.Keys.ToList())
            {
                // Find the timeframe filter in the config
                var filter = _config.Filter?.FirstOrDefault(x => timeframe == x.Timeframe) ?? default;

                if (filter == default)
                    continue;

                // Clean up history
                quickScan[timeframe] = quickScan[timeframe].Where(x => x.marketPrices.Max(_ => _.time) > DateTime.UtcNow.AddMinutes(-timeframe)).ToList();

                // Exclude markets
                foreach ((string baseCurrency, string quoteCurrency) in ParseMarketString(filter.ExcludedMarkets))
                {
                    if (string.IsNullOrEmpty(baseCurrency)) // Remove f.e. /BTC
                        quickScan[timeframe] = quickScan[timeframe].Where(x => !string.Equals(x.quoteCurrency, quoteCurrency, StringComparison.OrdinalIgnoreCase)).ToList();
                    else // Remove f.e. LTC/BTC
                        quickScan[timeframe] = quickScan[timeframe].Where(x => !(string.Equals(x.baseCurrency, baseCurrency, StringComparison.OrdinalIgnoreCase) && string.Equals(x.quoteCurrency, quoteCurrency, StringComparison.OrdinalIgnoreCase))).ToList();
                }

                // Filter items
                quickScan[timeframe] = ApplyMarketFilter(quickScan[timeframe], filter);
            }

            return quickScan;
        }

        /// <summary>
        /// Apply market filter
        /// </summary>
        private static List<Classes.Altrady.QuickScanEndpoint.Market> ApplyMarketFilter(List<Classes.Altrady.QuickScanEndpoint.Market> quickScan, Classes.Configuration.Filter filter)
        {
            if (filter.ExchangeMarket == default)
                return quickScan;

            var results = new List<Classes.Altrady.QuickScanEndpoint.Market>();

            foreach (var market in filter.ExchangeMarket)
            {
                var filteredMarket = new List<Classes.Altrady.QuickScanEndpoint.Market>();

                // Market
                if (ParseMarketString(market.Market).Any())
                {
                    foreach ((string baseCurrency, string quoteCurrency) in ParseMarketString(market.Market))
                    {
                        if (string.IsNullOrEmpty(baseCurrency)) // Add f.e. /BTC
                            filteredMarket.AddRange(quickScan.Where(x => string.Equals(x.quoteCurrency, quoteCurrency, StringComparison.OrdinalIgnoreCase)).ToList());
                        else // Add f.e. LTC/BTC
                            filteredMarket.AddRange(quickScan.Where(x => string.Equals(x.baseCurrency, baseCurrency, StringComparison.OrdinalIgnoreCase) && string.Equals(x.quoteCurrency, quoteCurrency, StringComparison.OrdinalIgnoreCase)).ToList());
                    }
                }
                else
                {
                    filteredMarket.AddRange(quickScan);
                }

                // Exchange
                filteredMarket = filteredMarket.Where(x => string.Equals(market.Exchange, x.exchangeCode, StringComparison.OrdinalIgnoreCase)).ToList();

                // Volume
                if (market.Volume != default)
                {
                    switch (market.Volume.Currency)
                    {
                        case "USD":
                            filteredMarket = filteredMarket.Where(x => x.usdVolume > market.Volume.Value).ToList();
                            break;
                        case "BTC":
                            filteredMarket = filteredMarket.Where(x => x.btcVolume > market.Volume.Value).ToList();
                            break;
                        default: break;
                    }
                }

                // Rise or drop
                filteredMarket = filteredMarket.Where(x => x.rise >= market.Rise || x.drop <= market.Drop).ToList();

                // Add to results
                results.AddRange(filteredMarket);
            }

            return results.Distinct().ToList();
        }

        /// <summary>
        /// Get new quick scan items
        /// </summary>
        private static Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> GetNewQuickScan(Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> previousQuickScan, Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> currentQuickScan)
        {
            var results = new Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>>();

            foreach (var timeframe in previousQuickScan.Keys.Concat(currentQuickScan.Keys).Distinct())
            {
                if (previousQuickScan.ContainsKey(timeframe) && currentQuickScan.ContainsKey(timeframe))
                {
                    results.Add(timeframe, currentQuickScan[timeframe].Except(previousQuickScan[timeframe], new Classes.Altrady.QuickScanEndpoint.MarketCompare()).ToList());
                }
                else if (currentQuickScan.ContainsKey(timeframe))
                {
                    results.Add(timeframe, currentQuickScan[timeframe]);
                }
            }

            return results;
        }

        /// <summary>
        /// Merge two quick scan lists to have a proper history list.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> PopulateQuickScan(Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> previousQuickScan, Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>> currentQuickScan)
        {
            var results = new Dictionary<int, List<Classes.Altrady.QuickScanEndpoint.Market>>();

            foreach (var timeframe in previousQuickScan.Keys.Concat(currentQuickScan.Keys).Distinct())
            {
                if (previousQuickScan.ContainsKey(timeframe))
                {
                    var items = new List<Classes.Altrady.QuickScanEndpoint.Market>(previousQuickScan[timeframe]);

                    if (currentQuickScan.ContainsKey(timeframe))
                        items.AddRange(currentQuickScan[timeframe]);

                    results.Add(
                        timeframe,
                        items.GroupBy(x => x.id).ToDictionary(k => k.Key, v => v.OrderByDescending(_ => _.marketPrices.Max(_ => _.time)).First()).Select(x => x.Value).ToList()
                        );
                }
                else if (currentQuickScan.ContainsKey(timeframe))
                {
                    results.Add(timeframe, currentQuickScan[timeframe]);
                }
            }

            return results;
        }
    }
}
