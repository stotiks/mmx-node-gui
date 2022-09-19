using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for HarvesterPage.xaml
    /// </summary>
    public partial class HarvesterPage : Page
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

                MessageBox.Show(String.Format("Can not read or parse {0}\n\n{1}", Node.harvesterConfigPath, ex.ToString()), "Error!");
            }

            DirListView.ItemsSource = _dirs;
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = (sender as FrameworkElement).Tag as Directory;
            _dirs.Remove(dir);
            SaveHavesterConfig();
            Node.RemovePlotDirTask(dir.Path).ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    var flayout = new ModernWpf.Controls.Flyout() { Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom };
                    flayout.Content = new TextBlock() { Text = "Harvester reloaded" };
                    flayout.ShowAt((FrameworkElement)DirCommandBar);
                    Console.WriteLine("Harvester reloaded");
                }));
            });
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var dirName = dialog.FileName;
                _dirs.Add(new Directory(dirName));
                SaveHavesterConfig();
                Node.AddPlotDirTask(dirName).ContinueWith(task =>
                {
                    Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        var x = new ModernWpf.Controls.Flyout() { Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom };
                        x.Content = new TextBlock() { Text = "Harvester reloaded" };
                        x.ShowAt(DirCommandBar);
                        Console.WriteLine("Harvester reloaded");
                    }));
                });
            }
        }
        private void SaveHavesterConfig()
        {
            string harvesterJson = File.ReadAllText(Node.harvesterConfigPath);
            var harvesterConfig = JObject.Parse(harvesterJson);
            ((JArray)harvesterConfig["plot_dirs"]).Clear();
            for (int i = 0; i < _dirs.Count; i++)
            {
                ((JArray)harvesterConfig["plot_dirs"]).Add(_dirs[i].Path);
            }
            File.WriteAllText(Node.harvesterConfigPath, harvesterConfig.ToString());
        }

        private void ReloadHarvesterButton_Click(object sender, RoutedEventArgs e)
        {
            Node.ReloadHarvester().ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker( delegate
                {
                    var x = new ModernWpf.Controls.Flyout() { Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom };
                    x.Content = new TextBlock() { Text = "Harvester reloaded" };
                    x.ShowAt(DirCommandBar);
                    Console.WriteLine("Harvester reloaded");
                }));
            });
        }
    }


    public class Directory// : INotifyPropertyChanged
    {
        public string Path { get; private set; }

        public Directory(string path)
        {
            Path = path;
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        public static Task<ObservableCollection<Directory>> GetDirectoriesAsync()
        {
            string harvesterJson = File.ReadAllText(Node.harvesterConfigPath);
            var harvesterConfig = JObject.Parse(harvesterJson);
            JArray plot_dirs = harvesterConfig.Value<JArray>("plot_dirs");

            var dirs = new ObservableCollection<Directory>();
            for (int i = 0; i < plot_dirs.Count; i++)
            {
                dirs.Add(new Directory(plot_dirs[i].ToString()));
            }

            return Task.FromResult(dirs);
        }

        public override string ToString()
        {
            return Path;
        }

    }
}
