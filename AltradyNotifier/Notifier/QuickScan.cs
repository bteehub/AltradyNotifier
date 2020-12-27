using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltradyNotifier.Notifier
{
    public partial class Altrady
    {
        private async Task<Dictionary<int, List<Entities.Altrady.QuickScanEndpoint.Market>>> GetQuickScanAsync()
        {
            var quickscan = new Dictionary<int, List<Entities.Altrady.QuickScanEndpoint.Market>>();

            foreach(var timeframe in _config.Filter.Select(x => x.Timeframe))            
                quickscan.Add(timeframe, await GetQuickScanAsync(timeframe));   

            return quickscan;
        }

        private async Task<List<Entities.Altrady.QuickScanEndpoint.Market>> GetQuickScanAsync(int timeframe) => await GetQuickScanAsync(timeframe.ToString());

        private async Task<List<Entities.Altrady.QuickScanEndpoint.Market>> GetQuickScanAsync(string timeframe)
        {
            List<Entities.Altrady.QuickScanEndpoint.Market> quickscan = null;

            while (quickscan == null && !_token.IsCancellationRequested)
                quickscan = Api.Parse.ParseQuickScan(await _apiRest.GetMarketsQuickScanAsync(timeframe));

            return quickscan;
        }
    }
}
