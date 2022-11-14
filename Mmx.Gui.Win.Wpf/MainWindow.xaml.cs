using CefSharp;
using CefSharp.Wpf;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using Mmx.Gui.Win.Wpf.Pages;
using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFLocalizeExtension.Engine;
using Mmx.Gui.Win.Common.Plotter;
using Mmx.Gui.Win.Wpf.Common;

namespace Mmx.Gui.Win.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WpfMainWindow
    {
        private readonly Node node = new Node();
        private readonly MMXBoundObject mmxBoundObject = new MMXBoundObject();

        private UpdateChecker _updateChecker = new UpdateChecker();
        public UpdateChecker UpdateChecker { get => _updateChecker; }

        ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser();
        private NodePage nodePage = new NodePage();
        private HarvesterPage harvesterPage = new HarvesterPage();
        private PlotterPage plotterPage = new PlotterPage();
        private SettingsPage settingsPage = new SettingsPage();

        public MainWindow()
        {
            if (Settings.Default.StartMinimized)
            {
                WindowState = WindowState.Minimized;
                if (Settings.MinimizeToNotification)
                {
                    Hide();
                }
            }

            InitializeLocalization();

            InitializeComponent();
            DataContext = this;

            InitializeCef();
            InitializeNode();

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == "NodePage").First();

            BeforeClose += async (o, e) =>
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
                await node.StopAsync();
            };

        }

        private void InitializeNode()
        {
            nodePage.Content = chromiumWebBrowser;

            node.BeforeStarted += (sender, e) => chromiumWebBrowser.LoadHtml(Node.waitStartHtml, Node.dummyUri.ToString());
            node.BeforeStop += (sender, e) => chromiumWebBrowser.LoadHtml(Node.logoutHtml, Node.dummyUri.ToString());

            node.Started += (sender, e) =>
            {
                if (Settings.Default.CheckForUpdates)
                {
                    UpdateChecker.CheckAsync();
                }                
            };

            node.StartAsync();
        }

        private void InitializeCef()
        {
            CefSharpSettings.WcfEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Register("mmx", mmxBoundObject, isAsync: false, options: BindingOptions.DefaultBinder);

            chromiumWebBrowser.MenuHandler = new CefUtils.SearchContextMenuHandler();
            chromiumWebBrowser.RequestHandler = new CefUtils.CustomRequestHandler();

            mmxBoundObject.KeysToPlotter += (json) => CopyKeysToPlotter(json);
        }

        private void InitializeLocalization()
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(Settings.Default.LanguageCode);

            Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Settings.Default.LanguageCode))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.LanguageCode);
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.LanguageCode);

                    LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(Settings.Default.LanguageCode);
                    //mmxBoundObject.Locale = Settings.Default.LanguageCode;
                }
            };
        }

        private void CopyKeysToPlotter(string json)
        {
            Dispatcher.BeginInvoke(new Action(async delegate
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == "PlotterPage").First();
                plotterPage.tabControl.SelectedItem = plotterPage.tabItemKeys;

                dynamic keys = JsonConvert.DeserializeObject(json);
                PlotterOptions.Instance.farmerkey.Value = keys["farmer_public_key"];
                PlotterOptions.Instance.nftplot.Value = false;
                PlotterOptions.Instance.poolkey.Value = keys["pool_public_key"];

                await Task.Delay(400);
                var x = new Flyout() { Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom};
                x.Content = new TextBlock() { Text = "Keys copied succesfully" }; //TODO i18n
                x.ShowAt(plotterPage.farmerKeyTextBox);

            }));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Content = settingsPage;
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                
                switch (selectedItem.Tag)
                {
                    case "NodePage":
                        contentFrame.Content = nodePage;
                        break;
                    case "HarvesterPage":
                        contentFrame.Content = harvesterPage;
                        break;
                    case "PlotterPage":
                        contentFrame.Content = plotterPage;
                        break;
                    case "SettingsPage":
                        contentFrame.Content = settingsPage;
                        break;
                }

            }
        }

        protected override async Task<bool> CanClose()
        {
            await Task.Yield();

            if (plotterPage.PlotterDialog.PlotterProcess.IsRunning == true)
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == "PlotterPage").First();

                var dialog = new ContentDialog();
                dialog.Title = "Stop plotter before exit!"; //TODO i18n
                dialog.PrimaryButtonText = "OK"; //TODO i18n
                dialog.DefaultButton = ContentDialogButton.Primary;

                var result = await dialog.ShowAsync();

                dialog.Hide();
                return false;
            }

            if (Settings.Default.ConfirmationOnExit)
            {
                var dialog = new ContentDialog();
                dialog.Title = "Do you want to close the application?"; //TODO i18n
                dialog.PrimaryButtonText = "Yes"; //TODO i18n
                dialog.SecondaryButtonText = "No"; //TODO i18n
                dialog.DefaultButton = ContentDialogButton.Secondary;

                var result = await dialog.ShowAsync();
                dialog.Hide();

                if (result != ContentDialogResult.Primary)
                {
                    return false;
                }

            }

            return true;
        }

    }
}
