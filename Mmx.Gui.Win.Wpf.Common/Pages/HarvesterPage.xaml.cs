using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common;
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
        private ObservableCollection<Directory> _dirs;
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _dirs = await Directory.GetDirectoriesAsync();
            } catch (Exception ex)
            {
                AddButton.IsEnabled = false;
                RemoveButton.IsEnabled = false;

                MessageBox.Show($"Can not read or parse {Node.harvesterConfigPath}\n\n{ex}", "Error!");
            }

            DirListView.ItemsSource = _dirs;
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = (sender as FrameworkElement).Tag as Directory;
            _dirs.Remove(dir);
            SaveHarvesterConfig();
            NodeApi.RemovePlotDirTask(dir.Path).ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    var flyout = new ModernWpf.Controls.Flyout
                    {
                        Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom,
                        Content = new TextBlock() { Text = "Harvester reloaded" }
                    };
                    flyout.ShowAt(DirCommandBar);
                    Console.WriteLine("Harvester reloaded");
                }));
            });
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
                NodeApi.AddPlotDirTask(dirName).ContinueWith(task =>
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
                });
            }
        }
        private void SaveHarvesterConfig()
        {
            var harvesterJson = File.ReadAllText(Node.harvesterConfigPath);
            var harvesterConfig = JObject.Parse(harvesterJson);
            ((JArray)harvesterConfig["plot_dirs"]).Clear();
            foreach (var dir in _dirs)
            {
                ((JArray)harvesterConfig["plot_dirs"]).Add(dir.Path);
            }
            File.WriteAllText(Node.harvesterConfigPath, harvesterConfig.ToString());
        }

        private void ReloadHarvesterButton_Click(object sender, RoutedEventArgs e)
        {
            NodeApi.ReloadHarvester().ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action( delegate
                {
                    new ModernWpf.Controls.Flyout
                    {
                        Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom,
                        Content = new TextBlock() { Text = "Harvester reloaded" }
                    }.ShowAt(DirCommandBar);

                    Console.WriteLine("Harvester reloaded");
                }));
            });
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
            string harvesterJson = File.ReadAllText(Node.harvesterConfigPath);
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
