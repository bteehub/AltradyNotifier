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
            [JsonProperty("markets")]
            public List<Market> Markets { get; set; }
        }

        public class Market
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("baseCurrency")]
            public string BaseCurrency { get; set; }

            [JsonProperty("quoteCurrency")]
            public string QuoteCurrency { get; set; }


            [JsonProperty("exchangeName")]
            public string ExchangeName { get; set; }

            [JsonProperty("exchangeCode")]
            public string ExchangeCode { get; set; }

            [JsonProperty("exchangeLogo")]
            public string ExchangeLogo { get; set; }

            
            [JsonProperty("longName")]
            public string LongName { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }


            [JsonProperty("volume")]
            public decimal Volume { get; set; }

            [JsonProperty("quoteVolume")]
            public decimal QuoteVolume { get; set; }

            [JsonProperty("btcVolume")]
            public decimal BtcVolume { get; set; }

            [JsonProperty("usdVolume")]
            public decimal UsdVolume { get; set; }


            [JsonProperty("lastPrice")]
            public decimal LastPrice { get; set; }

            [JsonProperty("highPrice")]
            public decimal HighPrice { get; set; }

            [JsonProperty("lowPrice")]
            public decimal LowPrice { get; set; }

            [JsonProperty("bidPrice")]
            public decimal BidPrice { get; set; }

            [JsonProperty("askPrice")]
            public decimal AskPrice { get; set; }

            [JsonProperty("rise")]
            public decimal? Rise { get; set; }

            [JsonProperty("drop")]
            public decimal? Drop { get; set; }

            [JsonProperty("fatFinger")]
            public bool FatFinger { get; set; }


            [JsonProperty("marketPrices")]
            public List<MarketPrice> MarketPrices { get; set; }
        }

        public class MarketPrice
        {
            [JsonProperty("price")]
            public decimal Price { get; set; }

            [JsonProperty("time")]
            [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime Time { get; set; }
        }

        public class MarketCompare : IEqualityComparer<Market>
        {
            public bool Equals(Market x, Market y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                if (x.Id == y.Id)
                    return true;

                return false;
            }

            public int GetHashCode(Market obj)
            {
                return HashCode.Combine(obj.Id);
            }
        }
    }
}
