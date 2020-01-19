using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AltradyNotifier.Api
{
    public static class Parse
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        
        public static List<Classes.Altrady.BasesEndpoint.Base> ParseBases(object result)
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");
                return null;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Classes.Altrady.BasesEndpoint.Response>(result as string);
                return response.bases;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static List<Classes.Altrady.MarketsEndpoint.Market> ParseMarkets(object result)
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");
                return null;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Classes.Altrady.MarketsEndpoint.Response>(result as string);
                return response.markets;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static List<Classes.Altrady.QuickScanEndpoint.Market> ParseQuickScan(object result)
        {
            if (result == null)
            {
                Log.Debug($"{nameof(result)} is null");
                return null;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Classes.Altrady.QuickScanEndpoint.Response>(result as string);
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
