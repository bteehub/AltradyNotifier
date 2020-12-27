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
            public List<Base> bases;
        }

        public class Base
        {
            public string id;
            public string baseCurrency;
            public string quoteCurrency;
            public string exchangeName;
            public string exchangeCode;
            public string longName;
            public string marketName;
            public string symbol;

            public decimal volume;
            public decimal quoteVolume;
            public decimal btcVolume;
            public decimal usdVolume;
            public decimal currentPrice;

            public LatestBase latestBase;

            public List<MarketStat> marketStats;
        }

        public class MarketStat
        {
            public string algorithm;
            public decimal ratio;
            public decimal medianDrop;
            public decimal medianBounce;
            public int hoursToRespected;
            public int crackedCount;
            public int respectedCount;
        }

        public class LatestBase
        {
            public string id;
            [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime time;
            public DateTime date;

            public decimal price;
            public decimal lowestPrice;
            public decimal bounce;
            public decimal current_drop;

            public DateTime createdAt;
            public DateTime? respectedAt;
            public bool isLowest;
        }
    }    
}
