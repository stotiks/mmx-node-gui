using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace MMX_NODE_GUI
{
    internal class UpdateChecker : INotifyPropertyChanged
    {
        private static Uri releasesUri = new Uri("https://api.github.com/repos/stotiks/mmx-node/releases");
        private readonly HttpClient httpClient = new HttpClient();
        private readonly Timer timer = new Timer();
        private bool _isUpdateAvailable = false;

        public UpdateChecker()
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");

            timer.Interval = 360000;
            timer.Elapsed += (o, e) => Check();
            timer.AutoReset = true;
            timer.Enabled = true;

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

        public void Check()
        {
            Task.Run(async () =>
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

            });
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
