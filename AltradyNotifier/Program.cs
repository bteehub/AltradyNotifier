using AltradyNotifier.Logic;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace AltradyNotifier
{
    public class Program
    {
        private static Pushover.Pushover _pushover = null;

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static async Task Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            Log.Debug($"Started");

            var jsonConfig = JsonConvert.DeserializeObject<Entities.Configuration.Global>(System.IO.File.ReadAllText($"{nameof(AltradyNotifier)}.json"));
            var cultureInfo = new CultureInfo(jsonConfig.CultureInfo);

            _pushover = new Pushover.Pushover(jsonConfig.Pushover.UserToken, jsonConfig.Pushover.ApplicationToken);
            await _pushover.SendMessageAsync($"Status @ {DateTime.Now.ToLongTimePattern(cultureInfo)}", "Started");

            var cancellationToken = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                try
                {
                    var altrady = new Notifier.Altrady(jsonConfig, cancellationToken.Token);
                    await altrady.RunAsync();
                }
                catch (TaskCanceledException) 
                {
                    // ignored
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex);
                    await _pushover.SendMessageAsync($"Exception occured @ {DateTime.Now.ToLongTimePattern(cultureInfo)}", $"{nameof(AltradyNotifier)} crashed: {ex.Message}");
                }
            }, cancellationToken.Token);

            Log.Info($"Start successful. Press any key to exit ... ");
            await Task.Run(() => Console.ReadKey());

            cancellationToken.Cancel();
            await Task.WhenAll(task);

            await _pushover.SendMessageAsync($"Status @ {DateTime.Now.ToLongTimePattern(cultureInfo)}", "Stopped");
            Log.Debug($"Stopped");
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(nameof(UnhandledExceptionTrapper));
            Log.Fatal(e.ToString());

            Log.Fatal((Exception)e.ExceptionObject);

            if (_pushover != null)
                _ = _pushover.SendMessageAsync($"Exception occured @ {DateTime.Now:HH:mm}", $"{nameof(AltradyNotifier)} crashed, an unhandled exception occured");
        }
    }
}
