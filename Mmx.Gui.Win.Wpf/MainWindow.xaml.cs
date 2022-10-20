using CefSharp;
using CefSharp.Wpf;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using Mmx.Gui.Win.Wpf.Dialogs;
using Mmx.Gui.Win.Wpf.Pages;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Navigation;
using WPFLocalizeExtension.Engine;
using static Mmx.Gui.Win.Common.NativeMethods;

namespace Mmx.Gui.Win.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        internal new void Close()
        {
            disableCloseToNotification = true;
            base.Close();
            disableCloseToNotification = false;
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
            Dispatcher.BeginInvoke(new MethodInvoker(async delegate
            {
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == "PlotterPage").First();
                plotterPage.tabControl.SelectedItem = plotterPage.tabItemKeys;

                dynamic keys = JsonConvert.DeserializeObject(json);
                plotterPage.PlotterOptions.farmerkey.Value = keys["farmer_public_key"];
                plotterPage.PlotterOptions.nftplot.Value = false;
                plotterPage.PlotterOptions.poolkey.Value = keys["pool_public_key"];

                await Task.Delay(400);
                var x = new Flyout() { Placement = ModernWpf.Controls.Primitives.FlyoutPlacementMode.Bottom};
                x.Content = new TextBlock() { Text = "Keys copied succesfully" };
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

        private bool closeCancel = true;
        private bool disableCloseToNotification = false;
        private bool closePending = false;

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Settings.CloseToNotification && !disableCloseToNotification)
            {
                Hide();
                e.Cancel = true;
                return;
            }

            e.Cancel = closeCancel;
            if (!closePending)
            {
                closePending = true;
                Restore();

                if (plotterPage.PlotterIsRunning == true)
                {
                    nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == "PlotterPage").First();

                    var dialog = new ContentDialog();
                    dialog.Title = "Stop plotter before exit!";
                    dialog.PrimaryButtonText = "OK";
                    dialog.DefaultButton = ContentDialogButton.Primary;

                    var result = await dialog.ShowAsync();

                    dialog.Hide();
                    closePending = false;
                    return;
                }

                if (Settings.Default.ConfirmationOnExit)
                {
                    var dialog = new ContentDialog();
                    dialog.Title = "Do you want to close the application?";
                    dialog.PrimaryButtonText = "Yes";
                    dialog.SecondaryButtonText = "No";
                    dialog.DefaultButton = ContentDialogButton.Secondary;

                    var result = await dialog.ShowAsync();

                    if (result != ContentDialogResult.Primary)
                    {
                        dialog.Hide();
                        closePending = false;
                        return;
                    }

                }

                closeCancel = false;
                nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
                {
                    await node.StopAsync();
                    Close();
                }

            }
        }

        public void Restore()
        {
            if (!closePending)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            SetForegroundWindow(new WindowInteropHelper(this).Handle);
        }


        //-------------------
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                Show();
                Restore();
            }

            return IntPtr.Zero;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (Settings.MinimizeToNotification && 
                WindowState == WindowState.Minimized)
            {
                Dispatcher.BeginInvoke(new MethodInvoker(delegate
                {
                    Hide();
                }));
            }
        }

        private void contentFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
    {
                e.Cancel = true;
            }
        }
    }
}
