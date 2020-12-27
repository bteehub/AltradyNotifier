using System.Collections.Generic;

namespace AltradyNotifier.Entities
{
    public class Configuration
    {
        public class Global
        {
            public string CultureInfo { get; set; }
            public int MaxPrecision { get; set; }
            public Altrady Altrady { get; set; }
            public Pushover Pushover { get; set; }
            public List<Filter> Filter { get; set; }
        }

        public class Altrady
        {
            public int MaxApiCallsPerHour { get; set; }
            public string ApiKey { get; set; }
        }

        public class Pushover
        {
            public string UserToken { get; set; }
            public string ApplicationToken { get; set; }
        }

        public class Filter
        {
            public int Timeframe { get; set; }
            public string ExcludedMarkets { get; set; }
            public List<ExchangeMarketFilter> ExchangeMarket { get; set; }
        }

        public class ExchangeMarketFilter
        {
            public string Exchange { get; set; }
            public string Market { get; set; }
            public Volume Volume { get; set; }
            public decimal Rise { get; set; }
            public decimal Drop { get; set; }

        }

        public class Volume
        {
            public string Currency { get; set; }
            public decimal Value { get; set; }
        }
    }
}
