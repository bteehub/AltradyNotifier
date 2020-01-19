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
                .Select(x => (x[0], x[1]))
                .Where(x => !(string.IsNullOrEmpty(x.Item1) && string.IsNullOrEmpty(x.Item2)))
                .ToList();
        }

        private int CalculatePrecision(decimal minTradeSize)
        {
            int precision = 0;

            while ((minTradeSize - (long)minTradeSize) > 0 && precision < _config.MaxPrecision)
            {
                precision++;
                minTradeSize = decimal.Multiply(minTradeSize, 10);
            }

            return precision;
        }
    }
}
