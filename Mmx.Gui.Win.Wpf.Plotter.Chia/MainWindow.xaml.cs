using System.ComponentModel;
using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Wpf.Plotter.Chia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly PlotterPage _plotterPage = new PlotterPage();

        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Content = _plotterPage;

            Closing += MainWindow_Closing;
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
