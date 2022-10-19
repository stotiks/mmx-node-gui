using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using static Mmx.Gui.Win.Common.NativeMethods;

namespace Mmx.Gui.Win.Wpf.Plotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlotterPage plotterPage = new PlotterPage();

        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Content = plotterPage;
        }

        private void contentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        private bool closeCancel = true;
        private bool closePending = false;

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = closeCancel;
            if (!closePending)
            {
                closePending = true;
                Restore();

                if (plotterPage.PlotterIsRunning == true)
                {
                    var dialog = new ContentDialog();
                    dialog.Title = "Stop plotter before exit!"; //TODO i18n
                    dialog.PrimaryButtonText = "OK"; //TODO i18n
                    dialog.DefaultButton = ContentDialogButton.Primary;

                    var result = await dialog.ShowAsync();

                    dialog.Hide();
                    closePending = false;
                    return;
                }

                closeCancel = false;
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

    }
}
