﻿using System;
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
    }
}
