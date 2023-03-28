using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Harvester;
using Mmx.Gui.Win.Common.Node;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Common.Pages
{
    /// <summary>
    /// Interaction logic for HarvesterPage.xaml
    /// </summary>
    public partial class HarvesterPage
    {
        public HarvesterPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public HarvesterPage(RemoteHarvesterProcess remoteHarvesterProcess) : this()
        {
            this.remoteHarvesterProcess = remoteHarvesterProcess;
        }

        private ObservableCollection<Directory> _dirs;
        private RemoteHarvesterProcess remoteHarvesterProcess;

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _dirs = await Directory.GetDirectoriesAsync();
            } catch (Exception ex)
            {
                AddButton.IsEnabled = false;
                RemoveButton.IsEnabled = false;

                MessageBox.Show($"Can not read or parse {NodeHelpers.harvesterConfigPath}\n\n{ex}", "Error!");
            }

            DirListView.ItemsSource = _dirs;
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = (sender as FrameworkElement).Tag as Directory;
            _dirs.Remove(dir);
            SaveHarvesterConfig();

            if (remoteHarvesterProcess != null) 
            {
                Task.Run(() => remoteHarvesterProcess.Restart()).ContinueWith(ShowFlyoutTask());
            } else
            {
                NodeApi.RemovePlotDirTask(dir.Path).ContinueWith(ShowFlyoutTask());
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var dirName = dialog.FileName;
                _dirs.Add(new Directory(dirName));
                SaveHarvesterConfig();

                if (remoteHarvesterProcess != null)
                {
                    Task.Run(() => remoteHarvesterProcess.Restart()).ContinueWith(ShowFlyoutTask());
                }
                else
                {
                    NodeApi.AddPlotDirTask(dirName).ContinueWith(ShowFlyoutTask());
                }
            }
        }

        private Action<Task> ShowFlyoutTask()
        {
            return task =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    new ModernWpf.Controls.Flyout
                    {
                        Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom,
                        Content = new TextBlock() { Text = "Harvester reloaded" }
                    }.ShowAt(DirCommandBar);

                    Console.WriteLine("Harvester reloaded");
                }));
            };
        }

        private void SaveHarvesterConfig()
        {
            var harvesterJson = File.ReadAllText(NodeHelpers.harvesterConfigPath);
            var harvesterConfig = JObject.Parse(harvesterJson);
            ((JArray)harvesterConfig["plot_dirs"]).Clear();
            foreach (var dir in _dirs)
            {
                ((JArray)harvesterConfig["plot_dirs"]).Add(dir.Path);
            }
            File.WriteAllText(NodeHelpers.harvesterConfigPath, harvesterConfig.ToString());
        }

        private void ReloadHarvesterButton_Click(object sender, RoutedEventArgs e)
        {
            if (remoteHarvesterProcess != null)
            {
                Task.Run(() => remoteHarvesterProcess.Restart()).ContinueWith(ShowFlyoutTask());
            }
            else
            {
                NodeApi.ReloadHarvester().ContinueWith(ShowFlyoutTask());
            }
        }
    }


    public class Directory
    {
        public string Path { get; private set; }

        public Directory(string path)
        {
            Path = path;
        }

        public static Task<ObservableCollection<Directory>> GetDirectoriesAsync()
        {
            string harvesterJson = File.ReadAllText(NodeHelpers.harvesterConfigPath);
            var harvesterConfig = JObject.Parse(harvesterJson);
            JArray plotDirs = harvesterConfig.Value<JArray>("plot_dirs");

            var dirs = new ObservableCollection<Directory>();
            foreach (var dir in plotDirs)
            {
                dirs.Add(new Directory(dir.ToString()));
            }

            return Task.FromResult(dirs);
        }

        public override string ToString()
        {
            return Path;
        }

    }
}
