using System.Windows;
using System.Windows.Navigation;
using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using Mmx.Gui.Win.Wpf.Pages;

namespace Mmx.Gui.Win.Wpf.Plotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private PlotterPage plotterPage = new PlotterPage();

        public MainWindow()
        {
            InitializeComponent();
            //contentFrame.Content = plotterPage;
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
