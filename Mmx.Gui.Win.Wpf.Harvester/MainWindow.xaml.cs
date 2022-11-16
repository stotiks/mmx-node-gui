using Mmx.Gui.Win.Wpf.Common;
using Mmx.Gui.Win.Wpf.Common.Pages;
using ModernWpf.Controls;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WpfMainWindow
    {
        private HarvesterPage harvesterPage = new HarvesterPage();
        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Content = harvesterPage;
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Tag)
            {
                case "ConnectPage":
                    contentFrame.Content = null;
                    break;
                case "HarvesterPage":
                    contentFrame.Content = harvesterPage;
                    break;
            }

        }

    }
}
