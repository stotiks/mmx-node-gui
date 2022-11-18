using Mmx.Gui.Win.Wpf.Common.Pages;
using Mmx.Gui.Win.Wpf.Harvester.Pages;
using ModernWpf.Controls;
using System.Linq;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly HarvesterPage _harvesterPage = new HarvesterPage();
        private readonly ConnectionPage _connectionPage = new ConnectionPage();
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Content = _connectionPage;

            nav.SelectedItem = nav.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            switch (selectedItem.Tag)
            {
                case "ConnectionPage":
                    ContentFrame.Content = _connectionPage;
                    break;
                case "HarvesterPage":
                    ContentFrame.Content = _harvesterPage;
                    break;
            }

        }
    }
}
