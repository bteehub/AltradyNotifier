using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AltradyNotifier.Api
{
    public static class Parse
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static List<Entities.Altrady.BasesEndpoint.Base> ParseBases(object result) 
            => ParseResponse<Entities.Altrady.BasesEndpoint.Response>(result)?.Bases;

        public static List<Entities.Altrady.MarketsEndpoint.Market> ParseMarkets(object result) 
            => ParseResponse<Entities.Altrady.MarketsEndpoint.Response>(result)?.Markets;

        public static List<Entities.Altrady.QuickScanEndpoint.Market> ParseQuickScan(object result) 
            => ParseResponse<Entities.Altrady.QuickScanEndpoint.Response>(result)?.Markets;

        private static T ParseResponse<T>(object result) where T : class
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");

                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(result as string);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }
    }
}
