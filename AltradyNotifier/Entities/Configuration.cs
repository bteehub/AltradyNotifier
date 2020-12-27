using System.Collections.Generic;

namespace AltradyNotifier.Entities
{
    public class Configuration
    {
        public class Global
        {
            public string CultureInfo;
            public int MaxPrecision;
            public Altrady Altrady;
            public Pushover Pushover;
            public List<Filter> Filter;
        }

        public class Altrady
        {
            public int MaxApiCallsPerHour;
            public string ApiKey;
        }

        public class Pushover
        {
            public string UserToken;
            public string ApplicationToken;
        }

        public class Filter
        {
            public int Timeframe;
            public string ExcludedMarkets;
            public List<ExchangeMarketFilter> ExchangeMarket;
        }

        public class ExchangeMarketFilter
        {
            public string Exchange;
            public string Market;
            public Volume Volume;
            public decimal Rise;
            public decimal Drop;

        }

        public class Volume
        {
            public string Currency;
            public decimal Value;
        }
    }
}
