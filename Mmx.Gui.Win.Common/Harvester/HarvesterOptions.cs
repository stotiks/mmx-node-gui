using Mmx.Gui.Win.Common.Node;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.IO;

namespace Mmx.Gui.Win.Common.Harvester
{
    public class HarvesterOptions
    {
        public ObservableCollection<Directory> Directories = new ObservableCollection<Directory>();

        private object _lock = new object();
        public void LoadConfig()
        {
            lock (_lock)
            {
                string harvesterJson = File.ReadAllText(NodeHelpers.harvesterConfigPath);
                var harvesterConfig = JObject.Parse(harvesterJson);
                JArray plotDirs = harvesterConfig.Value<JArray>("plot_dirs");

                Directories.Clear();
                foreach (var dir in plotDirs)
                {
                    Directories.Add(new Directory(dir.ToString()));
                }
            }
        }

        public void SaveConfig()
        {
            lock (_lock)
            {
                var harvesterJson = File.ReadAllText(NodeHelpers.harvesterConfigPath);
                var harvesterConfig = JObject.Parse(harvesterJson);
                ((JArray)harvesterConfig["plot_dirs"]).Clear();
                foreach (var dir in Directories)
                {
                    ((JArray)harvesterConfig["plot_dirs"]).Add(dir.Path);
                }
                File.WriteAllText(NodeHelpers.harvesterConfigPath, harvesterConfig.ToString());
            }
        }

        public class Directory
        {
            public string Path { get; private set; }

            public Directory(string path)
            {
                Path = path;
            }

            public override string ToString()
            {
                return Path;
            }

        }

    }
}
