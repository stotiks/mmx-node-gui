﻿using Microsoft.WindowsAPICodePack.Dialogs;
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
        private readonly HarvesterOptions _harvesterOptions = new HarvesterOptions();
        public HarvesterOptions HarvesterOptions => _harvesterOptions;

        public HarvesterPage()
        {        
            InitializeComponent();
            DataContext = this;

            //DirListView.ItemsSource = _harvesterOptions.Directories;
        }

        public HarvesterPage(RemoteHarvesterProcess remoteHarvesterProcess) : this()
        {
            this.remoteHarvesterProcess = remoteHarvesterProcess;
        }

        private readonly RemoteHarvesterProcess remoteHarvesterProcess;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _harvesterOptions.LoadConfig();
            }
            catch (Exception ex)
            {
                //AddButton.IsEnabled = false;
                //RemoveButton.IsEnabled = false;

                MessageBox.Show($"Can not read or parse {NodeHelpers.harvesterConfigPath}\n\n{ex}", "Error!");
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = (sender as FrameworkElement).Tag as HarvesterOptions.Directory;
            _harvesterOptions._directories.Remove(dir);

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
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}",
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var dirName = dialog.FileName;
                _harvesterOptions._directories.Add(new HarvesterOptions.Directory(dirName));

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
