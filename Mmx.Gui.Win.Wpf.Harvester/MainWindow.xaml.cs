using Mmx.Gui.Win.Common.Harvester;
using Mmx.Gui.Win.Common.Properties;
using Mmx.Gui.Win.Wpf.Common.Pages;
using Mmx.Gui.Win.Wpf.Harvester.Pages;
using ModernWpf.Controls;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly RemoteHarvesterProcess harvesterProcess = new RemoteHarvesterProcess();

        private readonly ConnectionPage _connectionPage;
        private readonly HarvesterPage _harvesterPage;
        private readonly HarvesterSettingsPage _harvesterSettingsPage = new HarvesterSettingsPage();

        public MainWindow(): base()
        {
            if (Settings.Default.StartMinimized)
            {
                WindowState = WindowState.Minimized;
                if (Settings.MinimizeToNotification)
                {
                    Hide();
                }
            }

            InitializeComponent();

            _harvesterPage = new HarvesterPage(harvesterProcess);
            _connectionPage = new ConnectionPage(harvesterProcess);
            ContentFrame.Content = _connectionPage;

            BeforeClose += async (o, e) =>
            {
                await harvesterProcess.StopAsync();
            };

            Closing += MainWindow_Closing;

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();

            if(Settings.Default.ConnectOnStart)
            {
                harvesterProcess.Start();
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Tag)
            {
                case "ConnectionPage":
                    ContentFrame.Content = _connectionPage;
                    break;
                case "HarvesterPage":
                    ContentFrame.Content = _harvesterPage;
                    break;
                case "HarvesterSettingsPage":
                    ContentFrame.Content = _harvesterSettingsPage;
                    break;
            }

        }

        private async Task MainWindow_Closing(object sender, CancelEventArgs args)
        {
            await Task.Yield();
            args.Cancel = false;

            if (Settings.Default.ConfirmationOnExit)
            {
                var dialog = new ContentDialog
                {
                    Title = "Do you want to close the application?", //TODO i18n
                    PrimaryButtonText = "Yes", //TODO i18n
                    SecondaryButtonText = "No", //TODO i18n
                    DefaultButton = ContentDialogButton.Secondary
                };

                var result = await dialog.ShowAsync();
                dialog.Hide();

                if (result != ContentDialogResult.Primary)
                {
                    args.Cancel = true;
                }

            }
        }
    }
}
