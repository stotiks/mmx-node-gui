using Mmx.Gui.Win.Common.Node;
using Mmx.Gui.Win.Common.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Common
{
    public class UpdateChecker : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private bool _isUpdateAvailable;
        private string _url;
        private Version _currentVersion;

        public UpdateChecker(string url, Version currentVersion)
        {
            _url = url;
            _currentVersion = currentVersion;

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");
            
            _timer.Elapsed += async (o, e) => await CheckAsync();
            _timer.AutoReset = true;

            _timer.Interval = Settings.Default.UpdateInterval * 1000;
            _timer.Enabled = Settings.Default.CheckForUpdates;

            Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Settings.Default.UpdateInterval))
                {
                    _timer.Interval = Settings.Default.UpdateInterval * 1000;
                }

                if (e.PropertyName == nameof(Settings.Default.CheckForUpdates))
                {
                    _timer.Enabled = Settings.Default.CheckForUpdates;

                    if(Settings.Default.CheckForUpdates)
                    {
                        Task.Run(CheckAsync);
                    }
                }
            };

        }

        public bool IsUpdateAvailable {
            get => _isUpdateAvailable;
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
            UpdateAvailable?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task CheckAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_url);
                var body = await response.Content.ReadAsStringAsync();
                JArray releases = JsonConvert.DeserializeObject<JArray>(body);
                var lastRelease = releases[0];
                var versionTag = lastRelease["tag_name"].ToString();
                var version = new Version(versionTag.Replace("v", ""));

                if (_currentVersion < version)
                {
                    IsUpdateAvailable = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateChecker: " + e.Message);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
