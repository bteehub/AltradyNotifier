using System;
using System.Globalization;

namespace AltradyNotifier.Logic
{
    public static class Extensions
    {
        public static string Format(this decimal rate, CultureInfo cultureInfo, int precision)
        {
            string format = "#,##0";

            if (precision > 0)
                format += $".{new string('0', precision)}";

            return rate.ToString(format, cultureInfo);
        }

        public static string ToLongTimePattern(this DateTime dateTime, CultureInfo cultureInfo)
        {
            return dateTime.ToString(cultureInfo.DateTimeFormat.LongTimePattern);
        }

        /// <summary>
        /// Taken from https://www.reddit.com/r/CryptoCurrency/comments/mt0nci/list_of_all_known_cryptocurrency_unicodes_and/
        /// </summary>
        public static string ToUnicodeSymbol(this string symbol)
        {
            return symbol.ToUpperInvariant() switch
            {
                "BAT" => "⟁",
                "BCH" => "Ƀ",
                "BSV" => "Ɓ",
                "BTC" => "₿",
                "DAI" => "◈",
                "DOGE" => "Ð",
                "EUR" => "€",
                "EURS" => "€",
                "EOS" => "ε",
                "ETC" => "ξ",
                "ETH" => "Ξ",
                "FIL" => "⨎",
                "GBP" => "£",
                "LSK" => "Ⱡ",
                "LTC" => "Ł",
                "NANO" => "Ñ",
                "NAV" => "Ꞥ",
                "NMC" => "ℕ",
                "PPC" => "Ᵽ",
                "RDD" => "Ɍ",
                "STEEM" => "ȿ",
                "THETA" => "ϑ",
                "TUSD" => "$",
                "USD" => "$",
                "USDC" => "$",
                "USDT" => "$",
                "XLM" => "🚀",
                "XMR" => "ɱ",
                "XPM" => "Ψ",
                "XTZ" => "ꜩ",
                "ZEC" => "ⓩ",
                _ => symbol.ToUpperInvariant(),
            };
        }

        public static bool HasUnicodeSymbol(this string symbol)
        {
            return !string.Equals(symbol.ToUnicodeSymbol(), symbol, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
