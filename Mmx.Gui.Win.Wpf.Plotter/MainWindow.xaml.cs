using Mmx.Gui.Win.Wpf.Common.Pages;
using System.Windows;
using System.Windows.Navigation;

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
    }
}
