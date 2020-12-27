using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace AltradyNotifier.Entities.Altrady
{
    public class BasesEndpoint
    {
        public class Response
        {
            [JsonProperty("bases")]
            public List<Base> Bases { get; set; }
        }

        public class Base
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("baseCurrency")]
            public string BaseCurrency { get; set; }

            [JsonProperty("quoteCurrency")]
            public string QuoteCurrency { get; set; }

            [JsonProperty("exchangeName")]
            public string ExchangeName { get; set; }

            [JsonProperty("exchangeCode")]
            public string ExchangeCode { get; set; }

            [JsonProperty("longName")]
            public string LongName { get; set; }

            [JsonProperty("marketName")]
            public string MarketName { get; set; }

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

            [JsonProperty("currentPrice")]
            public decimal CurrentPrice { get; set; }

            [JsonProperty("latestBase")]
            public LatestBase LatestBase { get; set; }

            [JsonProperty("marketStats")]
            public List<MarketStat> MarketStats { get; set; }
        }

        public class MarketStat
        {
            [JsonProperty("algorithm")]
            public string Algorithm { get; set; }

            [JsonProperty("ratio")]
            public decimal Ratio { get; set; }

            [JsonProperty("medianDrop")]
            public decimal MedianDrop { get; set; }

            [JsonProperty("medianBounce")]
            public decimal MedianBounce { get; set; }

            [JsonProperty("hoursToRespected")]
            public int HoursToRespected { get; set; }

            [JsonProperty("crackedCount")]
            public int CrackedCount { get; set; }

            [JsonProperty("respectedCount")]
            public int RespectedCount { get; set; }
        }

        public class LatestBase
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("time")]
            [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime Time { get; set; }

            [JsonProperty("date")]
            public DateTime Date { get; set; }

            [JsonProperty("price")]
            public decimal Price { get; set; }

            [JsonProperty("lowestPrice")]
            public decimal LowestPrice { get; set; }

            [JsonProperty("bounce")]
            public decimal Bounce { get; set; }

            [JsonProperty("current_drop")]
            public decimal CurrentDrop { get; set; }

            [JsonProperty("createdAt")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("respectedAt")]
            public DateTime? RespectedAt { get; set; }

            [JsonProperty("isLowest")]
            public bool IsLowest { get; set; }
        }
    }    
}
