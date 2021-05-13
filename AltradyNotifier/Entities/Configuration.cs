using Newtonsoft.Json;
using System.Collections.Generic;

namespace AltradyNotifier.Entities
{
    public class Configuration
    {
        public class Global
        {
            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string CultureInfo { get; set; }

            public int MaxPrecision { get; set; }
            public Altrady Altrady { get; set; }
            public Pushover Pushover { get; set; }
            public List<Filter> Filter { get; set; }
        }

        public class Altrady
        {
            public int MaxApiCallsPerHour { get; set; }

            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string ApiKey { get; set; }
        }

        public class Pushover
        {
            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string UserToken { get; set; }

            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string ApplicationToken { get; set; }
        }

        public class Filter
        {
            public int Timeframe { get; set; }

            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string ExcludedMarkets { get; set; }

            public List<ExchangeMarketFilter> ExchangeMarket { get; set; }
        }

        public class ExchangeMarketFilter
        {
            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string Exchange { get; set; }

            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string Market { get; set; }

            public Volume Volume { get; set; }
            public decimal? Rise { get; set; }
            public decimal? Drop { get; set; }
            public decimal? FatFinger { get; set; }
        }

        public class Volume
        {
            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string Currency { get; set; }
            public decimal Value { get; set; }
        }
    }
}
