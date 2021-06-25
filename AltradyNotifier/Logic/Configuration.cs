using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AltradyNotifier.Logic
{
    public static class Configuration
    {
        public static async Task<Entities.Configuration.Global> GetConfigurationAsync()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{nameof(AltradyNotifier)}.json");
            var content = await File.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject<Entities.Configuration.Global>(content);
        }
    }
}
