using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common.Harvester;
using Mmx.Gui.Win.Common.Node;
using System;
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
        public HarvesterOptions harvesterOptions = new HarvesterOptions();

        public HarvesterPage()
        {            
            InitializeComponent();
            DataContext = this;

            DirListView.ItemsSource = harvesterOptions.Directories;
        }

        public HarvesterPage(RemoteHarvesterProcess remoteHarvesterProcess) : this()
        {
            this.remoteHarvesterProcess = remoteHarvesterProcess;
        }

        private RemoteHarvesterProcess remoteHarvesterProcess;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                harvesterOptions.LoadConfig();
            }
            catch (Exception ex)
            {
                AddButton.IsEnabled = false;
                RemoveButton.IsEnabled = false;

                MessageBox.Show($"Can not read or parse {NodeHelpers.harvesterConfigPath}\n\n{ex}", "Error!");
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = (sender as FrameworkElement).Tag as HarvesterOptions.Directory;
            harvesterOptions.Directories.Remove(dir);
            harvesterOptions.SaveConfig();

            if (remoteHarvesterProcess != null) 
            {
                _ = Task.Run(() => remoteHarvesterProcess.Restart()).ContinueWith(ShowFlyoutTask());
            } else
            {
                _ = NodeApi.RemovePlotDirTask(dir.Path).ContinueWith(ShowFlyoutTask());
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
                harvesterOptions.Directories.Add(new HarvesterOptions.Directory(dirName));
                harvesterOptions.SaveConfig();

                if (remoteHarvesterProcess != null)
                {
                    _ = Task.Run(() => remoteHarvesterProcess.Restart()).ContinueWith(ShowFlyoutTask());
                }
                else
                {
                    _ = NodeApi.AddPlotDirTask(dirName).ContinueWith(ShowFlyoutTask());
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

        private void ReloadHarvesterButton_Click(object sender, RoutedEventArgs e)
        {
            if (remoteHarvesterProcess != null)
            {
                _ = Task.Run(() => remoteHarvesterProcess.Restart()).ContinueWith(ShowFlyoutTask());
            }
            else
            {
                _ = NodeApi.ReloadHarvester().ContinueWith(ShowFlyoutTask());
            }
        }

    }


}
