using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class UpdateChecker : INotifyPropertyChanged
    {
        //TODO
        private static Uri releasesUri = new Uri("https://api.github.com/repos/madMAx43v3r/mmx-node/releases");
        public static Uri releaseUri = new Uri("https://github.com/madMAx43v3r/mmx-node/releases");

        private readonly HttpClient httpClient = new HttpClient();
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private bool _isUpdateAvailable = false;

        public UpdateChecker()
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");
            
            timer.Elapsed += async (o, e) => await CheckAsync();
            timer.AutoReset = true;

            timer.Interval = Settings.Default.UpdateInterval * 1000;
            timer.Enabled = Settings.Default.CheckForUpdates;

            Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Settings.Default.UpdateInterval))
                {
                    timer.Interval = Settings.Default.UpdateInterval * 1000;
                }

                if (e.PropertyName == nameof(Settings.Default.CheckForUpdates))
                {
                    timer.Enabled = Settings.Default.CheckForUpdates;

                    if(Settings.Default.CheckForUpdates)
                    {
                        Task.Run(() => CheckAsync());
                    }
                }
            };

        }

        public bool IsUpdateAvailable {
            get {
                return _isUpdateAvailable;
            }
            private set 
            {
                _isUpdateAvailable = value;
                NotifyPropertyChanged();
                if(_isUpdateAvailable)
                {
                    OnUpdateAvailable();
                }
                
            } 
        }

        public event EventHandler UpdateAvailable;

        private void OnUpdateAvailable()
        {
            if (UpdateAvailable != null)
            {
                UpdateAvailable(this, EventArgs.Empty);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task CheckAsync()
        {
            try
            {
                var response = await httpClient.GetAsync(releasesUri);
                var body = await response.Content.ReadAsStringAsync();
                JArray releases = JsonConvert.DeserializeObject<JArray>(body);
                var lastRelease = releases[0];
                var versionTag = lastRelease["tag_name"].ToString();
                var version = new Version(versionTag.Replace("v", ""));

                if (Node.Version < version)
                {
                    IsUpdateAvailable = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateChecker: " + e.Message);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
