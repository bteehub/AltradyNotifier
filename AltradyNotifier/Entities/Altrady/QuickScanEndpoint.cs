using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace AltradyNotifier.Entities.Altrady
{
    public class QuickScanEndpoint
    {
        public class Response
        {
            public List<Market> markets;
        }

        public class Market
        {
            public int id;
            public string baseCurrency;
            public string quoteCurrency;

            public string exchangeName;
            public string exchangeCode;
            public string exchangeLogo;

            public string longName;
            public string symbol;

            public decimal volume;
            public decimal quoteVolume;
            public decimal btcVolume;
            public decimal usdVolume;

            public decimal lastPrice;
            public decimal highPrice;
            public decimal lowPrice;
            public decimal bidPrice;
            public decimal askPrice;
            public decimal? rise;
            public decimal? drop;
            public bool fatFinger;

            public List<MarketPrice> marketPrices;
        }

        public class MarketPrice
        {
            public decimal price;
            [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime time;
        }

        public class MarketCompare : IEqualityComparer<Market>
        {
            public bool Equals(Market x, Market y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                if (x.id == y.id)
                    return true;

                return false;
            }

            public int GetHashCode(Market obj)
            {
                return HashCode.Combine(obj.id);
            }
        }
    }
}
