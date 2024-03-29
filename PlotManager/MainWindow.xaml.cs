﻿using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using PlotManager.ExtensionMethods;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace PlotManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly PlotterPage _plotterPage = new PlotterPage(false);
        private static readonly Version FileVersion =
#if DEBUG
        new Version("0.0.4");
#else
    Assembly.GetExecutingAssembly().GetFileVersion();
#endif
        public string VersionTag => "v" + FileVersion.ToString();
        private readonly UpdateChecker updateChecker = new UpdateChecker(Settings.Default.PlotManager_GitHubApi_Releases, FileVersion);
        public UpdateChecker UpdateChecker => updateChecker;

        public MainWindow()
        {
            var _tmpAssemblyUssage = Assembly.GetExecutingAssembly().GetFileVersion();

            InitializeComponent();
            DataContext = this;

            ContentFrame.Content = _plotterPage;

            Closing += MainWindow_Closing;

            if (Settings.Default.CheckForUpdates)
            {
                _ = updateChecker.CheckAsync();
            }

        }

        private async Task MainWindow_Closing(object sender, CancelEventArgs args)
        {
            await Task.Yield();

            if (_plotterPage.PlotterDialog.PlotterProcess.IsRunning)
            {
                var dialog = new ContentDialog
                {
                    Title = "Stop plotter before exit!", //TODO i18n
                    PrimaryButtonText = "OK", //TODO i18n
                    DefaultButton = ContentDialogButton.Primary
                };

                await dialog.ShowAsync();

                dialog.Hide();
                args.Cancel = true;
            }
        }
    }
}
