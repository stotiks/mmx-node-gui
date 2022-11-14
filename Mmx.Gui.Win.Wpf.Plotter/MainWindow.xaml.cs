using Mmx.Gui.Win.Wpf.Common;
using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using System.Threading.Tasks;

namespace Mmx.Gui.Win.Wpf.Plotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WpfMainWindow
    {
        private PlotterPage plotterPage = new PlotterPage();

        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Content = plotterPage;
        }

        protected override async Task<bool> CanClose()
        {
            await Task.Yield();

            if (plotterPage.PlotterDialog.PlotterProcess.IsRunning == true)
            {
                var dialog = new ContentDialog();
                dialog.Title = "Stop plotter before exit!"; //TODO i18n
                dialog.PrimaryButtonText = "OK"; //TODO i18n
                dialog.DefaultButton = ContentDialogButton.Primary;

                var result = await dialog.ShowAsync();

                dialog.Hide();
                return false;
            }

            return true;
        }
    }
}
