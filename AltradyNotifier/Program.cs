using Newtonsoft.Json;
using System;
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

            _pushover = new Pushover.Pushover(jsonConfig.Pushover.UserToken, jsonConfig.Pushover.ApplicationToken);
            _pushover.SendMessage($"Status @ {DateTime.Now:HH:mm}", "Program started");

            var cancellationToken = new CancellationTokenSource();
            var t = Task.Run(async () =>
            {
                try
                {
                    var altrady = new Notifier.Altrady(jsonConfig, cancellationToken.Token);
                    await altrady.RunAsync();
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Log.Fatal(ex);
                    _pushover.SendMessage($"Exception occured @ {DateTime.Now:HH:mm}", $"{nameof(AltradyNotifier)} crashed: {ex.Message}");
                }
            }, cancellationToken.Token);

            Log.Info($"Start successful. Press any key to exit ... ");
            await Task.Run(() => Console.ReadKey());

            cancellationToken.Cancel();
            await Task.WhenAll(t);

            _pushover.SendMessage($"Status @ {DateTime.Now:HH:mm}", "Program stopped");
            Log.Debug($"Stopped");
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(nameof(UnhandledExceptionTrapper));
            Log.Fatal(e.ToString());

            Log.Fatal((Exception)e.ExceptionObject);

            if (_pushover != null)
                _pushover.SendMessage($"Exception occured @ {DateTime.Now:HH:mm}", $"{nameof(AltradyNotifier)} crashed, an unhandled exception occured");
        }
    }
}
