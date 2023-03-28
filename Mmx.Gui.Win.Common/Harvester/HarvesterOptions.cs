using Mmx.Gui.Win.Common.Node;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Harvester
{
    public class HarvesterOptions : INotifyPropertyChanged
    {
        private readonly object _lock = new object();

        public readonly ObservableCollection<Directory> _directories = new ObservableCollection<Directory>();
        public ObservableCollection<Directory> Directories => _directories;

        public bool _recursiveSearch = true;
        public int _reloadInterval = 3600;
        public int _numThreads = 16;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HarvesterOptions() 
        {
            _directories.CollectionChanged += Directories_CollectionChanged;
            PropertyChanged += SaveConfigOnPropertyChanged;
        }

        private void SaveConfigOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(ReloadInterval) || e.PropertyName == nameof(RecursiveSearch))
            {
                SaveConfig();
            }
        }

        private void Directories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Directories");
            SaveConfig();
        }

        public int ReloadInterval { 
            get => _reloadInterval;
            set
            {
                if (_reloadInterval != value)
                {
                    _reloadInterval = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int NumThreads
        {
            get => _numThreads;
            set
            {
                if (_numThreads != value)
                {
                    _numThreads = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool RecursiveSearch
        {
            get => _recursiveSearch;
            set
            {
                if (_recursiveSearch != value)
                {
                    _recursiveSearch = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void LoadConfig()
        {
            lock (_lock)
            {
                    string harvesterJson = File.ReadAllText(NodeHelpers.harvesterConfigPath);
                    var harvesterConfig = JObject.Parse(harvesterJson);
                    JArray plotDirs = harvesterConfig.Value<JArray>("plot_dirs");

                    _directories.CollectionChanged -= Directories_CollectionChanged;
                    _directories.Clear();
                    foreach (var dir in plotDirs)
                    {
                        _directories.Add(new Directory(dir.ToString()));
                    }
                    _directories.CollectionChanged += Directories_CollectionChanged;

                    ReloadInterval = harvesterConfig.Value<int>("reload_interval");
                    RecursiveSearch = harvesterConfig.Value<bool>("recursive_search");
                    NumThreads = harvesterConfig.Value<int>("num_threads");
                }
            }

        private void SaveConfig()
        {
            lock (_lock)
            {
                var harvesterJson = "{}";
                try
                {
                    harvesterJson = File.ReadAllText(NodeHelpers.harvesterConfigPath);
                } catch {}
                   
                var jObject = JObject.Parse(harvesterJson);

                if(jObject["plot_dirs"] == null)
                {
                    jObject["plot_dirs"] = new JArray();
                }

                ((JArray)jObject["plot_dirs"]).Clear();
                foreach (var dir in _directories)
                {
                    ((JArray)jObject["plot_dirs"]).Add(dir.Path);
                }
                jObject["reload_interval"] = ReloadInterval;
                jObject["recursive_search"] = RecursiveSearch;
                jObject["num_threads"] = NumThreads;

                var json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                File.WriteAllText(NodeHelpers.harvesterConfigPath, json);
            }
        }

        public class Directory: INotifyPropertyChanged
        {
            private string _path;
            public string Path { 
                get => _path;
                set {
                    if (_path != value)
                    {
                        _path = value;
                        NotifyPropertyChanged();
                    }                    
                }
            }

            public Directory(string path)
            {
                Path = path;
            }

            public override string ToString()
            {
                return Path;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
