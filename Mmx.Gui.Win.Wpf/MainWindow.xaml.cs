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
using System.ComponentModel;
using Mmx.Gui.Win.Common.Node;

namespace Mmx.Gui.Win.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly NodeProcess nodeProcess = new NodeProcess();
        private readonly MMXBoundObject mmxBoundObject = new MMXBoundObject();

        private readonly UpdateChecker updateChecker = new UpdateChecker();
        public UpdateChecker UpdateChecker => updateChecker;

        private ChromiumWebBrowser chromiumWebBrowser;

        private System.Windows.Controls.Page nodePage = new System.Windows.Controls.Page();
        private readonly HarvesterPage harvesterPage = new HarvesterPage();
        private readonly PlotterPage plotterPage = new PlotterPage();
        private readonly SettingsPage settingsPage = new SettingsPage();
        private readonly ConsoleLogsPage consoleLogsPage;

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

            if (Settings.Default.ShowConsole)
            {
                ConsoleLogsNavigationViewItem.Visibility = Visibility.Hidden;                
            } else
            {
                consoleLogsPage = new ConsoleLogsPage(nodeProcess);
            }

            if (!Settings.Default.LightMode)
            {
                InitializeCef();
            } else
            {
                nodePage = new LightModePage(nodeProcess);
            }

            InitializeNode();

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();

            BeforeClose += async (o, e) =>
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
                await nodeProcess.StopAsync();
            };

            Closing += MainWindow_Closing;
        }

        private void InitializeNode()
        {            
            nodeProcess.StartedAsync += async (sender, e) =>
            {
                if (Settings.Default.CheckForUpdates)
                {
                    await updateChecker.CheckAsync();
                }                
            };

            nodeProcess.StartAsync();
        }

        public void InitializeCef()
        {
            CefUtils.InitializeCefSharp(new CefSettings());

            chromiumWebBrowser = new ChromiumWebBrowser();
            nodePage.Content = chromiumWebBrowser;

            CefSharpSettings.WcfEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            chromiumWebBrowser.JavascriptObjectRepository.Register("mmx", mmxBoundObject, isAsync: false, options: BindingOptions.DefaultBinder);

            chromiumWebBrowser.MenuHandler = new CefUtils.SearchContextMenuHandler();
            chromiumWebBrowser.RequestHandler = new CefUtils.CustomRequestHandler();

            mmxBoundObject.KeysToPlotter += CopyKeysToPlotter;

            //node.BeforeStarted += (sender, e) => chromiumWebBrowser.LoadHtml(Node.waitStartHtml, Node.dummyUri.ToString());
            nodeProcess.BeforeStop += (sender, e) => chromiumWebBrowser.LoadHtml(NodeApi.loadingHtml, NodeApi.dummyUri.ToString());

            chromiumWebBrowser.LoadHtml(NodeApi.waitStartHtml, NodeApi.dummyUri.ToString());
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
            Dispatcher.BeginInvoke( new Func<Task>(async delegate
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First(item => item.Tag.ToString() == "PlotterPage");
                plotterPage.tabControl.SelectedItem = plotterPage.tabItemKeys;

                dynamic keys = JsonConvert.DeserializeObject(json);
                PlotterOptions.Instance.farmerkey.Value = keys["farmer_public_key"];
                PlotterOptions.Instance.nftplot.Value = false;
                PlotterOptions.Instance.poolkey.Value = keys["pool_public_key"];

                await Task.Delay(400);
                var x = new Flyout
                {
                    Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom,
                    Content = new TextBlock() { Text = "Keys copied successfully" } //TODO i18n
                };
                x.ShowAt(plotterPage.farmerKeyTextBox);

            }));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Content = settingsPage;
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                
                switch (selectedItem.Tag)
                {
                    case "NodePage":
                        ContentFrame.Content = nodePage;
                        break;
                    case "HarvesterPage":
                        ContentFrame.Content = harvesterPage;
                        break;
                    case "PlotterPage":
                        ContentFrame.Content = plotterPage;
                        break;
                    case "ConsoleLogsPage":
                        ContentFrame.Content = consoleLogsPage;
                        break;
                    case "SettingsPage":
                        ContentFrame.Content = settingsPage;
                        break;
                }

            }
        }

        private async Task MainWindow_Closing(object sender, CancelEventArgs args)
        {
            await Task.Yield();
            args.Cancel = false;

            if (plotterPage.PlotterDialog.PlotterProcess.IsRunning)
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First(item => item.Tag.ToString() == "PlotterPage");

                var dialog = new ContentDialog
                {
                    Title = "Stop plotter before exit!", //TODO i18n
                    PrimaryButtonText = "OK", //TODO i18n
                    DefaultButton = ContentDialogButton.Primary
                };

                await dialog.ShowAsync();

                dialog.Hide();
                args.Cancel = true;
                return;
            }

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
