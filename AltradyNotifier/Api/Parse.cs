using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AltradyNotifier.Api
{
    public static class Parse
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        
        public static List<Entities.Altrady.BasesEndpoint.Base> ParseBases(object result)
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");
                return null;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Entities.Altrady.BasesEndpoint.Response>(result as string);
                return response.bases;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static List<Entities.Altrady.MarketsEndpoint.Market> ParseMarkets(object result)
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");
                return null;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Entities.Altrady.MarketsEndpoint.Response>(result as string);
                return response.markets;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static List<Entities.Altrady.QuickScanEndpoint.Market> ParseQuickScan(object result)
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");
                return null;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Entities.Altrady.QuickScanEndpoint.Response>(result as string);
                return response.markets;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }
    }
}
