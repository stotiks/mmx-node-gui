using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;
using PlotSincGui.Page;

namespace PlotSincGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly PlotSincPage plotSincPage = new PlotSincPage();
        private readonly SimpleSettingsPage settingsPage = new SimpleSettingsPage();

        public MainWindow() : base()
        {
            InitializeComponent();
            DataContext = this;
            ContentFrame.Content = plotSincPage;
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
                    case "PlotSincPage":
                        ContentFrame.Content = plotSincPage;
                        break;
                    case "SettingsPage":
                        ContentFrame.Content = settingsPage;
                        break;
                }

            }
        }

    }
}
